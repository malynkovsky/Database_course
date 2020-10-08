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
using MySql.Data.MySqlClient;
using System.Data;

namespace DB_Store
{
    public partial class NewClientWindow : Window
    {
        //DB db;
        bool added;
        bool close = true;
        MySqlConnection conn;
        string Login_name;
        MySqlCommand cmd;
        MySqlDataAdapter da;
        DataTable dt;
        DataGrid dataGrid;

        public NewClientWindow()
        {
            InitializeComponent();

            //db = _db;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Animation animation = new Animation();
            animation.MoveNewClientWindow(this);
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            added = false;
            this.Close();
        }

        private void titleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            //db.InsertTableClientsStaff("Clients", textBoxSurnameClients.Text, textBoxNameClients.Text, textBoxPatronymicClients.Text, textBoxPhoneClients.Text, textBoxAddresClients.Text, false);
            //connecion = new SqlConnection("Data Source=localhost;Initial Catalog=android_test;User ID=" + textBoxLogin.Text + ";Password=" + passwordBox.Password + ";");
            String connString = "Server=" + "localhost" + ";Database=" + "android_test"
                + ";port=" + "3306" + ";User Id=" + "newuser2" + ";password=" + "Rock96862013";
            //Login_name = textBoxLogin.Text;
            string surname_mother = textBoxSurnameMother.Text;
            string surname_father = textBoxSurnameFather.Text;
            string name_mother = textBoxNameMother.Text;
            string name_father = textBoxNameFather.Text;
            string patronymic_mother = textBoxPatronymicMother.Text;
            string patronymic_father = textBoxPatronymicFather.Text;
            string phone_mother = textBoxPhoneMother.Text;
            string phone_father = textBoxPhoneFather.Text;
            string passport_mother = textBoxPassportMother.Text;
            string passport_father = textBoxPassortFather.Text;
            string password_user = textBoxPasswordReg.Text;

            conn = new MySqlConnection(connString);

            //return conn;

            try
            {
                //connecion.Open();
                conn.Open();
                cmd = new MySqlCommand("SELECT * FROM parents WHERE passport_mother=" + passport_mother + " and passport_father = " + passport_father, conn);
                da = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count == 1)
                {

                    cmd = new MySqlCommand("CREATE USER '" +dt.Rows[0][0].ToString() +"'@'localhost' IDENTIFIED BY '" + password_user + "'; grant select on * to '" + dt.Rows[0][0].ToString() + "'@'localhost';", conn);
                    textBoxNameMother.Text = "CREATE USER '" + dt.Rows[0][0].ToString() + "'@'localhost' IDENTIFIED BY '" + password_user + "'";
                    da = new MySqlDataAdapter(cmd);
                    cmd.ExecuteNonQuery();
                    //dt = new DataTable();
                    //da.Fill(dt);
                }
                conn.Close();
                MessageBox.Show("Успешно!   Логин: " + dt.Rows[0][0].ToString() + " Пароль: " + password_user);
            }
            catch (MySqlException)
            {
                MessageBox.Show("Неверный логин или пароль!", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
            }

            //if (conn.State == ConnectionState.Open)
            //{
            //    textBoxLogin.Text = "11111";
            //    close = false;

            //    //MainWindow mainWindow = new MainWindow();
            //    //mainWindow.Show();
            //    Close();
            //}
            added = true;
            //this.Close();
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            added = false;
            this.Close();
        }

        public bool GetStatus()
        {
            return added;
        }

        private void PreviewKeyDownCheck(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void PreviewTextInputCheck(object sender, TextCompositionEventArgs e)
        {
            if (!Checks.TextInput(sender as TextBox, e))
                e.Handled = true;
        }

        private void TextChangedCheck(object sender, TextChangedEventArgs e)
        {
            Checks.TextChanged(sender as TextBox, 0);
        }
    }
}