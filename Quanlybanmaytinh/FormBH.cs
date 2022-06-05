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
using COMExcel = Microsoft.Office.Interop.Excel;

namespace Quanlybanmaytinh
{
    public partial class FormBH : Form
    {
        string[] numText = "Không;Một;Hai;Ba;Bốn;Năm;Sáu;Bảy;Tám;Chín".Split(';');
        DataTable tableCTHD;
        public FormBH()
        {
            InitializeComponent();
        }
        private string hChuc(double so, bool full)
        {
            string str = "";
            
            Int64 hchuc = Convert.ToInt64(Math.Floor((double)(so / 10)));
            
            Int64 hdv = (Int64)so % 10;
            
            if (hchuc > 1)
            {
                str = " " + numText[hchuc] + " Mươi";
                if (hdv == 1)
                {
                    str += " Mốt";
                }
            }
            else if (hchuc == 1)
            {
                str = " Mười";
                if (hdv == 1)
                {
                    str += " Một";
                }
            }
            else if (full && hdv > 0)
            {
                str = " Lẻ";
            }
            if (hdv == 5 && hchuc >= 1)
            {
                str += " Lăm";
            }
            else if (hdv > 1 || (hdv == 1 && hchuc == 0))
            {
                str += " " + numText[hdv];
            }
            return str;
        }
        private string hTram(double n, bool full)
        {
            string str = " ";
            Int64 htram = Convert.ToInt64(Math.Floor((double) n/ 100));
            n = n % 100;
            if (full || htram > 0)
            {
                str = " " + numText[htram] + " Trăm";
                str += hChuc(n, true);
            }
            else
            {
                str = hChuc(n, false);
            }
            return str;
        }
        private string hTrieu(double n, bool full)
        {
            string str = " ";
           
            Int64 htrieu = Convert.ToInt64(Math.Floor((double)n / 1000000));
           
            n = n % 1000000;
            if (htrieu > 0)
            {
                str = hTram(htrieu, full) + " Triệu";
                full = true;
            }

            Int64 hnghin = Convert.ToInt64(Math.Floor((double)n / 1000));
            n = n % 1000;
            if (hnghin > 0)
            {
                str += hTram(hnghin, full) + " Nghìn";
                full = true;
            }
            if (n > 0)
            {
                str += hTram(n, full);
            }
            return str;
        }
        public string NumberConvert(double n)
        {
            if (n == 0)
                return numText[0];
            string str = "", nlast = "";
            Int64 hti;
            do
            {
                hti = Convert.ToInt64(Math.Floor((double)n / 1000000000));
                
                n = n % 1000000000;
                if (hti > 0)
                {
                    str = hTrieu(n, true) + nlast + str;
                }
                else
                {
                    str = hTrieu(n, false) + nlast + str;
                }
                nlast = " Tỷ";
            } while (hti > 0);
            return str + " Đồng";
        }

        private void FormBH_Load(object sender, EventArgs e)
        {
            label_Them.Enabled = true;
            label_Xoa.Enabled = false;
            label_Luu.Enabled = false;
            label_In.Enabled = false;
            textBox_IdHD.ReadOnly = true;
            textBox_TenNV.ReadOnly = true;
            textBox_TenKH.ReadOnly = true;
            textBox_TenSP.ReadOnly = true;
            textBox_DC.ReadOnly = true;
            textBox_SDT.ReadOnly = true;
            textBox_DonGia.ReadOnly = true;
            textBox_Tong.ReadOnly = true;
            textBox_TT.ReadOnly = true;
            textBox_Giam.Text = "0";
            textBox_TT.Text = "0";
            PhuongThucSQL.DataComboBox("SELECT IdKhachHang, TenKhachHang FROM tableKhachHang",comboBox_IdKH, "IdKhachHang", "IdKhachHang");
            comboBox_IdKH.SelectedIndex = -1;
            PhuongThucSQL.DataComboBox("SELECT IdNhanVien, TenNhanVien FROM tableNhanVien", comboBox_IdNV, "IdNhanVien", "TenKhachHang");
            comboBox_IdNV.SelectedIndex = -1;
            PhuongThucSQL.DataComboBox("SELECT IdHangHoa, TenHangHoa FROM tableHangHoa", comboBox_IdSP, "IdHangHoa", "IdHangHoa");
            comboBox_IdSP.SelectedIndex = -1;
           
            if (textBox_IdHD.Text != "")
            {
                LoadHD();
                label_Xoa.Enabled = true;
                label_In.Enabled = true;
            }
            LoadDataSQL();
        }

