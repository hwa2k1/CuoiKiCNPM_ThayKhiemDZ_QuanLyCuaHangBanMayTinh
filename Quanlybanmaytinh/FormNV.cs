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
    public partial class FormNV : Form
    {
        DataTable tableNhanVien;
        string SQL, checkGT;
        public FormNV()
        {
            InitializeComponent();
        }
        
        private void LoadDataSQL()
        {
            SQL = "SELECT IdNhanVien, TenNhanVien, GioiTinh, DiaChi, SdtNhanVien FROM tableNhanVien";
            tableNhanVien = PhuongThucSQL.GetData(SQL); 
            dataGridView_NV.DataSource = tableNhanVien;
            dataGridView_NV.Columns[0].HeaderText = "ID NV";
            dataGridView_NV.Columns[1].HeaderText = "TÊN NHÂN VIÊN";
            dataGridView_NV.Columns[2].HeaderText = "GIỚI TÍNH";
            dataGridView_NV.Columns[3].HeaderText = "ĐỊA CHỈ";
            dataGridView_NV.Columns[4].HeaderText = "ĐIỆN THOẠI";
      
            dataGridView_NV.Columns[0].Width = 100;
            dataGridView_NV.Columns[1].Width = 250;
            dataGridView_NV.Columns[2].Width = 100;
            dataGridView_NV.Columns[3].Width = 400;
            dataGridView_NV.Columns[4].Width = 200;

            dataGridView_NV.AllowUserToAddRows = false;
            dataGridView_NV.EditMode = DataGridViewEditMode.EditProgrammatically;

            foreach (DataGridViewColumn col in dataGridView_NV.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new Font("Times New Roman", 12F, FontStyle.Bold);
            }
        }

        private void FormNV_Load(object sender, EventArgs e)
        {
            textBox_IdNV.Enabled = false;
            label_Luu.Enabled = false;
            label_Bo.Enabled = true;
            LoadDataSQL();
        }

        private void dataGridView_NV_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (label_Them.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_IdNV.Focus();
                return;
            }
            textBox_IdNV.Text = dataGridView_NV.CurrentRow.Cells["IdNhanVien"].Value.ToString();
            textBox_TenNV.Text = dataGridView_NV.CurrentRow.Cells["TenNhanVien"].Value.ToString();
          
            if (dataGridView_NV.CurrentRow.Cells["GioiTinh"].Value.ToString() == "Nam") checkBox_Nam.Checked = true;
            else checkBox_Nam.Checked = false;
            textBox_DC.Text = dataGridView_NV.CurrentRow.Cells["DiaChi"].Value.ToString();
            textBox_SDT.Text = dataGridView_NV.CurrentRow.Cells["SdtNhanVien"].Value.ToString();

            label_Sua.Enabled = true;
            label_Xoa.Enabled = true;
            label_Bo.Enabled = true;
        }

        private void textBox_SDT_TextChanged(object sender, EventArgs e)
        {
                if (System.Text.RegularExpressions.Regex.IsMatch(textBox_SDT.Text, "[^0-9]" ))
                {
                MessageBox.Show("Vui lòng chỉ nhập số", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_SDT.Text = textBox_SDT.Text.Remove(textBox_SDT.Text.Length - 1);
                }
        }

        private void ResetData()
        {
            textBox_IdNV.Text = "";
            textBox_TenNV.Text = "";
            textBox_DC.Text = "";
            textBox_SDT.Text = "";
            checkBox_Nam.Checked = false;
        }

        private void label_Them_Click(object sender, EventArgs e)
        {
            label_Them.Enabled = false;
            label_Xoa.Enabled = false;
            label_Sua.Enabled = false;
            label_Luu.Enabled = true;
            label_Bo.Enabled = true;
            ResetData();
            textBox_IdNV.Enabled = true;
            textBox_IdNV.Focus();
        }

        private void label_Xoa_Click(object sender, EventArgs e)
        {
            if (textBox_IdNV.Text == " ")
            {
                MessageBox.Show("Vui lòng chọn dữ liệu cần xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Xác nhận xóa dữ liệu?", "Thông báo", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)
            {
                SQL = "DELETE tableNhanVien WHERE IdNhanVien= N'" + textBox_IdNV.Text + " ' ";
                PhuongThucSQL.DeleteSQL(SQL);
                LoadDataSQL();
                ResetData();
            }
        }

        private void label_Sua_Click(object sender, EventArgs e)
        {
            if (textBox_IdNV.Text == "")
            {
                MessageBox.Show("Vui lòng chọn dữ liệu cần sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (textBox_TenNV.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_TenNV.Focus();
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

            SQL = "UPDATE tableNhanVien SET TenNhanVien=N'" + textBox_TenNV.Text.Trim().ToString()
                                            + "',DiaChi=N'" + textBox_DC.Text.Trim().ToString()
                                            + "',SdtNhanVien='" + textBox_SDT.Text.ToString()
                                            + "',GioiTinh=N'" + checkGT
                                            + "' WHERE IdNhanVien=N'" + textBox_IdNV.Text + "'";

            PhuongThucSQL.OpenSQL(SQL);
            LoadDataSQL();
            ResetData();
            label_Bo.Enabled = true;
        }

        private void label_Luu_Click(object sender, EventArgs e)
        {
            if (textBox_IdNV.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập ID nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_IdNV.Focus();
                return;
            }

            if (textBox_TenNV.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập tên nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_TenNV.Focus();
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

            SQL = "SELECT IdNhanVien FROM tableNhanVien WHERE IdNhanVien = N'" + textBox_IdNV.Text.Trim() + " ' ";
            if (PhuongThucSQL.CheckPrimaryKey(SQL))
            {
                MessageBox.Show("ID nhân viên này đã tồn tại, hãy nhập một ID khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_IdNV.Focus();
                textBox_IdNV.Text = " ";
                return;
            }

            SQL = "INSERT INTO tableNhanVien (IdNhanVien,TenNhanVien,GioiTinh, DiaChi, SdtNhanVien) VALUES (N'" + textBox_IdNV.Text.Trim() + "',N'" + textBox_TenNV.Text.Trim() + "',N'" + checkGT + "',N'" + textBox_DC.Text.Trim() + "','" + textBox_SDT.Text + "')";
            PhuongThucSQL.OpenSQL(SQL);
            LoadDataSQL();
            ResetData();

            label_Them.Enabled = true;
            label_Xoa.Enabled = true;
            label_Sua.Enabled = true;
            label_Luu.Enabled = false;
            label_Bo.Enabled = true;
            textBox_IdNV.Enabled = false;
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
            textBox_IdNV.Enabled = false;
        }

        private void label_Thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

    }
}
