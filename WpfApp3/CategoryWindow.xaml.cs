using System;
using System.IO;
using System.Windows;

namespace WpfApp3
{
    public partial class CategoryWindow : Window
    {
        public CategoryWindow()
        {
            InitializeComponent();
        }

        private void AddCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            string categoryName = categoryTextBox.Text.Trim();

            if (!string.IsNullOrEmpty(categoryName))
            {
                using (StreamWriter sw = new StreamWriter($@"..\..\..\src\categories.txt", true))
                {
                    sw.WriteLine(categoryName);
                }


                MessageBox.Show("Kategória hozzáadva!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);

                this.DialogResult = true;
                this.Close(); 
            }
            else
            {
                MessageBox.Show("A kategória neve nem lehet üres!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }
    }
}
