using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;

namespace WpfApp3
{
    public partial class MainWindow : Window
    {
        private List<Filmek> filmek;
        private List<string> categories; // Kategóriák listája

        public MainWindow()
        {
            InitializeComponent();
            filmek = new List<Filmek>();
            categories = new List<string>();

            LoadCategories();

            using StreamReader sr = new($@"..\..\..\src\oscar.csv", encoding: System.Text.Encoding.UTF8);
            sr.ReadLine();
            while (!sr.EndOfStream)
            {
                filmek.Add(new Filmek(sr.ReadLine()));
            }

            var feladat2 = filmek.OrderByDescending(f => f.Ev).ToList();
            foreach (var f in feladat2)
            {
                var displayText = $"{f.Ev} - {f.Cim}";
                oscar.Items.Add(displayText);
            }

            categoryComboBox.ItemsSource = categories; 
        }

        private void LoadCategories()
        {
            if (File.Exists($@"..\..\..\src\categories.txt"))
            {
                categories = File.ReadAllLines($@"..\..\..\src\categories.txt").ToList();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string cimText = cim.Text.Trim();
            string evText = ev1.Text.Trim();
            string dijText = dij1.Text.Trim();
            string jelolText = jelol1.Text.Trim();
            string selectedCategory = categoryComboBox.SelectedItem?.ToString(); 

            if (string.IsNullOrEmpty(cimText) || string.IsNullOrEmpty(evText) || string.IsNullOrEmpty(dijText) || string.IsNullOrEmpty(jelolText))
            {
                MessageBox.Show("Minden mezőt ki kell tölteni, hogy új filmet adhass hozzá.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            else
            {
                if (cimText.Length < 4)
                {
                    MessageBox.Show("A címnek minimum 4 karakter hosszúnak kell lennie!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(evText, out _) || !int.TryParse(dijText, out _) || !int.TryParse(jelolText, out _))
                {
                    MessageBox.Show("Az év, díj és jelölés mezőknek számokat kell tartalmazniuk!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                MessageBox.Show("Elküldve!");
            }

            string azonosito = Azonosit(cimText, evText);

            var ujFilm = new Filmek($"{azonosito}\t{cimText}\t{evText}\t{dijText}\t{jelolText}\t{selectedCategory}"); 

            filmek.Add(ujFilm);

            var displayText = $"{evText} - {cimText}";
            oscar.Items.Add(displayText);

            cim.Clear();
            ev1.Clear();
            dij1.Clear();
            jelol1.Clear();
            categoryComboBox.SelectedIndex = -1; 

            using (StreamWriter sw = new StreamWriter($@"..\..\..\src\oscar.csv", true, encoding: System.Text.Encoding.UTF8))
            {
                sw.WriteLine($"{ujFilm.Azon}\t{ujFilm.Cim}\t{ujFilm.Ev}\t{ujFilm.JelolesSzam}\t{ujFilm.DijSzam}\t{ujFilm.Kategoria}"); 
            }
        }

        private string Azonosit(string cim, string ev)
        {
            string szoveg = cim.Substring(cim.Length - 4);
            string szam = ev.Length >= 4 ? ev.Substring(1, 2) : "00";
            return szoveg + szam;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (filmek.Count > 0)
            {
                var legtobbDij = filmek.OrderByDescending(f => f.DijSzam).FirstOrDefault();

                filmcim.Content = new TextBlock
                {
                    Inlines = {
                        new Run("Film címe: ") {Foreground = new SolidColorBrush(Colors.Gray)},
                        new Run(legtobbDij.Cim) {Foreground = new SolidColorBrush(Colors.Black)}
                    }
                };
            }
            else
            {
                filmcim.Content = "Üres a lista";
            }
        }

        private void KeresesButton_Click(object sender, RoutedEventArgs e)
        {
            string keresettCim = keresettfilm.Text.Trim().ToLower();
            var talalatok = filmek.Where(f => f.Cim.ToLower().Contains(keresettCim)).ToList();

            if (talalatok.Count > 0)
            {
                var random = new Random();
                var randomFilm = talalatok[random.Next(talalatok.Count)];

                talalatkiiras.Text = $" {randomFilm.Cim} ({randomFilm.Ev})";
                talalatkiiras.Foreground = new SolidColorBrush(Colors.Black);
            }
            else
            {
                talalatkiiras.Text = "Sajnos nincs ilyen film a listában.";
                talalatkiiras.Foreground = new SolidColorBrush(Colors.Black);
            }
            talal.Foreground = new SolidColorBrush(Colors.Gray);
        }

        private void KeresettFilm_GotFocus(object sender, RoutedEventArgs e)
        {
            if (keresettfilm.Text == "Keresett film")
            {
                keresettfilm.Text = "";
                keresettfilm.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        private void KeresettFilm_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(keresettfilm.Text))
            {
                keresettfilm.Text = "Keresett film";
                keresettfilm.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void listazas_Click(object sender, RoutedEventArgs e)
        {
            eredmeny.Items.Clear();

            string keresettSzo = keresettszo.Text.Trim().ToLower();
            var talalatok = filmek.Where(f => f.Cim.ToLower().Contains(keresettSzo)).ToList();

            foreach (var film in talalatok)
            {
                eredmeny.Items.Add($"{film.Azon} - {film.Cim}");
            }
        }

        private void KeresettSzo_GotFocus(object sender, RoutedEventArgs e)
        {
            if (keresettszo.Text == "Keresett szo")
            {
                keresettszo.Text = "";
            }
        }

        private void KeresettSzo_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(keresettszo.Text))
            {
                keresettszo.Text = "Keresett szo";
                keresettszo.Foreground = new SolidColorBrush(Colors.Gray);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (oscar.SelectedItem != null)
            {
                string selectedText = oscar.SelectedItem.ToString();

                var filmToDelete = filmek.FirstOrDefault(f => $"{f.Ev} - {f.Cim}" == selectedText);

                if (filmToDelete != null)
                {
                    filmek.Remove(filmToDelete);

                    oscar.Items.Remove(selectedText);

                    using (StreamWriter sw = new StreamWriter($@"..\..\..\src\oscar.csv", false, System.Text.Encoding.UTF8))
                    {
                        sw.WriteLine("Azon\tCim\tEv\tJelolesSzam\tDijSzam\tKategoria");

                        foreach (var film in filmek)
                        {
                            sw.WriteLine($"{film.Azon}\t{film.Cim}\t{film.Ev}\t{film.JelolesSzam}\t{film.DijSzam}\t{film.Kategoria}");
                        }
                    }

                    MessageBox.Show("Film sikeresen törölve!", "Törlés", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Kérjük, válasszon egy filmet a törléshez.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void OpenCategoryWindowButton_Click(object sender, RoutedEventArgs e)
        {
            CategoryWindow categoryWindow = new CategoryWindow();

            if (categoryWindow.ShowDialog() == true)
            {
                LoadCategories();

                categoryComboBox.ItemsSource = categories;
            }
        }


    }
}
