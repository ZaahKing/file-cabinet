using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Describe file cabinet writer interface.
    /// </summary>
    public interface IFileCabinetRecordWriter : IDisposable
    {
        /// <summary>
        /// Write collection.
        /// </summary>
        /// <param name="list">Read-only collection of file cabinet records.</param>
        void Write(IEnumerable<FileCabinetRecord> list);
    }
}
