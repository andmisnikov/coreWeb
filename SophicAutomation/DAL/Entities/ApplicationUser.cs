using System;
using DAL.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace DAL.Entities
{
    public class ApplicationUser : IdentityUser, IHasId<string>
    {
        public string Name { get; set; }

        public string Surname { get; set; }

        public string Street { get; set; }

        public string Zip { get; set; }

        public string City { get; set; }

        public DateTimeOffset? RegisterDate { get; set; }
    }
}
