using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2MD.Data;

namespace T2MD.Interfaces
{
    public interface IPedometer
    {
        /// <summary>
        /// Gets a value indicating whether reading the pedometer is supported on this device.
        /// </summary>
        bool IsSupported { get; }

        /// <summary>
        /// Gets a value indicating whether the pedometer is actively being monitored.
        /// </summary>
        bool IsMonitoring { get; }

        /// <summary>
        /// Occurs when pedometer reading changes.
        /// </summary>
        event EventHandler<PedometerData>? ReadingChanged;

        /// <summary>
        /// Start monitoring for changes to the pedometer.
        /// </summary>
        /// <remarks>
        /// Will throw <see cref="FeatureNotSupportedException"/> if not supported on device.
        /// </remarks>
        void Start();

        /// <summary>
        /// Stop monitoring for changes to the pedometer.
        /// </summary>
        void Stop();

        /// <summary>
        /// Grab current reading
        /// </summary>
        Task<int> GetCurrentReading();
    }
}
