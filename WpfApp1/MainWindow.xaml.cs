using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Diagnostics;
using System.ComponentModel;

namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string connectionString;
        SqlDataAdapter adapter;
        DataTable libraryTable;
        

        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox1.Text == "admin" && PasswordBox1.Password == "admin")
            {
                TextBox1.Clear();
                PasswordBox1.Clear();
                MainMenu.Visibility = Visibility.Hidden;
                DataBaseMenuAdmin.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Вы вошли как обычный пользователь." + "\n" + "Вам ограничены функции редактирования базы данных библиотеки.", "Авторизация", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                TextBox1.Clear();
                PasswordBox1.Clear();
                MainMenu.Visibility = Visibility.Hidden;
                DataBaseMenuUser.Visibility = Visibility.Visible;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM MyLibrary";
            libraryTable = new DataTable();
            SqlConnection connection = null;
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);

                // установка команды на добавление для вызова хранимой процедуры 
                adapter.InsertCommand = new SqlCommand("sp_InsertMyLibrary", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@Книга", SqlDbType.VarChar, 50, "Книга"));
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@Номер", SqlDbType.Int, 0, "Номер");
                parameter.Direction = ParameterDirection.Output;

                connection.Open();
                adapter.Fill(libraryTable);
                libraryGrid.ItemsSource = libraryTable.DefaultView;
                libraryGridUser.ItemsSource = libraryTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            string sq2 = "SELECT * FROM Автор";
            libraryTable = new DataTable();
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sq2, connection);
                adapter = new SqlDataAdapter(command);
                adapter.InsertCommand = new SqlCommand("sp_InsertАвтор", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@Автор", SqlDbType.VarChar, 50, "Автор"));
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@Номер", SqlDbType.Int, 0, "Номер");
                parameter.Direction = ParameterDirection.Output;
                connection.Open();
                adapter.Fill(libraryTable);
                AuthorGrid.ItemsSource = libraryTable.DefaultView;
                AuthorGridUser.ItemsSource = libraryTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            string sq3 = "SELECT * FROM Жанр";
            libraryTable = new DataTable();
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sq3, connection);
                adapter = new SqlDataAdapter(command);
                adapter.InsertCommand = new SqlCommand("sp_InsertЖанр", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@Жанр", SqlDbType.VarChar, 50, "Жанр"));
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@Номер", SqlDbType.Int, 0, "Номер");
                parameter.Direction = ParameterDirection.Output;
                connection.Open();
                adapter.Fill(libraryTable);
                GenreGrid.ItemsSource = libraryTable.DefaultView;
                GenreGridUser.ItemsSource = libraryTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
            string sq4 = "SELECT * FROM Издатель";
            libraryTable = new DataTable();
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sq4, connection);
                adapter = new SqlDataAdapter(command);
                adapter.InsertCommand = new SqlCommand("sp_InsertИздатель", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@Издатель", SqlDbType.VarChar, 50, "Жанр"));
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@Номер", SqlDbType.Int, 0, "Номер");
                parameter.Direction = ParameterDirection.Output;
                connection.Open();
                adapter.Fill(libraryTable);
                PublisherGrid.ItemsSource = libraryTable.DefaultView;
                PublisherGridUser.ItemsSource = libraryTable.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        private void UpdateDB()
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter);
            adapter.Update(libraryTable);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UpdateDB();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            if (libraryGrid.SelectedItems != null)
            {
                for (int i = 0; i < libraryGrid.SelectedItems.Count; i++)
                {
                    DataRowView datarowView = libraryGrid.SelectedItems[i] as DataRowView;
                    if (datarowView != null)
                    {
                        DataRow dataRow = (DataRow)datarowView.Row;
                        dataRow.Delete();
                    }
                }
            }
            UpdateDB();
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            DataBaseMenuAdmin.Visibility = Visibility.Hidden;
            MainMenu.Visibility = Visibility.Visible;

        }

        private void BackUserMenu_Click(object sender, RoutedEventArgs e)
        {
            DataBaseMenuUser.Visibility = Visibility.Hidden;
            MainMenu.Visibility = Visibility.Visible;
        }
    }
}
