using Beervolution.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Beervolution.ViewModels
{
    public class CreateBrewViewModel
    {
        public CreateBrewViewModel()
        {
            Brew = new Brew();
            Brew.Variables = new Variables();
        }

        [Required]
        public int BeerID { get; set; }

        public Brew Brew { get; set; }

        [Display(Name = "New Water Type")]
        public string NewWaterType { get; set; }

        public string OriginalWaterType { get; set; }

        [Display(Name = "New Fermentable Type")]
        public string NewFermentableType { get; set; }

        public string OriginalFermentableType { get; set; }
    }
}