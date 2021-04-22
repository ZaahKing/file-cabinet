using System;

namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Insert record command.
    /// </summary>
    internal class InsertCommandHandler : ServiceCommandHandlerBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InsertCommandHandler"/> class.
        /// </summary>
        /// <param name="service">File cabinet service.</param>
        public InsertCommandHandler(IFileCabinetService service)
            : base(service)
        {
        }

        /// <inheritdoc/>
        protected override string GetCommandClue() => "insert";

        /// <inheritdoc/>
        protected override void Make(AppCommandRequest commandRequest)
        {
            var record = new FileCabinetRecord();
            try
            {
                record = commandRequest.Parameters.GetRecordFromInsertCommandParameter();

                this.Service.GetValidator().ValidateParameters(record);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Example: insert (fieldNameOne, fieldNameTwo, ...) values (valueOne, valueTwo, ...)");
            }

            if (this.Service.FindRecordById(record.Id) is not null)
            {
                Console.WriteLine($"Record #{record.Id} is exist. Try once more.");
            }
            else
            {
                try
                {
                    this.Service.InsertRecord(record);
                    Console.WriteLine($"Record #{record.Id}, {record.FirstName}, {record.LastName}, {record.DateOfBirth:yyyy-MMM-dd}, {record.DigitKey}, {record.Account}, {record.Sex} is inserted.");
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
        }
    }
}
