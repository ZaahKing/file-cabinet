using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Default service for filecabinet data.
    /// </summary>
    public class FileCabinetDefaultService : FileCabinetService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetDefaultService"/> class.
        /// </summary>
        /// <param name="gateway">DAL.</param>
        /// <param name="validator">Get validator.</param>
        public FileCabinetDefaultService(IFileCabinetGateway gateway, IRecordValidator validator)
        {
            this.Validator = validator;
            foreach (var item in gateway.GetFileCabinetRecords())
            {
                this.CreateRecord(item);
            }
        }

        /// <summary>
        /// Validation method.
        /// </summary>
        /// <param name="record">File cabinet record.</param>
        protected override void ValidateParameters(FileCabinetRecord record)
        {
            this.Validator.CheckAll(record);
        }
    }
}
