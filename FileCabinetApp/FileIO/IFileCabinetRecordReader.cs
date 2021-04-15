using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Describe file cabinet reader interface.
    /// </summary>
    public interface IFileCabinetRecordReader
    {
        /// <summary>
        /// Load file cabinet records.
        /// </summary>
        /// <returns>File cabinet records collection.</returns>
        IEnumerable<FileCabinetRecord> Load();
    }
}
