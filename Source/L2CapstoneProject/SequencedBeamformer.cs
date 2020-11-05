using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace L2CapstoneProject
{
    class SequencedBeamformer : IBeamformer
    {
        List<PhaseAmplitudeOffset> offsets;

        public SequencedBeamformer()
        {
            //needs an initial offset list?
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

        void ConfigureSequence()
        {
            //if dynamic

            //if static
        }

        void InitiateSequence()
        {

        }

        void AbortSequence()
        {

        }
    }
}
