using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Quanlybanmaytinh
{
    public partial class FormHT : Form
    {
        public FormHT()
        {
            InitializeComponent();
        }

        private void pictureBox_QLHD_Click(object sender, EventArgs e)
        {
            FormQLHD form = new FormQLHD();
            form.StartPosition = FormStartPosition.CenterParent;
            form.ShowDialog();
        }

        private void pictureBox_TKDT_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đang phát triển, vui lòng quay lại sau !", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pictureBox_QLND_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Đang phát triển, vui lòng quay lại sau !", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
