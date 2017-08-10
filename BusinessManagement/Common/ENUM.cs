using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessManagement.Common
{
    public class ENUM
    {
        public enum EmailMessageType
        {
            UserEmailIdVerify = 1,
            ForgetPassword = 2,
            TwoFactorAuth = 3
        }
    }
}
