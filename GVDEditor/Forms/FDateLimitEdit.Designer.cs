namespace GVDEditor.Forms
{
    partial class FDateLimitEdit
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.exLabel1 = new ExControls.ExLabel();
            this.tbDateLimit = new ExControls.ExTextBox();
            this.dgvCalendar = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.bCheck = new ExControls.ExButton();
            this.bReset = new ExControls.ExButton();
            this.exLabel2 = new ExControls.ExLabel();
            this.tbOldDateLimit = new ExControls.ExTextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.bStorno = new ExControls.ExButton();
            this.bOK = new ExControls.ExButton();
            this.bNever = new ExControls.ExButton();
            this.bDaily = new ExControls.ExButton();
            this.timer = new System.Windows.Forms.Timer(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCalendar)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // exLabel1
            // 
            this.exLabel1.AutoSize = true;
            this.exLabel1.Location = new System.Drawing.Point(6, 39);
            this.exLabel1.Name = "exLabel1";
            this.exLabel1.Size = new System.Drawing.Size(36, 13);
            this.exLabel1.TabIndex = 2;
            this.exLabel1.Text = "Nové:";
            // 
            // tbDateLimit
            // 
            this.tbDateLimit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbDateLimit.BorderColor = System.Drawing.Color.DimGray;
            this.tbDateLimit.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbDateLimit.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbDateLimit.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDateLimit.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbDateLimit.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbDateLimit.HintText = null;
            this.tbDateLimit.Location = new System.Drawing.Point(65, 32);
            this.tbDateLimit.Name = "tbDateLimit";
            this.tbDateLimit.Size = new System.Drawing.Size(640, 20);
            this.tbDateLimit.TabIndex = 3;
            this.tbDateLimit.TextChanged += new System.EventHandler(this.TbDateLimit_TextChanged);
            // 
            // dgvCalendar
            // 
            this.dgvCalendar.AllowUserToAddRows = false;
            this.dgvCalendar.AllowUserToDeleteRows = false;
            this.dgvCalendar.AllowUserToResizeColumns = false;
            this.dgvCalendar.AllowUserToResizeRows = false;
            this.dgvCalendar.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dgvCalendar.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvCalendar.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvCalendar.Location = new System.Drawing.Point(0, 60);
            this.dgvCalendar.Name = "dgvCalendar";
            this.dgvCalendar.ReadOnly = true;
            this.dgvCalendar.RowHeadersVisible = false;
            this.dgvCalendar.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.dgvCalendar.Size = new System.Drawing.Size(800, 355);
            this.dgvCalendar.TabIndex = 1;
            this.dgvCalendar.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvCalendar_CellMouseDown);
            this.dgvCalendar.CellMouseMove += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvCalendar_CellMouseMove);
            this.dgvCalendar.CellMouseUp += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DgvCalendar_CellMouseUp);
            this.dgvCalendar.CellStateChanged += new System.Windows.Forms.DataGridViewCellStateChangedEventHandler(this.DgvCalendar_CellStateChanged);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.bCheck);
            this.panel1.Controls.Add(this.bReset);
            this.panel1.Controls.Add(this.exLabel2);
            this.panel1.Controls.Add(this.tbOldDateLimit);
            this.panel1.Controls.Add(this.exLabel1);
            this.panel1.Controls.Add(this.tbDateLimit);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Padding = new System.Windows.Forms.Padding(3);
            this.panel1.Size = new System.Drawing.Size(800, 60);
            this.panel1.TabIndex = 0;
            // 
            // bCheck
            // 
            this.bCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bCheck.Location = new System.Drawing.Point(711, 31);
            this.bCheck.Name = "bCheck";
            this.bCheck.Size = new System.Drawing.Size(83, 23);
            this.bCheck.TabIndex = 5;
            this.bCheck.Text = "Skontrolovať";
            this.bCheck.UseVisualStyleBackColor = true;
            this.bCheck.Click += new System.EventHandler(this.BCheck_Click);
            // 
            // bReset
            // 
            this.bReset.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.bReset.Location = new System.Drawing.Point(711, 4);
            this.bReset.Name = "bReset";
            this.bReset.Size = new System.Drawing.Size(83, 23);
            this.bReset.TabIndex = 4;
            this.bReset.Text = "Resetovať";
            this.bReset.UseVisualStyleBackColor = true;
            this.bReset.Click += new System.EventHandler(this.BReset_Click);
            // 
            // exLabel2
            // 
            this.exLabel2.AutoSize = true;
            this.exLabel2.Location = new System.Drawing.Point(6, 9);
            this.exLabel2.Name = "exLabel2";
            this.exLabel2.Size = new System.Drawing.Size(53, 13);
            this.exLabel2.TabIndex = 0;
            this.exLabel2.Text = "Pôvodné:";
            // 
            // tbOldDateLimit
            // 
            this.tbOldDateLimit.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tbOldDateLimit.BorderColor = System.Drawing.Color.DimGray;
            this.tbOldDateLimit.DisabledBackColor = System.Drawing.SystemColors.Control;
            this.tbOldDateLimit.DisabledBorderColor = System.Drawing.SystemColors.InactiveBorder;
            this.tbOldDateLimit.DisabledForeColor = System.Drawing.SystemColors.GrayText;
            this.tbOldDateLimit.HighlightColor = System.Drawing.SystemColors.Highlight;
            this.tbOldDateLimit.HintForeColor = System.Drawing.SystemColors.GrayText;
            this.tbOldDateLimit.HintText = null;
            this.tbOldDateLimit.Location = new System.Drawing.Point(65, 6);
            this.tbOldDateLimit.Name = "tbOldDateLimit";
            this.tbOldDateLimit.ReadOnly = true;
            this.tbOldDateLimit.Size = new System.Drawing.Size(640, 20);
            this.tbOldDateLimit.TabIndex = 1;
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.bStorno);
            this.panel2.Controls.Add(this.bOK);
            this.panel2.Controls.Add(this.bNever);
            this.panel2.Controls.Add(this.bDaily);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel2.Location = new System.Drawing.Point(0, 415);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(3);
            this.panel2.Size = new System.Drawing.Size(800, 35);
            this.panel2.TabIndex = 2;
            // 
            // bStorno
            // 
            this.bStorno.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bStorno.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.bStorno.Location = new System.Drawing.Point(719, 6);
            this.bStorno.Name = "bStorno";
            this.bStorno.Size = new System.Drawing.Size(75, 23);
            this.bStorno.TabIndex = 3;
            this.bStorno.Text = "Zrušiť";
            this.bStorno.UseVisualStyleBackColor = true;
            // 
            // bOK
            // 
            this.bOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.bOK.Location = new System.Drawing.Point(638, 6);
            this.bOK.Name = "bOK";
            this.bOK.Size = new System.Drawing.Size(75, 23);
            this.bOK.TabIndex = 2;
            this.bOK.Text = "Uložiť";
            this.bOK.UseVisualStyleBackColor = true;
            this.bOK.Click += new System.EventHandler(this.BOK_Click);
            // 
            // bNever
            // 
            this.bNever.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bNever.Location = new System.Drawing.Point(87, 6);
            this.bNever.Name = "bNever";
            this.bNever.Size = new System.Drawing.Size(75, 23);
            this.bNever.TabIndex = 1;
            this.bNever.Text = "Nikdy";
            this.bNever.UseVisualStyleBackColor = true;
            this.bNever.Click += new System.EventHandler(this.BNever_Click);
            // 
            // bDaily
            // 
            this.bDaily.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.bDaily.Location = new System.Drawing.Point(6, 6);
            this.bDaily.Name = "bDaily";
            this.bDaily.Size = new System.Drawing.Size(75, 23);
            this.bDaily.TabIndex = 0;
            this.bDaily.Text = "Denne";
            this.bDaily.UseVisualStyleBackColor = true;
            this.bDaily.Click += new System.EventHandler(this.BDaily_Click);
            // 
            // timer
            // 
            this.timer.Tick += new System.EventHandler(this.Timer_Tick);
            // 
            // FDateLimitEdit
            // 
            this.AcceptButton = this.bOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.bStorno;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.dgvCalendar);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FDateLimitEdit";
            this.ShowIcon = false;
            this.Text = "Nastavenie platnosti";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.FDateLimit_FormClosed);
            this.Load += new System.EventHandler(this.FDateLimit_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dgvCalendar)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private ExControls.ExLabel exLabel1;
        private ExControls.ExTextBox tbDateLimit;
        private DataGridView dgvCalendar;
        private Panel panel1;
        private Panel panel2;
        private ExControls.ExButton bStorno;
        private ExControls.ExButton bOK;
        private ExControls.ExButton bNever;
        private ExControls.ExButton bDaily;
        private ExControls.ExButton bReset;
        private ExControls.ExLabel exLabel2;
        private ExControls.ExTextBox tbOldDateLimit;
        private ExControls.ExButton bCheck;
        private Timer timer;
    }
}