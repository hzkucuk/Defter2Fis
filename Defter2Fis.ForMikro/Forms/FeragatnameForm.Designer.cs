namespace Defter2Fis.ForMikro.Forms
{
    partial class FeragatnameForm
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
            _tlpMain = new System.Windows.Forms.TableLayoutPanel();
            _lblBaslik = new Krypton.Toolkit.KryptonLabel();
            _rtbFeragatname = new System.Windows.Forms.RichTextBox();
            _chkKabulEdiyorum = new Krypton.Toolkit.KryptonCheckBox();
            _flpButtons = new System.Windows.Forms.FlowLayoutPanel();
            _btnReddet = new Krypton.Toolkit.KryptonButton();
            _btnKabulEt = new Krypton.Toolkit.KryptonButton();

            _tlpMain.SuspendLayout();
            _flpButtons.SuspendLayout();
            SuspendLayout();

            // _tlpMain
            _tlpMain.ColumnCount = 1;
            _tlpMain.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tlpMain.Controls.Add(_lblBaslik, 0, 0);
            _tlpMain.Controls.Add(_rtbFeragatname, 0, 1);
            _tlpMain.Controls.Add(_chkKabulEdiyorum, 0, 2);
            _tlpMain.Controls.Add(_flpButtons, 0, 3);
            _tlpMain.Dock = System.Windows.Forms.DockStyle.Fill;
            _tlpMain.Location = new System.Drawing.Point(0, 0);
            _tlpMain.Name = "_tlpMain";
            _tlpMain.Padding = new System.Windows.Forms.Padding(12);
            _tlpMain.RowCount = 4;
            _tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            _tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tlpMain.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.AutoSize));
            _tlpMain.Size = new System.Drawing.Size(734, 661);

            // _lblBaslik
            _lblBaslik.Anchor = System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            _lblBaslik.LabelStyle = Krypton.Toolkit.LabelStyle.TitlePanel;
            _lblBaslik.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            _lblBaslik.Name = "_lblBaslik";
            _lblBaslik.Size = new System.Drawing.Size(706, 29);
            _lblBaslik.TabIndex = 0;
            _lblBaslik.Values.Text = "\u26A0  KULLANIM KO\u015EULLARI VE SORUMLULUK REDD\u0130 FERAGATNAMES\u0130";

            // _rtbFeragatname
            _rtbFeragatname.BackColor = System.Drawing.Color.FromArgb(30, 30, 30);
            _rtbFeragatname.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            _rtbFeragatname.Dock = System.Windows.Forms.DockStyle.Fill;
            _rtbFeragatname.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, 162);
            _rtbFeragatname.ForeColor = System.Drawing.Color.FromArgb(220, 220, 220);
            _rtbFeragatname.Margin = new System.Windows.Forms.Padding(3);
            _rtbFeragatname.Name = "_rtbFeragatname";
            _rtbFeragatname.ReadOnly = true;
            _rtbFeragatname.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            _rtbFeragatname.Size = new System.Drawing.Size(706, 500);
            _rtbFeragatname.TabIndex = 1;
            _rtbFeragatname.Text = "";

            // _chkKabulEdiyorum
            _chkKabulEdiyorum.Anchor = System.Windows.Forms.AnchorStyles.Left;
            _chkKabulEdiyorum.Margin = new System.Windows.Forms.Padding(3, 10, 3, 6);
            _chkKabulEdiyorum.Name = "_chkKabulEdiyorum";
            _chkKabulEdiyorum.Size = new System.Drawing.Size(580, 24);
            _chkKabulEdiyorum.TabIndex = 2;
            _chkKabulEdiyorum.Values.Text = "Yukar\u0131daki feragatnameyi okudum, anlad\u0131m ve t\u00fcm ko\u015Fullar\u0131 kabul ediyorum.";
            _chkKabulEdiyorum.CheckedChanged += ChkKabulEdiyorum_CheckedChanged;

            // _flpButtons
            _flpButtons.AutoSize = true;
            _flpButtons.Controls.Add(_btnReddet);
            _flpButtons.Controls.Add(_btnKabulEt);
            _flpButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            _flpButtons.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft;
            _flpButtons.Margin = new System.Windows.Forms.Padding(3, 6, 3, 3);
            _flpButtons.Name = "_flpButtons";
            _flpButtons.Size = new System.Drawing.Size(706, 42);

            // _btnReddet
            _btnReddet.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            _btnReddet.Margin = new System.Windows.Forms.Padding(6, 3, 3, 3);
            _btnReddet.Name = "_btnReddet";
            _btnReddet.Size = new System.Drawing.Size(160, 36);
            _btnReddet.TabIndex = 4;
            _btnReddet.Values.Text = "Kabul Etmiyorum — \u00c7\u0131k\u0131\u015f";
            _btnReddet.Click += BtnReddet_Click;

            // _btnKabulEt
            _btnKabulEt.Enabled = false;
            _btnKabulEt.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            _btnKabulEt.Name = "_btnKabulEt";
            _btnKabulEt.Size = new System.Drawing.Size(160, 36);
            _btnKabulEt.TabIndex = 3;
            _btnKabulEt.Values.Text = "\u2714  Kabul Ediyorum";
            _btnKabulEt.Click += BtnKabulEt_Click;

            // FeragatnameForm
            AcceptButton = _btnKabulEt;
            AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = _btnReddet;
            ClientSize = new System.Drawing.Size(734, 661);
            Controls.Add(_tlpMain);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "FeragatnameForm";
            ShowIcon = false;
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Defter2Fis \u2014 Kullan\u0131m Ko\u015Fullar\u0131 ve Feragatname";

            _tlpMain.ResumeLayout(false);
            _tlpMain.PerformLayout();
            _flpButtons.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel _tlpMain;
        private Krypton.Toolkit.KryptonLabel _lblBaslik;
        private System.Windows.Forms.RichTextBox _rtbFeragatname;
        private Krypton.Toolkit.KryptonCheckBox _chkKabulEdiyorum;
        private System.Windows.Forms.FlowLayoutPanel _flpButtons;
        private Krypton.Toolkit.KryptonButton _btnKabulEt;
        private Krypton.Toolkit.KryptonButton _btnReddet;
    }
}
