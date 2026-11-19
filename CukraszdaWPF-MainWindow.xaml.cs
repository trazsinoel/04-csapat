using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.Win32;

namespace CukraszdaWPF
{
    // ===================== FŐABLAK =====================
    public partial class MainWindow : Window
    {
        private readonly List<Sutemeny> sutemenyek = new List<Sutemeny>();
        private ArlistaWindow arlistaWindow;

        public MainWindow()
        {
            InitializeComponent();
        }

        // Fájl / Megnyitás: a választék beolvasása
        private void Megnyitas_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog
            {
                Filter = "Szöveges fájlok (*.txt)|*.txt|Minden fájl (*.*)|*.*"
            };
            if (dlg.ShowDialog(this) != true)
                return;

            try
            {
                List<Sutemeny> beolvasott = new List<Sutemeny>();
                foreach (string sor in File.ReadAllLines(dlg.FileName))
                {
                    if (string.IsNullOrWhiteSpace(sor)) continue;
                    string[] r = sor.Split(';');
                    beolvasott.Add(new Sutemeny(r[0].Trim(), int.Parse(r[1].Trim())));
                }

                if (beolvasott.Count == 0)
                {
                    MessageBox.Show(this, "A fájl nem tartalmaz süteményt!", "Hiba",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                sutemenyek.Clear();
                sutemenyek.AddRange(beolvasott);

                arlistaWindow?.Close();
                arlistaWindow = null;

                SutemenyekMenuItem.IsEnabled = true;   // csak betöltés után elérhető
                InfoTextBlock.Text = $"Betöltve: {sutemenyek.Count} féle sütemény";
                ArlistaMegjelenites();                 // azonnal megjelenik az árlista
            }
            catch (Exception ex)
            {
                MessageBox.Show(this, "Hiba a fájl beolvasásakor:\n" + ex.Message, "Hiba",
                    MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Sutemenyek_Click(object sender, RoutedEventArgs e)
        {
            ArlistaMegjelenites();
        }

        private void ArlistaMegjelenites()
        {
            if (arlistaWindow == null)
            {
                arlistaWindow = new ArlistaWindow(sutemenyek) { Owner = this };
                arlistaWindow.Closed += (s, args) => arlistaWindow = null;
                arlistaWindow.Show();
            }
            else
            {
                arlistaWindow.WindowState = WindowState.Normal;
                arlistaWindow.Activate();
            }
        }

        private void Kilepes_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Nevjegy_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(this,
                "Cukrászda – rendelő alkalmazás (WPF)\n\n" +
                "Készítette: 04 csapat\n" +
                "Trázsi Noel, Juhász-Sala Párisz",
                "Névjegy", MessageBoxButton.OK, MessageBoxImage.Information);
        }
    }

    // ===================== ÁRLISTA ABLAK =====================
    public class ArlistaWindow : Window
    {
        private readonly List<Sutemeny> sutemenyek;
        private readonly List<CheckBox> checkBoxok = new List<CheckBox>();
        private readonly List<TextBox> adagok = new List<TextBox>();
        private readonly List<RendelesTetel> rendelesek = new List<RendelesTetel>();

        public ArlistaWindow(List<Sutemeny> sutemenyek)
        {
            this.sutemenyek = new List<Sutemeny>(sutemenyek);

            Title = "Árlista";
            Width = 440;
            SizeToContent = SizeToContent.Height;
            ResizeMode = ResizeMode.CanMinimize;
            WindowStartupLocation = WindowStartupLocation.CenterOwner;

            StackPanel panel = new StackPanel { Margin = new Thickness(15) };
            panel.Children.Add(new TextBlock
            {
                Text = "ÁRLISTA",
                FontFamily = new FontFamily("Algerian"),
                FontSize = 28,
                FontWeight = FontWeights.Bold,
                FontStyle = FontStyles.Italic,
                Foreground = Brushes.DarkOrange,
                HorizontalAlignment = HorizontalAlignment.Center,
                Margin = new Thickness(0, 0, 0, 10)
            });

            foreach (Sutemeny s in this.sutemenyek)
            {
                CheckBox cb = new CheckBox { Content = $"{s.Nev}({s.Ar} Ft)", Width = 280, VerticalAlignment = VerticalAlignment.Center };
                TextBox tb = new TextBox { Width = 40, IsEnabled = false };
                TextBlock lbl = new TextBlock { Text = "adag", Margin = new Thickness(5, 0, 0, 0), VerticalAlignment = VerticalAlignment.Center };

                cb.Checked += (o, e) => { tb.IsEnabled = true; tb.Text = "1"; };
                cb.Unchecked += (o, e) => { tb.IsEnabled = false; tb.Text = ""; };

                StackPanel sor = new StackPanel { Orientation = Orientation.Horizontal, Margin = new Thickness(0, 4, 0, 4) };
                sor.Children.Add(cb);
                sor.Children.Add(tb);
                sor.Children.Add(lbl);
                panel.Children.Add(sor);

                checkBoxok.Add(cb);
                adagok.Add(tb);
            }

            // Jobb egérgomb: context menü "Számlát kér" menüponttal
            MenuItem szamlatKer = new MenuItem { Header = "Számlát kér" };
            szamlatKer.Click += SzamlatKer_Click;
            ContextMenu menu = new ContextMenu();
            menu.Items.Add(szamlatKer);

            Button rendel = new Button
            {
                Content = "RENDEL",
                Width = 100,
                Height = 28,
                Margin = new Thickness(0, 15, 0, 0),
                ContextMenu = menu
            };
            rendel.Click += Rendel_Click;   // bal egérgomb
            panel.Children.Add(rendel);

            Content = panel;
        }

        // Bal egérgomb: rendelés leadása
        private void Rendel_Click(object sender, RoutedEventArgs e)
        {
            if (!checkBoxok.Any(cb => cb.IsChecked == true))
            {
                MessageBox.Show(this, "Nincs kijelölve egyetlen sütemény sem!", "Hiba",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<RendelesTetel> uj = new List<RendelesTetel>();
            for (int i = 0; i < checkBoxok.Count; i++)
            {
                if (checkBoxok[i].IsChecked != true) continue;
                if (!int.TryParse(adagok[i].Text, out int adag) || adag <= 0)
                {
                    MessageBox.Show(this, $"Hibás adagszám: {sutemenyek[i].Nev}", "Hiba",
                        MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                uj.Add(new RendelesTetel(sutemenyek[i], adag));
            }

            foreach (RendelesTetel t in uj)
            {
                RendelesTetel meglevo = rendelesek.FirstOrDefault(r => r.Sutemeny == t.Sutemeny);
                if (meglevo != null) meglevo.Adag += t.Adag;
                else rendelesek.Add(t);
            }

            MessageBox.Show(this, "A rendelést felvettük:\n\n" +
                string.Join("\n", uj.Select(t => $"{t.Sutemeny.Nev}: {t.Adag} adag")) +
                $"\n\nÉrték: {uj.Sum(t => t.Osszeg)} Ft",
                "Rendelés", MessageBoxButton.OK, MessageBoxImage.Information);

            foreach (CheckBox cb in checkBoxok) cb.IsChecked = false;
        }

        // Számlát kér: a számla egy RichTextBox-ban jelenik meg (külön ablakban)
        private void SzamlatKer_Click(object sender, RoutedEventArgs e)
        {
            if (rendelesek.Count == 0)
            {
                MessageBox.Show(this, "Még nem adott le rendelést!", "Hiba",
                    MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            string vonal = new string('=', 52);
            string tablazat = vonal + "\n" +
                $"{"Megnevezés",-22}{"Adag",6}{"Egységár",12}{"Összeg",12}\n" +
                new string('-', 52) + "\n" +
                string.Join("\n", rendelesek.Select(t =>
                    $"{t.Sutemeny.Nev,-22}{t.Adag,6}{t.Sutemeny.Ar + " Ft",12}{t.Osszeg + " Ft",12}")) +
                "\n" + vonal;

            FlowDocument doc = new FlowDocument();
            doc.Blocks.Add(new Paragraph(new Run("CUKRÁSZDA – SZÁMLA")) { FontSize = 18, FontWeight = FontWeights.Bold, Foreground = Brushes.Chocolate });
            doc.Blocks.Add(new Paragraph(new Run($"Kelt: {DateTime.Now:yyyy.MM.dd. HH:mm}")));
            doc.Blocks.Add(new Paragraph(new Run(tablazat)));
            doc.Blocks.Add(new Paragraph(new Run($"{"FIZETENDŐ:",-28}{rendelesek.Sum(t => t.Osszeg) + " Ft",24}"))
                { FontWeight = FontWeights.Bold, Foreground = Brushes.DarkRed });

            Window szamla = new Window
            {
                Title = "Számla",
                Width = 520,
                Height = 380,
                Owner = this,
                ResizeMode = ResizeMode.NoResize,
                WindowStartupLocation = WindowStartupLocation.CenterOwner,
                Content = new RichTextBox
                {
                    Document = doc,
                    IsReadOnly = true,
                    FontFamily = new FontFamily("Consolas"),
                    FontSize = 13,
                    Margin = new Thickness(10)
                }
            };
            szamla.ShowDialog();
            rendelesek.Clear();
        }
    }

    // ===================== ADAT OSZTÁLYOK =====================
    public class Sutemeny
    {
        public string Nev { get; }
        public int Ar { get; }
        public Sutemeny(string nev, int ar) { Nev = nev; Ar = ar; }
    }

    public class RendelesTetel
    {
        public Sutemeny Sutemeny { get; }
        public int Adag { get; set; }
        public int Osszeg => Sutemeny.Ar * Adag;
        public RendelesTetel(Sutemeny sutemeny, int adag) { Sutemeny = sutemeny; Adag = adag; }
    }
}
