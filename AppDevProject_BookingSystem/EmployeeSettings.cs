using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AppDevProject_BookingSystem
{
    public partial class EmployeeSettings : Form
    {
        // Declare this variable to use its method LoadEmployeesData() (button click) to reload data in the dataGridViewEmployees on ManageEmployees form
        ManageEmployees mngEmployee;
        Employees employee;

        private DataTable dataTable;
        private string firstName;
        private string lastName;
        private string email;
        private string password;
        private int accessLevelIndex;
        private int employeeId;

        MailMessage mailMessage;
        EmailSettings emailSettings;

        private DialogResult res;

        public EmployeeSettings(ManageEmployees _mngEmployee)
        {
            InitializeComponent();

            this.mngEmployee = _mngEmployee;

            employee = new Employees();
            dataTable = new DataTable();
        }

        private void EmployeeSettings_Load(object sender, EventArgs e)
        {
            GetEmployeeAccessLevels();

            dataTable = ManageEmployees.DtTable; // Getting dataTable with all users from ManageEmployees

            // Checking which event triggered opening EmployeeSettings Form. In this case - Edit employee button
            if (ManageEmployees.EmployeeSettingsClickEventFlag)
                LoadEmployeeSettingsToEdit();
        }

        private void LoadEmployeeSettingsToEdit()
        {
            employeeId = ManageEmployees.EmployeeId;

           //Loading Employee Info from passed datatable using selected in ManageEmployees module values: Employee ID
            var detailedEmployeeInfo =
               (from table in dataTable.AsEnumerable()
                where table.Field<int>("ID") == employeeId
                select new { emplFirstName = table["First Name"], emplLastName = table["Last Name"], emplEmail = table["Email"], emplPassword = table["password"], emplAccessLevel = table["Access Level"] });

            foreach (var detail in detailedEmployeeInfo)
            {
                firstName = detail.emplFirstName.ToString();
                lastName = detail.emplLastName.ToString();
                email = detail.emplEmail.ToString();
                password = detail.emplPassword.ToString();
                accessLevelIndex = cmbAccessLevel.FindString(detail.emplAccessLevel.ToString());

                txtFirstName.Text = firstName;
                txtLastName.Text = lastName;
                txtEmail.Text = email;
                txtPassword.Text = password;
                cmbAccessLevel.SelectedIndex = accessLevelIndex;
            }
        }

        // Loading into cmbAccessLevel a list of access levels from teh database
        private void GetEmployeeAccessLevels()
        {
            employee.GetAccessLevels();
            cmbAccessLevel.DataSource = employee.DataTable;
            cmbAccessLevel.DisplayMember = "accessLevel";
        }

        // Checking if we need to add another Administrator if it already exists in the database
        private bool AddAnotherAdministratorIfExists()
        {
            try
            {
                string accessLevelToCheck = "Administrator";

                if (cmbAccessLevel.Text == accessLevelToCheck)
                {
                    var adminUserExisted =
                        (from table in dataTable.AsEnumerable()
                         where table.Field<string>("Access Level") == "Administrator"
                         select table.Field<int>("ID"));

                    if (!String.IsNullOrEmpty(adminUserExisted.First().ToString())) // If user has been found
                    {
                        res = MessageBox.Show(String.Format("User with Administrator access level already exists. Do you want to add another one?"), "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                        if (res == DialogResult.No)
                            return false;
                    }
                }
                return true;
            }
            catch
            {
                throw;
            }
        }

        // Passing parameters to the Employees class
        private void PassingNewEmployeeParameters()
        {
            employee.restaurantId = 1;
            employee.firstName = txtFirstName.Text;
            employee.lastName = txtLastName.Text;
            employee.email = txtEmail.Text;
            employee.password = txtPassword.Text;
            employee.accessLevel = cmbAccessLevel.Text;
        }

        // Adding employee
        private void AddEmployee()
        {
            // Passing parameters to the Employees class
            PassingNewEmployeeParameters();

            // Checking if entered employee parameters are correct
            string checkEmployeeInfo = employee.CheckEmployeeInfo();
            if (!String.IsNullOrEmpty(checkEmployeeInfo))
            {
                MessageBox.Show(employee.Message);
                return;
            }

            if (!AddAnotherAdministratorIfExists()) // If we don't want to add another administrator if it exists
                return;

            bool addResult = employee.AddEmployee();
            if (!addResult)
            {
                MessageBox.Show(employee.Message);
                txtEmail.Text = email;
                txtEmail.Focus();
                txtEmail.SelectAll();
                return;
            }

            res = MessageBox.Show(String.Format(employee.Message + "\nDo you want to send a confirmation email to '{0}'?", txtEmail.Text), "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (res == DialogResult.Yes)
                SendEmailConfirmation(); // Sending email

            // After successfull updating, loading employees data again
            mngEmployee.LoadEmployeesData();

            this.Close();
        }

        // Method to update employee settings
        private void UpdateEmployeeSettings()
        {
            // Passing parameters to the Employees class
            PassingNewEmployeeParameters();

            // Checking if entered employee parameters are correct
            string checkEmployeeInfo = employee.CheckEmployeeInfo();
            if (!String.IsNullOrEmpty(checkEmployeeInfo))
            {
                MessageBox.Show(employee.Message);

                if (checkEmployeeInfo == "wrongFirstName") // If first name is empty, passing back its old name
                {
                    txtFirstName.Text = firstName;
                    txtFirstName.Focus();
                    txtFirstName.SelectAll();
                }

                if (checkEmployeeInfo == "wrongLastName") // If last name is empty, passing back its old name
                    txtLastName.Text = lastName;

                if (checkEmployeeInfo == "wrongEmail") // If email is empty, passing back its old name
                    txtEmail.Text = email;
                return;
            }

            if (!AddAnotherAdministratorIfExists()) // If we don't want to add another administrator if it exists
                return;

            // Updating employee info and checking what result has been returned
            bool updResult = employee.UpdateEmployee(employeeId);
            if (!updResult)
            {
                MessageBox.Show(employee.Message);
                txtEmail.Text = email;
                txtEmail.Focus();
                txtEmail.SelectAll();
                return;
            }

            res = MessageBox.Show(String.Format(employee.Message + "\nDo you want to send a confirmation email to '{0}'?", txtEmail.Text), "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
            if (res == DialogResult.Yes)
                SendEmailConfirmation(); // Sending email

            // After successfull updating, loading employees data again
            mngEmployee.LoadEmployeesData();

            this.Close();
        }

        // Sending email after cretaing or updating employee
        private void SendEmailConfirmation()
        {
            // Seting up MailMessage
            mailMessage = new MailMessage();
            emailSettings = new EmailSettings();

            mailMessage.Subject = "Employee Registration";
            string body = "Dear " + txtFirstName.Text.Trim() + " " + txtLastName.Text.Trim() + ",";
            body += "<br /><br />You has been registered as employee in our restaurant with '" + cmbAccessLevel.Text + "' access level!";
            body += "<br /><br />Your login information:";
            body += "<br />Email: " + txtEmail.Text;
            body += "<br />Password: " + txtPassword.Text;
            body += "<br /><br />Best regards,";
            body += "<br />Restaurant Reservation System";
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            // Sending recipient's email and MailMessage settings
            emailSettings.SmtpClient(txtEmail.Text, mailMessage);

            MessageBox.Show(emailSettings.Message);
        }

        // Press OK button
        private void Submit()
        {
            //Checking which event triggered opening EmployeeSettings Form.In this case - Edit table button
            if (ManageEmployees.EmployeeSettingsClickEventFlag)
            {
                // Checking if a user has changed table parameters. If not, do not update and closing the form
                if (firstName != txtFirstName.Text || lastName != txtLastName.Text || email != txtEmail.Text || password != txtPassword.Text || accessLevelIndex != cmbAccessLevel.SelectedIndex)
                    UpdateEmployeeSettings();
                else
                    this.Close();
            }
            else // If there was Add table button click
                AddEmployee();
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            Submit();
        }

        private void txtFirstName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Submit();
        }

        private void txtLastName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Submit();
        }

        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Submit();
        }

        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Submit();
        }

        private void cmbAccessLevel_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                Submit();
        }
    }
}
