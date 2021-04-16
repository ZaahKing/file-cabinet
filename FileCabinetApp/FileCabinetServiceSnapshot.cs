using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

namespace FileCabinetApp
{
    /// <summary>
    /// Keep file cabinrt records snapshot.
    /// </summary>
    public class FileCabinetServiceSnapshot
    {
        private FileCabinetRecord[] records;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        /// <param name="data"> Get snapshot.</param>
        public FileCabinetServiceSnapshot(FileCabinetRecord[] data)
        {
            this.records = data;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
        /// </summary>
        public FileCabinetServiceSnapshot()
        {
            this.records = Array.Empty<FileCabinetRecord>();
        }

        /// <summary>
        /// Records.
        /// </summary>
        /// <returns>Read-only collection.</returns>
        public IReadOnlyCollection<FileCabinetRecord> GetRecords()
        {
            return new ReadOnlyCollection<FileCabinetRecord>(this.records);
        }

        /// <summary>
        /// Load snapshot from CSV file.
        /// </summary>
        /// <param name="reader"> Get reader.</param>
        public void LoadFromCSV(FileStream reader)
        {
            this.Load(new FileCabinetRecordCsvReader(reader));
        }

        /// <summary>
        /// Save snapshot to CSV file.
        /// </summary>
        /// <param name="writer"> Get writer.</param>
        public void SaveToCSV(FileCabinetRecordCsvWriter writer)
        {
            writer.Write(this.records);
        }

        /// <summary>
        /// Load snapshot from XML file.
        /// </summary>
        /// <param name="reader"> Get reader.</param>
        public void LoadFromXml(FileStream reader)
        {
            this.Load(new FileCabinetRecordXmlSerializerReader(reader));
        }

        /// <summary>
        /// Save snapshot to XML file.
        /// </summary>
        /// <param name="writer"> Get writer.</param>
        public void SaveToXML(FileCabinetRecordXmlWriter writer)
        {
            writer.Write(this.records);
        }

        /// <summary>
        /// Load snapshot file using reader.
        /// </summary>
        /// <param name="writer"> Get writer.</param>
        public void Load(IFileCabinetRecordReader writer)
        {
            this.records = writer.Load().ToArray();
        }

        /// <summary>
        /// Save snapshot file using writer.
        /// </summary>
        /// <param name="writer"> Get writer.</param>
        public void Save(IFileCabinetRecordWriter writer)
        {
            writer.Write(this.records);
        }
    }
}
