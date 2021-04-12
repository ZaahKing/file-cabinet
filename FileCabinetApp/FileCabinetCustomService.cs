using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    /// <summary>
    /// Custom service for filecabinet data.
    /// </summary>
    public class FileCabinetCustomService : FileCabinetService
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileCabinetCustomService"/> class.
        /// </summary>
        /// <param name="gateway">DAL.</param>
        public FileCabinetCustomService(IFileCabinetGateway gateway)
        {
            this.Validator = new CustomValidator();
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
            this.Validator.CheckFirstName(record.FirstName);
            this.Validator.CheckLastName(record.LastName);
            this.Validator.CheckDateOfBirth(record.DateOfBirth);
            this.Validator.CheckDigitKey(record.DigitKey);
        }
    }
}
