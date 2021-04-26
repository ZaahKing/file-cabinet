namespace FileCabinetApp.Parser
{
    /// <summary>
    /// And operation.
    /// </summary>
    internal class AndOperation : BoolOperation, IExpressionBoolOperator
    {
        /// <inheritdoc/>
        public override bool Execute(FileCabinetRecord record)
        {
            return this.OperandA.Execute(record) && this.OperandB.Execute(record);
        }
    }
}
