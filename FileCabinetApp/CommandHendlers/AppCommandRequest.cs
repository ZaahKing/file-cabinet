namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Keep comand request data.
    /// </summary>
    public class AppCommandRequest
    {
        /// <summary>
        /// Gets or sets command name.
        /// </summary>
        /// <value>
        /// Command name.
        /// </value>
        public string Command { get; set; }

        /// <summary>
        /// Gets or sets command parameters.
        /// </summary>
        /// <value>
        /// Parameters in string.
        /// </value>
        public string Parameters { get; set; }
    }
}
