﻿using System;
using System.Windows.Forms;

namespace AppDevProject_BookingSystem
{
    /// <summary>
    /// Form for managing of the table settings, such as name, seats number and minimal seats number
    /// (c) Developed by Denis Klyucherov
    /// </summary>
    public partial class TableSettings : Form
    {
        // Declare this variable to use its method LoadTablesData() (button click) to reload data in the dataGridViewTables on ManageTables form
        ManageTables mngTable;
        Tables table;

        private string tableName;
        private byte numSeats;
        private byte minNumSeats;

        public TableSettings(ManageTables _mngTable)
        {
            InitializeComponent();

            this.mngTable = _mngTable;

            table = new Tables();
        }

        private void TableSettings_Load(object sender, EventArgs e)
        {
            Globals.tableSettingsChanged = false;

            GettingValuesForNumericsUpDown();
            
            // Checking which event triggered opening TableSettings Form. In this case - Edit table button
            if (Globals.TableSettingsClickEventFlag)
                LoadTableSettingsToEdit();
        }

        // Set up parameters for NumericUpDown controls
        private void GettingValuesForNumericsUpDown()
        {
            upDownNumSeats.Minimum = 1;
            upDownNumSeats.Maximum = table.maxTableNumSeats;

            upDownMinNumSeats.Minimum = 1;
            upDownMinNumSeats.Maximum = table.maxTableMinNumBookingSeats;
        }

        // Getting from ManageTables form chosen parameters: Table name, Seats number and Minimal seats number
        private void LoadTableSettingsToEdit()
        {
            tableName = Globals.listTableSelectedSettings[1]; // Getting Table name
            numSeats = Convert.ToByte(Globals.listTableSelectedSettings[2]); // Getting Seats number
            minNumSeats = Convert.ToByte(Globals.listTableSelectedSettings[3]); // Getting Minimal seats number

            txtTableName.Text = tableName;
            upDownNumSeats.Value = numSeats;
            upDownMinNumSeats.Value = minNumSeats;
        }

        // Passing parameters to the Tables class
        private void PassingNewTableParameters()
        {
            table.name = txtTableName.Text;
            table.numSeats = (byte)upDownNumSeats.Value;
            table.minNumBookingSeats = (byte)upDownMinNumSeats.Value;
        }

        // Adding a table
        private void AddTable()
        {
            // Passing parameters to the Tables class
            PassingNewTableParameters();

            // Checking if entered table parameters are correct (i.e., Table name has to be entered and numSeats >= minNumSeats)
            string checkTableInfo = table.CheckTableInfo();
            if (!String.IsNullOrEmpty(checkTableInfo))
            {
                MessageBox.Show(table.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool addResult = table.AddTable();
            if (!addResult)
            {
                MessageBox.Show(table.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            MessageBox.Show(table.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Globals.TableNameSelected = txtTableName.Text; // Passing table name to Globals in order to select it in ManageTables
            Globals.tableSettingsChanged = true; // If there were table changes, calling Retrieving in ConfigSystem() class

            // After successfull updating, loading tables data again
            mngTable.LoadTablesData();

            this.Close();
        }

        // Method to update table settings
        private void UpdateTableSettings()
        {
            // Passing parameters to the Tables class
            PassingNewTableParameters();

            // Checking if entered table parameters are correct (i.e., Table name has to be entered and numSeats >= minNumSeats)
            string checkTableInfo = table.CheckTableInfo();
            if (!String.IsNullOrEmpty(checkTableInfo))
            {
                MessageBox.Show(table.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                if (checkTableInfo == "wrongName") // If Table name is empty, passing back its old name
                    txtTableName.Text = tableName;
                txtTableName.Focus();
                txtTableName.SelectAll();
                return;
            }

            // Updating table info and checking what result has been returned
            bool updResult = table.UpdateTable(Convert.ToInt32(Globals.listTableSelectedSettings[0]));
            if (!updResult)
            {
                MessageBox.Show(table.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                txtTableName.Text = tableName;
                txtTableName.Focus();
                txtTableName.SelectAll();
                return;
            }

            MessageBox.Show(table.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Globals.TableNameSelected = txtTableName.Text; // Passing table name to Globals in order to select it in ManageTables
            Globals.tableSettingsChanged = true; // If there were table changes, calling Retrieving in ConfigSystem() class

            // After successfull updating, loading tables data again
            mngTable.LoadTablesData();

            this.Close();
        }

        // Press button OK
        private void Submit()
        {
            // Checking which event triggered opening TableSettings Form. In this case - Edit table button
            if (Globals.TableSettingsClickEventFlag)
            {
                // Checking if a user has changed table parameters. If not, do not update and closing the form
                if (tableName != txtTableName.Text || numSeats != upDownNumSeats.Value || minNumSeats != upDownMinNumSeats.Value)
                    UpdateTableSettings();
                else
                    this.Close();
            }
            else // If there was Add table button click
                AddTable();
        }

        // Ok button
        private void btnOk_Click(object sender, EventArgs e)
        {
            Submit();
        }

        // Selecting all after enter to the txtTableName
        private void txtTableName_Enter(object sender, EventArgs e)
        {
            txtTableName.SelectAll();
        }

        // Submit after a user pressed the Enter button in the txtTableName
        private void txtTableName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Submit();
        }

        // Submit after a user pressed the Enter button in the upDownNumSeats
        private void upDownNumSeats_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Submit();
        }

        // Submit after a user pressed the Enter button in the upDownMinNumSeats
        private void upDownMinNumSeats_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Submit();
        }
    }
}