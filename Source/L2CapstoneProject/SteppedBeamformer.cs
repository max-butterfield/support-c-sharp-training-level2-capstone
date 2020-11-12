using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NationalInstruments.ModularInstruments.NIRfsg;

namespace L2CapstoneProject
{
    class SteppedBeamformer : IBeamformer
    {
        private NIRfsg _rfsgSession;

        private bool generating = false;

        //PhaseAmplitudeOffset offset;

        public SteppedBeamformer()//needs a resource name and other values/info from controller form
        {
            
        }

        void IBeamformer.Connect(string resourceInput, double frequencyInput, double powerInput)
        {
            //power on

            //write to register?

            string resourceName = resourceInput;
            double frequency = frequencyInput;
            double power = powerInput;

            _rfsgSession = new NIRfsg(resourceName, true, false);
            _rfsgSession.RF.Configure(frequency, power);
        }

        void IBeamformer.Disconnect()
        {
            //write registers?

            //power off

            if(_rfsgSession != null)
            {
                _rfsgSession.RF.OutputEnabled = false;
                _rfsgSession.Close();
            }
            _rfsgSession = null;
            generating = false;
        }

        void WriteOffset(PhaseAmplitudeOffset offsetInput)
        {
            //write PhaseAmplitudeOffset to registers on the device
            
            if (generating)
            {
                _rfsgSession.Abort();
            }
            _rfsgSession.RF.PhaseOffset = offsetInput.PhaseOffset;
            _rfsgSession.RF.PowerLevel += offsetInput.AmplitudeOffset;

            if (generating)
            {
                _rfsgSession.Initiate();
            }
        }

        void StimulateDut()
        {
            _rfsgSession.Initiate();
            generating = true;
        }
    }
}
