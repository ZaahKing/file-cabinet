using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Parser
{
    /// <summary>
    /// Or oteration.
    /// </summary>
    internal class OrOperator : BoolOperation, IExpressionBoolOperator
    {
        /// <inheritdoc/>
        public override bool Execute()
        {
            return this.OperandA.Execute() || this.OperandB.Execute();
        }
    }
}
