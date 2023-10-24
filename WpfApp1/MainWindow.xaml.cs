using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Packaging;
using System.Windows.Annotations.Storage;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        SqlConnection sqlConnection;

        public MainWindow()
        {
            InitializeComponent();
            string connectionString = ConfigurationManager.ConnectionStrings["WpfApp1.Properties.Settings.YUDEYUEDATAConnectionString"].ConnectionString;
            sqlConnection = new SqlConnection(connectionString);
            DisplayStores();
            DisplayAllProducts();
       
        }

        private void DisplayStores()
        {
            try
            {
                string query = "SELECT * FROM Store";
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);

                using (sqlDataAdapter)
                {
                    DataTable dataTable = new DataTable();
                    sqlDataAdapter.Fill(dataTable);
                    storelist.DisplayMemberPath = "Name";
                    storelist.SelectedValuePath = "Id";
                    storelist.ItemsSource = dataTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void DisplayStoreInventory()
        {
            try
            {
                string query = "SELECT * FROM Product p INNER JOIN StoreInventory si on p.Id = si.ProductId WHERE si.StroeId = @StoreId ";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand);
                using (sqlDataAdapter)
                {
                    sqlCommand.Parameters.AddWithValue("@StoreId", storelist.SelectedValue);
                    DataTable inventoryTable = new DataTable();
                    sqlDataAdapter.Fill(inventoryTable);
                    storeInvList.DisplayMemberPath = "Brand";
                    storeInvList.SelectedValuePath = "Id";
                    storeInvList.ItemsSource = inventoryTable.DefaultView;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            DisplayStoreInventory();
        }

        private void DisplayAllProducts()
        {
            string query = "SELECT * FROM Product";
            SqlCommand sqlComnand = new SqlCommand(query, sqlConnection);
            SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection);
            using(sqlDataAdapter)
            {
                //sqlComnand.Parameters.AddWithValue("Id", productsList.SelectedValue);
                DataTable dataTable = new DataTable();
                sqlDataAdapter.Fill(dataTable);
                productsList.DisplayMemberPath="Brand";
                productsList.SelectedValuePath="Id";
                productsList.ItemsSource = dataTable.DefaultView;
            }
        }

        private void AddStore_Click(object sender, RoutedEventArgs e)
        {
            try { 
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@Name", SqlDbType.NVarChar){Value=storeName.Text},
                    new SqlParameter("@Street", SqlDbType.NVarChar){Value=storeStreet.Text},
                    new SqlParameter("@City", SqlDbType.NVarChar){Value=storeCity.Text},
                    new SqlParameter("@State", SqlDbType.NChar){Value=storeState.Text},
                    new SqlParameter("@Zip", SqlDbType.Int){Value=storeZipcode.Text}
                };
                string query = "INSERT INTO Store VALUES (@Name, @Street, @City, @State, @Zip)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                DataTable dataTable = new DataTable();
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    sqlDataAdapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                DisplayStores();
            }
        }

        private void AddInventory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "INSERT INTO StoreInventory VALUES (@StoreId, @ProductId)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@StoreId", storelist.SelectedValue);
                sqlCommand.Parameters.AddWithValue("@ProductId", productsList.SelectedValue);
                //sqlCommand.ExecuteScalar();
                DataTable dataTable = new DataTable();
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(query, sqlConnection))
                {
                    sqlDataAdapter.Fill(dataTable);
                }
            } 
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                DisplayStoreInventory();
            }   
        }

        private void AddProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                List<SqlParameter> sqlParameters = new List<SqlParameter>()
                {
                    new SqlParameter("@Manufacture", SqlDbType.NVarChar){Value=prodManu.Text},
                    new SqlParameter("@Brand", SqlDbType.NVarChar){Value=prodBrand.Text}
                };
                string query = "INSERT INTO Product VALUES (@Manufacture, @Brand)";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddRange(sqlParameters.ToArray());
                DataTable dataTable = new DataTable();
                using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlCommand))
                {
                    sqlDataAdapter.Fill(dataTable);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                DisplayAllProducts();
            }
        }

        private void DeleteStore_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                string query = "DELETE FROM Store WHERE Id = @StoreId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@StoreId", storelist.SelectedValue);
                sqlCommand.ExecuteScalar();
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                DisplayStores();
            }
        }

        private void DeleteInventory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "DELETE FROM StoreIventory WHERE ProductId = @ProductId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ProductId", storeInvList.SelectedValue);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                DisplayStoreInventory();
            }
        }

        private void DeleteProduct_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string query = "DELETE FROM Product WHERE Id = @ProductId";
                SqlCommand sqlCommand = new SqlCommand(query, sqlConnection);
                sqlConnection.Open();
                sqlCommand.Parameters.AddWithValue("@ProductId", productsList.SelectedValue);
                sqlCommand.ExecuteScalar();

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            finally
            {
                sqlConnection.Close();
                DisplayAllProducts();
            }
        }

    }
}
