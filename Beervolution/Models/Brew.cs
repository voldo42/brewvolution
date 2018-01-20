using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Beervolution.Models
{
    public class Brew
    {
        [Key]
        public int ID { get; set; }

        public Variables Variables { get; set; }

        [Display(Name="Starting Gravity")]
        public double StartingGravity { get; set; }

        [Display(Name = "Final Gravity")]
        public double FinalGravity { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Bottle Date")]
        public DateTime BottleDate { get; set; }

        [Display(Name = "Secondary Fermentation")]
        public bool SecondaryFermentation { get; set; }

        [Display(Name = "Secondary Fermentation Date")]
        public DateTime? SecondaryFermentationDate { get; set; }

        public List<Review> Reviews { get; set; }
    }
}