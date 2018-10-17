using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileHello
{
    // Form used to read and write sample CSV file and SQLite database.
    public partial class frmData : Form
    {
        const string FILE_NAME = @"data.csv";

        // Flag used to toggle timer on and off.
        bool _timerEnabled = true;

        public frmData()
        {
            InitializeComponent();
            this.Text = "Database and File Maintenance";
            timer1.Interval = 500;
            label1.Text = "Ready";
        }

        /// <summary>
        /// This timer is started and stopped when the user clicks btnStart_Click.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                // Write record to file
                FileText.WriteRandomText(textBox3.Text);

                // Write a record to database
                //Database.WriteData();
            }
            catch (Exception ex)
            {
                timer1.Enabled = false;
                string caption = "Unexpected Error";
                MessageBox.Show(this, ex.Message, caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk);
            }
        }

        // Start timer and write data
        private void btnStart_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox3.Text))
            {
                string message = "Filename not specified.";
                string caption = "Filename Missing";
                MessageBox.Show(this, message, caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk);
                return;
            }

            if (_timerEnabled)
            {
                btnStart.Text = "Stop";
                timer1.Enabled = true;
                timer1.Start();
            }
            else
            {
                btnStart.Text = "Start";
                timer1.Stop();
                timer1.Enabled = false;
            }

            // toggle timer on and off
            _timerEnabled = !_timerEnabled;
        }

        // Search file for string
        private void btnSearchFile_Click(object sender, EventArgs e)
        {
            try
            {
                timer1.Enabled = false;
                FileText.SearchTextFile(FILE_NAME, textBox1.Text);
            }
            catch (Exception ex)
            {
                string caption = "Unexpected Error";
                MessageBox.Show(this, ex.Message, caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk);
            }
        }

        // Search database for given datetime string
        private void btnSearchDB_Click(object sender, EventArgs e)
        {
            try
            {
                timer1.Enabled = false;
                string data = Database.FindByDateTime(textBox2.Text);

                if (String.IsNullOrEmpty(data))
                {
                    string message = "No records found.";
                    string caption = "Database Search";
                    MessageBox.Show(this, message, caption,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Asterisk);
                }
                else
                {
                    string caption = "Database Search";
                    MessageBox.Show(this, data, caption,
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Asterisk);
                }
            }
            catch (Exception ex)
            {
                string caption = "Unexpected Error";
                MessageBox.Show(this, ex.Message, caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk);
            }
        }

        // Create database
        private void btnCreate_Click(object sender, EventArgs e)
        {
            try
            {
                Database.Create();
                label1.Text = "Database created.";

                //string message = "Database table created.";
                //string caption = "Success";
                //MessageBox.Show(this, message, caption,
                //  MessageBoxButtons.OK,
                //  MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string caption = "Unexpected Error";
                MessageBox.Show(this, ex.Message, caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk);
            }

        }

        // Clear database
        private void btnClear_Click(object sender, EventArgs e)
        {
            try
            {
                Database.Clear();
                label1.Text = "Database table cleared.";

                //string message = "Database table cleared.";
                //string caption = "Success";
                //MessageBox.Show(this, message, caption,
                //  MessageBoxButtons.OK,
                //  MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                string caption = "Unexpected Error";
                MessageBox.Show(this, ex.Message, caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Asterisk);
            }
        }

        private void btnView_Click(object sender, EventArgs e)
        {
            frmShowData form4 = new frmShowData();
            form4.Show();
        }
    }
}
