namespace FileCabinetApp.CommandHendlers
{
    /// <summary>
    /// Command handler interface.
    /// </summary>
    public interface ICommandHandler
    {
        /// <summary>
        /// Set next command hendler.
        /// </summary>
        /// <param name="handler">Handler.</param>
        void SetNext(ICommandHandler handler);

        /// <summary>
        /// Meke job here and handle next.
        /// </summary>
        /// <param name="commandRequest">Command request.</param>
        void Handle(AppCommandRequest commandRequest);
    }
}
