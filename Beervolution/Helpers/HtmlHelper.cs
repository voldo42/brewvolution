using Beervolution.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Beervolution.Helpers
{
    public class HtmlHelper
    {
        private static BeerContext context = new BeerContext();

        public static HtmlString CreateBeerDropDown(int? selectedIndex)
        {
            var dropDown = new TagBuilder("select");
            dropDown.Attributes.Add("name", "Beer");
            dropDown.Attributes.Add("id", "Beer");
            dropDown.AddCssClass("dropdown-toggle form-control");

            // get beers
            List<Beer> beers = context.Beers.Distinct().ToList();

            StringBuilder beerList = new StringBuilder();
            // add a default item of 'Select...'
            var placeholderOption = new TagBuilder("option") { InnerHtml = "Select..." };
            beerList.AppendLine(placeholderOption.ToString());

            foreach (Beer beer in beers)
            {
                string beerName = String.Format("{0} ({1})", beer.Name, beer.Manufacturer);
                var option = new TagBuilder("option") { InnerHtml = beerName };
                option.Attributes.Add("value", beer.ID.ToString());

                if (selectedIndex == beer.ID)
                {
                    option.MergeAttribute("selected", "selected");
                }

                beerList.AppendLine(option.ToString());
            }

            dropDown.InnerHtml = beerList.ToString();

            return new HtmlString(dropDown.ToString());
        }
    }
}