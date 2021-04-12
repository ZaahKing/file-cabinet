using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace FileCabinetApp
{
    /// <summary>
    /// Write records to XML stream.
    /// </summary>
    public class FileCabinetRecordXmlWriter : IDisposable
    {
        private readonly XmlWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
        /// </summary>
        /// <param name="writer">Stream writer.</param>
        public FileCabinetRecordXmlWriter(StreamWriter writer)
        {
            XmlWriterSettings settings = new ();
            settings.Indent = true;
            settings.IndentChars = "\t";
            settings.OmitXmlDeclaration = true;
            this.writer = XmlWriter.Create(writer, settings);
        }

        /// <summary>
        /// Write XML header to a strem.
        /// </summary>
        /// <exception cref="IOException">When can not write the record.</exception>
        public void WriteHeader()
        {
            try
            {
                this.writer.WriteStartDocument();
                this.writer.WriteStartElement("records");
            }
            catch (Exception e)
            {
                throw new IOException("Can't write header.", e);
            }
        }

        /// <summary>
        /// Write XML header to a strem.
        /// </summary>
        /// <param name="record">File cabinet record.</param>
        /// <exception cref="IOException">When can not write the record.</exception>
        public void Write(FileCabinetRecord record)
        {
            try
            {
                this.writer.WriteStartElement("record");
                this.writer.WriteAttributeString("id", record.Id.ToString());

                this.writer.WriteStartElement("name");
                this.writer.WriteAttributeString("first", record.FirstName);
                this.writer.WriteAttributeString("last", record.LastName);
                this.writer.WriteEndElement();

                this.writer.WriteStartElement("dateOfBirth");
                this.writer.WriteString(record.DateOfBirth.ToString("dd/mm/yyyy"));
                this.writer.WriteEndElement();

                this.writer.WriteStartElement("digitKey");
                this.writer.WriteString(record.DigitKey.ToString());
                this.writer.WriteEndElement();

                this.writer.WriteStartElement("account");
                this.writer.WriteString(record.DigitKey.ToString());
                this.writer.WriteEndElement();

                this.writer.WriteStartElement("sex");
                this.writer.WriteString(record.DigitKey.ToString());
                this.writer.WriteEndElement();

                this.writer.WriteEndElement();
            }
            catch (Exception e)
            {
                throw new IOException("Can't write header.", e);
            }
        }

        /// <summary>
        /// Write XML footer to a strem.
        /// </summary>
        /// <exception cref="IOException">When can not write the record.</exception>
        public void WriteFooter()
        {
            try
            {
                this.writer.WriteEndElement();
                this.writer.WriteEndDocument();
            }
            catch (Exception e)
            {
                throw new IOException("Can't write header.", e);
            }
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
