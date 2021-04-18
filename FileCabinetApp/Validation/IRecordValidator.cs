using System;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// File cabinet record validation interface.
    /// </summary>
    public interface IRecordValidator
    {
        /// <summary>
        /// Check is file cabinet record correct.
        /// </summary>
        /// <param name="record">File cabinet record.</param>
        void ValidateParameters(FileCabinetRecord record);
    }
}
