namespace FileCabinetApp.Parser
{
    /// <summary>
    /// Equal or less bool oprerator.
    /// </summary>
    internal class LessOrEqualOperator : ExpressionBoolOperator, IExpressionBoolOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LessOrEqualOperator"/> class.
        /// </summary>
        /// <param name="a">OperandA.</param>
        /// <param name="b">OperandB.</param>
        public LessOrEqualOperator(ExpressionElement a, ExpressionElement b)
        : base(a, b)
        {
        }

        /// <inheritdoc/>
        public override bool Execute(FileCabinetRecord record)
        {
            var compareFunc = ComparisonFilterBulder.GetComterisonFunction(this.OperandA.Execute(), this.OperandB.Execute(), (a, b) => a.CompareTo(b) <= 0);
            return compareFunc(record);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{this.OperandA} <= {this.OperandB}";
        }
    }
}
