using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Beervolution.Models
{
    public class Beer
    {
        public Beer()
        {
            Brews = new List<Brew>();
            Comments = new List<UserComment>();
        }

        [Key]
        public int ID { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Type { get; set; }
      
        [ForeignKey("Manufacturer")]
        [Required(ErrorMessage = "Manufacturer is required")]
        [Display(Name = "Manufacturer")]
        public int ManufacturerID { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }  
        
        [Display(Name = "Percentage")]
        public double TargetPercentage { get; set; }

        [Display(Name = "Inclusive Kit")]
        public bool InclusiveKit { get; set; }

        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; }

        public List<Brew> Brews { get; set; }

        public List<UserComment> Comments { get; set; }

        public virtual User CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public bool Deleted { get; set; }
    }
}