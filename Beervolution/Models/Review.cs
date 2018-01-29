using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Beervolution.Models
{
    public class Review
    {
        [Key]
        public int ID { get; set; }

        [Display(Name = "Taste Rating")]
        public int TasteRating { get; set; }

        [Display(Name = "Head Rating")]
        public int HeadRating { get; set; }

        [Display(Name = "Overall Rating")]
        public int OverallRating { get; set; }

        public string Comments { get; set; }

        public User CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}