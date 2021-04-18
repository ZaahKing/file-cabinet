namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Base command handler.
    /// </summary>
    public abstract class CommandHandleBase : ICommandHandler
    {
        /// <summary>
        /// Gets or sets Keep hendler.
        /// </summary>
        /// <value>
        /// Keep hendler.
        /// </value>
        protected ICommandHandler NextHandler { get; set; }

        /// <summary>
        /// Meke job here and handle next.
        /// </summary>
        /// <param name="commandRequest">Command request.</param>
        public abstract void Handle(AppCommandRequest commandRequest);

        /// <summary>
        /// Set next command hendler.
        /// </summary>
        /// <param name="handler">Handler.</param>
        public void SetNext(ICommandHandler handler)
        {
            this.NextHandler = handler;
        }

        /// <summary>
        /// Split param string.
        /// </summary>
        /// <param name="parameters">String.</param>
        /// <returns>Two strings.</returns>
        protected static (string, string) SplitParam(string parameters)
        {
            string[] args = parameters.Split(' ', 2);
            if (args.Length < 2 || string.IsNullOrWhiteSpace(parameters))
            {
                return (default, default);
            }

            return (args[0].ToLower(), args[1].Trim('"'));
        }
    }
}
