﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Beervolution.Models
{
    public class Brew
    {
        public Brew()
        {
            Variables = new Variables();
        }

        [Key]
        public int ID { get; set; }

        [Display(Name = "Description")]
        public string Description { get; set; }

        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Bottle Date")]
        public DateTime? BottleDate { get; set; }

        [Display(Name="Starting Gravity")]
        public double StartingGravity { get; set; }

        [Display(Name = "Final Gravity")]
        public double? FinalGravity { get; set; }

        [Display(Name = "Secondary Fermentation")]
        public bool SecondaryFermentation { get; set; }

        public Variables Variables { get; set; }

        public List<Review> Reviews { get; set; }
    }
}