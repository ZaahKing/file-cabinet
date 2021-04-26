namespace FileCabinetApp.Parser
{
    /// <summary>
    /// Equal or bigger bool oprerator.
    /// </summary>
    internal class BiggerOrEqualOperator : ExpressionBoolOperator, IExpressionBoolOperator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BiggerOrEqualOperator"/> class.
        /// </summary>
        /// <param name="a">OperandA.</param>
        /// <param name="b">OperandB.</param>
        public BiggerOrEqualOperator(ExpressionElement a, ExpressionElement b)
        : base(a, b)
        {
        }

        /// <inheritdoc/>
        public override bool Execute(FileCabinetRecord record)
        {
            var compareFunc = ComparisonFilterBulder.GetComterisonFunction(this.OperandA.Execute(), this.OperandB.Execute(), (a, b) => a.CompareTo(b) >= 0);
            return compareFunc(record);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"{this.OperandA} >= {this.OperandB}";
        }
    }
}
