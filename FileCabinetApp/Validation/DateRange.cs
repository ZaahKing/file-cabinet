using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validation
{
    /// <summary>
    /// Range.
    /// </summary>
    public class DateRange
    {
        /// <summary>
        /// Gets or sets start date.
        /// </summary>
        /// <value>
        /// Start date.
        /// </value>
        public DateTime From { get; set; }

        /// <summary>
        /// Gets or sets end date.
        /// </summary>
        /// <value>
        /// End date.
        /// </value>
        public DateTime To { get; set; }
    }
}
