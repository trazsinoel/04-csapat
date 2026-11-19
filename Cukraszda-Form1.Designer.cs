namespace Cukraszda
{
    partial class Form1
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fajlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.megnyitasToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.kilepesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sutemenyekToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.nevjegyToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.labelCim = new System.Windows.Forms.Label();
            this.labelInfo = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            //
            // menuStrip1
            //
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fajlToolStripMenuItem,
            this.sutemenyekToolStripMenuItem,
            this.nevjegyToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(384, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            //
            // fajlToolStripMenuItem
            //
            this.fajlToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.megnyitasToolStripMenuItem,
            this.toolStripSeparator1,
            this.kilepesToolStripMenuItem});
            this.fajlToolStripMenuItem.Name = "fajlToolStripMenuItem";
            this.fajlToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fajlToolStripMenuItem.Text = "Fájl";
            //
            // megnyitasToolStripMenuItem
            //
            this.megnyitasToolStripMenuItem.Name = "megnyitasToolStripMenuItem";
            this.megnyitasToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.megnyitasToolStripMenuItem.Text = "Megnyitás...";
            this.megnyitasToolStripMenuItem.Click += new System.EventHandler(this.megnyitasToolStripMenuItem_Click);
            //
            // toolStripSeparator1
            //
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            //
            // kilepesToolStripMenuItem
            //
            this.kilepesToolStripMenuItem.Name = "kilepesToolStripMenuItem";
            this.kilepesToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            this.kilepesToolStripMenuItem.Text = "Kilépés";
            this.kilepesToolStripMenuItem.Click += new System.EventHandler(this.kilepesToolStripMenuItem_Click);
            //
            // sutemenyekToolStripMenuItem
            //
            this.sutemenyekToolStripMenuItem.Enabled = false;
            this.sutemenyekToolStripMenuItem.Name = "sutemenyekToolStripMenuItem";
            this.sutemenyekToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.sutemenyekToolStripMenuItem.Text = "sutemenyek";
            this.sutemenyekToolStripMenuItem.Click += new System.EventHandler(this.sutemenyekToolStripMenuItem_Click);
            //
            // nevjegyToolStripMenuItem
            //
            this.nevjegyToolStripMenuItem.Name = "nevjegyToolStripMenuItem";
            this.nevjegyToolStripMenuItem.Size = new System.Drawing.Size(60, 20);
            this.nevjegyToolStripMenuItem.Text = "Névjegy";
            this.nevjegyToolStripMenuItem.Click += new System.EventHandler(this.nevjegyToolStripMenuItem_Click);
            //
            // openFileDialog1
            //
            this.openFileDialog1.Filter = "Szöveges fájlok (*.txt)|*.txt|Minden fájl (*.*)|*.*";
            this.openFileDialog1.Title = "Választék megnyitása";
            //
            // labelCim
            //
            this.labelCim.Dock = System.Windows.Forms.DockStyle.Fill;
            this.labelCim.Font = new System.Drawing.Font("Segoe Script", 26F, System.Drawing.FontStyle.Bold);
            this.labelCim.ForeColor = System.Drawing.Color.Chocolate;
            this.labelCim.Name = "labelCim";
            this.labelCim.TabIndex = 1;
            this.labelCim.Text = "Cukrászda";
            this.labelCim.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // labelInfo
            //
            this.labelInfo.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.labelInfo.Name = "labelInfo";
            this.labelInfo.Size = new System.Drawing.Size(384, 40);
            this.labelInfo.TabIndex = 2;
            this.labelInfo.Text = "Nyissa meg a választékot a Fájl / Megnyitás menüponttal!";
            this.labelInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            //
            // Form1
            //
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.MistyRose;
            this.ClientSize = new System.Drawing.Size(384, 311);
            this.Controls.Add(this.labelCim);
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Cukrászda";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fajlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem megnyitasToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem kilepesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sutemenyekToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem nevjegyToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label labelCim;
        private System.Windows.Forms.Label labelInfo;
    }
}
