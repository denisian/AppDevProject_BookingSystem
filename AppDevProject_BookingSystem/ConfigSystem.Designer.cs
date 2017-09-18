namespace AppDevProject_BookingSystem
{
    partial class ConfigSystem
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
            this.btnOpenTableMap = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.btnAddTable = new System.Windows.Forms.Button();
            this.datePickerBooking = new System.Windows.Forms.DateTimePicker();
            this.btnShowBookings = new System.Windows.Forms.Button();
            this.cmbTimeBooking = new System.Windows.Forms.ComboBox();
            this.cbAnytimeBooking = new System.Windows.Forms.CheckBox();
            this.btnNewBooking = new System.Windows.Forms.Button();
            this.btnSaveTableMap = new System.Windows.Forms.Button();
            this.btnManageTables = new System.Windows.Forms.Button();
            this.btnManageUsers = new System.Windows.Forms.Button();
            this.lblLoggedInUser = new System.Windows.Forms.Label();
            this.btnReloadMap = new System.Windows.Forms.Button();
            this.dataGridViewBooking = new System.Windows.Forms.DataGridView();
            this.pnlTables = new AppDevProject_BookingSystem.DoubleBufferedPanel();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBooking)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOpenTableMap
            // 
            this.btnOpenTableMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenTableMap.Location = new System.Drawing.Point(11, 486);
            this.btnOpenTableMap.Name = "btnOpenTableMap";
            this.btnOpenTableMap.Size = new System.Drawing.Size(75, 23);
            this.btnOpenTableMap.TabIndex = 1;
            this.btnOpenTableMap.Text = "Open map";
            this.btnOpenTableMap.UseVisualStyleBackColor = true;
            this.btnOpenTableMap.Click += new System.EventHandler(this.OpenImage_Click);
            // 
            // btnExit
            // 
            this.btnExit.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnExit.Location = new System.Drawing.Point(935, 6);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(75, 23);
            this.btnExit.TabIndex = 4;
            this.btnExit.Text = "Logout";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // btnAddTable
            // 
            this.btnAddTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnAddTable.Location = new System.Drawing.Point(92, 486);
            this.btnAddTable.Name = "btnAddTable";
            this.btnAddTable.Size = new System.Drawing.Size(75, 23);
            this.btnAddTable.TabIndex = 5;
            this.btnAddTable.Text = "Add table";
            this.btnAddTable.UseVisualStyleBackColor = true;
            this.btnAddTable.Click += new System.EventHandler(this.btnAddTable_Click);
            // 
            // datePickerBooking
            // 
            this.datePickerBooking.Location = new System.Drawing.Point(11, 9);
            this.datePickerBooking.MaxDate = new System.DateTime(2030, 12, 31, 0, 0, 0, 0);
            this.datePickerBooking.MinDate = new System.DateTime(2017, 1, 1, 0, 0, 0, 0);
            this.datePickerBooking.Name = "datePickerBooking";
            this.datePickerBooking.Size = new System.Drawing.Size(195, 20);
            this.datePickerBooking.TabIndex = 6;
            // 
            // btnShowBookings
            // 
            this.btnShowBookings.Location = new System.Drawing.Point(370, 6);
            this.btnShowBookings.Name = "btnShowBookings";
            this.btnShowBookings.Size = new System.Drawing.Size(106, 23);
            this.btnShowBookings.TabIndex = 8;
            this.btnShowBookings.Text = "Show Bookings";
            this.btnShowBookings.UseVisualStyleBackColor = true;
            this.btnShowBookings.Click += new System.EventHandler(this.btnShowBookings_Click);
            // 
            // cmbTimeBooking
            // 
            this.cmbTimeBooking.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTimeBooking.FormattingEnabled = true;
            this.cmbTimeBooking.Location = new System.Drawing.Point(211, 8);
            this.cmbTimeBooking.Name = "cmbTimeBooking";
            this.cmbTimeBooking.Size = new System.Drawing.Size(67, 21);
            this.cmbTimeBooking.TabIndex = 9;
            // 
            // cbAnytimeBooking
            // 
            this.cbAnytimeBooking.AutoSize = true;
            this.cbAnytimeBooking.Location = new System.Drawing.Point(298, 12);
            this.cbAnytimeBooking.Name = "cbAnytimeBooking";
            this.cbAnytimeBooking.Size = new System.Drawing.Size(63, 17);
            this.cbAnytimeBooking.TabIndex = 10;
            this.cbAnytimeBooking.Text = "Anytime";
            this.cbAnytimeBooking.UseVisualStyleBackColor = true;
            this.cbAnytimeBooking.CheckedChanged += new System.EventHandler(this.cbAnytimeBooking_CheckedChanged);
            // 
            // btnNewBooking
            // 
            this.btnNewBooking.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNewBooking.Location = new System.Drawing.Point(914, 486);
            this.btnNewBooking.Name = "btnNewBooking";
            this.btnNewBooking.Size = new System.Drawing.Size(96, 23);
            this.btnNewBooking.TabIndex = 11;
            this.btnNewBooking.Text = "New Booking";
            this.btnNewBooking.UseVisualStyleBackColor = true;
            this.btnNewBooking.Click += new System.EventHandler(this.btnBooking_Click);
            // 
            // btnSaveTableMap
            // 
            this.btnSaveTableMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSaveTableMap.Location = new System.Drawing.Point(581, 486);
            this.btnSaveTableMap.Name = "btnSaveTableMap";
            this.btnSaveTableMap.Size = new System.Drawing.Size(75, 23);
            this.btnSaveTableMap.TabIndex = 13;
            this.btnSaveTableMap.Text = "Save map";
            this.btnSaveTableMap.UseVisualStyleBackColor = true;
            this.btnSaveTableMap.Click += new System.EventHandler(this.btnSaveTableMap_Click);
            // 
            // btnManageTables
            // 
            this.btnManageTables.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManageTables.Location = new System.Drawing.Point(812, 486);
            this.btnManageTables.Name = "btnManageTables";
            this.btnManageTables.Size = new System.Drawing.Size(96, 23);
            this.btnManageTables.TabIndex = 14;
            this.btnManageTables.Text = "Tables";
            this.btnManageTables.UseVisualStyleBackColor = true;
            this.btnManageTables.Click += new System.EventHandler(this.btnManageTables_Click);
            // 
            // btnManageUsers
            // 
            this.btnManageUsers.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnManageUsers.Location = new System.Drawing.Point(710, 486);
            this.btnManageUsers.Name = "btnManageUsers";
            this.btnManageUsers.Size = new System.Drawing.Size(96, 23);
            this.btnManageUsers.TabIndex = 15;
            this.btnManageUsers.Text = "Employees";
            this.btnManageUsers.UseVisualStyleBackColor = true;
            this.btnManageUsers.Click += new System.EventHandler(this.btnManageUsers_Click);
            // 
            // lblLoggedInUser
            // 
            this.lblLoggedInUser.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblLoggedInUser.AutoSize = true;
            this.lblLoggedInUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblLoggedInUser.ForeColor = System.Drawing.Color.Blue;
            this.lblLoggedInUser.Location = new System.Drawing.Point(791, 11);
            this.lblLoggedInUser.Name = "lblLoggedInUser";
            this.lblLoggedInUser.Size = new System.Drawing.Size(0, 15);
            this.lblLoggedInUser.TabIndex = 16;
            this.lblLoggedInUser.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnReloadMap
            // 
            this.btnReloadMap.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnReloadMap.Location = new System.Drawing.Point(500, 486);
            this.btnReloadMap.Name = "btnReloadMap";
            this.btnReloadMap.Size = new System.Drawing.Size(75, 23);
            this.btnReloadMap.TabIndex = 17;
            this.btnReloadMap.Text = "Reload map";
            this.btnReloadMap.UseVisualStyleBackColor = true;
            this.btnReloadMap.Click += new System.EventHandler(this.btnReloadMap_Click);
            // 
            // dataGridViewBooking
            // 
            this.dataGridViewBooking.AllowUserToAddRows = false;
            this.dataGridViewBooking.AllowUserToDeleteRows = false;
            this.dataGridViewBooking.AllowUserToOrderColumns = true;
            this.dataGridViewBooking.AllowUserToResizeRows = false;
            this.dataGridViewBooking.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewBooking.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewBooking.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridViewBooking.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewBooking.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridViewBooking.Location = new System.Drawing.Point(652, 36);
            this.dataGridViewBooking.MultiSelect = false;
            this.dataGridViewBooking.Name = "dataGridViewBooking";
            this.dataGridViewBooking.ReadOnly = true;
            this.dataGridViewBooking.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewBooking.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewBooking.Size = new System.Drawing.Size(358, 444);
            this.dataGridViewBooking.TabIndex = 7;
            this.dataGridViewBooking.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewBooking_CellDoubleClick);
            this.dataGridViewBooking.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridViewBooking_MouseClick);
            // 
            // pnlTables
            // 
            this.pnlTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlTables.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlTables.Location = new System.Drawing.Point(11, 36);
            this.pnlTables.Name = "pnlTables";
            this.pnlTables.Size = new System.Drawing.Size(635, 444);
            this.pnlTables.TabIndex = 3;
            // 
            // ConfigSystem
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1023, 521);
            this.Controls.Add(this.btnReloadMap);
            this.Controls.Add(this.lblLoggedInUser);
            this.Controls.Add(this.btnManageUsers);
            this.Controls.Add(this.btnManageTables);
            this.Controls.Add(this.btnSaveTableMap);
            this.Controls.Add(this.btnNewBooking);
            this.Controls.Add(this.cbAnytimeBooking);
            this.Controls.Add(this.cmbTimeBooking);
            this.Controls.Add(this.btnShowBookings);
            this.Controls.Add(this.dataGridViewBooking);
            this.Controls.Add(this.datePickerBooking);
            this.Controls.Add(this.btnAddTable);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.pnlTables);
            this.Controls.Add(this.btnOpenTableMap);
            this.MinimumSize = new System.Drawing.Size(1039, 560);
            this.Name = "ConfigSystem";
            this.Text = "Configuration";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConfigSystem_FormClosed);
            this.Load += new System.EventHandler(this.ConfigSystem_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewBooking)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOpenTableMap;
        private DoubleBufferedPanel pnlTables;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Button btnAddTable;
        private System.Windows.Forms.DateTimePicker datePickerBooking;
        private System.Windows.Forms.DataGridView dataGridViewBooking;
        private System.Windows.Forms.Button btnShowBookings;
        private System.Windows.Forms.ComboBox cmbTimeBooking;
        private System.Windows.Forms.CheckBox cbAnytimeBooking;
        private System.Windows.Forms.Button btnNewBooking;
        private System.Windows.Forms.Button btnSaveTableMap;
        private System.Windows.Forms.Button btnManageTables;
        private System.Windows.Forms.Button btnManageUsers;
        private System.Windows.Forms.Label lblLoggedInUser;
        private System.Windows.Forms.Button btnReloadMap;
    }
}