        private void LoadHD()
        {
            string SQL;
            SQL = "SELECT NgayBan FROM tableHoaDon WHERE IdHoaDon = N'" + textBox_IdHD.Text + "'";
            dateTimePicker_Time.Value = DateTime.Parse(PhuongThucSQL.GetValue(SQL));
            SQL = "SELECT IdNhanVien FROM tableHoaDon WHERE IdHoaDon = N'" + textBox_IdHD.Text + "'";
            comboBox_IdNV.Text = PhuongThucSQL.GetValue(SQL);
            SQL = "SELECT IdKhachHang FROM tableHoaDon WHERE IdHoaDon = N'" + textBox_IdHD.Text + "'";
            comboBox_IdKH.Text = PhuongThucSQL.GetValue(SQL);
            SQL = "SELECT TongTien FROM tableHoaDon WHERE IdHoaDon = N'" + textBox_IdHD.Text + "'";
            textBox_TT.Text = PhuongThucSQL.GetValue(SQL);
            label_BC.Text = "Bằng chữ: " + NumberConvert(Double.Parse(textBox_TT.Text));
        }
        private void LoadDataSQL()
        {
            string SQL;
            SQL = "SELECT CTHD.IdHangHoa, HH.TenHangHoa, CTHD.SoLuong, HH.GiaBan, CTHD.KhuyenMai, CTHD.ThanhToan FROM tableChiTietHoaDon AS CTHD, tableHangHoa AS HH WHERE CTHD.IdHoaDon = N'" + textBox_IdHD.Text + "' AND CTHD.IdHangHoa=HH.IdHangHoa";
            tableCTHD = PhuongThucSQL.GetData(SQL);
            dataGridView_HD.DataSource = tableCTHD;
            dataGridView_HD.Columns[0].HeaderText = "ID SP";
            dataGridView_HD.Columns[1].HeaderText = "TÊN SẢN PHẨM";
            dataGridView_HD.Columns[2].HeaderText = "SỐ LƯỢNG";
            dataGridView_HD.Columns[3].HeaderText = "ĐƠN GIÁ";
            dataGridView_HD.Columns[4].HeaderText = "GIẢM (%)";
            dataGridView_HD.Columns[5].HeaderText = "THÀNH TIỀN";

            dataGridView_HD.Columns[0].Width = 100;
            dataGridView_HD.Columns[1].Width = 300;
            dataGridView_HD.Columns[2].Width = 100;
            dataGridView_HD.Columns[3].Width = 200;
            dataGridView_HD.Columns[4].Width = 100;
            dataGridView_HD.Columns[5].Width = 220;
            dataGridView_HD.AllowUserToAddRows = false;
            dataGridView_HD.EditMode = DataGridViewEditMode.EditProgrammatically;

            foreach (DataGridViewColumn col in dataGridView_HD.Columns)
            {
                col.HeaderCell.Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
                col.HeaderCell.Style.Font = new Font("Times New Roman", 12F, FontStyle.Bold);
            }
        }
        private void ResetData()
        {
            textBox_IdHD.Text = "";
            dateTimePicker_Time.Value = DateTime.Now;
            comboBox_IdNV.Text = "";
            comboBox_IdKH.Text = "";
            textBox_TT.Text = "0";
            label_BC.Text = "Bằng chữ: ";
            comboBox_IdSP.Text = "";
            textBox_SL.Text = "";
            textBox_Giam.Text = "0";
            textBox_DonGia.Text = "";
            textBox_Tong.Text = "0";
            textBox_SDT.Text = "";
            textBox_DC.Text = "";
            textBox_TenKH.Text = "";
            textBox_TenNV.Text = "";
            textBox_TenSP.Text = "";
        }

