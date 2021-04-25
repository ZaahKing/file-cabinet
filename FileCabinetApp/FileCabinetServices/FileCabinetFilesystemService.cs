using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using FileCabinetApp.Validation;

namespace FileCabinetApp
{
    /// <summary>
    /// Service for filesytem filecabinet data.
    /// </summary>
    public class FileCabinetFilesystemService : IFileCabinetService, IDisposable
    {
        private const int FileCabinetRecordSize = 277;
        private const short DeleteFlagMask = 4;
        private readonly FileStream fileStream;
        private readonly IRecordValidator validator;
        private readonly BinaryReader reader;
        private readonly BinaryWriter writer;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetFilesystemService"/> class.
        /// </summary>
        /// <param name="stream">File stream.</param>
        /// <param name="validator">Validator.</param>
        public FileCabinetFilesystemService(FileStream stream, IRecordValidator validator)
        {
            this.fileStream = stream;
            this.validator = validator;
            this.reader = new BinaryReader(stream);
            this.writer = new BinaryWriter(stream);
        }

        /// <summary>
        /// Add new record.
        /// </summary>
        /// <param name="record">File cabinet record.</param>
        /// <returns>Returns Id of new record.</returns>
        public int CreateRecord(FileCabinetRecord record)
        {
            record.Id = this.GetRecordsYield().Max(x => x.Id) + 1;

            try
            {
                this.fileStream.Seek(this.fileStream.Length, SeekOrigin.Begin);
                this.WriteRecordInCurrentPosition(record);
                this.writer.Flush();
            }
            catch (IOException e)
            {
                throw new IOException("Cant write to file.", e);
            }

            return record.Id;
        }

