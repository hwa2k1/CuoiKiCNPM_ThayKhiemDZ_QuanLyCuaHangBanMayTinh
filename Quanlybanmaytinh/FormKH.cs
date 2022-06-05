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
    public partial class FormKH : Form
    {
        DataTable tableKhachHang;
        string SQL, checkGT;

        public FormKH()
        {
            InitializeComponent();
        }

        private void LoadDataSQL()
        {
            string SQL;
            SQL = "SELECT IdKhachHang, TenKhachHang, GioiTinh, DiaChiKH, SdtKH FROM tableKhachHang";
            tableKhachHang = PhuongThucSQL.GetData(SQL); 
            dataGridView_KH.DataSource = tableKhachHang;
            dataGridView_KH.Columns[0].HeaderText = "ID KH";
            dataGridView_KH.Columns[1].HeaderText = "TÊN KHÁCH HÀNG";
            dataGridView_KH.Columns[2].HeaderText = "GIỚI TÍNH";
            dataGridView_KH.Columns[3].HeaderText = "ĐỊA CHỈ";
            dataGridView_KH.Columns[4].HeaderText = "ĐIỆN THOẠI";

            dataGridView_KH.Columns[0].Width = 100;
            dataGridView_KH.Columns[1].Width = 250;
            dataGridView_KH.Columns[2].Width = 100;
            dataGridView_KH.Columns[3].Width = 400;
            dataGridView_KH.Columns[4].Width = 200;

            dataGridView_KH.AllowUserToAddRows = false;
            dataGridView_KH.EditMode = DataGridViewEditMode.EditProgrammatically;

            foreach (DataGridViewColumn col in dataGridView_KH.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new Font("Times New Roman", 12F, FontStyle.Bold);
            }
        }

        private void FormKH_Load(object sender, EventArgs e)
        {
            textBox_IdKH.Enabled = false;
            label_Luu.Enabled = false;
            label_Bo.Enabled = true;
            LoadDataSQL();
        }

        private void textBox_SDT_TextChanged(object sender, EventArgs e)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(textBox_SDT.Text, "[^0-9]"))
            {
                MessageBox.Show("Vui lòng chỉ nhập số", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_SDT.Text = textBox_SDT.Text.Remove(textBox_SDT.Text.Length - 1);
            }
        }

        private void dataGridView_KH_CellContentClick_1(object sender, DataGridViewCellEventArgs e)
        {
            if (label_Them.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_IdKH.Focus();
                return;
            }
            textBox_IdKH.Text = dataGridView_KH.CurrentRow.Cells["IdKhachHang"].Value.ToString();
            textBox_TenKH.Text = dataGridView_KH.CurrentRow.Cells["TenKhachHang"].Value.ToString();

            if (dataGridView_KH.CurrentRow.Cells["GioiTinh"].Value.ToString() == "Nam") checkBox_Nam.Checked = true;
            else checkBox_Nam.Checked = false;
            textBox_DC.Text = dataGridView_KH.CurrentRow.Cells["DiaChiKH"].Value.ToString();
            textBox_SDT.Text = dataGridView_KH.CurrentRow.Cells["SdtKH"].Value.ToString();

            label_Sua.Enabled = true;
            label_Xoa.Enabled = true;
            label_Bo.Enabled = true;
        }

        private void label_Them_Click(object sender, EventArgs e)
        {
            label_Them.Enabled = false;
            label_Xoa.Enabled = false;
            label_Sua.Enabled = false;
            label_Luu.Enabled = true;
            label_Bo.Enabled = true;
            ResetData();
            textBox_IdKH.Enabled = true;
            textBox_IdKH.Focus();
        }

        private void label_Xoa_Click(object sender, EventArgs e)
        {
            if (textBox_IdKH.Text == " ")
            {
                MessageBox.Show("Vui lòng chọn dữ liệu cần xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Xác nhận xóa dữ liệu?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                SQL = "DELETE tableKhachHang WHERE IdKhachHang= N'" + textBox_IdKH.Text + " ' ";
                PhuongThucSQL.DeleteSQL(SQL);
                LoadDataSQL();
                ResetData();
            }
        }

        private void label_Sua_Click(object sender, EventArgs e)
        {
            if (textBox_IdKH.Text == "")
            {
                MessageBox.Show("Vui lòng chọn dữ liệu cần sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (textBox_TenKH.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_TenKH.Focus();
                return;
            }
            if (textBox_DC.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập địa chỉ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_DC.Focus();
                return;
            }
            if (textBox_SDT.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập số điện thoại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_SDT.Focus();
                return;
            }

            if (checkBox_Nam.Checked == true)
                checkGT = "Nam";
            else
                checkGT = "Nữ";

            SQL = "UPDATE tableKhachHang SET TenKhachHang=N'" + textBox_TenKH.Text.Trim().ToString()
                                            + "',DiaChiKH=N'" + textBox_DC.Text.Trim().ToString()
                                            + "',SdtKH='" + textBox_SDT.Text.ToString()
                                            + "',GioiTinh=N'" + checkGT
                                            + "' WHERE IdKhachHang=N'" + textBox_IdKH.Text + "'";

            PhuongThucSQL.OpenSQL(SQL);
            LoadDataSQL();
            ResetData();
            label_Bo.Enabled = true;
        }

        private void label_Luu_Click(object sender, EventArgs e)
        {
            if (textBox_IdKH.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập ID khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_IdKH.Focus();
                return;
            }

            if (textBox_TenKH.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập tên khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_TenKH.Focus();
                return;
            }

            if (textBox_SDT.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập số điện thoại", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_SDT.Focus();
                return;
            }

            if (textBox_DC.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập địa chỉ", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_DC.Focus();
                return;
            }

            if (checkBox_Nam.Checked == true)
                checkGT = "Nam";
            else
                checkGT = "Nữ";

            SQL = "SELECT IdKhachHang FROM tableKhachHang WHERE IdKhachHang = N'" + textBox_IdKH.Text.Trim() + " ' ";
            if (PhuongThucSQL.CheckPrimaryKey(SQL))
            {
                MessageBox.Show("ID khách hàng này đã tồn tại, hãy nhập một ID khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_IdKH.Focus();
                textBox_IdKH.Text = " ";
                return;
            }

            SQL = "INSERT INTO tableKhachHang (IdKhachHang, TenKhachHang, GioiTinh, DiaChiKH, SdtKH) VALUES (N'" + textBox_IdKH.Text.Trim() + "',N'" + textBox_TenKH.Text.Trim() + "',N'" + checkGT + "',N'" + textBox_DC.Text.Trim() + "','" + textBox_SDT.Text + "')";
            PhuongThucSQL.OpenSQL(SQL);
            LoadDataSQL();
            ResetData();

            label_Them.Enabled = true;
            label_Xoa.Enabled = true;
            label_Sua.Enabled = true;
            label_Luu.Enabled = false;
            label_Bo.Enabled = true;
            textBox_IdKH.Enabled = false;
        }

        private void label_Bo_Click(object sender, EventArgs e)
        {
            ResetData();
            LoadDataSQL();
            label_Them.Enabled = true;
            label_Xoa.Enabled = true;
            label_Sua.Enabled = true;
            label_Luu.Enabled = false;
            label_Bo.Enabled = true;
            textBox_IdKH.Enabled = false;
        }

        private void label_Thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ResetData()
        {
            textBox_IdKH.Text = "";
            textBox_TenKH.Text = "";
            textBox_DC.Text = "";
            textBox_SDT.Text = "";
            checkBox_Nam.Checked = false;
        }
    }
}
