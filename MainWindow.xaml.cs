using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RecipeManager
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private Recipe.Type m_actualTypeOfDinner = new Recipe.Type();

        private List<Recipe> m_recipes = new List<Recipe>();
        private List<Recipe> m_actualRecipes = new List<Recipe>();

        ManagerRecipe manager = new ManagerRecipe();

        //members
        private int m_totalRecipeQuantity = -1;
        private int m_actualRecipeQuantity = -1;

        private int m_actualIndex = 0;

        //functions
        int GetNextIndex(int i_index)
        {
            if (i_index == m_actualRecipes.Count - 1)
                return 0;
            else return i_index + 1;
        }

        int GetPreviousIndex(int i_index)
        {
            if (i_index == 0)
                return m_actualRecipes.Count - 1;
            else return i_index - 1;
        }

        private void ShowRecipe(int i_index)
        {
            string contentRecipe ="";

            int i = 0;
            foreach(var step in m_actualRecipes[i_index].Steps)
            {
                i++;
                contentRecipe = contentRecipe + "<b>Krok "+i.ToString()+":</b><br>"+step+"<br><br>";
            }
            contentRecipe = contentRecipe + "<br><i>Polecane na "+m_actualRecipes[i_index].getDinnerType()+".</i>";

            RichBrowserRecipe.Document.Blocks.Clear();


            FlowDocument document = new FlowDocument();

            //title
            Paragraph p = new Paragraph(new Run(m_actualRecipes[i_index].Title));
            p.FontSize = 26;
            document.Blocks.Add(p);

            //time
            p = new Paragraph();
            p.FontSize = 13;
            p.Inlines.Add(new Bold(new Run("Czas Przygotowania: ")));
            p.Inlines.Add(new Run(m_actualRecipes[i_index].Time));
            document.Blocks.Add(p);

            //ingredients title
            p = new Paragraph();
            p.FontSize = 18;
            p.Inlines.Add(new Bold(new Run("Składniki: ")));
            document.Blocks.Add(p);

            //ingredients
            p = new Paragraph();
            p.FontSize = 15;

            for (int ind = 0; ind < m_actualRecipes[i_index].Ingredients.Count(); ind++)
            {
                p.Inlines.Add(new Bold(new Run("  - ")));
                p.Inlines.Add(new Run(m_actualRecipes[i_index].Ingredients.ElementAt(ind)));
                p.Inlines.Add(new Run(", "));
                p.Inlines.Add(new Run(m_actualRecipes[i_index].Quantity.ElementAt(ind)));
                p.Inlines.Add(new Run("\n"));
            }

            document.Blocks.Add(p);

            //steps title
            p = new Paragraph();
            p.FontSize = 18;
            p.Inlines.Add(new Bold(new Run("Sposób przygotowania:\n")));
            document.Blocks.Add(p);

            //steps
            p = new Paragraph();
            p.FontSize = 15;

            int ii = 0;
            foreach (var step in m_actualRecipes[i_index].Steps)
            {
                ii++;

                p.Inlines.Add(new Bold(new Run("Krok ")));
                p.Inlines.Add(new Run(ii.ToString()));
                p.Inlines.Add(new Run(":\n"));
                p.Inlines.Add(new Run(step));
                p.Inlines.Add(new Run("\n\n"));
            }

            document.Blocks.Add(p);

            //proposed for
            p = new Paragraph();
            p.FontSize = 16;
            p.Inlines.Add(new Italic(new Run("Polecane na " + m_actualRecipes[i_index].getDinnerType() + ".\n\n")));
            document.Blocks.Add(p);



            RichBrowserRecipe.Document = document;




            LabelId.Content = "Przepis numer " + m_actualRecipes[i_index].Id.ToString();

        }

        public void ShowOnlyRecipesContaing(List<string> i_ingredients)
        {

            m_actualRecipes = new List<Recipe>();
            foreach (var recipe in m_recipes)
            {
                foreach (var ing in recipe.Ingredients)
                {
                    if (i_ingredients.Contains(ing))
                        m_actualRecipes.Add(recipe);
                }
            }

            m_actualRecipeQuantity = m_actualRecipes.Count();
            ShowRecipe(0);
            LabelShownRecipes.Content = "Przepisów z podanymi składnikami jest łącznie " + m_actualRecipeQuantity.ToString();
        }

        //ctor
        public MainWindow()
        {
            InitializeComponent();

            m_recipes = manager.Recipes;
            m_actualRecipes = m_recipes;

            m_totalRecipeQuantity = manager.GetAmountOfRecipes();
            m_actualRecipeQuantity = m_totalRecipeQuantity;

            ShowRecipe(0);
            m_actualIndex = 0;

            LabelShownRecipes.Content = "Wyświetlono wszystkie " + m_totalRecipeQuantity.ToString() + " przepisów!";

            int actualTime = DateTime.Now.Hour;

            if (actualTime > 5 && actualTime < 12)
                m_actualTypeOfDinner = Recipe.Type.Sniadanie;
            else if (actualTime > 12 && actualTime < 17)
                m_actualTypeOfDinner = Recipe.Type.Obiad;
            else
                m_actualTypeOfDinner = Recipe.Type.Kolacja;


            string dinnerType="";
            if (m_actualTypeOfDinner == Recipe.Type.Sniadanie)
                dinnerType = "śniadanie";
            else if (m_actualTypeOfDinner == Recipe.Type.Obiad)
                dinnerType = "obiad";
            else if (m_actualTypeOfDinner == Recipe.Type.Kolacja)
                dinnerType = "kolację";

            RandomDinnerButton.Content = "Zaproponuj " + dinnerType;


        }


        //buttons events

        private void ShowAllButton_Click(object sender, RoutedEventArgs e)
        {
            m_actualRecipes = m_recipes;
            m_actualRecipeQuantity = m_totalRecipeQuantity;
            LabelShownRecipes.Content = "Wyświetlono wszystkie " + m_totalRecipeQuantity.ToString() + " przepisy!";

            m_actualIndex = 0;
            ShowRecipe(0);
        }

        private void IngredientsButton_Click(object sender, RoutedEventArgs e)
        {
            chooseIngredientsDialog dialog = new chooseIngredientsDialog() { Owner = this };
            if(dialog.ShowDialog() == true)
            {
                ShowOnlyRecipesContaing(dialog.Ingredients);
            }
        }

        private void RandomDinnerButton_Click(object sender, RoutedEventArgs e)
        {
            List<Recipe> temp_typeRecipes = new List<Recipe>();

            foreach (var recipe in m_actualRecipes)
            {
                if (recipe.TypeOfDinner == m_actualTypeOfDinner)
                    temp_typeRecipes.Add(recipe);
            }

            m_actualRecipes = temp_typeRecipes;
            m_actualRecipeQuantity = m_actualRecipes.Count();

            string dinnerType="";
            if (m_actualTypeOfDinner == Recipe.Type.Sniadanie)
                dinnerType = "sniadania";
            else if (m_actualTypeOfDinner == Recipe.Type.Obiad)
                dinnerType = "obiady";
            else if (m_actualTypeOfDinner == Recipe.Type.Kolacja)
                dinnerType = "kolacje";

            LabelShownRecipes.Content = "Wyswietlam " + m_actualRecipeQuantity.ToString() + " przepisy na " + dinnerType;

            Random rnd = new Random();
            int random_number = rnd.Next(0, m_actualRecipeQuantity - 1);

            ShowRecipe(random_number);
        }

        private void LeftArrowButton_Click(object sender, RoutedEventArgs e)
        {
            int index = GetPreviousIndex(m_actualIndex);
            m_actualIndex = index;
            ShowRecipe(index);
        }

        private void RandomRecipeButton_Click(object sender, RoutedEventArgs e)
        {
           Random rnd = new Random();
           int random_number = rnd.Next(0, m_actualRecipeQuantity - 1);

            ShowRecipe(random_number);
        }

        private void RightArrowButton_Click(object sender, RoutedEventArgs e)
        {
            int index = GetNextIndex(m_actualIndex);
            m_actualIndex = index;
            ShowRecipe(index);
        }


    }
}
