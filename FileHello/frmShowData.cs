using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SQLite;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileHello
{
    // Display SQLite database table in DataGridView
    public partial class frmShowData : Form
    {
        private DataGridView dataGridView1 = new DataGridView();

        public frmShowData()
        {
            InitializeComponent();
            this.Text = "DataGridView SQLite Demo";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            PopulateDataGridView();
        }

        private void SetupDataGridView()
        {
            // Set default properties for DataGridView control
            //dataGridView1.Dock = DockStyle.Fill;
            dataGridView1.MultiSelect = false;
            dataGridView1.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            dataGridView1.ReadOnly = true;
            dataGridView1.AllowUserToAddRows = false;
            dataGridView1.Width = this.Width - 25;
            dataGridView1.Height = this.Height - 85;

            dataGridView1.ColumnHeadersDefaultCellStyle.BackColor = Color.Navy;
            dataGridView1.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            dataGridView1.ColumnHeadersDefaultCellStyle.Font =
                new Font(dataGridView1.Font, FontStyle.Bold);

            dataGridView1.Name = "dataGridView1";
            dataGridView1.Location = new Point(4, 4);
            //dataGridView1.Size = new Size(250, 250);

            dataGridView1.AutoSizeRowsMode =
                DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            dataGridView1.ColumnHeadersBorderStyle =
                DataGridViewHeaderBorderStyle.Single;
            dataGridView1.CellBorderStyle = DataGridViewCellBorderStyle.Single;
            dataGridView1.GridColor = Color.Black;
            dataGridView1.RowHeadersVisible = false;

            //dataGridView1.Columns[0].Name = "Timestamp";
            //dataGridView1.Columns[1].Name = "Temp1";
            //dataGridView1.Columns[2].Name = "Temp2";

            //dataGridView1.CellFormatting +=
            //    new DataGridViewCellFormattingEventHandler(dataGridView_CellFormatting);

            // Set event handler
            //dataGridView.CellDoubleClick += dataGridView_CellDoubleClick;

            this.Controls.Add(dataGridView1);
        }

        private void PopulateDataGridView()
        {
            // Use list to populate DataGridView
            List<string[]> list = new List<string[]>();

            string connectionString = "Data Source=sample.sqlite;Version=3;";

            using (SQLiteConnection connection = new SQLiteConnection(connectionString))
            {
                connection.Open();

                using (SQLiteCommand command = new SQLiteCommand(connection))
                {
                    string sql = "SELECT * FROM Log ORDER BY strftime('%Y-%m-%d %H:%M:%f', timestamp) DESC";
                    command.CommandText = sql;
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        // Get data from database and add rows to list
                        while (reader.Read())
                        {
                            string[] row = new string[3];
                            row[0] = reader["timestamp"].ToString();
                            row[1] = reader["temp1"].ToString();
                            row[2] = reader["temp2"].ToString();
                            list.Add(row);
                        }
                    }
                }

                connection.Close();
            }

            // Create datatable
            DataTable table = new DataTable();
            table.Columns.Add("TIMESTAMP", typeof(string));
            table.Columns.Add("TEMP1", typeof(int));
            table.Columns.Add("TEMP2", typeof(double));

            // Add rows of data to DataTable
            foreach (string[] row in list)
            {
                table.Rows.Add(row);
            }

            // Bind DataTable to DataGridView
            dataGridView1.DataSource = table;

            dataGridView1.Columns[0].ReadOnly = true;
            dataGridView1.Columns[1].ReadOnly = true;
            dataGridView1.Columns[2].ReadOnly = true;

            // Set display order of columns
            dataGridView1.Columns[0].DisplayIndex = 0;
            dataGridView1.Columns[1].DisplayIndex = 1;
            dataGridView1.Columns[2].DisplayIndex = 2;

            // Resize the columns to fit the newly loaded data.
            dataGridView1.AutoResizeColumns();

            // Resize the height of the column headers. 
            dataGridView1.AutoResizeColumnHeadersHeight();

            // Resize all the row heights to fit the contents of all non-header cells.
            dataGridView1.AutoResizeRows(DataGridViewAutoSizeRowsMode.AllCellsExceptHeaders);
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            //SetupLayout();
            SetupDataGridView();
        }

        private void Form4_Resize(object sender, EventArgs e)
        {
            dataGridView1.Width = this.Width - 25;
            dataGridView1.Height = this.Height - 85;
        }
    }
}
