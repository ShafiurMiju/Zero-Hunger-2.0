﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MidAssignment.Auth
{
    public class ELogged : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (httpContext.Session["Email"] != null && httpContext.Session["User"] == "employee")
            {
                return true;
            }
            return false;
        }
    }
}