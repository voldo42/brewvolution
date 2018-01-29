using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Beervolution.Models
{
    public class Brew
    {
        public Brew()
        {
            Variables = new Variables();
            Reviews = new List<Review>();
            Comments = new List<UserComment>();
            StartDate = DateTime.Today;
        }

        [Key]
        public int ID { get; set; }

        [ForeignKey("Beer")]
        [Required (ErrorMessage = "Beer is required")]
        public int BeerID { get; set; }

        public virtual Beer Beer { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Bottle Date")]
        public DateTime? BottleDate { get; set; }

        [Display(Name="Starting Gravity")]
        public double? StartingGravity { get; set; }

        [Display(Name = "Final Gravity")]
        public double? FinalGravity { get; set; }

        [Display(Name = "Secondary Fermentation")]
        public bool SecondaryFermentation { get; set; }

        public Variables Variables { get; set; }

        public List<Review> Reviews { get; set; }

        public List<UserComment> Comments { get; set; }
    }
}