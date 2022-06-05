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
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
           PhuongThucSQL.Connect();
        }

        private Form Form;

        private void OpenForm(Form indexForm)
        {
            if (Form != null)
            {
                Form.Close();
            }
            Form = indexForm;
            indexForm.TopLevel = false;
            indexForm.Dock = DockStyle.Fill;
            pictureBox_Home.Controls.Add(indexForm);
            pictureBox_Home.Tag = indexForm;
            indexForm.BringToFront();
            indexForm.Show();
        }
        private void label_Thoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
            PhuongThucSQL.Disconnect();
        }

        private void pictureBox_Thoat_Click(object sender, EventArgs e)
        {
            Application.Exit();
            PhuongThucSQL.Disconnect();
        }

        private void pictureBox_Apple_Click(object sender, EventArgs e)
        {
            if (Form != null) Form.Close();
            label_Home.Text = "Trang Chủ";
        }
        private void label_SanPham_Click(object sender, EventArgs e)
        {
            OpenForm(new FormSP());
            label_Home.Text = label_SanPham.Text;
        }

        private void label_KhachHang_Click(object sender, EventArgs e)
        {
            OpenForm(new FormKH());
            label_Home.Text = label_NhanVien.Text;
        }
        private void label_PhanLoai_Click(object sender, EventArgs e)
        {
            OpenForm(new FormPL());
            label_Home.Text = label_PhanLoai.Text;
        }

        private void label_BanHang_Click(object sender, EventArgs e)
        {
            OpenForm(new FormBH());
            label_Home.Text = label_BanHang.Text;
        }

        private void label_Home_Click(object sender, EventArgs e)
        {

        }

        private void label_NhanVien_Click(object sender, EventArgs e)
        {
            OpenForm(new FormNV());
            label_Home.Text = label_NhanVien.Text;
        }

        private void label_HeThong_Click(object sender, EventArgs e)
        {
            OpenForm(new FormHT());
            label_Home.Text = label_HeThong.Text;
        }
    }
}
