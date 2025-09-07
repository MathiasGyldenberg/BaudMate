/*
    BaudMate
    Author: Mathias Gyldenberg (https://github.com/MathiasGyldenberg)
    License: GNU General Public License v3.0 (GPLv3)

    Disclaimer:
    BaudMate is provided as open source software under the GPLv3 license. It is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
    All modifications and derivative works must also remain open source under the same license.
*/

namespace BaudMate
{
    public partial class FrmSnapshot : Form
    {
        public FrmSnapshot()
        {
            InitializeComponent();
        }

        private void FrmSnapshot_Load(object sender, EventArgs e)
        {
            this.Icon = PrgResource.MainIcon;
        }

        private void FrmSnapshot_Shown(object sender, EventArgs e)
        {
            ObjText.ScrollToCaret();
            ObjText.DeselectAll();
        }
    }
}
