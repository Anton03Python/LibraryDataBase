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
        string NameDataBase;
        DataSet ds;
        private DataTable dsAutors;

        public MainWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
            connection = new SqlConnection(connectionString);
            connection.Open();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            SqlDataAdapter da;

            ConnectionDataBase("Book");
            BookGrid.ItemsSource = libraryTable.DefaultView;
            BookGridUser.ItemsSource = libraryTable.DefaultView;

            ConnectionDataBase("Author");
            AuthorGrid.ItemsSource = libraryTable.DefaultView;
            AuthorGridUser.ItemsSource = libraryTable.DefaultView;

            
            ConnectionDataComboBox(out da, out ds, "Author");
            dsAutors = ds.Tables[0];
            AuthorComboBox.ItemsSource = ds.Tables[0].DefaultView;
            AuthorComboBox.DisplayMemberPath = ds.Tables[0].Columns["Author"].ToString();

            ConnectionDataBase("Genre");
            GenreGrid.ItemsSource = libraryTable.DefaultView;
            GenreGridUser.ItemsSource = libraryTable.DefaultView;

            
            ConnectionDataComboBox(out da, out ds, "Genre");
            GenreComboBox.ItemsSource = ds.Tables[0].DefaultView;
            GenreComboBox.DisplayMemberPath = ds.Tables[0].Columns["Genre"].ToString();

            ConnectionDataBase("Publisher");
            PublisherGrid.ItemsSource = libraryTable.DefaultView;
            PublisherGridUser.ItemsSource = libraryTable.DefaultView;

            
            ConnectionDataComboBox(out da, out ds, "Publisher");
            PublisherComboBox.ItemsSource = ds.Tables[0].DefaultView;
            PublisherComboBox.DisplayMemberPath = ds.Tables[0].Columns["Publisher"].ToString();
        }

        private void ConnectionDataBase(string NameDataBase)
        {
            string sql = "SELECT * FROM " + NameDataBase;
            libraryTable = new DataTable();
            try
            {
                connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(sql, connection);
                adapter = new SqlDataAdapter(command);
                // установка команды на добавление для вызова хранимой процедуры 
                adapter.InsertCommand = new SqlCommand("sp_Insert" + NameDataBase, connection);
                adapter.InsertCommand.CommandType = CommandType.StoredProcedure;
                adapter.InsertCommand.Parameters.Add(new SqlParameter("@" + NameDataBase, SqlDbType.VarChar, 50, NameDataBase));
                SqlParameter parameter = adapter.InsertCommand.Parameters.Add("@Id" + NameDataBase, SqlDbType.Int, 0, "Id" + NameDataBase);
                parameter.Direction = ParameterDirection.Output;
                connection.Open();
                adapter.Fill(libraryTable);
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

        private void SignInMenu_Click(object sender, RoutedEventArgs e)
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

        private void BackReadOrCreatMenu_Click(object sender, RoutedEventArgs e)
        {
            ReadOrCreatMenu.Visibility = Visibility.Hidden;
            MainMenu.Visibility = Visibility.Visible;
        }

        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            ReadOrCreatMenu.Visibility = Visibility.Hidden;
            DataBaseReadMenuAdmin.Visibility = Visibility.Visible;


        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            UpdateDB();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result;
            if (BookGrid.SelectedItem == null && AuthorGrid.SelectedItem == null && GenreGrid.SelectedItem == null && PublisherGrid.SelectedItem == null)
            {
                result = MessageBox.Show("При попытке удаления вы не выделели ячейку данных, которую хотите удалить." + "\n" + "Повторите попытку ещё раз!", "Ошибка удаления данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                MessageBoxResult result1 = MessageBox.Show("Вы действительно хотите совершить удаление?" + "\n" + "Выделенные вами данные будут удалены безвозвратно!", "Подтверждение удаления данных", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result1 == MessageBoxResult.Yes)
                {
                    if (BookGrid.SelectedItems != null)
                    {
                        for (int i = 0; i < BookGrid.SelectedItems.Count; i++)
                        {
                            DataRowView datarowView = BookGrid.SelectedItems[i] as DataRowView;
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

        private void EditorButton_Click(object sender, RoutedEventArgs e)
        {
            DataBaseReadMenuAdmin.Visibility = Visibility.Hidden;
            UpdateMenu.Visibility = Visibility.Visible;

        }

        private void CreateButtonInReadMenuAdmin_Click(object sender, RoutedEventArgs e)
        {
            DataBaseReadMenuAdmin.Visibility = Visibility.Hidden;
            CreateMenu.Visibility = Visibility.Visible;
        }

        private void BackUpdateMenu_Click(object sender, RoutedEventArgs e)
        {
            UpdateMenu.Visibility = Visibility.Hidden;
            DataBaseReadMenuAdmin.Visibility = Visibility.Visible;
        }

        private void UpdateDB()
        {
            SqlCommandBuilder comandbuilder = new SqlCommandBuilder(adapter);
            adapter.Update(libraryTable);
        }

        private void ConnectionDataComboBox(out SqlDataAdapter da, out DataSet ds, string NameDataBase)
        {
            da = new SqlDataAdapter("Select " + NameDataBase + " FROM " + NameDataBase, connection);
            ds = new DataSet();
            da.Fill(ds, NameDataBase);
        }

        public void Insert(string value, string table)
        {
            connection.Open();
            SqlCommand command = new SqlCommand("INSERT INTO " + table + " VALUES (" + value + ")", connection);
            command.ExecuteNonQuery();
            connection.Close();
        }
        
        public string Select(string ComboBoxText, string NameDataBase)
        {
            DataSet dts = new DataSet();
            SqlCommand command = new SqlCommand("SELECT Id" + NameDataBase + " FROM " + NameDataBase + " WHERE " + NameDataBase + "=" + "'" + ComboBoxText + "'" , connection);

            using (SqlDataAdapter adapter = new SqlDataAdapter(command))
            {
                adapter.Fill(dts);
            }
            string id = dts.Tables[0].Rows[0][0].ToString();
            return id;
            
        }

        private void CreatMenuInUpdateMenu_Click(object sender, RoutedEventArgs e)
        {
            UpdateMenu.Visibility = Visibility.Hidden;
            CreateMenu.Visibility = Visibility.Visible;
        }

        private void SaveButtonInUpdateMenu_Click(object sender, RoutedEventArgs e)
        {
            var idAuthor = Select(AuthorComboBox.Text, "Author");
            var idGenre = Select(GenreComboBox.Text, "Genre");
            var idPublisher = Select(PublisherComboBox.Text, "Publisher");
            Insert(idAuthor + ", " + idGenre + ", " + idPublisher + ", '" + NameBookTextBox.Text + "'", "Book");
        }
    }
}