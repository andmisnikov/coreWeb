using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.IO;

namespace Common
{
    public class CsvExport : IDisposable
    {
        public CsvExport()
        {
            Delimiter = ";";
        }

        private bool _disposed;

        readonly IList<string> _fields = new List<string>();
        readonly IList<object[]> _rows = new List<object[]>();


        private object[] CurrentRow
        {
            set
            {
                this._rows[this._rows.Count - 1] = value;
            }
            get
            {
                return this._rows[this._rows.Count - 1];
            }
        }

        public object this[string field]
        {
            set
            {
                if (!this._fields.Contains(field))
                {
                    this._fields.Add(field);
                    object[] resizedArray = this.CurrentRow;
                    Array.Resize(ref resizedArray, resizedArray.Length + 1);
                    this.CurrentRow = resizedArray;
                }

                var id = this._fields.IndexOf(field);
                this.CurrentRow[id] = value;
            }
        }

        public void AddRow()
        {
            _rows.Add(new object[_fields.Count]);
        }

        /// <summary>
        /// Converts a value to how it should output in a csv file
        /// If it has a comma, it needs surrounding with double quotes
        /// Eg Sydney, Australia -> "Sydney, Australia"
        /// Also if it contains any double quotes ("), then they need to be replaced with quad quotes[sic] ("")
        /// Eg "Dangerous Dan" McGrew -> """Dangerous Dan"" McGrew"
        /// </summary>
        string MakeValueCsvFriendly(object value)
        {
            if (value == null) return string.Empty;
            var nullable1 = value as INullable;
            if (nullable1 != null && nullable1.IsNull) return string.Empty;
            if (value is DateTime)
            {
                var dateValue = (DateTime)value;
                if (dateValue == DateTime.MinValue.ToUniversalTime()) return string.Empty;
                if (dateValue.TimeOfDay.TotalSeconds == 0)
                    return dateValue.ToString("yyyy-MM-dd");
                return dateValue.ToString("yyyy-MM-dd HH:mm:ss");
            }
            string output = value.ToString();
            if (output.Contains("\n"))
            {
                output = output.Replace("\"", "\"\"");
                output = "\"" + output.Replace("\n", " ") /*ch10.ToString() + ch13.ToString())*/ + "\"";
            }
            else if (output.Contains(Delimiter) || output.Contains("\""))
                output = '"' + output.Replace("\"", "\"\"") + '"';
            return output;
        }

        public string Delimiter { get; set; }

        /// <summary>
        /// Exports as raw UTF8 bytes
        /// </summary>
        public byte[] ExportToBytes()
        {
            var stream = ExportToMemoryStream();
            return stream.ToArray();
        }

        public MemoryStream ExportToMemoryStream()
        {
            var s = new MemoryStream();
            ExportToStream(s);
            return s;
        }

        public long ExportToStream(Stream stream)
        {
            var writer = new StreamWriter(stream);
            // The header
            writer.Write(string.Join(Delimiter, _fields));
            writer.WriteLine();

            // The rows
            foreach (var row in _rows)
            {
                for (var i = 0; i < _fields.Count; i++)
                {
                    var value = row[i];
                    writer.Write(MakeValueCsvFriendly(value));
                    if (i != _fields.Count - 1)
                        writer.Write(Delimiter);
                }
                writer.WriteLine();
            }
            writer.Flush();
            stream.Position = 0;
            return writer.BaseStream.Length;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;
            if (disposing)
            {
                _fields.Clear();
                _rows.Clear();
            }
            _disposed = true;
        }
    }
}
