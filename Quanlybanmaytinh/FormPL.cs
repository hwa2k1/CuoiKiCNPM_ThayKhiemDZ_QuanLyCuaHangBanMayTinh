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
    public partial class FormPL : Form
    {
        DataTable tableMayTinh;
        string SQL;
        private void LoadDataSQL()
        {
            SQL = "SELECT IdMayTinh, TenMayTinh FROM tableMayTinh";

            tableMayTinh = PhuongThucSQL.GetData(SQL);
            dataGridView_PL.DataSource = tableMayTinh;
            dataGridView_PL.Columns[0].HeaderText = "ID HÃNG";
            dataGridView_PL.Columns[1].HeaderText = "TÊN HÃNG";
            dataGridView_PL.Columns[0].Width = 400;
            dataGridView_PL.Columns[1].Width = 650;
            dataGridView_PL.AllowUserToAddRows = false;
            dataGridView_PL.EditMode = DataGridViewEditMode.EditProgrammatically;

            foreach (DataGridViewColumn col in dataGridView_PL.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new Font("Times New Roman", 12F, FontStyle.Bold);
            }
        }
        public FormPL()
        {
            InitializeComponent();
        }

        private void FormPL_Load(object sender, EventArgs e)
        {
            textBox_IdMayTinh.Enabled = false;
            label_Luu.Enabled = false;
            label_Bo.Enabled = true;
            LoadDataSQL();
        }

        private void dataGridView_PL_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (label_Them.Enabled == false)
            {
                MessageBox.Show("Đang thực hiện việc thêm dữ liệu", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_IdMayTinh.Focus();
                return;
            }
            textBox_IdMayTinh.Text = dataGridView_PL.CurrentRow.Cells["IdMayTinh"].Value.ToString();
            textBox_TenMayTinh.Text = dataGridView_PL.CurrentRow.Cells["TenMayTinh"].Value.ToString();
            label_Sua.Enabled = true;
            label_Xoa.Enabled = true;
            label_Bo.Enabled = true;
        }

        private void ResetData()
        {
            textBox_IdMayTinh.Text = " ";
            textBox_TenMayTinh.Text = " ";
        }
        private void label_Them_Click(object sender, EventArgs e)
        {

            label_Them.Enabled = false;
            label_Xoa.Enabled = false;
            label_Sua.Enabled = false;
            label_Luu.Enabled = true;
            label_Bo.Enabled = true;
            ResetData();
            textBox_IdMayTinh.Enabled = true;
            textBox_IdMayTinh.Focus();
        }

        private void label_Xoa_Click(object sender, EventArgs e)
        {
            string SQL;
            if (textBox_IdMayTinh.Text == "") //nếu chưa chọn bản ghi nào
            {
                MessageBox.Show("Vui lòng chọn dữ liệu cần xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Xác nhận xoá dữ liệu ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                SQL = "DELETE tableMayTinh WHERE IdMayTinh = N'" + textBox_IdMayTinh.Text + " ' ";
                PhuongThucSQL.DeleteSQL(SQL);
                LoadDataSQL();
                ResetData();
            }
        }

        private void label_Sua_Click(object sender, EventArgs e)
        {
            string SQL;
            if (textBox_IdMayTinh.Text == " ")
            {
                MessageBox.Show("Vui lòng chọn dữ liệu cần sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (textBox_TenMayTinh.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập tên hãng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SQL = "UPDATE tableMayTinh SET TenMayTinh=N'" + textBox_TenMayTinh.Text.ToString() + "' WHERE IdMayTinh = N'" + textBox_IdMayTinh.Text + " ' ";
            PhuongThucSQL.OpenSQL(SQL);
            LoadDataSQL();
            ResetData();

            label_Bo.Enabled = true;
        }

        private void label_Luu_Click(object sender, EventArgs e)
        {
            string SQL;

            if (textBox_IdMayTinh.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập ID hãng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_IdMayTinh.Focus();
                return;
            }
            if (textBox_TenMayTinh.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập tên hãng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_TenMayTinh.Focus();
                return;
            }
            SQL = "Select IdMayTinh From tableMayTinh where IdMayTinh=N'" + textBox_IdMayTinh.Text.Trim() + " ' ";
            if (PhuongThucSQL.CheckPrimaryKey(SQL))
            {
                MessageBox.Show("ID hãng này đã tồn tại, hãy nhập một ID khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_IdMayTinh.Focus();
                return;
            }

            SQL = "INSERT INTO tableMayTinh VALUES(N'" + textBox_IdMayTinh.Text + "',N'" + textBox_TenMayTinh.Text + "')";
            PhuongThucSQL.OpenSQL(SQL);
            LoadDataSQL();
            ResetData();

            label_Them.Enabled = true;
            label_Xoa.Enabled = true;
            label_Sua.Enabled = true;
            label_Luu.Enabled = false;
            label_Bo.Enabled = true;
            textBox_IdMayTinh.Enabled = false;
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
            textBox_IdMayTinh.Enabled = false;
        }

        private void label_Thoat_Click(object sender, EventArgs e)
        {
            this.Close();
            
        }
    }
}
