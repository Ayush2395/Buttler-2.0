﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace Buttler.Test.Domain.Models
{
    public partial class UserDetails
    {
        public string Uid { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public int? Age { get; set; }

        public virtual AspNetUsers UidNavigation { get; set; }
    }
}