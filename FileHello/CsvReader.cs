using Microsoft.Win32.SafeHandles;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace FileHello
{
    /// <summary>
    /// Reading a CSV file
    /// Dealing with commas in a CSV file.
    /// By implementing IDisposable, you are announcing that
    /// instances of this type allocate scarce resources.
    /// http://stackoverflow.com/questions/769621/dealing-with-commas-in-a-csv-file/769713#769713
    /// </summary>
    class CsvReader : System.IDisposable
    {
        // Flag: Has Dispose already been called?
        bool disposed = false;
        
        // Instantiate a SafeHandle instance.
        SafeHandle handle = new SafeFileHandle(IntPtr.Zero, true);

        private long __rowno = 0;
        private StreamReader __reader;
        private static Regex rexCsvSplitter = new Regex(@",(?=(?:[^""]*""[^""]*"")*(?![^""]*""))");
        private static Regex rexRunOnLine = new Regex(@"^[^""]*(?:""[^""]*""[^""]*)*""[^""]*$");

        public CsvReader(string fileName) : 
            this(new FileStream(fileName, FileMode.Open, FileAccess.Read))
        {
        }

        public CsvReader(Stream stream)
        {
            __reader = new StreamReader(stream);
            if (__reader == null)
                throw new ApplicationException("Unable to create StreamReader in CsvReader constructor");
        }

        public long RowIndex { get { return __rowno; } }

        public System.Collections.IEnumerable RowEnumerator
        {
            get
            {
                if (__reader == null)
                    throw new ApplicationException("Unable to start reading without CSV input.");

                __rowno = 0;
                string sLine;
                string sNextLine;

                while ((sLine = __reader.ReadLine()) != null)
                {
                    while (rexRunOnLine.IsMatch(sLine) && (sNextLine = __reader.ReadLine()) != null)
                        sLine += "\n" + sNextLine;

                    __rowno++;
                    string[] values = rexCsvSplitter.Split(sLine);

                    for (int i = 0; i < values.Length; i++)
                        values[i] = Csv.Unescape(values[i]);

                    yield return values;
                }

                __reader.Close();
            }
        }

        // Implement IDisposable.
        // Do not make this method virtual.
        // A derived class should not be able to override this method.
        public void Dispose()
        {
            Dispose(true);

            // This object will be cleaned up by the Dispose method.
            // Therefore, you should call GC.SupressFinalize to
            // take this object off the finalization queue
            // and prevent finalization code for this object
            // from executing a second time.
            GC.SuppressFinalize(this);
        }

        // Dispose(bool disposing) executes in two distinct scenarios.
        // If disposing equals true, the method has been called directly
        // or indirectly by a user's code. Managed and unmanaged resources
        // can be disposed.
        // If disposing equals false, the method has been called by the
        // runtime from inside the finalizer and you should not reference
        // other objects. Only unmanaged resources can be disposed.
        // https://msdn.microsoft.com/en-us/library/system.idisposable(v=vs.110).aspx
        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;

            // If disposing equals true, dispose all managed resources.
            if (disposing)
            {
                handle.Dispose();
                // Free any other managed objects here.
                if (__reader != null) __reader.Dispose();
            }

            // free unmanaged resources here.

            disposed = true;
        }
    }

    /// <summary>
    /// Utility class with helper functions for dealing with CSV data.
    /// </summary>
    public static class Csv
    {
        private const string QUOTE = "\"";
        private const string ESCAPED_QUOTE = "\"\"";
        private static char[] CHARACTERS_THAT_MUST_BE_QUOTED = { ',', '"', '\n' };

        public static string Escape(string s)
        {
            if (s.Contains(QUOTE))
                s = s.Replace(QUOTE, ESCAPED_QUOTE);

            if (s.IndexOfAny(CHARACTERS_THAT_MUST_BE_QUOTED) > -1)
                s = QUOTE + s + QUOTE;

            return s;
        }

        public static string Unescape(string s)
        {
            if (s.StartsWith(QUOTE) && s.EndsWith(QUOTE))
            {
                s = s.Substring(1, s.Length - 2);

                if (s.Contains(ESCAPED_QUOTE))
                    s = s.Replace(ESCAPED_QUOTE, QUOTE);
            }

            return s;
        }
    }
}
