using Blogger.Models;
using KryptonitenBlog.Common;
using KryptonitenBlog.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Blogger.Inıt
{
    public class WebCommon : ICommon
    {
        public string GetCurrentUsername()
        {
            BlogUser user = SessionManager.User;
            if(user != null)
            {
                return user.Username;
            }

            return "system";
            
        }
    }
}