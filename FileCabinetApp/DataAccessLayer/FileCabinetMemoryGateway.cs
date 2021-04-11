using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp
{
    public class FileCabinetMemoryGateway : IFileCabinetGateway
    {
        public ICollection<FileCabinetRecord> GetFileCabinetRecords()
        {
            return new List<FileCabinetRecord>
            {
                new FileCabinetRecord { Id = 1, FirstName = "Alex", LastName = "Whiter", DateOfBirth = new DateTime(1982, 12, 6), DigitKey = 1234, Account = 45m, Sex = 'm' },
                new FileCabinetRecord { Id = 2, FirstName = "Alex", LastName = "Booter", DateOfBirth = new DateTime(1990, 10, 10), DigitKey = 1234, Account = 45m, Sex = 'm' },
                new FileCabinetRecord { Id = 3, FirstName = "Xena", LastName = "Queen", DateOfBirth = new DateTime(1982, 12, 6), DigitKey = 1234, Account = 45m, Sex = 'f' },
                new FileCabinetRecord { Id = 4, FirstName = "Anastatia", LastName = "Queen", DateOfBirth = new DateTime(1982, 12, 6), DigitKey = 1234, Account = 45m, Sex = 'f' },
            };
        }
    }
}
