using Beervolution.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Beervolution.ViewModels
{
    public class CreateBeerViewModel
    {
        public CreateBeerViewModel()
        {
            Beer = new Beer();
        }

        public Beer Beer { get; set; }

        [Display(Name = "New Manufacturer")]
        public string NewManufacturer { get; set; }

        public int OriginalManufacturer { get; set; }

        [Display(Name = "New Type")]
        public string NewType { get; set; }

        public string OriginalBeerType { get; set; }
    }
}