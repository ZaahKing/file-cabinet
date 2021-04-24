namespace FileCabinetApp.Parser
{
    /// <inheritdoc/>
    internal abstract class ExpressionBoolOperator : IExpressionBoolOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExpressionBoolOperator"/> class.
        /// </summary>
        /// <param name="a">operandA.</param>
        /// <param name="b">operandB.</param>
        protected ExpressionBoolOperator(ExpressionElement a, ExpressionElement b)
        {
            this.OperandA = a;
            this.OperandB = b;
        }

        /// <summary>
        /// Gets OperandA.
        /// </summary>
        /// <value>
        /// OperandA.
        /// </value>
        protected ExpressionElement OperandA { get; init; }

        /// <summary>
        /// Gets OperandB.
        /// </summary>
        /// <value>
        /// OperandB.
        /// </value>
        protected ExpressionElement OperandB { get; init; }

        /// <inheritdoc/>
        public abstract bool Execute();
    }
}
