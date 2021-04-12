using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Keep file cabinrt records snapshot.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private readonly FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="data"> Get snapshot.</param>
        public FileCabinetServiceSnapshot(FileCabinetRecord[] data)
        {
            this.records = data;
        }

        /// <summary>
        /// Save snapshot to CSV file.
        /// </summary>
        /// <param name="writer"> Get writer.</param>
        public void SaveToCSV(FileCabinetRecordCsvWriter writer)
        {
            try
            {
                writer.WriteHeader();
                foreach (var record in this.records)
                {
                    writer.Write(record);
                }
            }
            catch (IOException e)
            {
                throw new IOException("Can't write snapshot.", e);
            }
        }
    }
}
