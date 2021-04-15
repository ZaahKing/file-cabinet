using System;

namespace FileCabinetApp
{
    /// <summary>
    /// Describe personal data.
    /// </summary>
    public class FileCabinetRecord
    {
        /// <summary>
        /// Gets or sets idetification number.
        /// </summary>
        /// <returns> Identification number of user.</returns>
        /// <value>Idetification number.</value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets first name.
        /// </summary>
        /// <returns> First name of user.</returns>
        /// <value> First name.</value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets last name.
        /// </summary>
        /// <returns> Last name of user.</returns>
        /// <value> Last name.</value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets date of birth.
        /// </summary>
        /// <returns> Date of birth of user.</returns>
        /// <value> Date of birth.</value>
        public DateTime DateOfBirth { get; set; }

        /// <summary>
        /// Gets or sets digital access key.
        /// </summary>
        /// <returns> Digital access key of user.</returns>
        /// <value> Digital access key of birth.</value>
        public short DigitKey { get; set; }

        /// <summary>
        /// Gets or sets account.
        /// </summary>
        /// <returns> Account of user.</returns>
        /// <value> Account.</value>
        public decimal Account { get; set; }

        /// <summary>
        /// Gets or sets sex.
        /// </summary>
        /// <returns> Sex of user.</returns>
        /// <value> Sex.</value>
        public char Sex { get; set; }
    }
}
