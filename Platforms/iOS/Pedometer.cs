using CoreMotion;
using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2MD.Data;
using T2MD.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace T2MD
{
    partial class FeatureImplementation : IPedometer
    {
        readonly CMPedometer pedometer;

        public FeatureImplementation()
        {
            pedometer = new();
        }

        public bool IsSupported => CMPedometer.IsStepCountingAvailable;

        public bool IsMonitoring { get; private set; }

        public event EventHandler<PedometerData>? ReadingChanged;

        public async Task<int> GetCurrentReading()
        {
            if (!IsSupported)
            {
                throw new FeatureNotSupportedException();
            }

            DateTime start = DateTime.Now;
            DateTime.TryParse(Constants.StartDate, out start);

            DateTime end = DateTime.Now;

            if (start.Kind == DateTimeKind.Unspecified)
                start = DateTime.SpecifyKind(start, DateTimeKind.Local);

            if (end.Kind == DateTimeKind.Unspecified)
                end = DateTime.SpecifyKind(end, DateTimeKind.Local);

            var data = await pedometer.QueryPedometerDataAsync((NSDate)start, (NSDate)end);
            return (int)data.NumberOfSteps;
        }

        public void Start()
        {
            if (!IsSupported)
            {
                throw new FeatureNotSupportedException();
            }

            if (IsMonitoring)
            {
                return;
            }

            IsMonitoring = true;

            pedometer.StartPedometerUpdates(NSDate.Now, (data, error) =>
            {
                ReadingChanged?.Invoke(this, new()
                {
                    NumberOfSteps = (int)data.NumberOfSteps,
                    Timestamp = DateTimeOffset.Now,
                });
            });
        }

        public void Stop()
        {
            if (!IsSupported)
            {
                throw new FeatureNotSupportedException();
            }

            if (!IsMonitoring)
            {
                return;
            }

            pedometer.StopPedometerUpdates();

            IsMonitoring = false;
        }
    }
}
