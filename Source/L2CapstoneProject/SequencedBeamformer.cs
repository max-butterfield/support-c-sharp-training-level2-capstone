using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.ModularInstruments.NIRfsg;

namespace L2CapstoneProject
{
    class SequencedBeamformer : IBeamformer
    {
        private NIRfsg _rfsgSession;

        List<PhaseAmplitudeOffset> offsets;

        public SequencedBeamformer()
        {
            //needs an initial offset list?
        }

        void IBeamformer.Connect(string resourceInput, double frequencyInput, double powerInput)
        {
            //power on

            //write to register

            string resourceName = resourceInput;
            double frequency = frequencyInput;
            double power = powerInput;

            _rfsgSession = new NIRfsg(resourceName, true, false);
            _rfsgSession.RF.Configure(frequency, power);
        }

        void IBeamformer.Disconnect()
        {
            //write registers

            //power off

            if (_rfsgSession != null)
            {
                _rfsgSession.RF.OutputEnabled = false;
                _rfsgSession.Close();
            }
            _rfsgSession = null;
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
