﻿using Microsoft.AspNetCore.Identity;
using System;

namespace Application.Domain
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Salt { get; set; }
    }
}
