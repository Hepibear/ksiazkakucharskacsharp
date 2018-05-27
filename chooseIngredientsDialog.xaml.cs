using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RecipeManager
{
    /// <summary>
    /// Interaction logic for chooseIngredientsDialog.xaml
    /// </summary>
    public partial class chooseIngredientsDialog : Window
    {
        //The collection that the ListBox binds to
        public ObservableCollection<BoolStringClass> TheList { get; set; }

        private List<string> ingredients;

        public List<string> Ingredients
        {
            get { return ingredients; }
            set { ingredients = value; }
        }


        public chooseIngredientsDialog()
        {
            InitializeComponent();

            ManagerRecipe manager = new ManagerRecipe();

            List<string> allIngredients = manager.GetAllIngredients();


            TheList = new ObservableCollection<BoolStringClass>();

            foreach(var ingredient in allIngredients)
            {
                TheList.Add(new BoolStringClass { IsSelected = false, TheText = ingredient });
            }

            this.DataContext = this;
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //Get a List<BoolStringClass> that contains all selected items:
            ingredients = (
                    from item in TheList
                    where item.IsSelected == true
                    select item.TheText
                ).ToList<string>();

            this.DialogResult = true;
            this.Close();

        }

    }

    public class BoolStringClass : INotifyPropertyChanged
    {
        public string TheText { get; set; }

        //Provide change-notification for IsSelected
        private bool _fIsSelected = false;
        public bool IsSelected
        {
            get { return _fIsSelected; }
            set
            {
                _fIsSelected = value;
                this.OnPropertyChanged("IsSelected");
            }
        }

        
        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string strPropertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(strPropertyName));
        }

        #endregion
        
    }




}