        private void label_Them_Click(object sender, EventArgs e)
        {
            label_Xoa.Enabled = false;
            label_Luu.Enabled = true;
            label_In.Enabled = false;
            label_Them.Enabled = false;
            ResetData();
            textBox_IdHD.Text = PhuongThucSQL.MakeIdHD("HD");
            LoadDataSQL();
        }

        private void ResetSP()
        {
            comboBox_IdSP.Text = "";
            textBox_SL.Text = "";
            textBox_Giam.Text = "0";
            textBox_Tong.Text = "0";
        }
        private void label_Luu_Click(object sender, EventArgs e)
        {
            string SQL;
            double SL, SLcon, Tong, updTong;
            SQL = "SELECT IdHoaDon FROM tableHoaDon WHERE IdHoaDon=N'" + textBox_IdHD.Text + "'";
            if (!PhuongThucSQL.CheckPrimaryKey(SQL))
            {
                if (comboBox_IdNV.Text.Length == 0)
                {
                    MessageBox.Show("Vui lòng thêm ID nhân viên", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBox_IdNV.Focus();
                    return;
                }
                if (comboBox_IdKH.Text.Length == 0)
                {
                    MessageBox.Show("Vui lòng thêm ID khách hàng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    comboBox_IdKH.Focus();
                    return;
                }

                SQL = "INSERT INTO tableHoaDon (IdHoaDon, IdNhanVien, IdKhachHang, NgayBan, TongTien) VALUES (N'" + textBox_IdHD.Text.Trim() + "',N'" + comboBox_IdNV.SelectedValue + "',N'" +comboBox_IdKH.SelectedValue + "','" +
                        dateTimePicker_Time.Value + "'," + textBox_TT.Text + ")";
                PhuongThucSQL.OpenSQL(SQL);
            }
          
            if (comboBox_IdSP.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng thêm ID sản phẩm", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox_IdSP.Focus();
                return;
            }
            if ((textBox_SL.Text.Trim().Length == 0) || (textBox_SL.Text == "0"))
            {
                MessageBox.Show("Vui lòng nhập số lượng", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_SL.Text = "";
                textBox_SL.Focus();
                return;
            }
            if (textBox_Giam.Text.Trim().Length == 0)
            {
                MessageBox.Show("Vui lòng nhập % khuyến mãi", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox_Giam.Focus();
                return;
            }

            SQL = "SELECT IdHangHoa FROM tableChiTietHoaDon WHERE IdHangHoa =N'" + comboBox_IdSP.SelectedValue + "' AND IdHoaDon = N'" + textBox_IdHD.Text.Trim() + "'";
            if (PhuongThucSQL.CheckPrimaryKey(SQL))
            {
                MessageBox.Show("ID sản phẩm này đã tồn tại, vui lòng chọn ID khác", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                ResetSP();
                comboBox_IdSP.Focus();
                return;
            }
           
            SL = Convert.ToDouble(PhuongThucSQL.GetValue("SELECT SoLuong FROM tableHangHoa WHERE IdHangHoa = N'" + comboBox_IdSP.SelectedValue + "'"));
            if (Convert.ToDouble(textBox_SL.Text) > SL)
            {
                MessageBox.Show("Sản phẩm này còn " + SL + "số lượng trong kho", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information);
                textBox_SL.Text = "";
                textBox_SL.Focus();
                return;
            }

            SQL = "INSERT INTO tableChiTietHoaDon (IdHoaDon, IdHangHoa, SoLuong, DonGia, KhuyenMai, ThanhToan) VALUES(N'" + textBox_IdHD.Text.Trim() + "',N'" + comboBox_IdSP.SelectedValue + "'," + textBox_SL.Text + "," + textBox_DonGia.Text + "," + textBox_Giam.Text + "," + textBox_Tong.Text + ")";
            PhuongThucSQL.OpenSQL(SQL);
            LoadDataSQL();
            
            SLcon = SL - Convert.ToDouble(textBox_SL.Text);
            SQL = "UPDATE tableHangHoa SET SoLuong =" + SLcon + " WHERE IdHangHoa= N'" + comboBox_IdSP.SelectedValue + "'";
            PhuongThucSQL.OpenSQL(SQL);

            Tong = Convert.ToDouble(PhuongThucSQL.GetValue("SELECT TongTien FROM tableHoaDon WHERE IdHoaDon = N'" + textBox_IdHD.Text + "'"));
            updTong = Tong + Convert.ToDouble(textBox_Tong.Text);
            SQL = "UPDATE tableHoaDon SET TongTien =" + updTong + " WHERE IdHoaDon = N'" + textBox_IdHD.Text + "'";
            PhuongThucSQL.OpenSQL(SQL);
            textBox_TT.Text = updTong.ToString();
            label_BC.Text = "Bằng chữ: " + NumberConvert(Double.Parse(textBox_TT.Text));
            ResetSP();
            label_Xoa.Enabled = true;
            label_Them.Enabled = true;
            label_In.Enabled = true;
        }

        private void comboBox_IdNV_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SQL;
            if (comboBox_IdNV.Text == "") textBox_TenNV.Text = "";
            
            SQL = "SELECT TenNhanVien from tableNhanVien WHERE IdNhanVien =N'" + comboBox_IdNV.SelectedValue + "'";
            textBox_TenNV.Text = PhuongThucSQL.GetValue(SQL);
        }

        private void comboBox_IdSP_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SQL;
            if (comboBox_IdSP.Text == " ")
            {
                textBox_TenSP.Text = " ";
                textBox_DonGia.Text = " ";
            }
            
            SQL = "SELECT TenHangHoa FROM tableHangHoa WHERE IdHangHoa = N'" + comboBox_IdSP.SelectedValue + "'";
            textBox_TenSP.Text = PhuongThucSQL.GetValue(SQL);
            SQL = "SELECT GiaBan FROM tableHangHoa WHERE IdHangHoa =N'" + comboBox_IdSP.SelectedValue + "'";
            textBox_DonGia.Text = PhuongThucSQL.GetValue(SQL);
        }

        private void comboBox_IdKH_SelectedIndexChanged(object sender, EventArgs e)
        {
            string SQL;
            if (comboBox_IdKH.Text == "")
            {
                textBox_TenKH.Text = "";
                textBox_DC.Text = "";
                textBox_SDT.Text = "";
            }

            SQL = "SELECT TenKhachHang FROM tableKhachHang WHERE IdKhachHang = N'" + comboBox_IdKH.SelectedValue + "'";
            textBox_TenKH.Text = PhuongThucSQL.GetValue(SQL);

            SQL = "SELECT DiaChiKH FROM tableKhachHang WHERE IdKhachHang = N'" + comboBox_IdKH.SelectedValue + "'";
            textBox_DC.Text = PhuongThucSQL.GetValue(SQL);

            SQL = "SELECT SdtKH FROM tableKhachHang WHERE IdKhachHang = N'" + comboBox_IdKH.SelectedValue + "'";
            textBox_SDT.Text = PhuongThucSQL.GetValue(SQL);
        }

        private void textBox_SL_TextChanged(object sender, EventArgs e)
        {
            double SL, TT, gGoc, gGiam;
            if (textBox_SL.Text == "")
                SL = 0;
            else
                SL = Convert.ToDouble(textBox_SL.Text);
            if (textBox_Giam.Text == "")
                gGiam = 0;
            else
                gGiam = Convert.ToDouble(textBox_Giam.Text);
            if (textBox_DonGia.Text == "")
                gGoc = 0;
            else
                gGoc = Convert.ToDouble(textBox_DonGia.Text);
            TT = SL * gGoc - SL * gGoc * gGiam / 100;
            textBox_Tong.Text = TT.ToString();
        }

        private void textBox_Giam_TextChanged(object sender, EventArgs e)
        {
            double SL, TT, gGoc, gGiam;
            if (textBox_SL.Text == "")
                SL = 0;
            else
                SL = Convert.ToDouble(textBox_SL.Text);
            if (textBox_Giam.Text == "")
                gGiam = 0;
            else
                gGiam = Convert.ToDouble(textBox_Giam.Text);
            if (textBox_DonGia.Text == "")
                gGoc = 0;
            else
                gGoc = Convert.ToDouble(textBox_DonGia.Text);
            TT = SL * gGoc - SL * gGoc * gGiam / 100;
            textBox_Tong.Text = TT.ToString();
        }

        private void label_In_Click(object sender, EventArgs e)
        {
            COMExcel.Application exApp = new COMExcel.Application();
            COMExcel.Workbook exBook; 
            COMExcel.Worksheet exSheet; 
            COMExcel.Range exRange;
            string SQL;
            int col = 0, row = 0;
            DataTable tableHD, tableSP;
            exBook = exApp.Workbooks.Add(COMExcel.XlWBATemplate.xlWBATWorksheet);
            exSheet = exBook.Worksheets[1];
            
            exRange = exSheet.Cells[1, 1];
            exRange.Range["A1:Z300"].Font.Name = "Times New Roman";
            exRange.Range["A1:B3"].Font.Size = 10;
            exRange.Range["A1:B3"].Font.Bold = true;
            exRange.Range["A1:B3"].Font.ColorIndex = 3; 
            exRange.Range["A1:A1"].ColumnWidth = 8;
            exRange.Range["B1:B1"].ColumnWidth = 30;
            exRange.Range["A1:B1"].MergeCells = true;
            exRange.Range["A1:B1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A1:B1"].Value = "APPLE STORE";
            exRange.Range["A2:B2"].MergeCells = true;
            exRange.Range["A2:B2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A2:B2"].Value = "Q7 - TP.HCM";
            exRange.Range["A3:B3"].MergeCells = true;

            exRange.Range["C2:F2"].Font.Size = 20;
            exRange.Range["C2:F2"].Font.Bold = true;
            exRange.Range["C2:F2"].Font.ColorIndex = 25; 
            exRange.Range["C2:F2"].MergeCells = true;
            exRange.Range["C2:F2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C2:F2"].Value = "HÓA ĐƠN BÁN HÀNG";
           
            SQL = "SELECT HD.IdHoaDon, HD.NgayBan, HD.TongTien, KH.TenKhachHang, KH.DiaChiKH, KH.SdtKH, NV.TenNhanVien FROM tableHoaDon AS HD, tableKhachHang AS KH, tableNhanVien AS NV WHERE HD.IdHoaDon = N'" + textBox_IdHD.Text + "' AND HD.IdKhachHang = KH.IdKhachHang AND HD.IdNhanVien = NV.IdNhanVien";
            tableHD = PhuongThucSQL.GetData(SQL);
            exRange.Range["B6:C9"].Font.Size = 13;
            exRange.Range["B6:B6"].Value = "ID Hóa Đơn:";
            exRange.Range["C6:F6"].MergeCells = true;
            exRange.Range["C6:F6"].Value = tableHD.Rows[0][0].ToString();
            exRange.Range["B7:B7"].Value = "Tên Khách Hàng:";
            exRange.Range["C7:F7"].MergeCells = true;
            exRange.Range["C7:F7"].Value = tableHD.Rows[0][3].ToString();
            exRange.Range["B8:B8"].Value = "Địa Chỉ:";
            exRange.Range["C8:F8"].MergeCells = true;
            exRange.Range["C8:F8"].Value = tableHD.Rows[0][4].ToString();
            exRange.Range["B9:B9"].Value = "Số Điện Thoại:";
            exRange.Range["C9:F9"].MergeCells = true;
            exRange.Range["C9:F9"].Value = "'" + tableHD.Rows[0][5].ToString();
            
            SQL = "SELECT HH.TenHangHoa, CTHD.SoLuong, HH.GiaBan, CTHD.KhuyenMai, CTHD.ThanhToan " +
                  "FROM tableChiTietHoaDon AS CTHD , tableHangHoa AS HH WHERE CTHD.IdHoaDon = N'" +
                  textBox_IdHD.Text + "' AND CTHD.IdHangHoa = HH.IdHangHoa";
            tableSP = PhuongThucSQL.GetData(SQL);
           
            exRange.Range["A11:F11"].Font.Bold = true;
            exRange.Range["A11:F11"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["C11:F11"].ColumnWidth = 10;
            exRange.Range["A11:A11"].Value = "STT";
            exRange.Range["B11:B11"].ColumnWidth = 30;
            exRange.Range["B11:B11"].Value = "Tên Sản Phẩm";
            exRange.Range["C11:C11"].Value = "Số Lượng";
            exRange.Range["D11:D11"].ColumnWidth = 14;
            exRange.Range["D11:D11"].Value = "Đơn Giá";
            exRange.Range["E11:E11"].Value = "Giảm (%)";
            exRange.Range["F11:F11"].ColumnWidth = 14;
            exRange.Range["F11:F11"].Value = "Thành Tiền";
            for (row = 0; row < tableSP.Rows.Count; row++)
            {
                exSheet.Cells[1][row + 12] = row + 1;
                for (col = 0; col < tableSP.Columns.Count; col++)
                
                {
                    exSheet.Cells[col + 2][row + 12] = tableSP.Rows[row][col].ToString();
                    if (col == 3) exSheet.Cells[col + 2][row + 12] = tableSP.Rows[row][col].ToString() + "%";
                }
            }
            exRange = exSheet.Cells[col][row + 14];
            exRange.Font.Bold = true;
            exRange.Value2 = "Tổng Tiền:";
            exRange = exSheet.Cells[col + 1][row + 14];
            exRange.Font.Bold = true;
            exRange.Value2 = tableHD.Rows[0][2].ToString();
            exRange = exSheet.Cells[1][row + 15]; 
            exRange.Range["A1:F1"].MergeCells = true;
            exRange.Range["A1:F1"].Font.Bold = true;
            exRange.Range["A1:F1"].Font.Italic = true;
            exRange.Range["A1:F1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignRight;
            exRange.Range["A1:F1"].Value = "Bằng chữ: " + NumberConvert(Double.Parse(tableHD.Rows[0][2].ToString()));
            exRange = exSheet.Cells[4][row + 17]; 
            exRange.Range["A1:C1"].MergeCells = true;
            exRange.Range["A1:C1"].Font.Italic = true;
            exRange.Range["A1:C1"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            DateTime date = Convert.ToDateTime(tableHD.Rows[0][1]);
            exRange.Range["A1:C1"].Value = "TP.HCM, ngày " + date.Day + " tháng " + date.Month + " năm " + date.Year;
            exRange.Range["A2:C2"].MergeCells = true;
            exRange.Range["A2:C2"].Font.Italic = true;
            exRange.Range["A2:C2"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A2:C2"].Value = "Người Lập";
            exRange.Range["A4:C4"].MergeCells = true;
            exRange.Range["A4:C4"].Font.Bold = true;
            exRange.Range["A4:C4"].Font.Italic = true;
            exRange.Range["A4:C4"].HorizontalAlignment = COMExcel.XlHAlign.xlHAlignCenter;
            exRange.Range["A4:C4"].Value = tableHD.Rows[0][6];
            exSheet.Name = "Hóa Đơn";
            exApp.Visible = true;
        }

        private void label_TimKiem_Click(object sender, EventArgs e)
        {
            if (comboBox_IdHD.Text == "")
            {
                MessageBox.Show("Vui lòng chọn ID hóa đơn", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox_IdHD.Focus();
                return;
            }
            textBox_IdHD.Text = comboBox_IdHD.Text;
            LoadHD();
            LoadDataSQL();
            label_Xoa.Enabled = true;
            label_Luu.Enabled = true;
            label_In.Enabled = true;
            comboBox_IdHD.SelectedIndex = -1;
        }

        private void dataGridView_HD_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView_HD_CellDoubleClick_1(object sender, DataGridViewCellEventArgs e)
        {
            string delIdSP, SQL;
            Double delTT, delSL, SL, SLcon, Tong, updTong;

            if ((MessageBox.Show("Xác nhận xóa dữ liệu ?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes))
            {

                delIdSP = dataGridView_HD.CurrentRow.Cells["IdHangHoa"].Value.ToString();
                delSL = Convert.ToDouble(dataGridView_HD.CurrentRow.Cells["SoLuong"].Value.ToString());
                delTT = Convert.ToDouble(dataGridView_HD.CurrentRow.Cells["ThanhToan"].Value.ToString());
                SQL = "DELETE tableChiTietHoaDon WHERE IdHoaDon=N'" + textBox_IdHD.Text + "' AND IdHangHoa = N'" + delIdSP + "'";
                PhuongThucSQL.OpenSQL(SQL);

                SL = Convert.ToDouble(PhuongThucSQL.GetValue("SELECT SoLuong FROM tableHangHoa WHERE IdHangHoa = N'" + delIdSP + "'"));
                SLcon = SL + delSL;
                SQL = "UPDATE tableHangHoa SET SoLuong =" + SLcon + " WHERE IdHangHoa= N'" + delIdSP + "'";
                PhuongThucSQL.OpenSQL(SQL);

                Tong = Convert.ToDouble(PhuongThucSQL.GetValue("SELECT TongTien FROM tableHoaDon WHERE IdHoaDon = N'" + textBox_IdHD.Text + "'"));
                updTong = Tong - delTT;
                SQL = "UPDATE tableHoaDon SET TongTien =" + updTong + " WHERE IdHoaDon = N'" + textBox_IdHD.Text + "'";
                PhuongThucSQL.OpenSQL(SQL);
                textBox_TT.Text = updTong.ToString();
                label_BC.Text = "Bằng chữ: " + NumberConvert(Double.Parse(textBox_TT.Text));
                LoadDataSQL();
            }
        }

        private void textBox_Giam_KeyPress(object sender, KeyPressEventArgs e)
        {
        }

        private void comboBox_IdHD_DropDown(object sender, EventArgs e)
        {
            PhuongThucSQL.DataComboBox("SELECT IdHoaDon FROM tableHoaDon", comboBox_IdHD, "IdHoaDon", "IdHoaDon");
            comboBox_IdHD.SelectedIndex = -1;
        }

        private void label_Thoat_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void label_Xoa_Click(object sender, EventArgs e)
        {
            double SL, SLcon, delSL;
            if (MessageBox.Show("Bạn có chắc chắn muốn xóa không?", "Thông báo", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                string SQL = "SELECT IdHangHoa,SoLuong FROM tableChiTietHoaDon WHERE IdHoaDon = N'" + textBox_IdHD.Text + "'";
                DataTable tableHH = PhuongThucSQL.GetData(SQL);
                for (int HH = 0; HH <= tableHH.Rows.Count - 1; HH++)
                {
                    SL = Convert.ToDouble(PhuongThucSQL.GetValue("SELECT SoLuong FROM tableHangHoa WHERE IdHangHoa = N'" + tableHH.Rows[HH][0].ToString() + "'"));
                    delSL = Convert.ToDouble(tableHH.Rows[HH][1].ToString());
                    SLcon = SL + delSL;
                    SQL = "UPDATE tableHangHoa SET SoLuong =" + SLcon + " WHERE IdHangHoa= N'" + tableHH.Rows[HH][0].ToString() + "'";
                    PhuongThucSQL.OpenSQL(SQL);
                }

                SQL = "DELETE tableChiTietHoaDon WHERE IdHoaDon=N'" + textBox_IdHD.Text + "'";
                PhuongThucSQL.DeleteSQL(SQL);

                SQL = "DELETE tableHoaDon WHERE IdHoaDon=N'" + textBox_IdHD.Text + "'";
                PhuongThucSQL.DeleteSQL(SQL);
                ResetData();
                LoadDataSQL();
                label_Them.Enabled = true;
                label_Xoa.Enabled = false;
                label_In.Enabled = false;
            }
        }

        private void label_Bo_Click(object sender, EventArgs e)
        {
            ResetData();
            label_Them.Enabled = true;
            label_Xoa.Enabled = false;
            label_In.Enabled = false;
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
