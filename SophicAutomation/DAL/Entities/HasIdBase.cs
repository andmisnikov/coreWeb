using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using DAL.Interfaces;

namespace DAL.Entities
{
    public abstract class HasIdBase<TKey> : IHasId<TKey> where TKey : IEquatable<TKey>
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public virtual TKey Id { get; set; }
    }
}
