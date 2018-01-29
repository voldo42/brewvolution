using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Beervolution.Models
{
    public class Manufacturer
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [Display(Name = "Manufacturer")]
        public string Name { get; set; }

        public virtual List<Beer> Beers { get; set; }
    }
}