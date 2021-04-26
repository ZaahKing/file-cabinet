﻿namespace FileCabinetApp.Parser
{
    /// <summary>
    /// Or oteration.
    /// </summary>
    internal class OrOperator : BoolOperation, IExpressionBoolOperator
    {
        /// <inheritdoc/>
        public override bool Execute(FileCabinetRecord record)
        {
            return this.OperandA.Execute(record) || this.OperandB.Execute(record);
        }
    }
}
