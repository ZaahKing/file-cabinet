using System.Collections.Generic;

namespace FileCabinetApp
{
    /// <summary>
    /// Data access layer interface allows to get filecabinet data.
    /// </summary>
    public interface IFileCabinetGateway
    {
        /// <summary>
        /// This method returns list of FileCabinetRecords.
        /// </summary>
        /// <returns>Collection of FileCabinrtRecors.</returns>
        public ICollection<FileCabinetRecord> GetFileCabinetRecords();
    }
}
