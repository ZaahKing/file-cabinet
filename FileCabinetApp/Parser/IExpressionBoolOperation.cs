namespace FileCabinetApp.Parser
{
    /// <summary>
    /// Describe executavble bool operation like 'and', 'or'.
    /// </summary>
    internal interface IExpressionBoolOperation : IExpressionBoolOperator
    {
        /// <summary>
        /// Gets or sets First operand.
        /// </summary>
        /// <value>
        /// First operand.
        /// </value>
        public IExpressionBoolOperator OperandA { get; set; }

        /// <summary>
        /// Gets or sets  Second operand.
        /// </summary>
        /// <value>
        /// Second operand.
        /// </value>
        public IExpressionBoolOperator OperandB { get; set; }
    }
}
