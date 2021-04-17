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
    }
}
