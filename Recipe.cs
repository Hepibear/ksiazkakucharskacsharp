using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RecipeManager
{

    public class Recipe
    {

        public enum Type
        {
            Sniadanie,
            Obiad,
            Kolacja
        };

        public int Id { get; set; }
        public string Title { get; set; }
        public List<string> Ingredients { get; set; }
        public List<string> Quantity { get; set; }
        public List<string> Steps { get; set; }
        public Type TypeOfDinner { get; set; }
        public string Time { get; set; }

        public string getDinnerType()
        {
            if (TypeOfDinner == Type.Sniadanie)
                return "śniadanie";
            else if (TypeOfDinner == Type.Obiad)
                return "obiad";
            else if (TypeOfDinner == Type.Kolacja)
                return "kolację";
            else return "EROR 404";
        }

    }
}