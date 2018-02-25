using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Beervolution.Models
{
    public class User
    {
        public User()
        {
            Brews = new List<Brew>();
        }

        [Key]
        public int ID { get; set; }

        public string Name { get; set; }

        public string OID { get; set; }

        public List<Beer> Beers { get; set; }

        public List<Brew> Brews { get; set; }

        public DateTime CreatedDate { get; set; }
    }
}