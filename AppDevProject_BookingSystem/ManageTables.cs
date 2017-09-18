using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace AppDevProject_BookingSystem
{
    /// <summary>
    /// Form for managing tables in the dataGridView control
    /// (c) Developed by Denis Klyucherov
    /// </summary>
    public partial class ManageTables : Form
    {
        Tables table;
        private string tableName;
        private int selectedRowIndex; // Using to pass rowIndex in the EditTableSettings() method
        private int tableId; // Using in RemoveTable to find out the ID of a table

        // Declare this variable to use its method ShowBooking() (button click) to reload data in the dataGridViewBooking on ConfigSystem form
        ManageBookings mngBookings;

        DialogResult res;

        ContextMenuStrip contxtMenuTable;
        ToolStripMenuItem menuItemEditTable, menuItemRemoveTable;

        public ManageTables(ManageBookings _mngBookings)
        {
            InitializeComponent();
            this.mngBookings = _mngBookings;

            ContextMenuTables();

            table = new Tables();
        }

        private void ManageTables_Load(object sender, EventArgs e)
        {
            SettingAccessLevel();
            LoadTablesData();
        }

        // Load tables into dataGridViewTables and selecting the row which has been passed from ManageBookings.cs (using Globals.TableNameSelected)
        // Method is public because it is called from TableSettings class
        public void LoadTablesData()
        {
            tableName = Globals.TableNameSelected;

            table.ShowTables();
            dataGridViewTables.DataSource = table.DataTable;

            int rowNumber = 0;

            foreach (DataGridViewColumn col in dataGridViewTables.Columns)
            {
                if (col.Name == "Table name")
                    foreach (DataGridViewRow row in dataGridViewTables.Rows)
                        if (row.Cells[col.Index].Value.ToString() == tableName)
                            rowNumber = row.Index;
            }

            if (dataGridViewTables.Rows.Count > 0)
            {
                dataGridViewTables.Rows[rowNumber].Selected = true;
                dataGridViewTables_CellClick(dataGridViewTables, new DataGridViewCellEventArgs(0, rowNumber)); // Getting an id of the selected row
            }
        }

        // Setting accessibility of form' controls depending on an access level
        private void SettingAccessLevel()
        {
            if (LoginForm.AccessLevel != "Administrator")
            {
                btnAddTable.Enabled = false;
                btnEditTable.Enabled = false;
                btnRemoveTable.Enabled = false;
                this.dataGridViewTables.CellDoubleClick -= new DataGridViewCellEventHandler(dataGridViewTables_CellDoubleClick);
            }
        }

        // Passing the new tableName in Globals.TableNameSelected after a user selects the row
        private void dataGridViewTables_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = this.dataGridViewTables.Rows[e.RowIndex];
                tableName = row.Cells["Table name"].Value.ToString();

                selectedRowIndex = e.RowIndex; // Use in btnEditTable_Click() event to pass rowIndex in EditTableSettings()
                tableId = (Int32)row.Cells["Table ID"].Value; // Use in RemoveTable to find out the ID of a table
            }
        }

        // Starting TableSettings Form
        private void LoadTableSettingsForm()
        {
            TableSettings tblForm = new TableSettings(this);
            tblForm.StartPosition = FormStartPosition.CenterScreen;
            tblForm.ShowDialog();
        }

        // Adding table
        private void AddTable()
        {
            // Set the value of the global variable to remeber which event triggered opening TableSettings Form (in this case - Add)
            Globals.TableSettingsClickEventFlag = false;

            // Starting TableSettings Form
            LoadTableSettingsForm();
        }

        // Add button
        private void btnAddTable_Click(object sender, EventArgs e)
        {
            AddTable();
        }

        // Method to edit table settings
        private void EditTableSettings(int selectedRowIndex)
        {
            // Set the value of the global variable to remeber which event triggered opening TableSettings Form (in this case - Edit)
            Globals.TableSettingsClickEventFlag = true;

            // Getting values of the selected row and adding them into list to pass them later into TableSettings Form
            List<string> listTableSelectedSettings = new List<string>();
            foreach (DataGridViewColumn col in dataGridViewTables.Columns)
                listTableSelectedSettings.Add(dataGridViewTables[col.Index, selectedRowIndex].Value.ToString());

            // Passing list of Table settings to Singleton class to use in TableSettings Form
            Globals.listTableSelectedSettings = listTableSelectedSettings;

            // Starting TableSettings Form
            LoadTableSettingsForm();
        }

        // Show TableSettings form after double-clicking
        private void dataGridViewTables_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EditTableSettings(e.RowIndex);
        }

        // Edit table button
        private void btnEditTable_Click(object sender, EventArgs e)
        {
            EditTableSettings(selectedRowIndex);
        }

        // Removing table
        private void RemoveTable()
        {
            res = MessageBox.Show(String.Format("Are you sure you want to remove the '{0}' table?", tableName), "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                table.DeleteTable(tableId);
                MessageBox.Show(table.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadTablesData();

                Globals.tableSettingsChanged = true; // If there were table changes, calling Retrieving in ConfigSystem() class
            }
        }

        // Remove table button
        private void btnRemoveTable_Click(object sender, EventArgs e)
        {
            RemoveTable();
        }

        // Ok button
        private void btnOk_Click(object sender, EventArgs e)
        {
            Globals.TableNameSelected = tableName;
            this.Close();

            // Calling the UpdatingTableNameBooking method of the ManageBookings.cs form to reload table
            mngBookings.UpdatingTableForBooking();
        }

        // Create context menu
        private void ContextMenuTables()
        {
            contxtMenuTable = new ContextMenuStrip();
            menuItemEditTable = new ToolStripMenuItem();
            menuItemRemoveTable = new ToolStripMenuItem();

            menuItemEditTable.Text = "Edit table";
            menuItemEditTable.Click += new EventHandler(menuItemEditTable_Click);
            contxtMenuTable.Items.Add(menuItemEditTable);

            menuItemRemoveTable.Text = "Remove table";
            menuItemRemoveTable.Click += new EventHandler(menuItemRemoveTable_Click);
            contxtMenuTable.Items.Add(menuItemRemoveTable);
        }

        // Context menu "Edit table"
        private void menuItemEditTable_Click(object sender, EventArgs e)
        {
            EditTableSettings(selectedRowIndex);
        }

        // Context menu "Remove table"
        private void menuItemRemoveTable_Click(object sender, EventArgs e)
        {
            RemoveTable();
        }

        // Show context menu
        private void dataGridViewTables_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                dataGridViewTables.ContextMenuStrip = contxtMenuTable;
        }

        // Retrieving updated table data after changing table settings
        private void ManageTables_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (Globals.tableSettingsChanged)
                mngBookings.RetrievingTableMapFromDatabase();
        }
    }
}
