using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2CapstoneProject
{
    class SteppedBeamformer : IBeamformer
    {
        PhaseAmplitudeOffset offset;

        public SteppedBeamformer()
        {
            //needs an initial offset?
        }

        void IBeamformer.Connect()
        {
            //power on

            //write to register
        }

        void IBeamformer.Disconnect()
        {
            //write registers

            //power off
        }

        void WriteOffset()
        {
            //write PhaseAmplitudeOffset to registers on the device
        }
    }
}
