using System;
using System.Windows.Forms;

namespace L2CapstoneProject
{
    public partial class frmOffset : Form
    {
        public PhaseAmplitudeOffset offset;

        public enum Mode { Add, Edit }

        public Mode ViewMode { get; }

        public frmOffset(Mode mode)
        {
            InitializeComponent();
            ViewMode = mode;

            switch (ViewMode)
            {
                case Mode.Add:
                    this.Text = "Add New Offset";
                    break;
                case Mode.Edit:
                    this.Text = "Edit Offset";
                    break;
            }
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            offset.AmplitudeOffset = decimal.ToDouble(this.numAmp.Value);
            offset.PhaseOffset = decimal.ToDouble(this.numPhase.Value);

            Close();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
