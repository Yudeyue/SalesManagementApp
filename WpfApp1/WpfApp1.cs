using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        /*public MainWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["WpfApp1.Properties.Settings.YUDEYUEDATAConnectionString"].ConnectionString;
                
        }

        private void DisplayStores()
        {
            try
            {
                string query = "SELECT * FROM Store";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, SqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable dataTable= new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    storelist.DisplayMemberPath = "Name";
                    storelist.SelectedValuePath= "Id";
                    storelist.ItemsSource = dataTable.DefaultView;
                }
            }catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }*/
    }
}
