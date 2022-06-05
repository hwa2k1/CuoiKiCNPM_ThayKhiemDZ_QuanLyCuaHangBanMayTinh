using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Quanlybanmaytinh
{
    public partial class FormQLHD : Form
    {
        DataTable tableQLHD;
        public FormQLHD()
        {
            InitializeComponent();
        }

        private void FormQLHD_Load(object sender, EventArgs e)
        {
            LoadDataSQL();
        }
        private void ResetData()
        {
            textBox_IdKH.Text = "";
            textBox_IdHD.Text = "";
            textBox_IdNV.Text = "";
            textBox_N.Text = "";
            textBox_TT.Text = "";
            textBox_TT.Text = "";
            LoadDataSQL();
        }

        private void dataGridView_QLHD_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            textBox_IdHD.Text = dataGridView_QLHD.CurrentRow.Cells["IdHoaDon"].Value.ToString();
            textBox_IdNV.Text = dataGridView_QLHD.CurrentRow.Cells["IdNhanVien"].Value.ToString();
            textBox_IdKH.Text = dataGridView_QLHD.CurrentRow.Cells["IdKhachHang"].Value.ToString();
        }

        private void label_Tim_Click(object sender, EventArgs e)
        {
            if ((textBox_IdHD.Text == "") && (textBox_T.Text == "") && (textBox_N.Text == "") && (textBox_IdNV.Text == "") && (textBox_IdKH.Text == "") && (textBox_TT.Text == ""))
            {
                MessageBox.Show("Vui lòng nhập dữ liệu để tìm kiếm !", "Thông tin", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            string SQL;
            SQL = "SELECT * FROM tableHoaDon WHERE 1 = 1";
            if (textBox_IdHD.Text != "")
                SQL = SQL + " AND IdHoaDon LIKE N'%" + textBox_IdHD.Text + "%'";
            if (textBox_T.Text != "")
                SQL = SQL + " AND MONTH(NgayBan) =" + textBox_T.Text;
            if (textBox_N.Text != "")
                SQL = SQL + " AND YEAR (NgayBan) =" + textBox_N.Text;
            if (textBox_IdNV.Text != "")
                SQL = SQL + " AND IdNhanVien Like N'%" + textBox_IdNV.Text + "%'";
            if (textBox_IdKH.Text != "")
                SQL = SQL + " AND IdKhachHang Like N'%" + textBox_IdKH.Text + "%'";
            if (textBox_TT.Text != "")
                SQL = SQL + " AND TongTien <=" + textBox_TT.Text;
            tableQLHD = PhuongThucSQL.GetData(SQL);
            if (tableQLHD.Rows.Count == 0)
            {
                MessageBox.Show("Không có dữ liệu trùng khớp !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
                MessageBox.Show("Có " + tableQLHD.Rows.Count + " dữ liệu trùng khớp !", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            dataGridView_QLHD.DataSource = tableQLHD;
        }
        private void LoadDataSQL()
        {
            string SQL;
            SQL = "SELECT * FROM tableHoaDon";
            tableQLHD = PhuongThucSQL.GetData(SQL);
            dataGridView_QLHD.DataSource = tableQLHD;
            dataGridView_QLHD.Columns[0].HeaderText = "ID Hóa Đơn";
            dataGridView_QLHD.Columns[1].HeaderText = "ID Nhân Viên";
            dataGridView_QLHD.Columns[2].HeaderText = "ID Khách Hàng";
            dataGridView_QLHD.Columns[3].HeaderText = "Ngày Bán";
            dataGridView_QLHD.Columns[4].HeaderText = "Tổng Tiền";
            dataGridView_QLHD.Columns[0].Width = 250;
            dataGridView_QLHD.Columns[1].Width = 150;
            dataGridView_QLHD.Columns[2].Width = 150;
            dataGridView_QLHD.Columns[3].Width = 160;
            dataGridView_QLHD.Columns[4].Width = 250;
            dataGridView_QLHD.AllowUserToAddRows = false;
            dataGridView_QLHD.EditMode = DataGridViewEditMode.EditProgrammatically;
            foreach (DataGridViewColumn col in dataGridView_QLHD.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new Font("Times New Roman", 12F, FontStyle.Bold);
            }
        }

        private void label_Thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label_Bo_Click(object sender, EventArgs e)
        {
            ResetData();
        }

        private void dataGridView_QLHD_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string IdHD;
            if (MessageBox.Show("Hiển thị thông tin chi tiết ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                IdHD = dataGridView_QLHD.CurrentRow.Cells["IdHoaDon"].Value.ToString();
                FormBH form = new FormBH();
                form.textBox_IdHD.Text = IdHD;
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
            }
        }

        private void label_Xoa_Click(object sender, EventArgs e)
        {
            string IdHD;
            if (MessageBox.Show("Hiển thị thông tin chi tiết ?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                IdHD = dataGridView_QLHD.CurrentRow.Cells["IdHoaDon"].Value.ToString();
                FormBH form = new FormBH();
                form.textBox_IdHD.Text = IdHD;
                form.StartPosition = FormStartPosition.CenterParent;
                form.ShowDialog();
            }
        }
    }
}
