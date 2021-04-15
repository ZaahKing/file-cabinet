using System;
using FileCabinetApp;

namespace FileCabinetGenerator
{
    /// <summary>
    /// Generate file cabinet records using in-memory service.
    /// </summary>
    internal class FileCabinetRecordsGenerator
    {
        private readonly string[] firstNames = new string[]
        {
            "James", "Mary", "John", "Patricia ", "Robert", "Jennifer", "Michael", "Linda", "William", "Elizabeth",
            "David", "Barbara", "Richard", "Susan", "Joseph", "Jessica", "Thomas", "Sarah", "Charles", "Karen",
            "Christopher", "Nancy", "Daniel", "Lisa", "Matthew", "Margaret", "Anthony", "Betty", "Donald", "Sandra",
            "Mark", "Ashley", "Paul", "Dorothy", "Steven", "Kimberly", "Andrew", "Emily", "Kenneth", "Donna",
            "Joshua", "Michelle", "Kevin", "Carol", "Brian", "Amanda", "George", "Melissa", "Edward", "Deborah",
        };

        private readonly string[] lastNames = new string[]
        {
            "Smith", "Johnson", "Williams", "Brown", "Jones", "Garcia", "Miller", "Davis", "Rodriguez", "Martinez",
            "Hernandez", "Lopez", "Gonzalez", "Wilson", "Anderson", "Thomas", "Taylor", "Moore", "Jackson", "Martin",
            "Lee", "Perez", "Thompson", "White", "Harris", "Sanchez", "Clark", "Ramirez", "Lewis", "Robinson",
            "Walker", "Young", "Allen", "King", "Wright", "Scott", "Torres", "Nguyen", "Hill", "Flores",
            "Green", "Adams", "Nelson", "Baker", "Hall", "Rivera", "Campbell", "Mitchell", "Carter", "Roberts",
        };

        /// <summary>
        /// Add generated record using service.
        /// </summary>
        /// <param name="recordAmount">Genereted record amount.</param>
        /// <param name="startId">Start with id.</param>
        /// <returns>Array of records.</returns>
        public FileCabinetRecord[] Generate(int recordAmount, int startId = 1)
        {
            var startDate = new DateTime(1950, 1, 1);
            TimeSpan period = DateTime.Now - startDate;
            var random = new Random();
            FileCabinetRecord[] result = new FileCabinetRecord[recordAmount];
            for (int i = 0; i < recordAmount; ++i)
            {
                result[i] = new FileCabinetRecord
                {
                    Id = i + startId,
                    FirstName = this.firstNames[random.Next(this.firstNames.Length)],
                    LastName = this.lastNames[random.Next(this.lastNames.Length)],
                    DateOfBirth = startDate + new TimeSpan(0, random.Next(0, (int)period.TotalMinutes), 0),
                    DigitKey = (short)random.Next(10000),
                    Account = random.Next(10000),
                    Sex = (random.Next(100) < 50) ? 'm' : 'f',
                };
            }

            return result;
        }
    }
}