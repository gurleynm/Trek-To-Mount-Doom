using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using T2MD.Interfaces;

namespace T2MD.Data
{
    partial class FeatureImplementation : IPedometer
    {
        // This usually is a placeholder as .NET MAUI apps typically don't run on .NET generic targets unless through unit tests and such
        public bool IsSupported => false;

        public bool IsMonitoring => throw new NotImplementedException();

        public event EventHandler<PedometerData>? ReadingChanged;

        public Task<int> GetCurrentReading()
        {
            throw new NotImplementedException();
        }

        public void Start()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
