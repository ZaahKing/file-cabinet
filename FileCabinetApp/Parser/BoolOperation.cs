using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Parser
{
    /// <inheritdoc/>
    internal abstract class BoolOperation : IExpressionBoolOperation
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

        /// <inheritdoc/>
        public abstract bool Execute();
    }
}
