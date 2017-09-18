using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net.Mail;
using System.Windows.Forms;

namespace AppDevProject_BookingSystem
{
    /// <summary>
    /// Module for managing bookings and checking availability tables
    /// </summary>
    public partial class ManageBookings : Form
    {
        private DataTable dataTable;

        Singleton bookingSingleton;
        Customers customer;
        MailMessage mailMessage;
        EmailSettings emailSettings;

        // Using selected on the form variables to create/update a booking and in the MainBookingParametersChanged method to remember chosen value
        private DateTime bookingDateTimeSelected;
        private string bookingTimeSelected;
        private string bookingPartySizeSelected;
        private int bookingTableIdSelected;

        // Using to update already exisitng booking (passing from dataGridViewBooking)
        private DateTime bookingDateTimeToUpdate;
        private string bookingTimeToUpdate;
        private string bookingPartySizeToUpdate;
        private string bookingTableNameToUpdate;

        // If there were table changes, calling Retrieving in ConfigSystem() class

        private bool ifBtnCheckAvailabilityPressed = false;

        private DialogResult res;

        // Declare this variable to use its method ShowBooking() (button click) to reload data in the dataGridViewBooking on ConfigSystem form
        ConfigSystem confSystem;

        public ManageBookings(ConfigSystem _confSystem)
        {
            InitializeComponent();
            this.confSystem = _confSystem;

            //booking = new Bookings();
            bookingSingleton = Singleton.GetInstance();

            dataTable = new DataTable();
        }

        private void ManageBookings_Load(object sender, EventArgs e)
        {
            LoadBookingTimeSizeOccasion();

            // If there was an event clicking on dataGridVewBooking - substitute booking parameters
            if (Globals.ConfigSystemClickEventFlag)
            {
                btnBookTable.Text = "Update";
                //btnCheckAvailability.Enabled = false;
                grBoxCustomerInfo.Enabled = true;
                GettingBookingData();
            }
            else
            {
                btnBookTable.Text = "Book table";
                grBoxCustomerInfo.Enabled = false;
            }

            customer = new Customers();
        }

        private void LoadBookingTimeSizeOccasion()
        {
            // Preventing SelectedValueChanged events when DataSource is bound
            this.cmbTimeBooking.SelectedValueChanged -= new EventHandler(cmbTimeBooking_SelectedValueChanged);
            this.cmbSizeBooking.SelectedValueChanged -= new EventHandler(cmbSizeBooking_SelectedValueChanged);

            // Loading Booking Time from Bookings class and choosing current time
            // Getting parameters from the Booking Class using Singleton pattern
            List<string> timeList = bookingSingleton.booking.ListTimeBooking();
            cmbTimeBooking.DataSource = timeList;
            int timeStart = Convert.ToInt32(timeList.First().Substring(0, timeList.First().IndexOf(':'))); // Getting a minimum hour
            int timeEnd = Convert.ToInt32(timeList.Last().Substring(0, timeList.Last().IndexOf(':'))); // Getting a maximum hour
            int currentTime = DateTime.Now.Hour;
            if (currentTime >= timeStart && currentTime <= timeEnd)
                cmbTimeBooking.SelectedIndex = cmbTimeBooking.FindString(currentTime.ToString()); // Selecting current hour

            // Loading Booking Party Size from Bookings class
            cmbSizeBooking.DataSource = bookingSingleton.booking.ListPartySize();

            // Loading Booking Occasion from Bookings class
            cmbOccasion.DataSource = bookingSingleton.booking.OccasionList();

            // Adding SelectedValueChanged events back
            this.cmbTimeBooking.SelectedValueChanged += new EventHandler(cmbTimeBooking_SelectedValueChanged);
            this.cmbSizeBooking.SelectedValueChanged += new EventHandler(cmbSizeBooking_SelectedValueChanged);
        }

