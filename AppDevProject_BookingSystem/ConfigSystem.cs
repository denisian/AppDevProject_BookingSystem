using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace AppDevProject_BookingSystem
{
    /// <summary>
    /// Configuration and the most important module of the system
    /// Module to configure a table map, allocate tables on it, load and manage bookings
    /// (c) Developed by Denis Klyucherov
    /// </summary>
    public partial class ConfigSystem : Form
    {
        private OpenFileDialog dlgTablesMap;

        private int dimPicBoxTableFull; // Dimension of the picBoxTableAdded array (full number of tables in the "Tables" table)
        private int dimPicBoxTableOnMap; //Dimension of the picBoxTableOnMap array (number of tables allocated on the map)

        private int indexRemovedTable; // Use to pass removed Table index from picBox_MouseClick to RemovePictureBox() (from context menu)
        private int idRemovedTable; // Use to pass removed Table id from picBox_MouseClick to RemovePictureBox() (from context menu)

        private int selectedRowIndex; // Use to pass rowIndex in the EditBooking() method

        private List<int> listPixBoxTablesIdFromDb; // Keep saved in the database tables Id in order to add new ones on the map without undublicated
        private SortedList listPixBoxTablesIdFree; // Difference between dtPicBoxTables (Full list of tables) and listPixBoxTablesIdFromDb (saved tables) - free tables id
        private SortedList listPicBoxRemovedTables; // List of removed tables

        private Bitmap MyImage;
        private PictureBox[] picBoxTableAdded;
        private Point location = Point.Empty;

        private ContextMenuStrip contxtMenuTables, contxtMenuBookings;
        private ToolStripMenuItem menuItemRemoveTable, menuItemEditBooking, menuItemRemoveBooking;

        private DataTable dtBookings;
        private DataTable dtPicBoxTables;

        StoringMapSettings mapSettings;
        Employees employee;
        Singleton bookingSingleton;

        private DateTime bookingDateTime; // Using in RemoveTable to find out the booking ID

        private static byte restaurantId = 1; // Set restaurant ID
        private static int colorTransparency = 120; // Set background color transparency for picture boxes
        private static Color colorBackground = Color.Gray; // Set background color for picture boxes

        DialogResult res;

        public ConfigSystem()
        {
            InitializeComponent();

            GetLoggedInEmployeeName();

            mapSettings = new StoringMapSettings();
            listPixBoxTablesIdFromDb = new List<int>();
            listPixBoxTablesIdFree = new SortedList();
            listPicBoxRemovedTables = new SortedList();

            RetrievingTableMapFromDatabase(restaurantId);

            // Create an instance of Booking Class using Singleton pattern (to use the same instance between ConfigSystem and ManageBookings classes)
            // It is necessary because I need to use the same data of Datatable in Booking Class between classes
            bookingSingleton = Singleton.GetInstance();

            // Getting time list bookings
            LoadListTimeBookings();

            dtBookings = new DataTable();
            dlgTablesMap = new OpenFileDialog();

            ContextMenuConfiguration();

            SettingAccessLevel();
        }

        private void ConfigSystem_Load(object sender, EventArgs e)
        {
            ShowBookings();
        }

        // Get First and Last name of logged-in employee and show them in the txtLoggedInUser control
        private void GetLoggedInEmployeeName()
        {
            if (!string.IsNullOrEmpty(LoginForm.Email)) // If user has been found
            {
                employee = new Employees();
                employee.GetEmployeeFirstLastNameByEmail(LoginForm.Email); // Sending users' email
                DataTable dt = new DataTable();
                dt = employee.DataTable;

                lblLoggedInUser.Text = dt.Rows[0]["firstName"].ToString() + " " + dt.Rows[0]["lastName"].ToString();
            }
        }

        // Setting accessibility of form' controls depending on an access level
        private void SettingAccessLevel()
        {
            if (LoginForm.AccessLevel == "Manager")
                btnManageUsers.Enabled = false;
            else if (LoginForm.AccessLevel == "Employee")
            {
                this.MaximizeBox = false;
                this.MinimizeBox = false;
                this.FormBorderStyle = FormBorderStyle.FixedSingle;

                int indPicBox = 0;
                foreach (PictureBox picBox in pnlTables.Controls)
                {
                    picBox.MouseDown -= new MouseEventHandler(picBox_MouseDown);
                    picBox.MouseMove -= new MouseEventHandler((s, e) => picBox_MouseMove(s, e, indPicBox));
                    picBox.MouseUp -= new MouseEventHandler(picBox_MouseUp);
                    menuItemRemoveTable.Dispose();
                    indPicBox++;
                }

                btnOpenTableMap.Enabled = false;
                btnAddTable.Enabled = false;
                btnReloadMap.Enabled = false;
                btnSaveTableMap.Enabled = false;
                btnManageUsers.Enabled = false;
            }
        }

        #region START IMAGE (TABLE MAP) SECTION
        private void ShowMyImage(String fileToDisplay)
        {
            // Sets up an image object to be displayed.
            if (MyImage != null)
            {
                MyImage.Dispose();
            }

            // Stretches the image to fit the pictureBox.
            pnlTables.BackgroundImageLayout = ImageLayout.Stretch;
            MyImage = new Bitmap(fileToDisplay);
            pnlTables.BackgroundImage = (Image)MyImage;
        }

        // Open map button
        private void OpenImage_Click(object sender, EventArgs e)
        {
            dlgTablesMap.Filter = "Image files (*.jpg,*.jpeg,*.png,*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|JPG (*.jpg,*.jpeg)|*.jpg;*.jpeg|PNG (*.png)|*.png|BMP (*.bmp)|*.bmp";
            dlgTablesMap.FilterIndex = 1;

            if (dlgTablesMap.ShowDialog() == DialogResult.OK)
            {
                string fileName = dlgTablesMap.FileName;
                ShowMyImage(fileName);
                btnSaveTableMap.Enabled = true;
            }
        }

        // Converting an image file to binary to save it in the database
        private byte[] ConvertImageToBinary()
        {
            if (pnlTables.BackgroundImage != null)
            {
                MemoryStream memStream = new MemoryStream();
                pnlTables.BackgroundImage.Save(memStream, ImageFormat.Jpeg);
                byte[] imageByte = memStream.ToArray();

                return imageByte;
            }
            else
                return new byte[0];
        }

        // Getting form size, table id and table location to save them in the database
        private string GetTableMapParameters()
        {
            string picTableParameters = "0;" + this.Size.Width + ";" + this.Size.Height + "@"; // Getting ConfigSystem Form size (add "0" to remember that it's a form parameter)

            foreach (PictureBox picbox in pnlTables.Controls)
                picTableParameters += picbox.Name + ";" + picbox.Location.X + ";" + picbox.Location.Y + "@"; // Getting table ID (I put it in the picbox.Name) and location of each table

            if (picTableParameters != "")
            {
                picTableParameters += "@";
                picTableParameters = picTableParameters.Replace("@@", "");
            }
            return picTableParameters;
        }

        // Saving form parameters in the database
        private void SavingTableMapToDatabase()
        {
            byte[] mapImage = ConvertImageToBinary();
            string mapConfig = GetTableMapParameters();

            if (mapSettings.SavingTableMap(restaurantId, mapImage, mapConfig)) // If saving was successful
                MessageBox.Show(mapSettings.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Getting Table names by their ID when retrieving table data from database (using in CreatePictureBox to create new tables on a map using tableId from the database)
        private string GetTableNameById(int tableId)
        {
            try
            {
                var getTableName =
                    (from table in dtPicBoxTables.AsEnumerable()
                     where table.Field<int>("Table ID") == tableId
                     select table.Field<string>("Table Name"));

                return getTableName.First();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return "";
            }
        }

        // Getting the number of tables, defining dimension and creating of the picBoxTableAdded array (full number of tables in the "Tables" table)
        private void GetTablesData()
        {
            Tables table = new Tables();
            table.ShowTables();
            dtPicBoxTables = table.DataTable;

            dimPicBoxTableFull = dtPicBoxTables.Rows.Count;
            picBoxTableAdded = new PictureBox[dimPicBoxTableFull];
        }

        // Retrieving form size, table id and table location from the database
        // Making this method private to use it in the ManageBookings and ManageTables class to reload the map after updating tables
        public void RetrievingTableMapFromDatabase(byte restaurantId)
        {
            GetTablesData();

            if (mapSettings.RetrievingTableMap(restaurantId)) // If retrieving was successful
            {
                // Delete tables which have not been saved
                pnlTables.Controls.Clear();
                foreach (PictureBox picbox in pnlTables.Controls)
                    picbox.Dispose();

                // Getting table map
                if (mapSettings.TableMapImage != null)
                {
                    pnlTables.BackgroundImage = Image.FromStream(new MemoryStream(mapSettings.TableMapImage));
                    pnlTables.BackgroundImageLayout = ImageLayout.Stretch; // Stretches the image to fit the pictureBox.
                }

                listPixBoxTablesIdFromDb.Clear();
                listPicBoxRemovedTables.Clear();

                // Getting table parameters
                if (!String.IsNullOrEmpty(mapSettings.TableMapConfig))
                {
                    string[] picBoxTableOnMap = mapSettings.TableMapConfig.Split('@');
                    dimPicBoxTableOnMap = picBoxTableOnMap.Length - 1; // the number tables saved in the database (-1 because we save also form size)

                    int tableIndex = 0;
                    foreach (string mapParameter in picBoxTableOnMap) // For each table we are getting its parameters (id, locationX and locationY)
                    {
                        int[] parameter = mapParameter.Split(';').Select(int.Parse).ToArray(); // Parse string array to int array

                        if (parameter[0] == 0) // If a parameter of the first row is 0 (form size), getting value ans skip one iteration
                        {
                            this.Size = new Size(parameter[1], parameter[2]); // Getting form size
                            continue;
                        }

                        int tableId = parameter[0];
                        listPixBoxTablesIdFromDb.Add(tableId); // use in AddPictureBox()
                        int tableLocationX = parameter[1];
                        int tableLocationY = parameter[2];
                        CreatePictureBox(false, tableIndex, tableId, tableLocationX, tableLocationY); // Creating new pictureBoxes on the pnlTables using table parameters from the database
                        tableIndex++;
                    }
                }
                GettingFreeTablesIdToAdd(dimPicBoxTableOnMap); // Getting list of free tables id, which are not in the database (not in the listPixBoxTablesIdFromDb) and passing there index to start

                DisableAddTableBtn();
            }
            else
                MessageBox.Show(mapSettings.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Getting sorted list of free tables id, which are not saved in the database (not in the listPixBoxTablesIdFromDb).
        private void GettingFreeTablesIdToAdd(int numFreeTables)
        {
            listPixBoxTablesIdFree.Clear();

            int listKey = numFreeTables; // Starting key from dimPicBoxTableOnMap because we need to continue tables indexes in the CreatePictureBox(). Fisstly, we generating tables from the database, secondly - from listPixBoxTablesIdFree and thirdly - from listRemovedTables
            foreach (DataRow dr in dtPicBoxTables.Rows)
            {
                bool flag = true;
                foreach (int tableIdDb in listPixBoxTablesIdFromDb)
                    if ((int)dr["Table ID"] == tableIdDb)
                    {
                        flag = false;
                        continue;
                    }
                if (flag)
                {
                    listPixBoxTablesIdFree.Add(listKey, (int)dr["Table ID"]);
                    listKey++;
                }
            }
        }

        //Creating Picture Boxes depending on source (generating tables from the database or new ones)
        // Source: If it's true - create picture boxes (tables) from the Tables table, false - from the saved in the database parameters (RetrievingTableMapFromDatabase)
        private void CreatePictureBox(bool source,  int index, int tableId, int tableLocationX, int tableLocationY)
        {
            picBoxTableAdded[index] = new PictureBox();

            string tableName = GetTableNameById(tableId);
            if (source) // Getting tableId and tableName from dtPicBoxTables, but not from the database (in case generating new tables on a map)
            {
                tableLocationX = (pnlTables.Width - picBoxTableAdded[index].Width) / 2;
                tableLocationY = (pnlTables.Height - picBoxTableAdded[index].Height) / 2;
            }

            picBoxTableAdded[index].Name = tableId.ToString();
            picBoxTableAdded[index].Size = new Size(70, 40);
            // Create PictureBoxes in the middle of pnlTale
            picBoxTableAdded[index].Location = new Point(tableLocationX, tableLocationY);
            picBoxTableAdded[index].BorderStyle = BorderStyle.FixedSingle;
            picBoxTableAdded[index].Anchor = AnchorStyles.None;
            picBoxTableAdded[index].Cursor = Cursors.Hand;

            picBoxTableAdded[index].BackColor = Color.FromArgb(colorTransparency, colorBackground);

            picBoxTableAdded[index].MouseDown += new MouseEventHandler(picBox_MouseDown);
            picBoxTableAdded[index].MouseMove += new MouseEventHandler((s, e) => picBox_MouseMove(s, e, index));
            picBoxTableAdded[index].MouseUp += new MouseEventHandler(picBox_MouseUp);
            picBoxTableAdded[index].MouseClick += new MouseEventHandler((s, e) => picBox_MouseClick(s, e, index));
            picBoxTableAdded[index].MouseDoubleClick += new MouseEventHandler((s, e) => picBox_MouseDoubleClick(s, e, index));
            picBoxTableAdded[index].Paint += new PaintEventHandler((s, e) => picBox_Paint(s, e, index, tableName));

            pnlTables.Controls.Add(picBoxTableAdded[index]);
        }

        // Adding tables (Picture Box controls) on the map
        private void AddPicBoxTable()
        {
            int tableIndex;
            int tableId;

            if (listPixBoxTablesIdFree.Count > 0) // If there are free tables (full number of tables minus number tables on the map)
            {
                tableIndex = (int)listPixBoxTablesIdFree.GetKey(0); // Getting table index
                tableId = (int)listPixBoxTablesIdFree.GetByIndex(0); // Getting table ID

                CreatePictureBox(true, tableIndex, tableId, 0, 0); // Create a new table
                listPixBoxTablesIdFree.Remove(tableIndex); // Remove it from the list
            }
            else if (listPicBoxRemovedTables.Count > 0) // If there is at least one removed table
            {
                tableIndex = (int)listPicBoxRemovedTables.GetKey(0);
                tableId = (int)listPicBoxRemovedTables.GetByIndex(0);

                CreatePictureBox(true, tableIndex, tableId, 0, 0);
                listPicBoxRemovedTables.Remove(tableIndex);
            }

            DisableAddTableBtn();
        }

        // Disabling Add table button after adding of the whole tables from tha database
        private void DisableAddTableBtn()
        {
            // If there are no tables in the listPixBoxTablesIdFree and listRemovedTables lists, making btnAddTable disable
            if (listPixBoxTablesIdFree.Count == 0 && listPicBoxRemovedTables.Count == 0)
                btnAddTable.Enabled = false;
            else
                btnAddTable.Enabled = true;
        }

        // Button "Add table"
        private void btnAddTable_Click(object sender, EventArgs e)
        {
            AddPicBoxTable();
        }

        // Removing tables from the map
        private void RemovePicBoxTable()
        {
            listPicBoxRemovedTables.Add(indexRemovedTable, idRemovedTable); // Saving in the listRemovedTables index and ID of removed tables

            pnlTables.Controls.Remove(picBoxTableAdded[indexRemovedTable]); // Getting indexPicBoxTable from picBox_MouseClick and removing the table

            if (!btnAddTable.Enabled)
                btnAddTable.Enabled = true;
        }

        //Moving PictureBoxes with preventing moving out of the pnlTables
        private void picBox_MouseMove(object sender, MouseEventArgs e, int indPicBox)
        {
            if (location != Point.Empty)
            {
                Point newlocation = this.picBoxTableAdded[indPicBox].Location;
                newlocation.X = Math.Min(Math.Max(picBoxTableAdded[indPicBox].Left + (e.X - location.X), 0), picBoxTableAdded[indPicBox].Parent.Width - picBoxTableAdded[indPicBox].Width);
                newlocation.Y = Math.Min(Math.Max(picBoxTableAdded[indPicBox].Top + (e.Y - location.Y), 0), picBoxTableAdded[indPicBox].Parent.Height - picBoxTableAdded[indPicBox].Height);
                picBoxTableAdded[indPicBox].Location = newlocation;
            }
        }

        private void picBox_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                location = new Point(e.X, e.Y);
            }
        }

        private void picBox_MouseUp(object sender, MouseEventArgs e)
        {
            location = Point.Empty;
        }

        // Mouse clicking event on a table. Saving the table id to remove
        private void picBox_MouseClick(object sender, MouseEventArgs e, int indPicBox)
        {
            if (e.Button == MouseButtons.Right)
            {
                picBoxTableAdded[indPicBox].ContextMenuStrip = contxtMenuTables;
                indexRemovedTable = indPicBox; // Save removed table index to pass it to RemovePictureBox()
                idRemovedTable = Convert.ToInt32(picBoxTableAdded[indPicBox].Name); // Save removed table ID to pass it to RemovePictureBox()
            }
        }

        // Double-clicking on a pictureBox table loading the New booking form
        private void picBox_MouseDoubleClick(object sender, MouseEventArgs e, int indPicBox)
        {
            try
            {
                string tableName = GetTableNameById(Convert.ToInt32(picBoxTableAdded[indPicBox].Name));

                // If there are no data in dtBookings (no bookings at this date/time), making New booking
                if (!SameBookingDateTime())
                {
                    // Set global variable to false and open Booking Form without substitution of booking parameters
                    Globals.ConfigSystemClickEventFlag = false;
                    ShowBookingForm();
                    return;
                }

                string dateTime = dtBookings.Rows[0]["Booking Date"].ToString();
                DateTime fullBookingDate = Convert.ToDateTime(dateTime);

                var getPartySize =
                    (from table in dtBookings.AsEnumerable()
                     where table.Field<DateTime>("Booking Date") == fullBookingDate && table.Field<string>("Table Name") == tableName
                     select table.Field<byte>("Party Size")).FirstOrDefault();

                if (getPartySize == 0) // If query doesn't return data
                {
                    // Set global variable to false and open Booking Form without substitution of booking parameters
                    Globals.ConfigSystemClickEventFlag = false;
                    ShowBookingForm();
                    return;
                }

                // Getting values of the selected row and adding them into list to pass them later into Booking Form
                List<string> listBookingDetails = new List<string>();
                listBookingDetails.Add(dateTime);
                listBookingDetails.Add(getPartySize.ToString());
                listBookingDetails.Add(tableName);

                PassingBookingParamToEdit(listBookingDetails); // Calling PassingBookingParamToEdit method and passing there booking parameters
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Drawing the picture box table
        private void picBox_Paint(object sender, PaintEventArgs e, int indPicBox, string tableName)
        {
            Font myFont = new Font("Arial", 12, FontStyle.Bold);

            SizeF textSize = e.Graphics.MeasureString(tableName, myFont);
            PointF locationToDraw = new PointF();

            locationToDraw.X = ((picBoxTableAdded[indPicBox].Width - textSize.Width) / 2);
            locationToDraw.Y = ((picBoxTableAdded[indPicBox].Height - textSize.Height) / 2);

            e.Graphics.DrawString(tableName, myFont, Brushes.Black, locationToDraw);

            if (picBoxTableAdded[indPicBox].Tag == null)
                picBoxTableAdded[indPicBox].Tag = Color.Blue; //Sets a default color

            ControlPaint.DrawBorder(e.Graphics, picBoxTableAdded[indPicBox].ClientRectangle, (Color)picBoxTableAdded[indPicBox].Tag, ButtonBorderStyle.Solid);
        }

        // Save map button
        private void btnSaveTableMap_Click(object sender, EventArgs e)
        {
            SavingTableMapToDatabase();
            ShowBookings();
        }

        // Colouring the picture box depending on if table is free or not
        private void ColorFillBusyTables(bool makeFill)
        {
            try
            {
                if (makeFill)
                {
                    var getTableIds =
                        (from table in dtBookings.AsEnumerable()
                         select table.Field<int>("Table ID"));

                    foreach (PictureBox picBox in pnlTables.Controls)
                    {
                        picBox.BackColor = Color.FromArgb(colorTransparency, Color.Green);
                        foreach (int tableId in getTableIds)
                            if (Convert.ToInt32(picBox.Name) == tableId)
                            {
                                picBox.BackColor = Color.FromArgb(colorTransparency, Color.Red);
                                continue;
                            }
                    }
                }
                else
                    foreach (PictureBox picBox in pnlTables.Controls)
                        picBox.BackColor = Color.FromArgb(colorTransparency, colorBackground);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        // Loading booking data from table Bookings
        // Method is public because we use it in ManageBookings.cs to reload data after adding or updating a booking
        public void LoadBookingsData(string dateBooking, string timeBooking)
        {
            if (dateBooking != "")
                datePickerBooking.Value = Convert.ToDateTime(dateBooking);
            if (cmbTimeBooking.Text != "")
                cmbTimeBooking.SelectedIndex = cmbTimeBooking.FindString(timeBooking);

            dataGridViewBooking.DataSource = null;
            // Passing parameters into instance of Booking Class using Singleton pattern
            bookingSingleton.booking.LoadBookingInfo(dateBooking, timeBooking);
            dtBookings = bookingSingleton.booking.Datatable;

            // Choosing certain columns in booking.Datatable to show them in dataGridViewBooking (we don't need to show bookingId and customerId)
            if (dtBookings.Rows.Count > 0)
            {
                string[] selectedColumns = new string[] { "Booking Date", "Party Size", "Table Name" };
                DataTable dt = new DataView(dtBookings).ToTable(false, selectedColumns);
                dataGridViewBooking.DataSource = dt;

                DataGridViewColumn column = dataGridViewBooking.Columns[0];
                column.Width = 130;
            }

            if ((dateBooking != "" && cmbTimeBooking.Text != "Any") || SameBookingDateTime()) // If there is a chosen data and time OR all bookings at the same date+time, fill picture boxes
                ColorFillBusyTables(true);
            else
                ColorFillBusyTables(false);
        }

        // Checking if all bookings in the datatable have the same date and time
        private bool SameBookingDateTime()
        {
            bool sameDateTime = true;
            if (dtBookings.Rows.Count == 0)
                sameDateTime = false;
            else
            {
                string dtRow = dtBookings.Rows[0]["Booking Date"].ToString();
                foreach (DataRow row in dtBookings.Rows)
                    if (row["Booking Date"].ToString() != dtRow)
                        sameDateTime = false;
            }
            return sameDateTime;
        }

        // Loading the list of time bookings from Bookings class
        private void LoadListTimeBookings()
        {
            // Getting parameters from the Booking Class using Singleton pattern
            List<string> timeList = bookingSingleton.booking.ListTimeBooking();
            int timeStart = Convert.ToInt32(timeList.First().Substring(0, timeList.First().IndexOf(':'))); // Getting a minimum hour
            int timeEnd = Convert.ToInt32(timeList.Last().Substring(0, timeList.Last().IndexOf(':'))); // Getting a maximum hour

            timeList.Insert(0, "Any"); // Adding "Any" in the top of the List
            cmbTimeBooking.DataSource = timeList;

            int currentTime = DateTime.Now.Hour;
            if (currentTime >= timeStart && currentTime <= timeEnd)
                cmbTimeBooking.SelectedIndex = cmbTimeBooking.FindString(currentTime.ToString()); // Selecting current hour
        }

        // Method to load booking for the whole or certain period
        // Making this method private to use it in the ManageBookings and ManageTables class to reload the map after updating tables
        public void ShowBookings()
        {
            // If pressed Checkbox "Anytime", loading all bookings
            if (cbAnytimeBooking.Checked)
                LoadBookingsData("", "");
            else
            {
                // Casting date format to the format in database
                string dateBooking = datePickerBooking.Value.Date.ToString("dd/MM/yyyy");
                // If selected Any time
                if (cmbTimeBooking.SelectedItem.ToString() == "Any")
                {
                    LoadBookingsData(dateBooking, "");
                }
                else
                    LoadBookingsData(dateBooking, cmbTimeBooking.SelectedItem.ToString());
            }
        }

        // Button Show Bookings
        private void btnShowBookings_Click(object sender, EventArgs e)
        {
            ShowBookings();
        }

        // If pressed comboBox "Anytime" datePickerBooking and cmbTimeBooking are disabled
        private void cbAnytimeBooking_CheckedChanged(object sender, EventArgs e)
        {
            if (cbAnytimeBooking.Checked)
            {
                datePickerBooking.Enabled = false;
                cmbTimeBooking.Enabled = false;
            }
            else
            {
                datePickerBooking.Enabled = true;
                cmbTimeBooking.Enabled = true;
            }
        }

        // Method to open Booking Form
        private void ShowBookingForm()
        {
            ManageBookings tblForm = new ManageBookings(this);
            tblForm.StartPosition = FormStartPosition.CenterScreen;
            tblForm.ShowDialog();
        }

        // Button New Booking
        private void btnBooking_Click(object sender, EventArgs e)
        {
            // Set global variable to false and open Booking Form without substitution of booking parameters
            Globals.ConfigSystemClickEventFlag = false;

            ShowBookingForm();
        }

        // Button Manage Tables
        private void btnManageTables_Click(object sender, EventArgs e)
        {
            ManageBookings mngBooking = new ManageBookings(this);
            ManageTables mngTable = new ManageTables(mngBooking);
            mngTable.btnOk.Visible = false; // Hiding btnOk to avoid exception
            mngTable.StartPosition = FormStartPosition.CenterScreen;
            mngTable.ShowDialog();
        }

        private void PassingBookingParamToEdit(List<string> listBookingDetails)
        {
            // Passing list of Booking Details to Singleton class to use in Booking Form
            bookingSingleton.listBookingSelectedDetails = listBookingDetails;

            // Set global variable to true and open Booking Form with substitution of booking parameters
            Globals.ConfigSystemClickEventFlag = true;

            // Starting Booking Form
            ShowBookingForm();
        }

        // Editing booking
        private void EditBooking(int selectedRowIndex)
        {
            if (dataGridViewBooking.Rows.Count != 0)
            {
                // Getting values of the selected row and adding them into list to pass them later into Booking Form
                List<string> listBookingDetails = new List<string>();
                foreach (DataGridViewColumn col in dataGridViewBooking.Columns)
                    listBookingDetails.Add(dataGridViewBooking[col.Index, selectedRowIndex].Value.ToString());

                PassingBookingParamToEdit(listBookingDetails); // Calling PassingBookingParamToEdit method and passing there booking parameters
            }
            else
                MessageBox.Show("Please, select a row to edit!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        // Getting Booking details using double click on a row and passing them into Booking Form
        private void dataGridViewBooking_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            EditBooking(e.RowIndex);
        }

        // Create context menu for PictureBoxes and dataGridViewBooking
        private void ContextMenuConfiguration()
        {
            contxtMenuTables = new ContextMenuStrip();
            contxtMenuBookings = new ContextMenuStrip();

            menuItemRemoveTable = new ToolStripMenuItem();
            menuItemRemoveTable.Text = "Remove table";
            menuItemRemoveTable.Click += new EventHandler(menuItemRemoveTable_Click);
            contxtMenuTables.Items.Add(menuItemRemoveTable);

            menuItemEditBooking = new ToolStripMenuItem();
            menuItemEditBooking.Text = "Edit booking";
            menuItemEditBooking.Click += new EventHandler(menuItemEditBooking_Click);
            contxtMenuBookings.Items.Add(menuItemEditBooking);

            menuItemRemoveBooking = new ToolStripMenuItem();
            menuItemRemoveBooking.Text = "Remove booking";
            menuItemRemoveBooking.Click += new EventHandler(menuItemRemoveBooking_Click);
            contxtMenuBookings.Items.Add(menuItemRemoveBooking);
        }

        // Context menu "Remove table"
        private void menuItemRemoveTable_Click(object sender, EventArgs e)
        {
            RemovePicBoxTable();
        }

        // Context menu "Edit booking"
        private void menuItemEditBooking_Click(object sender, EventArgs e)
        {
            EditBooking(selectedRowIndex);
        }

        // Context menu "Remove booking"
        private void menuItemRemoveBooking_Click(object sender, EventArgs e)
        {
            RemoveBooking();
        }

        // Getting Table name of the selected row
        private string GetTableNameOfSelectedRow(int selectedRowIndex)
        {
            if (dataGridViewBooking.Rows.Count != 0)
            {
                DataGridViewRow row = this.dataGridViewBooking.Rows[selectedRowIndex];
                return row.Cells["Table name"].Value.ToString();
            }
            else
                return "";
        }

        // Getting DateTime parameter of the selected row
        private DateTime GetBookingDateTimeOfSelectedRow(int selectedRowIndex)
        {
            DataGridViewRow row = this.dataGridViewBooking.Rows[selectedRowIndex];
            return Convert.ToDateTime(row.Cells["Booking Date"].Value.ToString());
        }

        // Method to remove a booking
        private void RemoveBooking()
        {
            string tableName = GetTableNameOfSelectedRow(selectedRowIndex);
            if (String.IsNullOrEmpty(tableName))
            {
                MessageBox.Show("Please, select a row to remove!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bookingDateTime = GetBookingDateTimeOfSelectedRow(selectedRowIndex);

            res = MessageBox.Show("Are you sure you want to remove the selected booking?", "Attention", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (res == DialogResult.Yes)
            {
                int bookingId = GetBookingId(dtBookings, bookingDateTime, tableName);
                bookingSingleton.booking.DeleteBooking(bookingId);

                MessageBox.Show(bookingSingleton.booking.Message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ShowBookings();
            }
        }

        // Method to get a booking id (use in RemoveBooking())
        private int GetBookingId(DataTable dtSourceTable, DateTime date, string tName)
        {
            try
            {
                var getBookingId =
                    (from table in dtSourceTable.AsEnumerable()
                     where table.Field<DateTime>("Booking Date") == date && table.Field<string>("Table Name") == tName
                     select table.Field<int>("Booking ID"));

                return getBookingId.First();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return 0;
            }
        }

        // Getting selectedRowIndex of the clicked row
        private void dataGridViewBooking_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
                selectedRowIndex = e.RowIndex; // Use in menuItemEditBooking_Click() event to pass rowIndex in EditBooking()
        }

        // Employees button
        private void btnManageUsers_Click(object sender, EventArgs e)
        {
            ManageEmployees tblForm = new ManageEmployees();
            tblForm.StartPosition = FormStartPosition.CenterScreen;
            tblForm.ShowDialog();
        }

        // Reload map button
        private void btnReloadMap_Click(object sender, EventArgs e)
        {
            RetrievingTableMapFromDatabase(restaurantId);
            ShowBookings();
        }

        // Click event to show context menu for a booking
        private void dataGridViewBooking_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
                if (dataGridViewBooking.Rows.Count > 0)
                    dataGridViewBooking.ContextMenuStrip = contxtMenuBookings;
        }

        // Logout button
        private void btnExit_Click(object sender, EventArgs e)
        {
            LoginForm loginForm = new LoginForm();
            loginForm.StartPosition = FormStartPosition.CenterScreen;
            loginForm.Show();
            this.Hide();
        }

        // Exit form application after form is closed
        private void ConfigSystem_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }
    }
}