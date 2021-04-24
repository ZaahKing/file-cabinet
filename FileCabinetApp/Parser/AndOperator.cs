using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Parser
{
    /// <summary>
    /// And operation.
    /// </summary>
    internal class AndOperator : BoolOperation, IExpressionBoolOperator
    {
        /// <inheritdoc/>
        public override bool Execute()
        {
            return this.OperandA.Execute() && this.OperandB.Execute();
        }
    }
}
