using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RecipeManager
{
    class ManagerRecipe
    {

        //members
        private List<Recipe> m_recipes = new List<Recipe>();

        public List<Recipe> Recipes
        {
            get { return m_recipes; }
            set { m_recipes = value; }
        }


        //ctor
        public ManagerRecipe()
            {
                LoadRecipesFromDatabase();
            }

            //functions
            public List<string> GetAllIngredients()
            {
                List<string> temp_ingredients = new List<string>();

                foreach (var rec in m_recipes)
                {
                    foreach (var ing in rec.Ingredients)
                    {
                        if (!temp_ingredients.Contains(ing))
                            temp_ingredients.Add(ing);
                    }

                }

                return temp_ingredients;
            }

            public int GetAmountOfRecipes()
            {
                return m_recipes.Count;
            }

            private void LoadRecipesFromDatabase()
            {
            string content;

            using (StreamReader sr = new StreamReader("database.txt"))
            {
                // Read the stream to a string, and write the string to the console.
                content = sr.ReadToEnd();
            }




            List<string> queries = content.Split(';').ToList();


            List<List<string>> raw_recipes = new List<List<string>>();



            string a;
            int ii = 0;


            List<string> temp_query = new List<string>();

            foreach (string query in queries)
            {

                ii++;
                temp_query.Add(query);

                if (ii == 1)
                    a = query;

                if ((query == "sniadanie") ||
                            (query == "obiad") ||
                            (query == "kolacja"))
                {
                    raw_recipes.Add(temp_query);
                    //temp_query.Clear();
                    temp_query = new List<string>();
                }


            }

            List<string> reca = new List<string>();
            reca = raw_recipes[0];

            string ind = raw_recipes[0][0];
            

            foreach (List<string> rec in raw_recipes)
            {
                Recipe temp_recipe = new Recipe();

                temp_recipe.Id = Int32.Parse(rec[0]);

                temp_recipe.Title = rec.ElementAt(1);

                int i = 2;

                List<string> temp_ingredients = new List<string>();
                List<string> temp_quantity = new List<string>();
                while (rec[i] != "koniec skladnikow")
                {
                    temp_ingredients.Add(rec[i]);
                    i++;
                    temp_quantity.Add(rec[i]);
                    i++;
                }

                i++; //pomijamy "koniec skladnikow"

                temp_recipe.Ingredients = temp_ingredients;
                temp_recipe.Quantity = temp_quantity;

                List<string> temp_steps = new List<string>();
                while (rec[i] != "koniec krokow")
                {
                    temp_steps.Add(rec[i]);
                    i++;
                }
                i++;//pomijamy "koniec krokow"

                temp_recipe.Steps = temp_steps;

                temp_recipe.Time = rec[i];
                i++;

                if (rec[i] == "sniadanie")
                    temp_recipe.TypeOfDinner = Recipe.Type.Sniadanie;
                else if (rec[i] == "obiad")
                    temp_recipe.TypeOfDinner = Recipe.Type.Obiad;
                else if (rec[i] == "kolacja")
                    temp_recipe.TypeOfDinner = Recipe.Type.Kolacja;


                m_recipes.Add(temp_recipe);
            }
        }


        }
    



}
