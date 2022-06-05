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

    public partial class FormSP : Form
    {
        DataTable tableHangHoa;
        string SQL;
        public FormSP()
        {
            InitializeComponent();
        }
        private void LoadDataSQL()
        {
            SQL = "SELECT * from tableHangHoa";
            tableHangHoa = PhuongThucSQL.GetData(SQL);
            dataGridView_SP.DataSource = tableHangHoa;
            dataGridView_SP.Columns[0].HeaderText = "ID SP";
            dataGridView_SP.Columns[1].HeaderText = "TÊN SẢN PHẨM";
            dataGridView_SP.Columns[2].HeaderText = "ID HÃNG";
            dataGridView_SP.Columns[3].HeaderText = "GIÁ NHẬP VÀO";
            dataGridView_SP.Columns[4].HeaderText = "GIÁ BÁN RA";
            dataGridView_SP.Columns[5].HeaderText = "MÔ TẢ";
            dataGridView_SP.Columns[6].HeaderText = "SỐ LƯỢNG";
            dataGridView_SP.Columns[7].HeaderText = "LINK ẢNH";

            dataGridView_SP.Columns[0].Width = 100;
            dataGridView_SP.Columns[1].Width = 250;
            dataGridView_SP.Columns[2].Width = 100;
            dataGridView_SP.Columns[3].Width = 130;
            dataGridView_SP.Columns[4].Width = 130;
            dataGridView_SP.Columns[5].Width = 200;
            dataGridView_SP.Columns[6].Width = 100;
            dataGridView_SP.Columns[7].Width = 100;

            dataGridView_SP.AllowUserToAddRows = false;
            dataGridView_SP.EditMode = DataGridViewEditMode.EditProgrammatically;

            foreach (DataGridViewColumn col in dataGridView_SP.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new Font("Times New Roman", 12F, FontStyle.Bold);
            }
        }
        private void ResetData()
        {
            textBox_IdSP.Text = "";
            textBox_TenSP.Text = "";
            comboBox_PL.Text = "";
            textBox_SL.Text = "0";
            textBox_GiaNhap.Text = "0";
            textBox_GiaBan.Text = "0";
            textBox_MT.Text = "";
            textBox_LinkAnh.Text = "";
            pictureBox_SP.Image = null;
        }

        private void label_Them_Click(object sender, EventArgs e)
        {
            label_Them.Enabled = false;
            label_Xoa.Enabled = false;
            label_Sua.Enabled = false;
            label_Luu.Enabled = true;
            label_Bo.Enabled = true;
            ResetData();

            textBox_IdSP.Enabled = true;
            textBox_IdSP.Focus();
            textBox_SL.Enabled = true;
            textBox_GiaNhap.Enabled = true;
            textBox_GiaBan.Enabled = true;
        }

        private void dataGridView_SP_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (label_Them.Enabled == false)
            {
                MessageBox.Show("Đang ở chế độ thêm mới!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_IdSP.Focus();
                return;
            }
            textBox_IdSP.Text = dataGridView_SP.CurrentRow.Cells["IdHangHoa"].Value.ToString();
            textBox_TenSP.Text = dataGridView_SP.CurrentRow.Cells["TenHangHoa"].Value.ToString();
            string IdMayTinh = dataGridView_SP.CurrentRow.Cells["IdMayTinh"].Value.ToString();

            SQL = "SELECT TenMayTinh FROM tableMayTinh WHERE IdMayTinh = N'" + IdMayTinh + "'";
            comboBox_PL.Text = PhuongThucSQL.GetValue(SQL);
            textBox_SL.Text = dataGridView_SP.CurrentRow.Cells["SoLuong"].Value.ToString();
            textBox_GiaNhap.Text = dataGridView_SP.CurrentRow.Cells["GiaNhap"].Value.ToString();
            textBox_GiaBan.Text = dataGridView_SP.CurrentRow.Cells["GiaBan"].Value.ToString();
            textBox_MT.Text = dataGridView_SP.CurrentRow.Cells["MoTa"].Value.ToString();
            textBox_LinkAnh.Text = dataGridView_SP.CurrentRow.Cells["LinkAnh"].Value.ToString();
            pictureBox_SP.Image = Image.FromFile(textBox_LinkAnh.Text);

            label_Sua.Enabled = true;
            label_Xoa.Enabled = true;
            label_Bo.Enabled = true;
        }

        private void label_Luu_Click(object sender, EventArgs e)
        {
            if (textBox_IdSP.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập ID sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_IdSP.Focus();
                return;
            }

            if (textBox_TenSP.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_TenSP.Focus();
                return;
            }
            if (comboBox_PL.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập tên hãng SX", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox_PL.Focus();
                return;
            }
            if (textBox_LinkAnh.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng thêm ảnh minh hoạ cho sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                pictureBox_SP.Focus();
                return;
            }

            SQL = "SELECT IdHangHoa FROM tableHangHoa WHERE IdHangHoa=N'" + textBox_IdSP.Text.Trim() + " ' ";
            if (PhuongThucSQL.CheckPrimaryKey(SQL))
            {
                MessageBox.Show("ID sản phẩm này đã tồn tại,hãy chọn một ID khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_IdSP.Focus();
                return;
            }

            SQL = "INSERT INTO tableHangHoa (IdHangHoa,TenHangHoa,IdMayTinh,SoLuong,GiaNhap,GiaBan,MoTa,LinkAnh) VALUES(N'"
                + textBox_IdSP.Text.Trim() + "',N'" + textBox_TenSP.Text.Trim() +
                "',N'" + comboBox_PL.SelectedValue.ToString() +
                "'," + textBox_SL.Text.Trim() + "," + textBox_GiaNhap.Text +
                "," + textBox_GiaBan.Text + ",N'" + textBox_MT.Text.Trim() + "',N'" + textBox_LinkAnh.Text + "')";

            PhuongThucSQL.OpenSQL(SQL);
            LoadDataSQL();
            ResetData();

            label_Them.Enabled = true;
            label_Xoa.Enabled = true;
            label_Sua.Enabled = true;
            label_Luu.Enabled = false;
            label_Bo.Enabled = true;
            textBox_IdSP.Enabled = false;
        }

        private void label_Xoa_Click(object sender, EventArgs e)
        {
            if (textBox_IdSP.Text == "")
            {
                MessageBox.Show("Vui lòng chọn dữ liệu cần xóa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (MessageBox.Show("Xác nhận xoá dữ liệu?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                SQL = "DELETE tableHangHoa WHERE IdHangHoa = N'" + textBox_IdSP.Text + "'";
                PhuongThucSQL.DeleteSQL(SQL);
                LoadDataSQL();
                ResetData();
            }
        }

        private void label_Sua_Click(object sender, EventArgs e)
        {
            if (textBox_IdSP.Text == "")
            {
                MessageBox.Show("Vui lòng chọn dữ liệu cần sửa", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_IdSP.Focus();
                return;
            }
            if (textBox_TenSP.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập tên sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_TenSP.Focus();
                return;
            }
            if (comboBox_PL.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng chọn hãng SX", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox_PL.Focus();
                return;
            }
            if (textBox_LinkAnh.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng thêm ảnh minh hoạ cho sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                pictureBox_SP.Focus();
                return;
            }

            SQL = "UPDATE tableHangHoa SET TenHangHoa=N'" + textBox_TenSP.Text.Trim().ToString() +
                "',IdMayTinh=N'" + comboBox_PL.SelectedValue.ToString() +
                "',GiaNhap=" + textBox_GiaNhap.Text +
                ",GiaBan=" + textBox_GiaBan.Text +
                ",SoLuong=" + textBox_SL.Text +
                ",MoTa=N'" + textBox_MT.Text + "',LinkAnh=N'" + textBox_LinkAnh.Text + "' WHERE IdHangHoa=N'" + textBox_IdSP.Text + "'";

            PhuongThucSQL.OpenSQL(SQL);
            LoadDataSQL();
            ResetData();
            label_Bo.Enabled = true;
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
            textBox_IdSP.Enabled = false;
        }

        private void label_Thoat_Click(object sender, EventArgs e)
        {
            MainForm form = new MainForm();
        }

        private void FormSP_Load(object sender, EventArgs e)
        {
            string SQL;
            SQL = "SELECT * FROM tableMayTinh";
            textBox_IdSP.Enabled = false;
            label_Luu.Enabled = false;
            label_Bo.Enabled = true;
            LoadDataSQL();
            PhuongThucSQL.DataComboBox(SQL, comboBox_PL, "IdMayTinh", "TenMayTinh");
            comboBox_PL.SelectedIndex = -1;
            ResetData();
        }

        private void textBox_SL_TextChanged(object sender, EventArgs e)
        {

        }

        private void label_TimKiem_Click(object sender, EventArgs e)
        {
            string SQL;
            if ((textBox_IdSP.Text == " ") && (textBox_TenSP.Text == " ") && (comboBox_PL.Text == " ") && (textBox_MT.Text == " "))
            {
                MessageBox.Show("Vui lòng nhập dữ liệu tìm kiếm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            SQL = "SELECT * FROM tableHangHoa WHERE 1 = 1";
            if (textBox_IdSP.Text != "")
                SQL += " AND IdHangHoa LIKE N'%" + textBox_IdSP.Text + "%'";
            if (textBox_TenSP.Text != "")
                SQL += " AND TenHangHoa LIKE N'%" + textBox_TenSP.Text + "%'";
            if (comboBox_PL.Text != "")
                SQL += " AND IdMayTinh LIKE N'%" + comboBox_PL.SelectedValue + "%'";
            if (textBox_MT.Text != "")
                SQL += " AND MoTa LIKE N'%" + textBox_MT.Text + "%'";

            tableHangHoa = PhuongThucSQL.GetData(SQL);

            if (tableHangHoa.Rows.Count == 0)
                MessageBox.Show("Không có dữ liệu trùng khớp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            else MessageBox.Show("Tìm thấy " + tableHangHoa.Rows.Count + " dữ liệu trùng khớp", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            dataGridView_SP.DataSource = tableHangHoa;
            label_Bo.Enabled = true;
        }

        private void label_Anh_Click(object sender, EventArgs e)
        {
            {
                printPreviewDialog1.Document = printDocument1;
                printPreviewDialog1.PrintPreviewControl.Zoom = 1;
                printPreviewDialog1.ShowDialog();    
            }
        }

        private void pictureBox_SP_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Filter = "Bitmap(*.bmp)|*.bmp|JPEG(*.jpg)|*.jpg|GIF(*.gif)|*.gif|All files(*.*)|*.*";
            dialog.FilterIndex = 2;
            dialog.Title = "Chọn ảnh minh hoạ cho sản phẩm";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                pictureBox_SP.Image = Image.FromFile(dialog.FileName);
                textBox_LinkAnh.Text = dialog.FileName;
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            int h = dataGridView_SP.Height;
            dataGridView_SP.Height = (dataGridView_SP.RowCount * dataGridView_SP.RowTemplate.Height * 2);
            Bitmap bmp = new Bitmap(dataGridView_SP.Width, dataGridView_SP.Height);
            dataGridView_SP.DrawToBitmap(bmp, new Rectangle(0, 0, dataGridView_SP.Width, dataGridView_SP.Height));
            dataGridView_SP.Height = h;
            e.Graphics.DrawImage(bmp, 0, 10);
        }
    }
}
