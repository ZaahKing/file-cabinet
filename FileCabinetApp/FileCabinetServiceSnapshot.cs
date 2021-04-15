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
            writer.Write(this.records);
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
        /// Save snapshot file using writer.
        /// </summary>
        /// <param name="writer"> Get writer.</param>
        public void Save(IFileCabinetRecordWriter writer)
        {
            writer.Write(this.records);
        }
    }
}
