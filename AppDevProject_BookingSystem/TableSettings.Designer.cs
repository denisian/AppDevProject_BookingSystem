namespace AppDevProject_BookingSystem
{
    partial class TableSettings
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
            this.lblTableName = new System.Windows.Forms.Label();
            this.lblNumSeats = new System.Windows.Forms.Label();
            this.lblMinNumSeats = new System.Windows.Forms.Label();
            this.txtTableName = new System.Windows.Forms.TextBox();
            this.btnOk = new System.Windows.Forms.Button();
            this.upDownNumSeats = new System.Windows.Forms.NumericUpDown();
            this.upDownMinNumSeats = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.upDownNumSeats)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownMinNumSeats)).BeginInit();
            this.SuspendLayout();
            // 
            // lblTableName
            // 
            this.lblTableName.AutoSize = true;
            this.lblTableName.Location = new System.Drawing.Point(12, 20);
            this.lblTableName.Name = "lblTableName";
            this.lblTableName.Size = new System.Drawing.Size(35, 13);
            this.lblTableName.TabIndex = 0;
            this.lblTableName.Text = "Name";
            // 
            // lblNumSeats
            // 
            this.lblNumSeats.AutoSize = true;
            this.lblNumSeats.Location = new System.Drawing.Point(12, 62);
            this.lblNumSeats.Name = "lblNumSeats";
            this.lblNumSeats.Size = new System.Drawing.Size(72, 13);
            this.lblNumSeats.TabIndex = 1;
            this.lblNumSeats.Text = "Seats number";
            // 
            // lblMinNumSeats
            // 
            this.lblMinNumSeats.AutoSize = true;
            this.lblMinNumSeats.Location = new System.Drawing.Point(12, 102);
            this.lblMinNumSeats.Name = "lblMinNumSeats";
            this.lblMinNumSeats.Size = new System.Drawing.Size(108, 13);
            this.lblMinNumSeats.TabIndex = 2;
            this.lblMinNumSeats.Text = "Minimal seats number";
            // 
            // txtTableName
            // 
            this.txtTableName.Location = new System.Drawing.Point(102, 12);
            this.txtTableName.Name = "txtTableName";
            this.txtTableName.Size = new System.Drawing.Size(100, 20);
            this.txtTableName.TabIndex = 3;
            this.txtTableName.Enter += new System.EventHandler(this.txtTableName_Enter);
            this.txtTableName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtTableName_KeyDown);
            // 
            // btnOk
            // 
            this.btnOk.Location = new System.Drawing.Point(135, 154);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(67, 23);
            this.btnOk.TabIndex = 6;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // upDownNumSeats
            // 
            this.upDownNumSeats.Location = new System.Drawing.Point(158, 62);
            this.upDownNumSeats.Name = "upDownNumSeats";
            this.upDownNumSeats.Size = new System.Drawing.Size(44, 20);
            this.upDownNumSeats.TabIndex = 4;
            this.upDownNumSeats.KeyDown += new System.Windows.Forms.KeyEventHandler(this.upDownNumSeats_KeyDown);
            // 
            // upDownMinNumSeats
            // 
            this.upDownMinNumSeats.Location = new System.Drawing.Point(158, 95);
            this.upDownMinNumSeats.Name = "upDownMinNumSeats";
            this.upDownMinNumSeats.Size = new System.Drawing.Size(44, 20);
            this.upDownMinNumSeats.TabIndex = 5;
            this.upDownMinNumSeats.KeyDown += new System.Windows.Forms.KeyEventHandler(this.upDownMinNumSeats_KeyDown);
            // 
            // TableSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 189);
            this.Controls.Add(this.upDownMinNumSeats);
            this.Controls.Add(this.upDownNumSeats);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.txtTableName);
            this.Controls.Add(this.lblMinNumSeats);
            this.Controls.Add(this.lblNumSeats);
            this.Controls.Add(this.lblTableName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(239, 228);
            this.Name = "TableSettings";
            this.Text = "Table Settings";
            this.Load += new System.EventHandler(this.TableSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.upDownNumSeats)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.upDownMinNumSeats)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblTableName;
        private System.Windows.Forms.Label lblNumSeats;
        private System.Windows.Forms.Label lblMinNumSeats;
        private System.Windows.Forms.TextBox txtTableName;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.NumericUpDown upDownNumSeats;
        private System.Windows.Forms.NumericUpDown upDownMinNumSeats;
    }
}