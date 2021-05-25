using System;
using System.ComponentModel.DataAnnotations;

namespace BL.Dto
{
    public class UserDto
    {
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Name { get; set; }

        public string Surname { get; set; }

        public string Street { get; set; }

        public string Zip { get; set; }

        public string City { get; set; }

        [Display(Name = "Register Date")]
        [DisplayFormat(DataFormatString = "{0:d}", ApplyFormatInEditMode = true)]
        public DateTimeOffset RegisterDate { get; set; }

        public string ConcurrencyStamp { get; set; }

        public string Id { get; set; }
    }
}
