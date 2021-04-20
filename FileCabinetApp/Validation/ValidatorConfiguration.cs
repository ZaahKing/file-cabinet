namespace FileCabinetApp.Validation
{
    /// <summary>
    /// ValidatorConfiguration.
    /// </summary>
    public class ValidatorConfiguration
    {
        /// <summary>
        /// Gets or sets firstname length range.
        /// </summary>
        /// <value>
        /// Firstname length range.
        /// </value>
        public Range Firstname { get; set; }

        /// <summary>
        /// Gets or sets lastname length range.
        /// </summary>
        /// <value>
        /// Lastname length range.
        /// </value>
        public Range Lastname { get; set; }

        /// <summary>
        /// Gets or sets date of birth range.
        /// </summary>
        /// <value>
        /// Dateofbirth length range.
        /// </value>
        public DateRange Dateofbirth { get; set; }
    }
}
