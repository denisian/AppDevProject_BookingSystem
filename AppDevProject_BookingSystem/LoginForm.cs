﻿using System;
using System.IO;
using System.Windows.Forms;

namespace AppDevProject_BookingSystem
{
    /// <summary>
    /// Login Form
    /// After successfull login, redirect to ConfigSystem Form
    /// (c) Developed by Denis Klyucherov and Yevgeniy Stenyushkin
    /// </summary>
    public partial class LoginForm : Form
    {
        Authorisation authorisation;

        private static string _email;
        public static string Email { get {return _email; } } // Saving global value of the email

        private static string _accessLevel; 
        public static string AccessLevel { get { return _accessLevel; } } // Saving global value of the access level

        public LoginForm()
        {
            InitializeComponent();
        }

        private void LoginInSystem()
        {
            authorisation = new Authorisation();

            _accessLevel = authorisation.Login(txtEmail.Text, txtPassword.Text);

            if (_accessLevel != "") // If user has been found
            {
                _email = txtEmail.Text; // Saving email to get First and Last name in ConfigSystem Form

                ConfigSystem tblForm = new ConfigSystem();
                tblForm.StartPosition = FormStartPosition.CenterScreen;
                tblForm.Show();
                this.Hide();
            }
            else
                MessageBox.Show(authorisation.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        private void LoginForm_Load(object sender, EventArgs e)
        {
            GetDatabaseFile();
        }

        // Check if database file exist. If it doesn't, asking a user to choose it manually
        private void GetDatabaseFile()
        {
            string defaultConnection = Properties.Settings.Default.MyConnection;
            string defaultKeyWordStart = "AttachDbFilename=";
            string defaultKeyWordEnd = ";Integrated";
            int startPositionURL = defaultConnection.IndexOf(defaultKeyWordStart) + defaultKeyWordStart.Length;
            int lengthURL = defaultConnection.IndexOf(defaultKeyWordEnd) - startPositionURL;

            string defaultURL = defaultConnection.Substring(startPositionURL, lengthURL);

            if (!File.Exists(defaultURL))
            {
                DialogResult res;

                res = MessageBox.Show("The database file is not found! Do you want to choose it manually? ", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.Yes)
                {
                    OpenFileDialog fileDlg = new OpenFileDialog();

                    fileDlg.Filter = "MDF (*.mdf)|*.mdf;";

                    if (fileDlg.ShowDialog() == DialogResult.OK)
                    {
                        string newURL = fileDlg.FileName;
                        defaultConnection = defaultConnection.Replace(defaultURL, newURL);
                        Properties.Settings.Default.MyConnection = defaultConnection;
                        Properties.Settings.Default.Save();
                    }
                }
                else
                    Application.Exit();
            }
        }

        // Login button
        private void btnLogin_Click(object sender, EventArgs e)
        {
            LoginInSystem();
        }

        // Selecting all text in the txtEmail after entering to the control
        private void txtEmail_Enter(object sender, EventArgs e)
        {
            txtEmail.SelectAll();
        }

        // Selecting all text in the txtPassword after entering to the control
        private void txtPassword_Enter(object sender, EventArgs e)
        {
            txtPassword.SelectAll();
        }

        // Submit after a user pressed the Enter button in the txtEmail
        private void txtEmail_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                LoginInSystem();
        }

        // Submit after a user pressed the Enter button in the txtPassword
        private void txtPassword_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                LoginInSystem();
        }

        // Stop application processes after form is closed
        private void LoginForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}