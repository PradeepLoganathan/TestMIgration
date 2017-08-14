using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace testmigration.Models.ManageViewModels
{
    public class IndexViewModel
    {
        public bool HasPassword { get; set; }

        public IList<UserLoginInfo> Logins { get; set; }

        public string PhoneNumber { get; set; }

        public bool TwoFactor { get; set; }

        public bool BrowserRemembered { get; set; }

        public bool IsAccessToAddEditUser { get; set; }

        public string FullName { get; set; }
        public string DOB { get; set; }
        public string PictureUrl { get; set; }
        public string Response { get; set; }
        public string Identifier { get; set; }
    }
}
