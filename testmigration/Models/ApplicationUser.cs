using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace testmigration.Models
{
    // Add profile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        //public virtual UserData UserData { get; set; }
        public DateTime? DOB { get; set; }
        public string FullName { get; set; }
        public string PictureUrl { get; set; }
    }
    public class UserData
    {
        public string UserId { get; set; }
        public string DOB { get; set; }
        public string PictureUrl { get; set; }
        public string Name { get; set; }
        public int Gender { get; set; }
    }
}
