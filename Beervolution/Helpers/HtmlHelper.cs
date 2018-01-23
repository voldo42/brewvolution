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
            dropDown.Attributes.Add("name", "BeerID");
            dropDown.Attributes.Add("id", "BeerID");
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

        public static HtmlString CreateWaterTypeDropDown(string selectedWaterType)
        {
            var dropDown = new TagBuilder("select");
            dropDown.Attributes.Add("name", "WaterType");
            dropDown.Attributes.Add("id", "WaterType");
            dropDown.AddCssClass("dropdown-toggle form-control");

            // Get water types
            List<string> waterTypes = context.Brews.Select(s => s.Variables.WaterType).Distinct().ToList();
            // Remove blanks
            waterTypes.Remove("");
            waterTypes.Remove(null);

            StringBuilder waterTypeList = new StringBuilder();

            // add a default item of 'Select...'
            var placeholderOption = new TagBuilder("option") { InnerHtml = "Select..." };
            waterTypeList.AppendLine(placeholderOption.ToString());

            foreach (string type in waterTypes)
            {
                var option = new TagBuilder("option") { InnerHtml = type };
                option.Attributes.Add("value", type);

                if (selectedWaterType == type)
                {
                    option.MergeAttribute("selected", "selected");
                }

                waterTypeList.AppendLine(option.ToString());
            }

            // add option for new water type
            var newWaterTypeOption = new TagBuilder("option") { InnerHtml = "New Water Type" };
            waterTypeList.AppendLine(newWaterTypeOption.ToString());

            dropDown.InnerHtml = waterTypeList.ToString();
            return new HtmlString(dropDown.ToString());
        }

        public static HtmlString CreateFermentableTypeDropDown(string selectedFermentableType)
        {
            var dropDown = new TagBuilder("select");
            dropDown.Attributes.Add("name", "FermentableType");
            dropDown.Attributes.Add("id", "FermentableType");
            dropDown.AddCssClass("dropdown-toggle form-control");

            // Get fermentable types
            List<string> fermentableTypes = context.Brews.Select(s => s.Variables.FermentableType).Distinct().ToList();
            // Remove blanks
            fermentableTypes.Remove("");
            fermentableTypes.Remove(null);

            StringBuilder fermentableTypeList = new StringBuilder();

            // add a default item of 'Select...'
            var placeholderOption = new TagBuilder("option") { InnerHtml = "Select..." };
            fermentableTypeList.AppendLine(placeholderOption.ToString());

            foreach (string type in fermentableTypes)
            {
                var option = new TagBuilder("option") { InnerHtml = type };
                option.Attributes.Add("value", type);

                if (selectedFermentableType == type)
                {
                    option.MergeAttribute("selected", "selected");
                }

                fermentableTypeList.AppendLine(option.ToString());
            }

            // add option for new fermentable type
            var newWaterTypeOption = new TagBuilder("option") { InnerHtml = "New Fermentable Type" };
            fermentableTypeList.AppendLine(newWaterTypeOption.ToString());

            dropDown.InnerHtml = fermentableTypeList.ToString();
            return new HtmlString(dropDown.ToString());
        }
    }
}