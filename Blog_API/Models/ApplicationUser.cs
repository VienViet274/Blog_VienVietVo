﻿using Microsoft.AspNetCore.Identity;

namespace Blog_API.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; }
    }
}
