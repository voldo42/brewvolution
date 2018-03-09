using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Beervolution.Models
{
    public class Review
    {
        public Review() { }

        public Review(int brewID)
        {
            BrewID = brewID;
        }

        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Taste")]
        public int TasteRating { get; set; }

        [Required]
        [Display(Name = "Head Retention")]
        public int HeadRating { get; set; }

        [Required]
        [Display(Name = "Clarity")]
        public int ClarityRating { get; set; }

        [Required]
        [Display(Name = "Overall Rating")]
        public int OverallRating { get; set; }

        public string Comments { get; set; }

        public User CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        [ForeignKey("Brew")]
        public int BrewID { get; set; }

        public virtual Brew Brew { get; set; }
    }
}