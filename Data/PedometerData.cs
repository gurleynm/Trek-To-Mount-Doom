using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace T2MD.Data
{
    public class PedometerData
    {
        /// <summary>
        /// Gets the timestamp for this sensor reading.
        /// </summary>
        public DateTimeOffset Timestamp { get; internal set; }

        /// <summary>
        /// Gets the total number of steps since the current measuring session started.
        /// </summary>
        public int NumberOfSteps { get; internal set; }
    }
}
