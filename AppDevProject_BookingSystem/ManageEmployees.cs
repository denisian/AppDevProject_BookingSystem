using System;
using System.Data;
using System.Windows.Forms;

namespace AppDevProject_BookingSystem
{
    /// <summary>
    /// Form for managing empoloyees in the dataGridView control
    /// (c) Developed by Denis Klyucherov
    /// </summary>
    public partial class ManageEmployees : Form
    {
        Employees employee;

        private static DataTable _dtTable;
        public static DataTable DtTable { get { return _dtTable; } } // Share _dtTable with EmployeeSettings class

        private static int _employeeId;
        public static int EmployeeId { get { return _employeeId; } } // Share _employeeId with EmployeeSettings class

        // Use in EmployeeSettings.cs to know which event triggered opening EmployeeSettings Form (Add or Edit)
        // If it was clicking on the Add Employee button - open EmployeeSettings Form with default parameters
        // Otherwise, if it was Edit Employee button pressing - open EmployeeSettings Form with substitution Employee parameters
        private static bool _employeeSettingsClickEventFlag;
        public static bool EmployeeSettingsClickEventFlag { get { return _employeeSettingsClickEventFlag; } }

        private int selectedRowIndex; // Use to pass rowIndex in the EditEmployeeSettings() method

        DialogResult res;

        ContextMenuStrip contxtMenuEmployee;
        ToolStripMenuItem menuItemEditEmployee, menuItemRemoveEmployee;

        public ManageEmployees()
        {
            InitializeComponent();

            ContextMenuEmployee();

            employee = new Employees();
            _dtTable = new DataTable();
        }

        private void ManageEmployees_Load(object sender, EventArgs e)
        {
            LoadEmployeesData();
        }

        // Method is public because it is called from EmployeeSettings class
        public void LoadEmployeesData()
        {
            employee.ShowEmployees();
            _dtTable = employee.DataTable;

            // Choosing certain columns in employee.Datatable to show them in dataGridViewEmployees (we don't need to show password)
            if (_dtTable.Rows.Count > 0)
            {
                string[] selectedColumns = new string[] { "ID", "First name", "Last name", "Email", "Access Level" };
                DataTable dt = new DataView(_dtTable).ToTable(false, selectedColumns);
                dataGridViewEmployees.DataSource = dt;
            }
        }

        // Starting EmployeeSettings Form
        private void LoadEmployeeSettingsForm()
        {
            EmployeeSettings emplForm = new EmployeeSettings(this);
            emplForm.StartPosition = FormStartPosition.CenterScreen;
            emplForm.ShowDialog();
        }

        // Open TableSettings form after double-clicking
        private void dataGridViewUsers_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EditEmployeeSettings(e.RowIndex);
        }

        // Adding employee
        private void AddEmployee()
        {
            // Set the value of the variable to remeber which event triggered opening EmployeeSettings Form (in this case - Add)
            _employeeSettingsClickEventFlag = false;

            // Starting TableSettings Form
            LoadEmployeeSettingsForm();
        }

        // Add button
        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            AddEmployee();
        }

        // Getting Employee ID of the selected row
        private int GetEmployeeIDOfSelectedRow(int selectedRowIndex)
        {
            return Convert.ToInt32(dataGridViewEmployees["ID", selectedRowIndex].Value.ToString());
        }

        // Getting Employee name of the selected row
        private string GetEmployeeNameOfSelectedRow(int selectedRowIndex)
        {
            DataGridViewRow row = this.dataGridViewEmployees.Rows[selectedRowIndex];
            return row.Cells["First Name"].Value + " " + row.Cells["Last Name"].Value;
        }

        // Edit employee method
        private void EditEmployeeSettings(int selectedRowIndex)
        {
            // Set the value of the variable to remeber which event triggered opening EmployeeSettings Form (in this case - Edit)
            _employeeSettingsClickEventFlag = true;

            // Getting Employee ID of the selected row and pass it later into EmployeeSettings Form
            _employeeId = GetEmployeeIDOfSelectedRow(selectedRowIndex);

            // Starting Employee Settings Form
            LoadEmployeeSettingsForm();
        }

        // Edit button
        private void btnEditEmployee_Click(object sender, EventArgs e)
        {
            EditEmployeeSettings(selectedRowIndex);
        }

        // Saving row index after clicking on it
        private void dataGridViewEmployees_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                selectedRowIndex = e.RowIndex; // Use in btnEditEmployee_Click() event to pass rowIndex in EditEmployeeSettings()
        }

        // Removing employee
        private void RemoveEmployee()
        {
            _employeeId = GetEmployeeIDOfSelectedRow(selectedRowIndex);
            string employeeName = GetEmployeeNameOfSelectedRow(selectedRowIndex);

            res = MessageBox.Show(String.Format("Are you sure you want to remove '{0}' account?", employeeName), "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                employee.DeleteEmployee(_employeeId);
                MessageBox.Show(employee.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadEmployeesData();
            }
        }

        // Remove button
        private void btnRemoveEmployee_Click(object sender, EventArgs e)
        {
            RemoveEmployee();
        }

        // Create context menu
        private void ContextMenuEmployee()
        {
            contxtMenuEmployee = new ContextMenuStrip();
            menuItemEditEmployee = new ToolStripMenuItem();
            menuItemRemoveEmployee = new ToolStripMenuItem();

            menuItemEditEmployee.Text = "Edit employee";
            menuItemEditEmployee.Click += new EventHandler(menuItemEditEmployee_Click);
            contxtMenuEmployee.Items.Add(menuItemEditEmployee);

            menuItemRemoveEmployee.Text = "Remove employee";
            menuItemRemoveEmployee.Click += new EventHandler(menuItemRemoveEmployee_Click);
            contxtMenuEmployee.Items.Add(menuItemRemoveEmployee);
        }

        // Context menu "Edit employee"
        private void menuItemEditEmployee_Click(object sender, EventArgs e)
        {
            EditEmployeeSettings(selectedRowIndex);
        }

        // Context menu "Remove employee"
        private void menuItemRemoveEmployee_Click(object sender, EventArgs e)
        {
            RemoveEmployee();
        }

        // Show context menu
        private void dataGridViewEmployees_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                dataGridViewEmployees.ContextMenuStrip = contxtMenuEmployee;
        }
    }
}
