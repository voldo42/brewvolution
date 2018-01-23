using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Beervolution.Models
{
    public class Variables
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Fermentable Type")]
        public string FermentableType { get; set; }

        // measured mass as kg
        [Display(Name = "Fermentable Amount")]
        public double FermentableAmount { get; set; }

        [Required]
        [Display(Name = "Water Type")]
        public string WaterType { get; set; }

        [Display(Name = "Total Volume")]
        // volume of whole brew (litres)
        public double TotalVolume { get; set; }

        [Display(Name = "Hop Details")]
        public string HopDetails { get; set; }

        [Display(Name = "Brew Temperature")]
        public double BrewTemp { get; set; }
    }
}