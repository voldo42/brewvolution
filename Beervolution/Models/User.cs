using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Beervolution.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string SID { get; set; }

        public List<Brew> Brews { get; set; }

        [Display(Name = "Permission Group")]
        public Group PermissionGroup { get; set; }

        public enum Group
        {
            Admin,
            Reviewer,
            Guest
        };
    }
}