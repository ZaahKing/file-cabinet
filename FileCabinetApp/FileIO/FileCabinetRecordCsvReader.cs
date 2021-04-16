using System;
using System.Collections.Generic;
using System.IO;

namespace FileCabinetApp
{
    /// <summary>
    /// Read file cabinet records from csv file.
    /// </summary>
    public class FileCabinetRecordCsvReader : IFileCabinetRecordReader, IDisposable
    {
        private readonly FileStream fileStream;
        private readonly TextReader reader;
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.
        /// </summary>
        /// <param name="fileStream">Stream.</param>
        public FileCabinetRecordCsvReader(FileStream fileStream)
        {
            this.fileStream = fileStream;
            this.reader = new StreamReader(this.fileStream);
        }

        /// <summary>
        /// Get records.
        /// </summary>
        /// <returns>File Cabinet Records.</returns>
        public IEnumerable<FileCabinetRecord> Load()
        {
            if (this.ReadHeader() != @"Id,First Name,Last Name,Digit Key,Account,Sex")
            {
                throw new FormatException("Wrong data format.");
            }

            var list = new List<FileCabinetRecord>();
            string line;
            while ((line = this.reader.ReadLine()) != null)
            {
                var fields = line.Split(',');
                list.Add(new FileCabinetRecord
                {
                    Id = int.Parse(fields[0]),
                    FirstName = fields[1],
                    LastName = fields[2],
                    DateOfBirth = DateTime.Parse(fields[3]),
                    DigitKey = short.Parse(fields[4]),
                    Account = decimal.Parse(fields[5]),
                    Sex = fields[6][0],
                });
            }

            return list;
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose.
        /// </summary>
        /// <param name="disposing">Вisposing.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    this.reader?.Dispose();
                }

                this.disposedValue = true;
            }
        }

        private string ReadHeader()
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);
            return this.reader.ReadLine();
        }
    }
}
