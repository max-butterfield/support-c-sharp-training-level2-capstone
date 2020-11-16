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
        protected NIRfsg _rfsgSession;
        
        protected Dictionary<string, List<PhaseAmplitudeOffset>> _allOffsets;
        protected List<PhaseAmplitudeOffset> _activeOffsets;

        double _externalAttenuation;
        double _refClockFreq;
        string _ports;

        double _bbRate;
        double _bbPeriod;
        int _bbSamples;

        public SequencedBeamformer()
        {
            //needs an initial offset list?
            _allOffsets = new Dictionary<string, List<PhaseAmplitudeOffset>>();
            _activeOffsets = new List<PhaseAmplitudeOffset>();

            //set carrier frequency and power
            _externalAttenuation = 0.0; // dB
            _refClockFreq = 10.0e6; // MHz
            _ports = "";

            _bbRate = 10.0e6;
            _bbPeriod = 100.0e-6;
            _bbSamples = (int)Math.Round(_bbRate * _bbPeriod);
        }

        void IBeamformer.Connect(string resource, double freq, double power)
        {
            //power on

            //write to register

            // Initiate session and configure setup
            _rfsgSession = new NIRfsg(resource, true, false);
            _rfsgSession.SignalPath.SelectedPorts = _ports;
            _rfsgSession.FrequencyReference.Configure(RfsgFrequencyReferenceSource.OnboardClock, _refClockFreq);

            // Set system for waveform generation
            _rfsgSession.Arb.GenerationMode = RfsgWaveformGenerationMode.ArbitraryWaveform;
            _rfsgSession.RF.Configure(freq, power);
            _rfsgSession.RF.ExternalGain = -1.0 * _externalAttenuation;

            // Set the desired user sample rate and signal bandwidth
            _rfsgSession.Arb.IQRate = _bbRate;
            _bbRate = _rfsgSession.Arb.IQRate; // Get the sample rate actually chosen by ARB
            _rfsgSession.Arb.SignalBandwidth = _bbRate * 0.8;
        }

        void IBeamformer.Disconnect()
        {
            //write registers

            //power off

            if (_rfsgSession != null)
            {
                _rfsgSession.RF.OutputEnabled = false;
                _rfsgSession.Close();
                _rfsgSession = null;
            }
        }

        void ConfigureSequence(string name, List<PhaseAmplitudeOffset> offsets)
        {
            bool offsetSaved = _allOffsets.ContainsKey(name);

            if (!offsetSaved)
            {
                int size = (int) (offsets.Count * _bbSamples);
                double[] iData = new double[size];
                double[] qData = new double[size];
                int index = 0;

                // Havent seen this sequence before, lets built it for the user
                foreach (var offset in offsets)
                {
                    double[] iDataTmp = new double[_bbSamples];
                    double[] qDataTmp = new double[_bbSamples];

                    GenerateIQTone(ref iDataTmp, ref qDataTmp, _bbRate, _bbPeriod, 0.0e6, 1.0, 0.0);

                    for (int i = 0; i < _bbSamples; i++)
                    {
                        iData[(index * _bbSamples) + i] = iDataTmp[i];
                        qData[(index * _bbSamples) + i] = qDataTmp[i];
                    }
                    index++;
                }

                // Load waveform into the generator
                _allOffsets.Add(name, offsets);
                _rfsgSession.Arb.WriteWaveform(name, iData, qData);
            }
        }

        void InitiateSequence()
        {

        }

        void AbortSequence()
        {

        }

        private void GenerateIQTone(ref double[] iData, ref double[] qData, double bbRate, double bbPeriod, 
                                    double toneFreq = 0.0, double ampitudeOffset=1.0, double phaseOffset=0.0)
        {
            int iqSamples = (int)Math.Round(bbRate*bbPeriod);
            double phaseDegrees = 0.0 + phaseOffset;
            double phase90Degrees = 90.0 + phaseOffset;

            for (int i = 0; i < iqSamples; i++)
            {
                iData[i] = ampitudeOffset * Math.Sin((2 * Math.PI * i * toneFreq) / _bbRate + Math.PI * phaseDegrees / 180);
                qData[i] = ampitudeOffset * Math.Sin((2 * Math.PI * i * toneFreq) / _bbRate - Math.PI * phase90Degrees / 180);
            }
        }
    }
}
