using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;
using System.Data.SqlClient;

namespace Quanlybanmaytinh
{
    internal class PhuongThucSQL
    {
        public static SqlConnection SQLConnect;

        public static string GetValue(string SQL)
        {
            string val = "";
            SqlCommand SQLcmd = new SqlCommand(SQL, SQLConnect);
            SQLcmd.CommandText = SQL;
            SqlDataReader reader;
            reader = SQLcmd.ExecuteReader();
            while (reader.Read())
                val = reader.GetValue(0).ToString();
            reader.Close();
            return val;
           
        }
public static void Connect()
        {
            SQLConnect = new SqlConnection();
            SQLConnect.ConnectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=D:\K23\CNPM\Cuoiki\Quanlybanmaytinh\Quanlybanmaytinh.mdf;Integrated Security=True;Connect Timeout=30";
            SQLConnect.Open();                  
            
            if (SQLConnect.State == ConnectionState.Open) MessageBox.Show("Connected");
            else MessageBox.Show("Disconnected");

        }
        public static void Disconnect()
        {
            if (SQLConnect.State == ConnectionState.Open)
            {
                SQLConnect.Close();
                SQLConnect.Dispose();
                SQLConnect = null;
            }
        }

        public static DataTable GetData(string SQL)
        {
            SqlDataAdapter SQLAdapter = new SqlDataAdapter(SQL, SQLConnect); 
            
            DataTable table = new DataTable();
            SQLAdapter.Fill(table);
            return table;
        }

        public static void OpenSQL(string SQL)
        {
            SqlCommand SQLcmd; 
            SQLcmd = new SqlCommand();
            SQLcmd.Connection = SQLConnect; 
            SQLcmd.CommandText = SQL; 
            try
            {
                SQLcmd.ExecuteNonQuery(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            SQLcmd.Dispose();
            SQLcmd = null;
        }
        public static void DeleteSQL(string SQL)
        {
            SqlCommand SQLcmd = new SqlCommand();
            SQLcmd.Connection = SQLConnect;
            SQLcmd.CommandText = SQL;
            try
            {
                SQLcmd.ExecuteNonQuery();
            }
            catch (Exception exept)
            {
  
                MessageBox.Show(exept.ToString());
            }
            SQLcmd.Dispose();
            SQLcmd = null;
        }
        public static void DataComboBox(string SQL, ComboBox box, string value, string name)
        {
            SqlDataAdapter SQLAdapter = new SqlDataAdapter(SQL, SQLConnect);
            DataTable table = new DataTable();
            SQLAdapter.Fill(table);
            box.DataSource = table;
            box.ValueMember = value; 
            box.DisplayMember = name; 
        }
        public static bool CheckPrimaryKey(string SQL)
        {
            SqlDataAdapter SQLAdapter = new SqlDataAdapter(SQL, SQLConnect);
            DataTable table = new DataTable();
            SQLAdapter.Fill(table);
            if (table.Rows.Count > 0)
                return true;
            else return false;
        }
        public static string ConvertTime(string hour)
        {
            string h = "";
            switch (hour)
            {
                case "1":
                    h = "13";
                    break;
                case "2":
                    h = "14";
                    break;
                case "3":
                    h = "15";
                    break;
                case "4":
                    h = "16";
                    break;
                case "5":
                    h = "17";
                    break;
                case "6":
                    h = "18";
                    break;
                case "7":
                    h = "19";
                    break;
                case "8":
                    h = "20";
                    break;
                case "9":
                    h = "21";
                    break;
                case "10":
                    h = "22";
                    break;
                case "11":
                    h = "23";
                    break;
                case "12":
                    h = "0";
                    break;
            }
            return h;
        }
        public static string MakeIdHD(string val)
        {
            string ID = val;
            string[] partsDay;
            partsDay = DateTime.Now.ToShortDateString().Split('/');
           
            string day = String.Format("{0}{1}{2}", partsDay[0], partsDay[1], partsDay[2]);
            ID = ID + day;
            string[] partsTime;
            partsTime = DateTime.Now.ToLongTimeString().Split(':');
            
            if (partsTime[2].Substring(3, 2) == "PM")
                partsTime[0] = ConvertTime(partsTime[0]);
            if (partsTime[2].Substring(3, 2) == "AM")
                if (partsTime[0].Length == 1)
                    partsTime[0] = "0" + partsTime[0];
            
            partsTime[2] = partsTime[2].Remove(2, 3);
            string time;
            time = String.Format("_{0}{1}{2}", partsTime[0], partsTime[1], partsTime[2]);
            ID = ID + time;
            return ID;
        }
    }
}
