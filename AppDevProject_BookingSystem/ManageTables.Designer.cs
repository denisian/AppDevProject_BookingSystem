namespace AppDevProject_BookingSystem
{
    partial class ManageTables
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
            this.dataGridViewTables = new System.Windows.Forms.DataGridView();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnAddTable = new System.Windows.Forms.Button();
            this.btnEditTable = new System.Windows.Forms.Button();
            this.btnRemoveTable = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTables)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridViewTables
            // 
            this.dataGridViewTables.AllowUserToAddRows = false;
            this.dataGridViewTables.AllowUserToDeleteRows = false;
            this.dataGridViewTables.AllowUserToOrderColumns = true;
            this.dataGridViewTables.AllowUserToResizeRows = false;
            this.dataGridViewTables.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridViewTables.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridViewTables.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.DisplayedCells;
            this.dataGridViewTables.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewTables.GridColor = System.Drawing.SystemColors.ControlLight;
            this.dataGridViewTables.Location = new System.Drawing.Point(13, 13);
            this.dataGridViewTables.MultiSelect = false;
            this.dataGridViewTables.Name = "dataGridViewTables";
            this.dataGridViewTables.ReadOnly = true;
            this.dataGridViewTables.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.AutoSizeToAllHeaders;
            this.dataGridViewTables.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataGridViewTables.Size = new System.Drawing.Size(316, 263);
            this.dataGridViewTables.TabIndex = 0;
            this.dataGridViewTables.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTables_CellClick);
            this.dataGridViewTables.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewTables_CellDoubleClick);
            this.dataGridViewTables.MouseClick += new System.Windows.Forms.MouseEventHandler(this.dataGridViewTables_MouseClick);
            // 
            // btnOk
            // 
            this.btnOk.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.btnOk.Location = new System.Drawing.Point(136, 282);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(75, 23);
            this.btnOk.TabIndex = 1;
            this.btnOk.Text = "Ok";
            this.btnOk.UseVisualStyleBackColor = true;
            this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
            // 
            // btnAddTable
            // 
            this.btnAddTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddTable.Location = new System.Drawing.Point(345, 15);
            this.btnAddTable.Name = "btnAddTable";
            this.btnAddTable.Size = new System.Drawing.Size(75, 23);
            this.btnAddTable.TabIndex = 2;
            this.btnAddTable.Text = "Add";
            this.btnAddTable.UseVisualStyleBackColor = true;
            this.btnAddTable.Click += new System.EventHandler(this.btnAddTable_Click);
            // 
            // btnEditTable
            // 
            this.btnEditTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnEditTable.Location = new System.Drawing.Point(345, 54);
            this.btnEditTable.Name = "btnEditTable";
            this.btnEditTable.Size = new System.Drawing.Size(75, 23);
            this.btnEditTable.TabIndex = 3;
            this.btnEditTable.Text = "Edit";
            this.btnEditTable.UseVisualStyleBackColor = true;
            this.btnEditTable.Click += new System.EventHandler(this.btnEditTable_Click);
            // 
            // btnRemoveTable
            // 
            this.btnRemoveTable.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnRemoveTable.Location = new System.Drawing.Point(345, 92);
            this.btnRemoveTable.Name = "btnRemoveTable";
            this.btnRemoveTable.Size = new System.Drawing.Size(75, 23);
            this.btnRemoveTable.TabIndex = 4;
            this.btnRemoveTable.Text = "Remove table";
            this.btnRemoveTable.UseVisualStyleBackColor = true;
            this.btnRemoveTable.Click += new System.EventHandler(this.btnRemoveTable_Click);
            // 
            // ManageTables
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(436, 317);
            this.Controls.Add(this.btnRemoveTable);
            this.Controls.Add(this.btnEditTable);
            this.Controls.Add(this.btnAddTable);
            this.Controls.Add(this.btnOk);
            this.Controls.Add(this.dataGridViewTables);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(452, 356);
            this.Name = "ManageTables";
            this.Text = "Tables";
            this.Load += new System.EventHandler(this.ManageTables_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewTables)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewTables;
        public System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnAddTable;
        private System.Windows.Forms.Button btnEditTable;
        private System.Windows.Forms.Button btnRemoveTable;
    }
}