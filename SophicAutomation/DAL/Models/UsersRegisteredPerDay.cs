using System;

namespace DAL.Models
{
    public class UsersRegisteredPerDay
    {
        public DateTime Day { get; set; }
        public int UsersRegistered { get; set; }
    }
}
