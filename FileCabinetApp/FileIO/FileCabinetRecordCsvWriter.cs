using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Write records to stream.
    /// </summary>
    public class FileCabinetRecordCsvWriter : IFileCabinetRecordWriter
    {
        private readonly TextWriter writer;
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
        /// </summary>
        /// <param name="writer">Text writer.</param>
        public FileCabinetRecordCsvWriter(TextWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Write CSV header to text strem.
        /// </summary>
        /// <exception cref="IOException">When can not write the record.</exception>
        public void WriteHeader()
        {
            try
            {
                this.writer.WriteLine("Id,First Name,Last Name,Digit Key,Account,Sex");
            }
            catch (Exception e)
            {
                throw new IOException("Can't write header.", e);
            }
        }

        /// <summary>
        /// Write record to text strem.
        /// </summary>
        /// <param name="record">File cabinet record.</param>
        /// <exception cref="IOException">When can not write the record.</exception>
        public void WriteRecord(FileCabinetRecord record)
        {
            try
            {
                this.writer.WriteLine($"{record.Id},{record.FirstName},{record.LastName},{record.DigitKey},{record.Account},{record.Sex}");
            }
            catch (Exception e)
            {
                throw new IOException("Can't write file cabinet record.", e);
            }
        }

        /// <summary>
        /// Write collection.
        /// </summary>
        /// <param name="list">Read-only collection of file cabinet records.</param>
        public void Write(IEnumerable<FileCabinetRecord> list)
        {
            try
            {
                this.WriteHeader();
                foreach (var record in list)
                {
                    this.WriteRecord(record);
                }
            }
            catch (IOException e)
            {
                throw new IOException("Can't write snapshot.", e);
            }
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing"> Disposing clue.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.writer?.Dispose();
                }

                this.disposedValue = true;
            }
        }
    }
}
