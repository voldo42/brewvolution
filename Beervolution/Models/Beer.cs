using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Beervolution.Models
{
    public class Beer
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
        public string Manufacturer { get; set; }

        public string Type { get; set; }

        [Display(Name="Inclusive Kit")]
        // an inclusive kit will contain all fermentables, and single can kit will need additional fermentables.
        public bool InclusiveKit { get; set; }

        [Display(Name = "Percentage")]
        public double TargetPercentage { get; set; }

        public List<Brew> Brews { get; set; }
    }
}