using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileHello
{
    // Display settings in DataGridView
    // Using Settings in C#
    //
    // Create settings file.
    // Right click on the project in Solution Explorer, choose Properties. 
    // Select the Settings tab, click on the hyperlink if settings doesn't exist.
    //
    // User settings are saved to:
    // C:\Users\username\AppData\Local\Hello\user.config
    // OR
    // C:\Users\username\AppData\Roaming\Hello\user.config
    //
    // https://msdn.microsoft.com/en-us/library/aa730869(v=vs.80).aspx 
    // https://msdn.microsoft.com/en-us/library/bb397755(v=vs.110).aspx
    public partial class frmSettings : Form
    {
        private DataGridView dataGridView1 = new DataGridView();

        public frmSettings()
        {
            InitializeComponent();
            this.Text = "DataGridView Settings Demo";
        }

        // How to: Automatically Resize Cells When Content Changes
        // in the Windows Forms DataGridView Control
        // Sizing Options in the Windows Forms DataGridView Control
        private void SetupDataGridView()
        {
            // Set default properties for DataGridView control
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = false;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Width = this.Width - 10;
            dataGridView1.Height = this.Height - 100;
            dataGridView1.Anchor = (AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right);

            dataGridView1.ColumnCount = 2;

            // Set default styles for column headers
            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font =
                new Font(dataGridView1.Font, FontStyle.Bold);

            dataGridView1.Name = "dataGridView1";
            //dataGridView.Location = new Point(8, 8);
            //dataGridView.Size = new Size(500, 250);

            dataGridView1.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.GridColor = Color.Black;
            dataGridView1.RowHeadersVisible = false;

            // Indicate how row heights are determined
            dataGridView1.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;

            // Configure columns to automatically adjust their widths when the data changes.
            dataGridView1.AutoSizeColumnsMode =
                DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader;

            // Set names of column headers
            dataGridView1.Columns[0].Name = "Name";
            dataGridView1.Columns[1].Name = "Value";

            // Make first column readonly
            dataGridView1.Columns[0].ReadOnly = true;

            // Set event handler
            //dataGridView.CellDoubleClick += dataGridView_CellDoubleClick;

            this.Controls.Add(dataGridView1);
        }

        // Populate grid with user and application settings
        private void PopulateDataGridView()
        {
            List<string[]> list;

            list = GetSettings();

            foreach (string[] array in list)
            {
                dataGridView1.Rows.Add(array);
            }

            // Make cell with connection string readonly
            dataGridView1[1, 0].ReadOnly = true;

            // Resize the columns to fit the newly loaded data.
            // Use this method if AutoSizeColumnsMode property is not set.
            //dataGridView1.AutoResizeColumns();

            // Resize the height of the column headers
            dataGridView1.AutoResizeColumnHeadersHeight();

            // Resize all the row heights to fit the contents of all non-header cells
            dataGridView1.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);
        }

        // Sample code to demo how to read and write settings
        private static void ReadWriteSettings()
        {
            string currentDirName = Directory.GetCurrentDirectory();

            // Read setting
            string location = Properties.Settings.Default.Location;
            Console.WriteLine("Location: " + location);

            // Write setting
            //Properties.Settings.Default.Location = currentDirName;

            // Save all settings
            //Properties.Settings.Default.Save();
        }

        // Get settings as a List of string arrays
        // Using a List
        // https://www.dotnetperls.com/list
        private List<string[]> GetSettings()
        {
            string name, value;
            List<string[]> list = new List<string[]>();

            foreach (SettingsProperty currentProperty in Properties.Settings.Default.Properties)
            {
                name = currentProperty.Name;
                value = Properties.Settings.Default[currentProperty.Name].ToString();

                string[] array = new string[2];

                array[0] = name;
                array[1] = value;

                if (!String.IsNullOrEmpty(name))
                {
                    list.Add(array);
                }
            }

            return list;
        }

        // DataGridView.Rows Property
        // Using Application Settings and User Settings
        private void SaveSettings()
        {
            string name, value;

            // Loop thru datagrid and save values to settings
            foreach (DataGridViewRow row in dataGridView1.Rows)
            {
                name = row.Cells[0].Value.ToString();
                value = row.Cells[1].Value.ToString();

                // Do not overwrite application connection string
                if (!name.Equals("NorthwindConnectionString"))
                {
                    Properties.Settings.Default[name] = value;
                }
            }

            try
            {
                // Save settings
                Properties.Settings.Default.Save();
            }
            catch (IOException ex)
            {
                string message = "An unexpected error occured saving user settings: " + ex.Message;
                string caption = "Error Saving User Settings";
                MessageBox.Show(this, message, caption,
                    MessageBoxButtons.OKCancel,
                    MessageBoxIcon.Asterisk);
            }
        }

        public void CreateMainMenu()
        {
            // Create an empty MainMenu.
            MainMenu mainMenu1 = new MainMenu();

            MenuItem menuItem1 = new MenuItem();
            MenuItem menuItem2 = new MenuItem();
            MenuItem menuItem3 = new MenuItem();

            menuItem1.Text = "&File";
            menuItem2.Text = "&Save";
            menuItem3.Text = "E&xit";

            // Add MenuItem objects to the MainMenu.
            mainMenu1.MenuItems.Add(menuItem1);

            // Add menuItem2 and menuItem3 to menuItem1.
            menuItem1.MenuItems.Add(menuItem2);
            menuItem1.MenuItems.Add(menuItem3);

            // Add functionality to the menu items using the Click event. 
            menuItem2.Click += new EventHandler(this.menuItem2_Click);
            menuItem3.Click += new EventHandler(this.menuItem3_Click);

            // Bind the MainMenu to Form1.
            this.Menu = mainMenu1;
        }

        private void menuItem2_Click(object sender, System.EventArgs e)
        {
            // Create a new OpenFileDialog and display it.
            OpenFileDialog fd = new OpenFileDialog();
            fd.DefaultExt = "*.*";
            fd.ShowDialog();
        }

        private void menuItem3_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            SetupDataGridView();
            PopulateDataGridView();
            CreateMainMenu();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            SaveSettings();
        }
    }
}
