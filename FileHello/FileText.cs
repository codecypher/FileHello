using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FileHello
{
    /// <summary>
    /// Read and write sample CSV text file.
    /// </summary>
    class FileText
    {
        // Use this Random object to choose random numbers to write to file.
        static Random random = new Random();

        // Use an enum for status
        // Since the status has several states, use an enum for it.
        // Use the params keyword
        // It allows for a variable number of arguments.
        // It must be the last parameter in a method declaration.
        // Example: params string[] arguments
        public enum FileStatus
        {
            Success,
            FileNotFound,
            AccessViolation,
            ImproperOrNoArgumentsFound,
            TextFileManipulationFailed,
            LogWriteFailed,
            IOException
        }

        // Populate file with random data
        // How to: Write to a Text File
        // https://msdn.microsoft.com/en-us/library/8bh11f1k.aspx
        public static void WriteRandomText(string fileName)
        {           
            // Get current directory of application.
            //string currentDirName = Directory.GetCurrentDirectory();

            // Combine the current directory and file name to make file path.
            //string filePath1 = Path.Combine(currentDirName, FILE_NAME);

            // Get current time
            string timeString = String.Format("{0:yyyy-MM-dd HH:mm:ss.fff}", DateTime.Now);

            // Get random integer between 0 and 100
            int randomInteger = random.Next(100);
            
            // Get random double between 0.0 and 100.0
            double randomDouble = random.NextDouble() * 100;

            // Append text to file if it exists or create file if it does not exist.
            try
            {
                using (StreamWriter writer = File.AppendText(fileName))
                {
                    writer.WriteLine("{0}, {1:d3}, {2:F2}", timeString, randomInteger, randomDouble);
                }
            }
            catch (AccessViolationException ex)
            {
                // You don't have the permission to open this file.
                string message = String.Format("You do not have the permission to open the file: {0}. {1}.{2}: {3}",
                    fileName,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ex.Message);
                throw new ApplicationException(message);
            }
            catch (IOException ex)
            {
                string message = String.Format("An unexpected IOException occured in {0}.{1}: {2}",
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ex.Message);
                throw new ApplicationException(message);
            }
        }


        // Search file for given text string.
        public static void SearchTextFile(string fileName, string searchString)
        {
            //if (reader == null) throw new ArgumentNullException("reader");

            if (string.IsNullOrEmpty(searchString))
                throw new ArgumentException("Search string may not be null or empty.");

            if (!File.Exists(fileName))
            {
                string message = "File does not exist";
                string caption = "File not found";
                MessageBox.Show(message, caption,
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Information);
                return;
            }

            try
            {
                using (CsvReader reader = new CsvReader(fileName))
                {
                    foreach (string[] values in reader.RowEnumerator)
                    {
                        if (values[0].Equals(searchString))
                        {
                            string message =
                                String.Format("Found: {0} {1} {2}",
                                values[0], values[1], values[2]);
                            string caption = "Search was successful";
                            MessageBox.Show(message, caption,
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                    }
                }
            }
            catch (FileNotFoundException ex)
            {
                // The file does not exist
                string message = String.Format("The file does not exist: {0}. {1}.{2}: {3}",
                    fileName,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ex.Message);
                throw new ApplicationException(message);
            }
            catch (AccessViolationException ex)
            {
                // You don't have the permission to open this file
                string message = String.Format("You do not have the permission to open the file: {0}. {1}.{2}: {3}",
                    fileName,
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ex.Message);
                throw new ApplicationException(message);
            }
            catch (IOException ex)
            {
                string message = String.Format("An unexpected IOException occured in {0}.{1}: {2}",
                    System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Name,
                    System.Reflection.MethodBase.GetCurrentMethod().Name,
                    ex.Message);
                throw new ApplicationException(message);
            }
        }
    }
}
