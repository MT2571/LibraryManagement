using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using System.Data.Common;

namespace QuanLyThuVien
{
    /// <summary>
    /// Interaction logic for W_QuanLyDocGia.xaml
    /// </summary>
    public partial class W_QuanLyDocGia : Window
    {
        SqlConnection sqlConnection;
        public W_QuanLyDocGia()
        {
            InitializeComponent();

            string connectionString = ConfigurationManager.ConnectionStrings["QuanLyThuVien.Properties.Settings.QLTV_DBConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            HienThiDanhSachDocGia();
        }

        private void HienThiDanhSachDocGia()
        {
            sqlConnection.Open();
            string query = "SELECT * FROM DOCGIA";
            SqlDataAdapter da = new SqlDataAdapter(query, sqlConnection);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView.ItemsSource = dt.DefaultView;
            sqlConnection.Close();
        }

        private void dataGridView_SelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            DataGrid gd = (DataGrid)sender;
            DataRowView row_selected = gd.SelectedItem as DataRowView;
            if (row_selected != null)
            {
                txtHoTen.Text = row_selected.Row[2].ToString();
            }

        }

        private void btnThem_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO DOCGIA (MaDocGia, HoVaTen) VALUES (@MaDocGia, @HoVaTen)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@MaDocGia", txtMaDocGia.Text);
                sqlCommand.Parameters.AddWithValue("@HoVaTen", txtHoTen.Text);
                sqlCommand.ExecuteScalar();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                HienThiDanhSachDocGia();
            }
        }

        private void btnXoa_Click(object sender, RoutedEventArgs e)
        {
            string query = "DELETE FROM DOCGIA WHERE MaDocGia = @MaDocGia";

            SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
            //The SqlDataAdapter can be imagined like an Interface to make Tables usable by C#-Objects
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);

            try
            {
                DataGrid gd = (DataGrid)sender;
                DataRowView row_selected = gd.SelectedItem as DataRowView;
                if (row_selected != null)
                {
                    sqlCommand.Parameters.AddWithValue("@MaDocGia", row_selected.Row[2].ToString());
                }
                using (sqlDataAdapter)
                {
                    DataTable dt = new DataTable();
                    sqlDataAdapter.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            HienThiDanhSachDocGia();
            
        }
    }
}
