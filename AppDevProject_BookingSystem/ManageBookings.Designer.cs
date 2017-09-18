namespace AppDevProject_BookingSystem
{
    partial class ManageBookings
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
            this.datePickerBooking = new System.Windows.Forms.DateTimePicker();
            this.cmbTimeBooking = new System.Windows.Forms.ComboBox();
            this.cmbSizeBooking = new System.Windows.Forms.ComboBox();
            this.lblDate = new System.Windows.Forms.Label();
            this.lblTime = new System.Windows.Forms.Label();
            this.lblPartySize = new System.Windows.Forms.Label();
            this.btnCheckAvailability = new System.Windows.Forms.Button();
            this.grBoxCustomerInfo = new System.Windows.Forms.GroupBox();
            this.cmbOccasion = new System.Windows.Forms.ComboBox();
            this.lblNotes = new System.Windows.Forms.Label();
            this.lblOccasion = new System.Windows.Forms.Label();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.btnBookTable = new System.Windows.Forms.Button();
            this.lblTitle = new System.Windows.Forms.Label();
            this.txtEmail = new System.Windows.Forms.TextBox();
            this.lblEmail = new System.Windows.Forms.Label();
            this.lblPhon = new System.Windows.Forms.Label();
            this.txtPhone = new System.Windows.Forms.TextBox();
            this.txtLastName = new System.Windows.Forms.TextBox();
            this.txtFirstName = new System.Windows.Forms.TextBox();
            this.lblLastName = new System.Windows.Forms.Label();
            this.lblFirstName = new System.Windows.Forms.Label();
            this.txtTitle = new System.Windows.Forms.TextBox();
            this.linkLblTable = new System.Windows.Forms.LinkLabel();
            this.grBoxCustomerInfo.SuspendLayout();
            this.SuspendLayout();
            // 
            // datePickerBooking
            // 
            this.datePickerBooking.Location = new System.Drawing.Point(75, 13);
            this.datePickerBooking.Name = "datePickerBooking";
            this.datePickerBooking.Size = new System.Drawing.Size(200, 20);
            this.datePickerBooking.TabIndex = 0;
            this.datePickerBooking.ValueChanged += new System.EventHandler(this.datePickerBooking_ValueChanged);
            // 
            // cmbTimeBooking
            // 
            this.cmbTimeBooking.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTimeBooking.FormattingEnabled = true;
            this.cmbTimeBooking.Location = new System.Drawing.Point(75, 39);
            this.cmbTimeBooking.Name = "cmbTimeBooking";
            this.cmbTimeBooking.Size = new System.Drawing.Size(69, 21);
            this.cmbTimeBooking.TabIndex = 1;
            this.cmbTimeBooking.SelectedValueChanged += new System.EventHandler(this.cmbTimeBooking_SelectedValueChanged);
            // 
            // cmbSizeBooking
            // 
            this.cmbSizeBooking.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbSizeBooking.FormattingEnabled = true;
            this.cmbSizeBooking.Location = new System.Drawing.Point(75, 66);
            this.cmbSizeBooking.Name = "cmbSizeBooking";
            this.cmbSizeBooking.Size = new System.Drawing.Size(69, 21);
            this.cmbSizeBooking.TabIndex = 2;
            this.cmbSizeBooking.SelectedValueChanged += new System.EventHandler(this.cmbSizeBooking_SelectedValueChanged);
            // 
            // lblDate
            // 
            this.lblDate.AutoSize = true;
            this.lblDate.Location = new System.Drawing.Point(12, 19);
            this.lblDate.Name = "lblDate";
            this.lblDate.Size = new System.Drawing.Size(30, 13);
            this.lblDate.TabIndex = 3;
            this.lblDate.Text = "Date";
            // 
            // lblTime
            // 
            this.lblTime.AutoSize = true;
            this.lblTime.Location = new System.Drawing.Point(12, 47);
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(30, 13);
            this.lblTime.TabIndex = 4;
            this.lblTime.Text = "Time";
            // 
            // lblPartySize
            // 
            this.lblPartySize.AutoSize = true;
            this.lblPartySize.Location = new System.Drawing.Point(12, 74);
            this.lblPartySize.Name = "lblPartySize";
            this.lblPartySize.Size = new System.Drawing.Size(54, 13);
            this.lblPartySize.TabIndex = 5;
            this.lblPartySize.Text = "Party Size";
            // 
            // btnCheckAvailability
            // 
            this.btnCheckAvailability.Location = new System.Drawing.Point(182, 114);
            this.btnCheckAvailability.Name = "btnCheckAvailability";
            this.btnCheckAvailability.Size = new System.Drawing.Size(93, 23);
            this.btnCheckAvailability.TabIndex = 6;
            this.btnCheckAvailability.Text = "Check Seats";
            this.btnCheckAvailability.UseVisualStyleBackColor = true;
            this.btnCheckAvailability.Click += new System.EventHandler(this.btnCheckAvailability_Click);
            // 
            // grBoxCustomerInfo
            // 
            this.grBoxCustomerInfo.Controls.Add(this.cmbOccasion);
            this.grBoxCustomerInfo.Controls.Add(this.lblNotes);
            this.grBoxCustomerInfo.Controls.Add(this.lblOccasion);
            this.grBoxCustomerInfo.Controls.Add(this.txtNotes);
            this.grBoxCustomerInfo.Controls.Add(this.btnBookTable);
            this.grBoxCustomerInfo.Controls.Add(this.lblTitle);
            this.grBoxCustomerInfo.Controls.Add(this.txtEmail);
            this.grBoxCustomerInfo.Controls.Add(this.lblEmail);
            this.grBoxCustomerInfo.Controls.Add(this.lblPhon);
            this.grBoxCustomerInfo.Controls.Add(this.txtPhone);
            this.grBoxCustomerInfo.Controls.Add(this.txtLastName);
            this.grBoxCustomerInfo.Controls.Add(this.txtFirstName);
            this.grBoxCustomerInfo.Controls.Add(this.lblLastName);
            this.grBoxCustomerInfo.Controls.Add(this.lblFirstName);
            this.grBoxCustomerInfo.Controls.Add(this.txtTitle);
            this.grBoxCustomerInfo.Location = new System.Drawing.Point(15, 143);
            this.grBoxCustomerInfo.Name = "grBoxCustomerInfo";
            this.grBoxCustomerInfo.Size = new System.Drawing.Size(281, 344);
            this.grBoxCustomerInfo.TabIndex = 7;
            this.grBoxCustomerInfo.TabStop = false;
            this.grBoxCustomerInfo.Text = "Customer Info";
            // 
            // cmbOccasion
            // 
            this.cmbOccasion.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbOccasion.FormattingEnabled = true;
            this.cmbOccasion.Location = new System.Drawing.Point(69, 192);
            this.cmbOccasion.Name = "cmbOccasion";
            this.cmbOccasion.Size = new System.Drawing.Size(90, 21);
            this.cmbOccasion.TabIndex = 14;
            // 
            // lblNotes
            // 
            this.lblNotes.AutoSize = true;
            this.lblNotes.Location = new System.Drawing.Point(6, 237);
            this.lblNotes.Name = "lblNotes";
            this.lblNotes.Size = new System.Drawing.Size(35, 13);
            this.lblNotes.TabIndex = 14;
            this.lblNotes.Text = "Notes";
            // 
            // lblOccasion
            // 
            this.lblOccasion.AutoSize = true;
            this.lblOccasion.Location = new System.Drawing.Point(6, 201);
            this.lblOccasion.Name = "lblOccasion";
            this.lblOccasion.Size = new System.Drawing.Size(52, 13);
            this.lblOccasion.TabIndex = 2;
            this.lblOccasion.Text = "Occasion";
            // 
            // txtNotes
            // 
            this.txtNotes.Location = new System.Drawing.Point(69, 230);
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(191, 61);
            this.txtNotes.TabIndex = 14;
            this.txtNotes.Enter += new System.EventHandler(this.txtNotes_Enter);
            // 
            // btnBookTable
            // 
            this.btnBookTable.Location = new System.Drawing.Point(173, 306);
            this.btnBookTable.Name = "btnBookTable";
            this.btnBookTable.Size = new System.Drawing.Size(87, 23);
            this.btnBookTable.TabIndex = 17;
            this.btnBookTable.Text = "Book Table";
            this.btnBookTable.UseVisualStyleBackColor = true;
            this.btnBookTable.Click += new System.EventHandler(this.btnBookTable_Click);
            // 
            // lblTitle
            // 
            this.lblTitle.AutoSize = true;
            this.lblTitle.Location = new System.Drawing.Point(6, 27);
            this.lblTitle.Name = "lblTitle";
            this.lblTitle.Size = new System.Drawing.Size(27, 13);
            this.lblTitle.TabIndex = 1;
            this.lblTitle.Text = "Title";
            // 
            // txtEmail
            // 
            this.txtEmail.Location = new System.Drawing.Point(69, 158);
            this.txtEmail.Name = "txtEmail";
            this.txtEmail.Size = new System.Drawing.Size(191, 20);
            this.txtEmail.TabIndex = 13;
            this.txtEmail.Enter += new System.EventHandler(this.txtEmail_Enter);
            // 
            // lblEmail
            // 
            this.lblEmail.AutoSize = true;
            this.lblEmail.Location = new System.Drawing.Point(6, 165);
            this.lblEmail.Name = "lblEmail";
            this.lblEmail.Size = new System.Drawing.Size(32, 13);
            this.lblEmail.TabIndex = 3;
            this.lblEmail.Text = "Email";
            // 
            // lblPhon
            // 
            this.lblPhon.AutoSize = true;
            this.lblPhon.Location = new System.Drawing.Point(6, 130);
            this.lblPhon.Name = "lblPhon";
            this.lblPhon.Size = new System.Drawing.Size(42, 13);
            this.lblPhon.TabIndex = 13;
            this.lblPhon.Text = "Phone*";
            // 
            // txtPhone
            // 
            this.txtPhone.Location = new System.Drawing.Point(69, 123);
            this.txtPhone.Name = "txtPhone";
            this.txtPhone.Size = new System.Drawing.Size(191, 20);
            this.txtPhone.TabIndex = 12;
            this.txtPhone.Enter += new System.EventHandler(this.txtPhone_Enter);
            // 
            // txtLastName
            // 
            this.txtLastName.Location = new System.Drawing.Point(69, 88);
            this.txtLastName.Name = "txtLastName";
            this.txtLastName.Size = new System.Drawing.Size(191, 20);
            this.txtLastName.TabIndex = 11;
            this.txtLastName.Enter += new System.EventHandler(this.txtLastName_Enter);
            // 
            // txtFirstName
            // 
            this.txtFirstName.Location = new System.Drawing.Point(69, 53);
            this.txtFirstName.Name = "txtFirstName";
            this.txtFirstName.Size = new System.Drawing.Size(191, 20);
            this.txtFirstName.TabIndex = 10;
            this.txtFirstName.Enter += new System.EventHandler(this.txtFirstName_Enter);
            // 
            // lblLastName
            // 
            this.lblLastName.AutoSize = true;
            this.lblLastName.Location = new System.Drawing.Point(6, 93);
            this.lblLastName.Name = "lblLastName";
            this.lblLastName.Size = new System.Drawing.Size(58, 13);
            this.lblLastName.TabIndex = 9;
            this.lblLastName.Text = "Last Name";
            // 
            // lblFirstName
            // 
            this.lblFirstName.AutoSize = true;
            this.lblFirstName.Location = new System.Drawing.Point(6, 60);
            this.lblFirstName.Name = "lblFirstName";
            this.lblFirstName.Size = new System.Drawing.Size(61, 13);
            this.lblFirstName.TabIndex = 8;
            this.lblFirstName.Text = "First Name*";
            // 
            // txtTitle
            // 
            this.txtTitle.Location = new System.Drawing.Point(69, 20);
            this.txtTitle.Name = "txtTitle";
            this.txtTitle.Size = new System.Drawing.Size(60, 20);
            this.txtTitle.TabIndex = 0;
            this.txtTitle.Enter += new System.EventHandler(this.txtTitle_Enter);
            // 
            // linkLblTable
            // 
            this.linkLblTable.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.linkLblTable.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.linkLblTable.Location = new System.Drawing.Point(182, 47);
            this.linkLblTable.Name = "linkLblTable";
            this.linkLblTable.Size = new System.Drawing.Size(93, 40);
            this.linkLblTable.TabIndex = 8;
            this.linkLblTable.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.linkLblTable.TextChanged += new System.EventHandler(this.linkLblTable_TextChanged);
            this.linkLblTable.Click += new System.EventHandler(this.linkLblTable_Click);
            // 
            // ManageBookings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(308, 508);
            this.Controls.Add(this.linkLblTable);
            this.Controls.Add(this.grBoxCustomerInfo);
            this.Controls.Add(this.btnCheckAvailability);
            this.Controls.Add(this.lblPartySize);
            this.Controls.Add(this.lblTime);
            this.Controls.Add(this.lblDate);
            this.Controls.Add(this.cmbSizeBooking);
            this.Controls.Add(this.cmbTimeBooking);
            this.Controls.Add(this.datePickerBooking);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(324, 547);
            this.MinimizeBox = false;
            this.Name = "ManageBookings";
            this.Text = "Book a table";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ManageBookings_FormClosed);
            this.Load += new System.EventHandler(this.ManageBookings_Load);
            this.grBoxCustomerInfo.ResumeLayout(false);
            this.grBoxCustomerInfo.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DateTimePicker datePickerBooking;
        private System.Windows.Forms.ComboBox cmbTimeBooking;
        private System.Windows.Forms.ComboBox cmbSizeBooking;
        private System.Windows.Forms.Label lblDate;
        private System.Windows.Forms.Label lblTime;
        private System.Windows.Forms.Label lblPartySize;
        private System.Windows.Forms.Button btnCheckAvailability;
        private System.Windows.Forms.GroupBox grBoxCustomerInfo;
        private System.Windows.Forms.Button btnBookTable;
        private System.Windows.Forms.Label lblTitle;
        private System.Windows.Forms.TextBox txtEmail;
        private System.Windows.Forms.Label lblEmail;
        private System.Windows.Forms.Label lblPhon;
        private System.Windows.Forms.TextBox txtPhone;
        private System.Windows.Forms.TextBox txtLastName;
        private System.Windows.Forms.TextBox txtFirstName;
        private System.Windows.Forms.Label lblLastName;
        private System.Windows.Forms.Label lblFirstName;
        private System.Windows.Forms.TextBox txtTitle;
        private System.Windows.Forms.Label lblNotes;
        private System.Windows.Forms.Label lblOccasion;
        private System.Windows.Forms.TextBox txtNotes;
        private System.Windows.Forms.LinkLabel linkLblTable;
        private System.Windows.Forms.ComboBox cmbOccasion;
    }
}