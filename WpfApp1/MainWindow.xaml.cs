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
        SqlConnection connection = null;

        public void ConnectionOpenAndAdapter()
        {
            connection.Open();
            adapter.Fill(libraryTable);
        }

        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            string sql = "SELECT * FROM Book";
            libraryTable = new DataTable();
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                // установка команды на добавление для вызова хранимой процедуры 
                adapter.InsertCommand = new SqlCommand("sp_InsertBook", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@Book", SqlDbType.VarChar, 50, "Book"));
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@IdBook", SqlDbType.Int, 0, "IdBook");
                parameter.Direction = ParameterDirection.Output;
                ConnectionOpenAndAdapter();
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
            string sq2 = "SELECT * FROM Author";
            libraryTable = new DataTable();
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sq2, connection);
                adapter = new SqlDataAdapter(command);
                adapter.InsertCommand = new SqlCommand("sp_InsertAuthor", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@Author", SqlDbType.VarChar, 50, "Auhtor"));
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@IdAuthor", SqlDbType.Int, 0, "IdAuthor");
                parameter.Direction = ParameterDirection.Output;
                ConnectionOpenAndAdapter();
                AuthorGrid.ItemsSource = libraryTable.DefaultView;
                AuthorGridUser.ItemsSource = libraryTable.DefaultView;
                SqlDataAdapter da = new SqlDataAdapter("Select Author FROM Author", connection);
                DataSet ds = new DataSet();
                da.Fill(ds, "Author");
                Author.ItemsSource = ds.Tables[0].DefaultView;
                Author.DisplayMemberPath = ds.Tables[0].Columns["Author"].ToString();
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
            string sq3 = "SELECT * FROM Genre";
            libraryTable = new DataTable();
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sq3, connection);
                adapter = new SqlDataAdapter(command);
                adapter.InsertCommand = new SqlCommand("sp_InsertGenre", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@Genre", SqlDbType.VarChar, 50, "Genre"));
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@IdGenre", SqlDbType.Int, 0, "IdGenre");
                parameter.Direction = ParameterDirection.Output;
                ConnectionOpenAndAdapter();
                GenreGrid.ItemsSource = libraryTable.DefaultView;
                GenreGridUser.ItemsSource = libraryTable.DefaultView;

                SqlDataAdapter da = new SqlDataAdapter("Select Genre FROM Genre", connection);
                DataSet ds = new DataSet();
                da.Fill(ds, "Genre");
                Genre.ItemsSource = ds.Tables[0].DefaultView;
                Genre.DisplayMemberPath = ds.Tables[0].Columns["Genre"].ToString();
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
            string sq4 = "SELECT * FROM Publisher";
            libraryTable = new DataTable();
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sq4, connection);
                adapter = new SqlDataAdapter(command);
                adapter.InsertCommand = new SqlCommand("sp_InsertPublisher", connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@Publisher", SqlDbType.VarChar, 50, "Publisher"));
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@IdPublisher", SqlDbType.Int, 0, "IdPublisher");
                parameter.Direction = ParameterDirection.Output;
                ConnectionOpenAndAdapter();
                PublisherGrid.ItemsSource = libraryTable.DefaultView;
                PublisherGridUser.ItemsSource = libraryTable.DefaultView;

                SqlDataAdapter da = new SqlDataAdapter("Select Publisher FROM Publisher", connection);
                DataSet ds = new DataSet();
                da.Fill(ds, "Publisher");
                Edition.ItemsSource = ds.Tables[0].DefaultView;
                Edition.DisplayMemberPath = ds.Tables[0].Columns["Publisher"].ToString();
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
        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            connection = new SqlConnection(connectionString);
            adapter.InsertCommand = new SqlCommand("sp_InsertBook", connection);
            adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
            adapter.InsertCommand.Parameters.Add("@Book", SqlDbType.VarChar).Value = BookTextBox.Text;
            adapter.InsertCommand.Parameters.Add("@IdAuthor", SqlDbType.Int).Value = 1;
            adapter.InsertCommand.Parameters.Add("@IdGenre", SqlDbType.Int).Value = 1;
            adapter.InsertCommand.Parameters.Add("@IdPublisher", SqlDbType.Int).Value = 1;
            adapter.InsertCommand.Parameters.Add("@IdBook", SqlDbType.Int).Value = 1;

            connection.Open();
            adapter.InsertCommand.ExecuteNonQuery();
            connection.Close();
            UpdateDB();
        }
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox1.Text == "admin" && PasswordBox1.Password == "admin")
            {
                TextBox1.Clear();
                PasswordBox1.Clear();
                MainMenu.Visibility = Visibility.Hidden;
                ReadOrCreatMenu.Visibility = Visibility.Visible;
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
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите совершить удаление?" + "\n" + "Выделенные вами данные будут удалены безвозвратно!", "Подтверждение удаления данных", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
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
            else
            {

            }
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            DataBaseReadMenuAdmin.Visibility = Visibility.Hidden;
            ReadOrCreatMenu.Visibility = Visibility.Visible;
        }

        private void BackUserMenu_Click(object sender, RoutedEventArgs e)
        {
            DataBaseMenuUser.Visibility = Visibility.Hidden;
            MainMenu.Visibility = Visibility.Visible;
        }

        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            ReadOrCreatMenu.Visibility = Visibility.Hidden;
            DataBaseReadMenuAdmin.Visibility = Visibility.Visible;
        }

        private void BackReadOrCreatMenu_Click(object sender, RoutedEventArgs e)
        {
            ReadOrCreatMenu.Visibility = Visibility.Hidden;
            MainMenu.Visibility = Visibility.Visible;
        }

        private void EditorButton_Click(object sender, RoutedEventArgs e)
        {
            DataBaseReadMenuAdmin.Visibility = Visibility.Hidden;
            UpdateMenu.Visibility = Visibility.Visible;
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            DataBaseReadMenuAdmin.Visibility = Visibility.Hidden;
            CreateMenu.Visibility = Visibility.Visible;
        }

        
    }
}
