using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

namespace FileCabinetApp
{
    /// <summary>
    /// Write records to XML stream.
    /// </summary>
    public class FileCabinetRecordXmlSerializerWriter : IFileCabinetRecordWriter
    {
        private readonly StreamWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlSerializerWriter"/> class.
        /// </summary>
        /// <param name="writer">Stream writer.</param>
        public FileCabinetRecordXmlSerializerWriter(StreamWriter writer)
        {
            this.writer = writer;
        }

        /// <summary>
        /// Write collection.
        /// </summary>
        /// <param name="list">Read-only collection of file cabinet records.</param>
        public void Write(IEnumerable<FileCabinetRecord> list)
        {
            XmlSerializer xmlSerializer = new (list.GetType());
            xmlSerializer.Serialize(this.writer, list);
        }

        /// <summary>
        /// Dispose mothod.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose mothod.
        /// </summary>
        /// <param name="disposed">Disposing clue.</param>
        protected virtual void Dispose(bool disposed)
        {
            this.writer.Dispose();
        }
    }
}
