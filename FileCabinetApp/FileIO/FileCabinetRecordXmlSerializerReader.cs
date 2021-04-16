using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Read file cabinet records from XML file.
    /// </summary>
    public class FileCabinetRecordXmlSerializerReader : IFileCabinetRecordReader, IDisposable
    {
        private readonly FileStream fileStream;
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlSerializerReader"/> class.
        /// </summary>
        /// <param name="fileStream">Stream.</param>
        public FileCabinetRecordXmlSerializerReader(FileStream fileStream)
        {
            this.fileStream = fileStream;
        }

        /// <summary>
        /// Get records.
        /// </summary>
        /// <returns>File Cabinet Records.</returns>
        public IEnumerable<FileCabinetRecord> Load()
        {
            var serializer = new XmlSerializer(typeof(FileCabinetRecord[]));
            var list = (FileCabinetRecord[])serializer.Deserialize(this.fileStream);
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
                this.disposedValue = true;
            }
        }
    }
}