        /// <summary>
        /// Edit existing record found by Id.
        /// </summary>
        /// <param name="record">File cabinet record.</param>
        public void EditRecord(FileCabinetRecord record)
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);
            this.fileStream.Seek(2, SeekOrigin.Current);
            int offset = FileCabinetRecordSize - 4;
            do
            {
                int id = this.reader.ReadInt32();
                if (id == record.Id)
                {
                    this.WriteRecordWithoutIDInCurrentPosition(record);
                    this.writer.Flush();
                    this.fileStream.Seek(0, SeekOrigin.Begin);
                    this.fileStream.Flush();
                    return;
                }

                this.fileStream.Seek(offset, SeekOrigin.Current);
            }
            while (this.fileStream.Position + offset < this.fileStream.Length);
            this.fileStream.Seek(0, SeekOrigin.Begin);
        }

        /// <inheritdoc/>
        public void InsertRecord(FileCabinetRecord record)
        {
            try
            {
                this.fileStream.Seek(this.fileStream.Length, SeekOrigin.Begin);
                this.WriteRecordInCurrentPosition(record);
                this.writer.Flush();
            }
            catch (IOException e)
            {
                throw new IOException("Cant write to file.", e);
            }
        }

        /// <summary>
        /// Helps to find record by Id.
        /// </summary>
        /// <param name="id">Id.</param>
        /// <returns>FileCabinetRecord. </returns>
        public FileCabinetRecord FindRecordById(int id)
        {
            return this.GetRecordsYield().FirstOrDefault(x => x.Id == id);
        }

        /// <summary>
        /// Returns all FileCabinetRecords.
        /// </summary>
        /// <returns>Array of FileCabinetRecords.</returns>
        public IEnumerable<FileCabinetRecord> GetRecords()
        {
            return this.GetRecordsYield();
        }

        /// <summary>
        /// Returns count of records.
        /// </summary>
        /// <returns>Count of records.</returns>
        public int GetStat()
        {
            return (int)(this.fileStream.Length / FileCabinetRecordSize);
        }

        /// <summary>
        /// Returns count records for purging.
        /// </summary>
        /// <returns>Count of deleted records.</returns>
        public int GetStatDeleted()
        {
            return this.GetFileElementsYeld().Count(x => x.IsDeleted);
        }

        /// <summary>
        /// Returns validator used in service.
        /// </summary>
        /// <returns>Validator.</returns>
        public IRecordValidator GetValidator()
        {
            return this.validator;
        }

        /// <summary>
        /// Make file cabinet records snapshot.
        /// </summary>
        /// <returns>FileCabinetServiceSnapshot.</returns>
        public FileCabinetServiceSnapshot MakeSnapshot()
        {
            var list = this.GetRecordsYield().ToArray();
            return new FileCabinetServiceSnapshot(list);
        }

        /// <summary>
        /// Restore file cabinet records from snapshot.
        /// </summary>
        /// <param name="snapshot">Snapshot object.</param>
        /// <returns>Cout of added records.</returns>
        public int Restore(FileCabinetServiceSnapshot snapshot)
        {
            int errorsCount = 0;
            this.fileStream.SetLength(0);
            this.fileStream.Flush();
            var list = snapshot.GetRecords();
            foreach (var record in list)
            {
                try
                {
                    this.CreateRecord(record);
                }
                catch (Exception e)
                {
                    errorsCount++;
                    Console.WriteLine(e.Message);
                }
            }

            return list.Count - errorsCount;
        }

        /// <summary>
        /// Remove record.
        /// </summary>
        /// <param name="id">Id for delation.</param>
        public void RemoveRecord(int id)
        {
            FileElement? nullableItem = this.GetFileElementsYeld().FirstOrDefault(x => x.Record.Id == id);
            if (nullableItem is null)
            {
                return;
            }

            var item = nullableItem.Value;
            this.fileStream.Seek(item.Position, SeekOrigin.Begin);
            this.writer.Write(item.Header | DeleteFlagMask);
            this.fileStream.Flush();
        }

        /// <summary>
        /// Purge storage.
        /// </summary>
        /// <returns>Zero becauce inmemroty storage do not need of purging.</returns>
        public int PurgeStorage()
        {
            long[] deletetBlocksPositions = this.GetFileElementsYeld().Where(x => x.IsDeleted).Select(record => record.Position).ToArray();
            if (deletetBlocksPositions.Length == 0)
            {
                return 0;
            }

            long positionToWrite = deletetBlocksPositions[0];
            long blockForMoveLength = 0;
            for (int i = 1; i < deletetBlocksPositions.Length; ++i)
            {
                blockForMoveLength = deletetBlocksPositions[i] - deletetBlocksPositions[i - 1] - FileCabinetRecordSize;
                if (blockForMoveLength > 0)
                {
                    this.MoveBlocks(deletetBlocksPositions[i - 1] + FileCabinetRecordSize, positionToWrite, blockForMoveLength);
                    positionToWrite += blockForMoveLength;
                }
            }

            blockForMoveLength = 0;
            if (deletetBlocksPositions[^1] + FileCabinetRecordSize < this.fileStream.Length - 1)
            {
                blockForMoveLength = this.fileStream.Length - deletetBlocksPositions[^1] - FileCabinetRecordSize;
                this.MoveBlocks(deletetBlocksPositions[^1] + FileCabinetRecordSize, positionToWrite, blockForMoveLength);
            }

            this.fileStream.SetLength(positionToWrite + blockForMoveLength);

            return deletetBlocksPositions.Length;
        }

        /// <summary>
        /// Dispose object.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Dispose pattern implementation.
        /// </summary>
        /// <param name="disposed">Disposed.</param>
        protected virtual void Dispose(bool disposed)
        {
            this.reader?.Dispose();
            this.writer?.Dispose();
        }

        private static char[] StringToChars(string data, int arrayLength)
        {
            char[] result = new char[arrayLength];
            for (int i = 0; i < data.Length; ++i)
            {
                result[i] = data[i];
            }

            return result;
        }

        private IEnumerable<FileElement> GetFileElementsYeld()
        {
            this.fileStream.Seek(0, SeekOrigin.Begin);
            while (this.fileStream.Position < this.fileStream.Length)
            {
                FileElement element;
                var record = new FileCabinetRecord();
                element.Position = this.fileStream.Position;
                element.Header = this.reader.ReadInt16();
                record.Id = this.reader.ReadInt32();
                record.FirstName = new string(this.reader.ReadChars(120)).TrimEnd('\0');
                record.LastName = new string(this.reader.ReadChars(120)).TrimEnd('\0');
                record.DateOfBirth = new DateTime(this.reader.ReadInt32(), this.reader.ReadInt32(), this.reader.ReadInt32());
                record.DigitKey = this.reader.ReadInt16();
                record.Account = this.reader.ReadDecimal();
                record.Sex = this.reader.ReadChar();
                element.Record = record;
                yield return element;
            }

            this.fileStream.Flush();
            this.fileStream.Seek(0, SeekOrigin.Begin);
        }

        private IEnumerable<FileCabinetRecord> GetRecordsYield()
        {
            foreach (var fileElement in this.GetFileElementsYeld())
            {
                if (!fileElement.IsDeleted)
                {
                    yield return fileElement.Record;
                }
            }
        }

        private void MoveBlocks(long positionFrom, long positionTo, long size)
        {
            this.fileStream.Seek(positionFrom, SeekOrigin.Begin);
            var buffer = this.reader.ReadBytes((int)size);
            this.fileStream.Seek(positionTo, SeekOrigin.Begin);
            this.writer.Write(buffer);
            this.fileStream.Flush();
        }

        private void WriteRecordWithoutIDInCurrentPosition(FileCabinetRecord record)
        {
            char[] buffer = StringToChars(record.FirstName, 120);
            this.writer.Write(buffer);
            buffer = StringToChars(record.LastName, 120);
            this.writer.Write(buffer);
            this.writer.Write(record.DateOfBirth.Year);
            this.writer.Write(record.DateOfBirth.Month);
            this.writer.Write(record.DateOfBirth.Day);
            this.writer.Write(record.DigitKey);
            this.writer.Write(record.Account);
            this.writer.Write(record.Sex);
        }

        private void WriteRecordInCurrentPosition(FileCabinetRecord record)
        {
            this.writer.Write((short)0);
            this.writer.Write(record.Id);
            this.WriteRecordWithoutIDInCurrentPosition(record);
        }

        private struct FileElement
        {
            public short Header;
            public long Position;
            public FileCabinetRecord Record;

            public bool IsDeleted
            {
                get { return (this.Header & 4) != 0; }
            }
        }
    }
}