        // Getting from ConfigSystem form chosen parameters: Date, Time, PartySize and Table name
        private void InitDateTimeSIzeTableToUpdateBooking()
        {
            bookingDateTimeToUpdate = Convert.ToDateTime(bookingSingleton.listBookingSelectedDetails[0]); // Getting full date (date + time)
            bookingTimeToUpdate = bookingDateTimeToUpdate.ToString("HH:mm"); // Converting time to 24-hour format
            bookingPartySizeToUpdate = bookingSingleton.listBookingSelectedDetails[1]; // Getting PartySize
            bookingTableNameToUpdate = bookingSingleton.listBookingSelectedDetails[2]; // Getting TableName
        }

        // Getting detailed Booking and Customer information
        private void GettingBookingData()
        {
            try
            {
                // Using datatable from the Singleton pattern which has been passed from Config system module
                dataTable = bookingSingleton.booking.Datatable;

                // Calling the method to initialite Date, Time, PartySize and Table
                InitDateTimeSIzeTableToUpdateBooking();

                // Passing Booking Date
                datePickerBooking.Value = bookingDateTimeToUpdate.Date;

                // Selecting passed from datatable booking Time
                cmbTimeBooking.SelectedIndex = cmbTimeBooking.FindString(bookingTimeToUpdate);

                // Selecting passed from datatable booking PartySize
                cmbSizeBooking.SelectedIndex = cmbSizeBooking.FindString(bookingPartySizeToUpdate);

                // Passing Tables' name
                linkLblTable.Text = bookingTableNameToUpdate;

                // Loading Customer Info from passed datatable using selected in ConfigSystem module values: Booking Date and Table Name
                var detailedCustomerInfo =
                    (from table in dataTable.AsEnumerable()
                     where table.Field<DateTime>("Booking Date") == bookingDateTimeToUpdate && table.Field<string>("Table Name") == bookingTableNameToUpdate
                     select new { custTitle = table["Title"], custFirstName = table["First Name"], custLastName = table["Last Name"], custEmail = table["Email"], custPhone = table["Phone"], custOccasion = table["Occasion"], custNotes = table["Notes"] });

                foreach (var detail in detailedCustomerInfo)
                {
                    txtTitle.Text = detail.custTitle.ToString();
                    txtFirstName.Text = detail.custFirstName.ToString();
                    txtLastName.Text = detail.custLastName.ToString();
                    txtEmail.Text = detail.custEmail.ToString();
                    txtPhone.Text = detail.custPhone.ToString();
                    cmbOccasion.SelectedIndex = cmbOccasion.FindString(detail.custOccasion.ToString());
                    txtNotes.Text = detail.custNotes.ToString();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Getting selected values
        private void InitSelectedDateTimeSize()
        {
            bookingTimeSelected = cmbTimeBooking.SelectedValue.ToString();
            bookingDateTimeSelected = Convert.ToDateTime(datePickerBooking.Value.ToShortDateString() + " " + bookingTimeSelected);
            bookingPartySizeSelected = cmbSizeBooking.SelectedValue.ToString();
        }

        // Button Check Availability tables
        private void btnCheckAvailability_Click(object sender, EventArgs e)
        {
            // Getting values for bookingDateTime and bookingSize
            InitSelectedDateTimeSize();

            // Calling the method CheckingAvailability() to check if table is available for the bookingDate and partySize
            // OUTPUT DATA: idChosenTable, nameChosenTable, message
            var bookData = bookingSingleton.booking.CheckingAvailability(bookingDateTimeSelected.ToString(), bookingPartySizeSelected);

            if (bookData.Item1 != 0) // If method returns tableId
            {
                linkLblTable.Text = bookData.Item2.ToString(); // Show tableName in linkLblTable

                bookingTableIdSelected = bookData.Item1; // Getting tableID

                ifBtnCheckAvailabilityPressed = true; // Remember the state of the button. Using in btnBookTable_Click and MainBookingParametersChanged methods

                MainBookingParametersChanged(); // Change visibility of forms' controls
            }
            else
            {
                MessageBox.Show(bookData.Item3.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                linkLblTable.Text = "";
            }
        }

        // Initialisation customer info
        private void InitCustomerInfo()
        {
            customer.email = txtEmail.Text;
            customer.password = "";
            customer.accountActivated = false;
            customer.typeAccount = false; // Guest
            customer.title = txtTitle.Text;
            customer.firstName = txtFirstName.Text;
            customer.lastName = txtLastName.Text;
            customer.phone = txtPhone.Text;
        }

        // Initialisation booking info
        private void InitBookingInfo()
        {
            InitSelectedDateTimeSize();

            bookingSingleton.booking.restaurantId = 1;
            bookingSingleton.booking.idChosenTable = bookingTableIdSelected;
            bookingSingleton.booking.bookingDateTime = bookingDateTimeSelected;
            bookingSingleton.booking.partySize = bookingPartySizeSelected;
            bookingSingleton.booking.notes = txtNotes.Text;

            // Checking Occasion (can be empty value)
            if (cmbOccasion.SelectedIndex == -1)
                return;

            bookingSingleton.booking.occasion = cmbOccasion.SelectedValue.ToString();
        }

        // Sending email after cretaing or updating booking
        private void SendEmailConfirmation()
        {
            // Seting up MailMessage
            mailMessage = new MailMessage();
            emailSettings = new EmailSettings();

            mailMessage.Subject = "Booking Confirmation";
            string body = "Dear " + txtFirstName.Text.Trim() + ",";
            body += "<br /><br />Thank you for booking a table in our restaurant.";
            body += "<br /><br />Booking date: " + bookingDateTimeSelected.ToLongDateString();
            body += "<br />Booking time: " + bookingTimeSelected;
            body += "<br />Party Size: " + bookingPartySizeSelected;
            body += "<br />Occasion: " + cmbOccasion.SelectedValue.ToString();
            body += "<br />Notes: " + txtNotes.Text;
            body += "<br /><br />Best regards,";
            body += "<br />Restaurant Reservation System";
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            // Sending recipient's email and MailMessage settings
            emailSettings.SmtpClient(txtEmail.Text, mailMessage);

            MessageBox.Show(emailSettings.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Book/Update button
        private void btnBookTable_Click(object sender, EventArgs e)
        {
            try
            {
                InitCustomerInfo();
                InitBookingInfo();

                // Checking First Name, Email and Phone fields
                string checkCustomerInfo = customer.CheckCustomerInfo();
                if (!String.IsNullOrEmpty(checkCustomerInfo))
                {
                    MessageBox.Show(customer.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    if (checkCustomerInfo == "wrongName")
                    {
                        txtFirstName.Focus();
                        txtFirstName.SelectAll();
                    }

                    else if (checkCustomerInfo == "wrongPhone")
                    {
                        txtPhone.Focus();
                        txtPhone.SelectAll();
                    }

                    else if (checkCustomerInfo == "wrongEmail")
                    {
                        txtEmail.Focus();
                        txtEmail.SelectAll();
                    }

                    return;
                }

                // Checking Occasion field
                // If metod returned false, showing a message
                if (!bookingSingleton.booking.CheckBookingInfo())
                {
                    MessageBox.Show(bookingSingleton.booking.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // If there was an Update event (clicking on dataGridviewBooking)
                if (Globals.ConfigSystemClickEventFlag)
                {
                    // Loading Customer Info from passed datatable using selected in ConfigSystem module values: Booking Date and Table Name
                    var getCustIdBookIdTableId =
                        (from table in dataTable.AsEnumerable()
                         where table.Field<DateTime>("Booking Date") == bookingDateTimeToUpdate && table.Field<string>("Table Name") == bookingTableNameToUpdate
                         select new { customerId = table["Customer ID"], bookingId = table["Booking ID"], tableId = table["Table ID"] }).First();

                    int customerId = (Int32)getCustIdBookIdTableId.customerId;
                    int bookingId = (Int32)getCustIdBookIdTableId.bookingId;

                    // Checking if there was no pressing of CheckAvailability button and getting tableId from dataTable (otherwise, tableId = 0)
                    if (!ifBtnCheckAvailabilityPressed)
                        bookingSingleton.booking.idChosenTable = (Int32)getCustIdBookIdTableId.tableId;

                    // Updating booking
                    bool updResult = bookingSingleton.booking.UpdateBooking(bookingId);
                    if (!updResult)
                    {
                        MessageBox.Show(bookingSingleton.booking.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    // Updating customer
                    customer.UpdateCustomer(customerId);

                    string message = bookingSingleton.booking.Message;

                    if (String.IsNullOrEmpty(txtEmail.Text.Trim())) // If there is not an email, don't send confirmation email
                        MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    else
                    {
                        res = MessageBox.Show(String.Format(message + "\nDo you want to send a confirmation email to '{0}'?", txtEmail.Text), "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                        if (res == DialogResult.Yes)
                            SendEmailConfirmation(); // Sending email
                    }

                    this.Close();
                }
                else // If there was an New Booking event (clicking on the New Booking button)
                {
                    if (bookingTableIdSelected != 0) // If method returns tableId
                    {
                        // Add there are no problems, add a customer
                        if (!customer.AddCustomer())
                        {
                            MessageBox.Show(customer.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            return;
                        }

                        // Getting Customer ID to add Booking
                        bookingSingleton.booking.customerId = customer.CustomerId;

                        if (bookingSingleton.booking.AddBooking()) // If booking was successfull
                        {
                            string message = bookingSingleton.booking.Message;

                            if (String.IsNullOrEmpty(txtEmail.Text.Trim())) // If there is not an email, don't send confirmation email
                                MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            else
                            {
                                res = MessageBox.Show(String.Format(message + "\nDo you want to send a confirmation email to '{0}'?", txtEmail.Text), "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information);
                                if (res == DialogResult.Yes)
                                    SendEmailConfirmation(); // Sending email
                            }

                            this.Close();
                        }
                        else
                        {
                            customer.DeleteCustomer(); // If booking adding has been unsuccessfull, deleting customer
                            MessageBox.Show(bookingSingleton.booking.Message, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    else
                        MessageBox.Show("Table is unavailable. Please try to choose another date or time", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }

                // Calling the LoadBookingsData method (button click) of the ConfigSystem form to reload data
                string dateBooking = datePickerBooking.Value.Date.ToString("dd/MM/yyyy"); // Casting date format to the format in database
                confSystem.LoadBookingsData(dateBooking, cmbTimeBooking.SelectedItem.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void linkLblTable_Click(object sender, EventArgs e)
        {
            // Open form when table is displayed (when New Booking creates)
            if (!String.IsNullOrEmpty(linkLblTable.Text))
            {
                Globals.TableNameSelected = linkLblTable.Text;
                ManageTables tblForm = new ManageTables(this);
                tblForm.StartPosition = FormStartPosition.CenterScreen;
                tblForm.ShowDialog();
            }
        }

        // Method to update Table after choosing it in ManageTables form
        // Method is public because we use it in ManageTables.cs
        public void UpdatingTableForBooking()
        {
            Tables table = new Tables();
            DataTable dt = new DataTable();
       
            string tableName = Globals.TableNameSelected; // Getting a new table name from ManageTables.cs

            table.GetTableParameters(tableName);  // Getting tableID, numSeats and minNumBookingSeats
            dt = table.DataTable;

            if (dt.Rows.Count == 0) // Check if we've got rows using the table name
                return;

            int tableId = Convert.ToInt32(dt.Rows[0]["id"].ToString());
            int tableNumSeats = Convert.ToInt32(dt.Rows[0]["numSeats"].ToString());
            int tableMinNumBookingSeats = Convert.ToInt32(dt.Rows[0]["minNumBookingSeats"].ToString());

            // Check if chosen the same table - do not do anything
            if (tableName == linkLblTable.Text)
                return;

            res = MessageBox.Show(String.Format("You are about to change the table {0} by {1}.\nThis table has {2} seats and minimum {3} seats must be booked.\nAre you sure you want to change the table?", linkLblTable.Text, Globals.TableNameSelected, tableNumSeats, tableMinNumBookingSeats), "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                linkLblTable.Text = tableName;

                bookingTableIdSelected = tableId;

                // Getting values for bookingDateTime and bookingSize
                InitSelectedDateTimeSize();

                ifBtnCheckAvailabilityPressed = true; // Remember the state of the button. Using in btnBookTable_Click and MainBookingParametersChanged methods

                MainBookingParametersChanged(); // Change visibility of forms' controls
            }
        }

        // Method to control visibility of the forms' controls (depending on choosing default values for Date, Time or PartySize)
        private void formControlsStateChanging(bool state)
        {
            if (state)
            {
                //btnCheckAvailability.Enabled = false;
                grBoxCustomerInfo.Enabled = true;
            }
            else
            {
                //btnCheckAvailability.Enabled = true;
                grBoxCustomerInfo.Enabled = false;
            }
        }

        // If a user changed Date and/or Time and/or PartySize after clicking on dataGridVewBooking (to see/update a booking) buttons and groupbox of the ManageBookings form are getting disabled
        private void MainBookingParametersChanged()
        {
            DateTime bookingDateTime = datePickerBooking.Value.Date;
            string bookingTime = cmbTimeBooking.SelectedValue.ToString();
            string bookingPartySize = cmbSizeBooking.SelectedValue.ToString();

            // If there was an event of clicking on dataGridVewBooking - substitute booking parameters
            if (!ifBtnCheckAvailabilityPressed)
            {
                if (bookingDateTime == bookingDateTimeToUpdate.Date && bookingTime == bookingTimeToUpdate && bookingPartySize == bookingPartySizeToUpdate)
                    formControlsStateChanging(true);
                else
                    formControlsStateChanging(false);
            }
            else // Otherwise, if there wes an event of pressing the button "New Booking"
            {
                if (bookingDateTime == bookingDateTimeSelected.Date && bookingTime == bookingTimeSelected && bookingPartySize == bookingPartySizeSelected)
                    formControlsStateChanging(true);
                else
                    formControlsStateChanging(false);
            }
        }

        private void datePickerBooking_ValueChanged(object sender, EventArgs e)
        {
            MainBookingParametersChanged();
        }

        private void cmbTimeBooking_SelectedValueChanged(object sender, EventArgs e)
        {
            MainBookingParametersChanged();
        }

        private void cmbSizeBooking_SelectedValueChanged(object sender, EventArgs e)
        {
            MainBookingParametersChanged();
        }

        private void linkLblTable_TextChanged(object sender, EventArgs e)
        {
            if (linkLblTable.Text != "")
                MainBookingParametersChanged();
        }

        private void txtTitle_Enter(object sender, EventArgs e)
        {
            txtTitle.SelectAll();
        }

        private void txtFirstName_Enter(object sender, EventArgs e)
        {
            txtFirstName.SelectAll();
        }

        private void txtLastName_Enter(object sender, EventArgs e)
        {
            txtLastName.SelectAll();
        }

        private void txtPhone_Enter(object sender, EventArgs e)
        {
            txtPhone.SelectAll();
        }

        private void txtEmail_Enter(object sender, EventArgs e)
        {
            txtEmail.SelectAll();
        }

        private void txtNotes_Enter(object sender, EventArgs e)
        {
            txtNotes.SelectAll();
        }

        // Retrieving updated table data after changing table settings
        public void RetrievingTableMapFromDatabase()
        {
            if (Globals.tableSettingsChanged)
            {
                confSystem.RetrievingTableMapFromDatabase(1);
                confSystem.ShowBookings();
                Globals.tableSettingsChanged = false;
            }
        }

        private void ManageBookings_FormClosed(object sender, FormClosedEventArgs e)
        {
            RetrievingTableMapFromDatabase();
        }
    }

}
