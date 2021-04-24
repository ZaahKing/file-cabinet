namespace FileCabinetApp.Parser
{
    /// <summary>
    /// Expretion element.
    /// </summary>
    public class ExpressionElement
    {
        private readonly string element;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionElement"/> class.
        /// </summary>
        /// <param name="element">Element.</param>
        public ExpressionElement(string element)
        {
            this.element = element;
        }

        /// <summary>
        /// Execute element.
        /// </summary>
        /// <returns>Element.</returns>
        public string Execute()
        {
            return this.element;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return this.element;
        }
    }
}
