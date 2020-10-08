using System;
using System.Windows;
using System.Windows.Input;
using System.Data;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;


namespace DB_Store
{
    public partial class LoginWindow : Window
    {
        //private MySqlConnection connection;
        bool close = true;
        MySqlConnection conn;
        string Login_name;

        public LoginWindow()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Animation animation = new Animation();
            animation.MoveLoginWindow(this);
        }

        private void grid_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (InvalidOperationException) { }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            close = true;
            Close();
        }

        public bool GetStatus()
        {
            return close;
        }

        public MySqlConnection GetConnection()
        {
            return conn;
        }

        public string GetUserName()
        {
            return Login_name;
        }

        private void buttonLogin_Click(object sender, RoutedEventArgs e)
        {
            //connecion = new SqlConnection("Data Source=localhost;Initial Catalog=android_test;User ID=" + textBoxLogin.Text + ";Password=" + passwordBox.Password + ";");
            String connString = "Server=" + "localhost" + ";Database=" + "android_test"
                + ";port=" + "3306" + ";User Id=" + textBoxLogin.Text + ";password=" + passwordBox.Password;
            Login_name = textBoxLogin.Text;
            conn = new MySqlConnection(connString);

            //return conn;

            try
            {
                //connecion.Open();
                conn.Open();
            }
            catch (MySqlException)
            {
                MessageBox.Show("Неверный логин или пароль!", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            
            if (conn.State == ConnectionState.Open)
            {
                textBoxLogin.Text = "11111";
                close = false;
                
                //MainWindow mainWindow = new MainWindow();
                //mainWindow.Show();
                Close();
            }
        }

        private void textBoxLogin_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            int code = Convert.ToChar(e.Text);
            //if ((code >= 65 && code <= 90) || (code >= 97 && code <= 122))
            //    return;
            //else
            //    e.Handled = true;
        }

        private void textBoxLogin_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void textBoxLogin_LostFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxLogin.Text == "")
                textBoxLogin.Text = "Имя пользователя";
        }

        private void textBoxLogin_GotFocus(object sender, RoutedEventArgs e)
        {
            if (textBoxLogin.Text == "Имя пользователя")
                textBoxLogin.Text = "";
        }

        private void passwordBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Password == "Пароль")
                passwordBox.Password = "";
        }

        private void passwordBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Password == "")
                passwordBox.Password = "Пароль";
        }

        private void ButtonReg_Click(object sender, RoutedEventArgs e)
        {
            NewClientWindow form_reg = new NewClientWindow();
            form_reg.ShowDialog();
        }
    }
}