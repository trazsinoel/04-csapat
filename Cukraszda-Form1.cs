using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace Cukraszda
{
    // ===================== FŐ FORM =====================
    public partial class Form1 : Form
    {
        private readonly List<Sutemeny> sutemenyek = new List<Sutemeny>();
        private ArlistaForm arlistaForm;

        public Form1()
        {
            InitializeComponent();
        }

        // Fájl / Megnyitás: a választék beolvasása
        private void megnyitasToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            try
            {
                List<Sutemeny> beolvasott = new List<Sutemeny>();
                foreach (string sor in File.ReadAllLines(openFileDialog1.FileName))
                {
                    if (string.IsNullOrWhiteSpace(sor)) continue;
                    string[] r = sor.Split(';');
                    beolvasott.Add(new Sutemeny(r[0].Trim(), int.Parse(r[1].Trim())));
                }

                if (beolvasott.Count == 0)
                {
                    MessageBox.Show("A fájl nem tartalmaz süteményt!", "Hiba",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                sutemenyek.Clear();
                sutemenyek.AddRange(beolvasott);

                if (arlistaForm != null && !arlistaForm.IsDisposed)
                    arlistaForm.Close();
                arlistaForm = null;

                sutemenyekToolStripMenuItem.Enabled = true;   // csak betöltés után elérhető
                ArlistaMegjelenites();                        // azonnal megjelenik az árlista
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hiba a fájl beolvasásakor:\n" + ex.Message, "Hiba",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void sutemenyekToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ArlistaMegjelenites();
        }

        private void ArlistaMegjelenites()
        {
            if (arlistaForm == null || arlistaForm.IsDisposed)
            {
                arlistaForm = new ArlistaForm(sutemenyek);
                arlistaForm.Show(this);
            }
            else
            {
                arlistaForm.WindowState = FormWindowState.Normal;
                arlistaForm.Activate();
            }
        }

        private void kilepesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void nevjegyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show(
                "Cukrászda – rendelő alkalmazás (Windows Forms)\n\n" +
                "Készítette: 04 csapat\n" +
                "Trázsi Noel, Juhász-Sala Párisz",
                "Névjegy", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }

    // ===================== ÁRLISTA FORM =====================
    public class ArlistaForm : Form
    {
        private readonly List<Sutemeny> sutemenyek;
        private readonly List<CheckBox> checkBoxok = new List<CheckBox>();
        private readonly List<TextBox> adagok = new List<TextBox>();
        private readonly List<RendelesTetel> rendelesek = new List<RendelesTetel>();

        public ArlistaForm(List<Sutemeny> sutemenyek)
        {
            this.sutemenyek = new List<Sutemeny>(sutemenyek);

            Text = "Árlista";
            ClientSize = new Size(424, 100 + sutemenyek.Count * 30);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            StartPosition = FormStartPosition.CenterParent;

            Controls.Add(new Label
            {
                Text = "ÁRLISTA",
                Font = new Font("Algerian", 20F, FontStyle.Bold | FontStyle.Italic),
                ForeColor = Color.DarkOrange,
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(12, 9),
                Size = new Size(400, 40)
            });

            int y = 60;
            foreach (Sutemeny s in this.sutemenyek)
            {
                CheckBox cb = new CheckBox { Text = $"{s.Nev}({s.Ar} Ft)", Location = new Point(30, y), AutoSize = true };
                TextBox tb = new TextBox { Location = new Point(310, y - 2), Width = 40, Enabled = false };
                Label lbl = new Label { Text = "adag", Location = new Point(355, y + 1), AutoSize = true };

                cb.CheckedChanged += (o, e) =>
                {
                    tb.Enabled = cb.Checked;
                    tb.Text = cb.Checked ? "1" : "";
                };

                Controls.AddRange(new Control[] { cb, tb, lbl });
                checkBoxok.Add(cb);
                adagok.Add(tb);
                y += 30;
            }

            // Jobb egérgomb: context menü "Számlát kér" menüponttal
            ContextMenuStrip menu = new ContextMenuStrip();
            menu.Items.Add("Számlát kér", null, SzamlatKer_Click);

            Button rendel = new Button
            {
                Text = "RENDEL",
                Size = new Size(100, 28),
                Location = new Point(162, y + 10),
                ContextMenuStrip = menu
            };
            rendel.Click += Rendel_Click;   // bal egérgomb
            Controls.Add(rendel);
        }

        // Bal egérgomb: rendelés leadása
        private void Rendel_Click(object sender, EventArgs e)
        {
            if (!checkBoxok.Any(cb => cb.Checked))
            {
                MessageBox.Show("Nincs kijelölve egyetlen sütemény sem!", "Hiba",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            List<RendelesTetel> uj = new List<RendelesTetel>();
            for (int i = 0; i < checkBoxok.Count; i++)
            {
                if (!checkBoxok[i].Checked) continue;
                if (!int.TryParse(adagok[i].Text, out int adag) || adag <= 0)
                {
                    MessageBox.Show($"Hibás adagszám: {sutemenyek[i].Nev}", "Hiba",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
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

            MessageBox.Show("A rendelést felvettük:\n\n" +
                string.Join("\n", uj.Select(t => $"{t.Sutemeny.Nev}: {t.Adag} adag")) +
                $"\n\nÉrték: {uj.Sum(t => t.Osszeg)} Ft",
                "Rendelés", MessageBoxButtons.OK, MessageBoxIcon.Information);

            foreach (CheckBox cb in checkBoxok) cb.Checked = false;
        }

        // Számlát kér: a számla egy richTextBox-ban jelenik meg (külön formon)
        private void SzamlatKer_Click(object sender, EventArgs e)
        {
            if (rendelesek.Count == 0)
            {
                MessageBox.Show("Még nem adott le rendelést!", "Hiba",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Form szamla = new Form
            {
                Text = "Számla",
                ClientSize = new Size(484, 330),
                StartPosition = FormStartPosition.CenterParent,
                FormBorderStyle = FormBorderStyle.FixedDialog,
                MaximizeBox = false,
                MinimizeBox = false
            };
            RichTextBox rtb = new RichTextBox
            {
                Dock = DockStyle.Fill,
                ReadOnly = true,
                BackColor = Color.White,
                Font = new Font("Consolas", 10F)
            };
            szamla.Controls.Add(rtb);

            string vonal = new string('=', 52);
            rtb.SelectionFont = new Font("Consolas", 14F, FontStyle.Bold);
            rtb.SelectionColor = Color.Chocolate;
            rtb.AppendText("CUKRÁSZDA – SZÁMLA\n");
            rtb.SelectionFont = rtb.Font;
            rtb.SelectionColor = Color.Black;
            rtb.AppendText($"Kelt: {DateTime.Now:yyyy.MM.dd. HH:mm}\n{vonal}\n");
            rtb.AppendText($"{"Megnevezés",-22}{"Adag",6}{"Egységár",12}{"Összeg",12}\n");
            rtb.AppendText(new string('-', 52) + "\n");
            foreach (RendelesTetel t in rendelesek)
                rtb.AppendText($"{t.Sutemeny.Nev,-22}{t.Adag,6}{t.Sutemeny.Ar + " Ft",12}{t.Osszeg + " Ft",12}\n");
            rtb.AppendText(vonal + "\n");
            rtb.SelectionFont = new Font("Consolas", 10F, FontStyle.Bold);
            rtb.SelectionColor = Color.DarkRed;
            rtb.AppendText($"{"FIZETENDŐ:",-28}{rendelesek.Sum(t => t.Osszeg) + " Ft",24}\n");

            szamla.ShowDialog(this);
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
