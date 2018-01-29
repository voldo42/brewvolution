using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Beervolution.Models
{
    public class UserComment
    {
        [Key]
        public int ID { get; set; }

        public string Comment { get; set; }

        public User User { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}