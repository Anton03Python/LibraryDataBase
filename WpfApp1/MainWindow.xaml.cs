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
using System.Windows.Navigation;
using System.Windows.Shapes;


namespace WpfApp1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (TextBox1.Text == "admin" && PasswordBox1.Password == "admin")
            {
                MainMenu.Visibility = Visibility.Hidden;
                DataBaseMenu.Visibility = Visibility.Visible;
            }
            else
            {
                MessageBox.Show("Вы ввели неверный логин или пароль." + "\n" + "Пожалуйста повторите попытку ещё раз.", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
                TextBox1.Clear();
                PasswordBox1.Clear();
            }
        }
    }
}
