using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileHello
{
    // DataGridView Demo
    // Shows how to populate unbound DataGridView with data
    // DataGridView.Rows Property
    public partial class frmDataGrid : Form
    {
        private DataGridView dataGridView1 = new DataGridView();
        private Panel buttonPanel = new Panel();
        private Button addNewRowButton = new Button();
        private Button deleteRowButton = new Button();

        public frmDataGrid()
        {
            InitializeComponent();
            CreateMainMenu();
            this.Text = "Unbound DataGridView Demo";
        }

        // Create the MainMenu for the application.
        public void CreateMainMenu()
        {
            MainMenu mainMenu1 = new MainMenu();

            MenuItem menuItem1 = new MenuItem();
            MenuItem menuItem2 = new MenuItem();
            MenuItem menuItem3 = new MenuItem();
            MenuItem menuItem4 = new MenuItem();
            MenuItem menuItem5 = new MenuItem();

            menuItem1.Text = "&File";
            menuItem2.Text = "&Data Administration";
            menuItem3.Text = "&View Database";
            menuItem4.Text = "&View Settings";
            menuItem5.Text = "E&xit";

            // Add MenuItem objects to the MainMenu.
            mainMenu1.MenuItems.Add(menuItem1);

            // Add menuItem2 thru menuItem5 to menuItem1.
            menuItem1.MenuItems.Add(menuItem2);
            menuItem1.MenuItems.Add(menuItem3);
            menuItem1.MenuItems.Add(menuItem4);
            menuItem1.MenuItems.Add(menuItem5);

            // Add functionality to the menu items using the Click event. 
            menuItem2.Click += new EventHandler(this.menuItem2_Click);
            menuItem3.Click += new EventHandler(this.menuItem3_Click);
            menuItem4.Click += new EventHandler(this.menuItem4_Click);
            menuItem5.Click += new EventHandler(this.menuItem5_Click);

            // Bind the MainMenu to Form1.
            this.Menu = mainMenu1;
        }

        private void menuItem2_Click(object sender, System.EventArgs e)
        {
            var form = new frmData();
            form.Show();
        }

        private void menuItem3_Click(object sender, System.EventArgs e)
        {
            var form = new frmShowData();
            form.Show();
        }

        private void menuItem4_Click(object sender, System.EventArgs e)
        {
            var form = new frmSettings();
            form.Show();
        }

        private void menuItem5_Click(object sender, System.EventArgs e)
        {
            this.Close();
        }

        /*
        private void SetupLayout()
        {
            this.Size = new Size(600, 500);

            addNewRowButton.Text = "Add Row";
            addNewRowButton.Location = new Point(10, 10);
            addNewRowButton.Click += new EventHandler(addNewRowButton_Click);

            deleteRowButton.Text = "Delete Row";
            deleteRowButton.Location = new Point(100, 10);
            deleteRowButton.Click += new EventHandler(deleteRowButton_Click);

            buttonPanel.Controls.Add(addNewRowButton);
            buttonPanel.Controls.Add(deleteRowButton);
            buttonPanel.Height = 50;
            buttonPanel.Dock = DockStyle.Bottom;

            this.Controls.Add(this.buttonPanel);
        }
        */

        private void SetupDataGridView()
        {
            // Set default properties for DataGridView control
            dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            dataGridView1.Width = this.Width - 10;
            dataGridView1.Height = this.Height - 40;

            dataGridView1.ColumnCount = 5;

            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font =
                new Font(dataGridView1.Font, FontStyle.Bold);

            dataGridView1.Name = "dataGridView1";
            dataGridView1.Location = new Point(8, 8);
            dataGridView1.Size = new Size(500, 250);
            dataGridView1.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView1.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.GridColor = Color.Black;
            dataGridView1.RowHeadersVisible = false;

            dataGridView1.Columns[0].Name = "Release Date";
            dataGridView1.Columns[1].Name = "Track";
            dataGridView1.Columns[2].Name = "Title";
            dataGridView1.Columns[3].Name = "Artist";
            dataGridView1.Columns[4].Name = "Album";
            dataGridView1.Columns[4].DefaultCellStyle.Font =
                new Font(dataGridView1.DefaultCellStyle.Font, FontStyle.Italic);

            dataGridView1.CellFormatting +=
                new DataGridViewCellFormattingEventHandler(dataGridView_CellFormatting);

            // Set event handler
            //dataGridView.CellDoubleClick += dataGridView_CellDoubleClick;

            this.Controls.Add(dataGridView1);
        }

        private void PopulateDataGridView()
        {
            // Setup data to be populated
            string[] row0 = { "11/22/1968", "29", "Revolution 9",
                "Beatles", "The Beatles [White Album]" };
            string[] row1 = { "1960", "6", "Fools Rush In",
                "Frank Sinatra", "Nice 'N' Easy" };
            string[] row2 = { "11/11/1971", "1", "One of These Days",
                "Pink Floyd", "Meddle" };
            string[] row3 = { "1988", "7", "Where Is My Mind?",
                "Pixies", "Surfer Rosa" };
            string[] row4 = { "5/1981", "9", "Can't Find My Mind",
                "Cramps", "Psychedelic Jungle" };
            string[] row5 = { "6/10/2003", "13",
                "Scatterbrain. (As Dead As Leaves.)",
                "Radiohead", "Hail to the Thief" };
            string[] row6 = { "6/30/1992", "3", "Dress", "P J Harvey", "Dry" };
            object[] rows = new object[] { row1, row2, row3, row4, row5 };


            // Add rows of data to DataGridView
            foreach (string[] rowArray in rows)
            {
                dataGridView1.Rows.Add(rowArray);
            }

            // Set display order of columns
            dataGridView1.Columns[0].DisplayIndex = 3;
            dataGridView1.Columns[1].DisplayIndex = 4;
            dataGridView1.Columns[2].DisplayIndex = 0;
            dataGridView1.Columns[3].DisplayIndex = 1;
            dataGridView1.Columns[4].DisplayIndex = 2;

            // Resize the columns to fit the newly loaded data.
            dataGridView1.AutoResizeColumns();

            // Resize the height of the column headers. 
            dataGridView1.AutoResizeColumnHeadersHeight();

            // Resize all the row heights to fit the contents of all non-header cells.
            dataGridView1.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //SetupLayout();
            SetupDataGridView();
            PopulateDataGridView();
        }

        private void dataGridView_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            string format = "M/dd/yyyy";
            string formattedDate;
            DateTime dateValue;
            CultureInfo invariant = CultureInfo.InvariantCulture;
            CultureInfo enUS = new CultureInfo("en-US");

            if (e != null)
            {
                if (dataGridView1.Columns[e.ColumnIndex].Name == "Release Date")
                {
                    // Custom Date and Time Format Strings
                    // DateTime.TryParseExact
                    if (e.Value != null)
                    {
                        string dateString = e.Value.ToString();
                        if (DateTime.TryParseExact(dateString, format, invariant,
                            DateTimeStyles.None, out dateValue))
                        {
                            formattedDate = String.Format("{0:MM/dd/yyyy}", dateValue);
                            e.Value = formattedDate;
                        }
                        else
                        {
                            Console.WriteLine("{0} is not a valid date.", e.Value.ToString());
                        }
                    }
                }
            }
        }
    }
}
