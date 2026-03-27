namespace Defter2Fis.ForMikro.Forms
{
    partial class HakkindaForm
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
            _lblBaslik = new System.Windows.Forms.Label();
            _lblAciklama = new System.Windows.Forms.Label();
            _lblVersiyon = new System.Windows.Forms.Label();
            _lblTelif = new System.Windows.Forms.Label();
            _btnTamam = new System.Windows.Forms.Button();
            _pnlUst = new System.Windows.Forms.Panel();
            _tblAna = new System.Windows.Forms.TableLayoutPanel();

            _pnlUst.SuspendLayout();
            _tblAna.SuspendLayout();
            SuspendLayout();

            // _pnlUst
            _pnlUst.BackColor = System.Drawing.Color.FromArgb(40, 60, 90);
            _pnlUst.Controls.Add(_lblBaslik);
            _pnlUst.Dock = System.Windows.Forms.DockStyle.Top;
            _pnlUst.Location = new System.Drawing.Point(0, 0);
            _pnlUst.Name = "_pnlUst";
            _pnlUst.Size = new System.Drawing.Size(400, 70);

            // _lblBaslik
            _lblBaslik.AutoSize = true;
            _lblBaslik.Font = new System.Drawing.Font("Segoe UI", 16F, System.Drawing.FontStyle.Bold);
            _lblBaslik.ForeColor = System.Drawing.Color.White;
            _lblBaslik.Location = new System.Drawing.Point(16, 18);
            _lblBaslik.Name = "_lblBaslik";
            _lblBaslik.Size = new System.Drawing.Size(250, 30);
            _lblBaslik.Text = "DEFTER2F\u0130\u015E";

            // _tblAna
            _tblAna.ColumnCount = 1;
            _tblAna.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblAna.Controls.Add(_lblAciklama, 0, 0);
            _tblAna.Controls.Add(_lblVersiyon, 0, 1);
            _tblAna.Controls.Add(_lblTelif, 0, 2);
            _tblAna.Controls.Add(_btnTamam, 0, 3);
            _tblAna.Dock = System.Windows.Forms.DockStyle.Fill;
            _tblAna.Location = new System.Drawing.Point(0, 70);
            _tblAna.Name = "_tblAna";
            _tblAna.Padding = new System.Windows.Forms.Padding(16, 12, 16, 12);
            _tblAna.RowCount = 4;
            _tblAna.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblAna.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblAna.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tblAna.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tblAna.Size = new System.Drawing.Size(400, 180);

            // _lblAciklama
            _lblAciklama.AutoSize = true;
            _lblAciklama.Font = new System.Drawing.Font("Segoe UI", 9F);
            _lblAciklama.Margin = new System.Windows.Forms.Padding(3, 8, 3, 8);
            _lblAciklama.Name = "_lblAciklama";
            _lblAciklama.Size = new System.Drawing.Size(360, 30);
            _lblAciklama.Text = "E-Defter Yevmiye XML dosyalar\u0131ndan\r\nMikro ERP V16 Jump muhasebe fi\u015fleri olu\u015fturur.";

            // _lblVersiyon
            _lblVersiyon.AutoSize = true;
            _lblVersiyon.Font = new System.Drawing.Font("Segoe UI", 9F);
            _lblVersiyon.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            _lblVersiyon.Name = "_lblVersiyon";
            _lblVersiyon.Size = new System.Drawing.Size(100, 15);
            _lblVersiyon.Text = "Versiyon: 1.0.0";

            // _lblTelif
            _lblTelif.AutoSize = true;
            _lblTelif.Font = new System.Drawing.Font("Segoe UI", 8F);
            _lblTelif.ForeColor = System.Drawing.SystemColors.GrayText;
            _lblTelif.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
            _lblTelif.Name = "_lblTelif";
            _lblTelif.Size = new System.Drawing.Size(200, 15);
            _lblTelif.Text = "\u00A9 2025 Prima Havac\u0131l\u0131k. T\u00fcm haklar\u0131 sakl\u0131d\u0131r.";

            // _btnTamam
            _btnTamam.Anchor = System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right;
            _btnTamam.DialogResult = System.Windows.Forms.DialogResult.OK;
            _btnTamam.Location = new System.Drawing.Point(301, 140);
            _btnTamam.Name = "_btnTamam";
            _btnTamam.Size = new System.Drawing.Size(80, 28);
            _btnTamam.Text = "Tamam";
            _btnTamam.UseVisualStyleBackColor = true;
            _btnTamam.Click += BtnTamam_Click;

            // HakkindaForm
            AcceptButton = _btnTamam;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(400, 250);
            Controls.Add(_tblAna);
            Controls.Add(_pnlUst);
            Font = new System.Drawing.Font("Segoe UI", 9F);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "HakkindaForm";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Hakk\u0131nda — Defter2Fi\u015F";

            _pnlUst.ResumeLayout(false);
            _pnlUst.PerformLayout();
            _tblAna.ResumeLayout(false);
            _tblAna.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.Panel _pnlUst;
        private System.Windows.Forms.Label _lblBaslik;
        private System.Windows.Forms.TableLayoutPanel _tblAna;
        private System.Windows.Forms.Label _lblAciklama;
        private System.Windows.Forms.Label _lblVersiyon;
        private System.Windows.Forms.Label _lblTelif;
        private System.Windows.Forms.Button _btnTamam;
    }
}
