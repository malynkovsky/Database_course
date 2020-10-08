using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Data;
using System.Globalization;
using Excel = Microsoft.Office.Interop.Excel;
using MySql.Data.MySqlClient;

namespace DB_Store
{
    public partial class MainWindow : Window
    {
        DB db;
        DispatcherTimer timer;
        DateTime time;
        Grid currGrid = new Grid();
        Button currButton = new Button();
        string currStyle;
        DataRowView row;
        bool canSelect = true;
        int countDevices;
        DataTable dt = new DataTable();
        string Log_name;
        string current_kid_id;

        public MainWindow()
        {
            InitializeComponent();

            LoginWindow form = new LoginWindow();
            form.ShowDialog();
            if (form.GetStatus())
                Close();
            else
            {
                Log_name = form.GetUserName();
                Show();
                db = new DB(dataGrid);
                db.SetConnection(form.GetConnection());
                if (!(Log_name.Contains("newuser")) && (Log_name != "1Book"))
                {
                    buttonStorage.Visibility = Visibility.Hidden;
                    db.SelectTable("Kids", Log_name);
                    HideButton(0);
                    current_kid_id = dt.Rows[0][0].ToString();
                    textBoxFilter.AppendText(current_kid_id);
                    //currGrid.Visibility = Visibility.Hidden;
                    gr_sc_Copy.Visibility = Visibility.Hidden;
                    gr_sc_Copy1.Visibility = Visibility.Hidden;
                    gr_sc_Copy2.Visibility = Visibility.Hidden;
                    gr_sc_Copy3.Visibility = Visibility.Hidden;
                    gr_sc_Copy4.Visibility = Visibility.Hidden;
                    textBoxCountOrder_Copy.Visibility = Visibility.Hidden;
                    comboBoxOrdersStaff_Copy.Visibility = Visibility.Hidden;
                    comboBoxOrdersStaff_Copy1.Visibility = Visibility.Hidden;
                    comboBoxOrdersStaff_Copy2.Visibility = Visibility.Hidden;
                    comboBoxOrdersStaff_Copy3.Visibility = Visibility.Hidden;
                    buttonDelete_Copy.Visibility = Visibility.Hidden;
                    buttonAdd_Copy.Visibility = Visibility.Hidden;
                    buttonDelivery.Visibility = Visibility.Hidden;
                }
                else
                {
                    if(Log_name == "newuser2")
                    {
                        //buttonOrders.Visibility = Visibility.Visible;
                        buttonBrands.Visibility = Visibility.Visible;
                        buttonTypes.Visibility = Visibility.Visible;
                        buttonModels.Visibility = Visibility.Visible;
                        buttonApplications.Visibility = Visibility.Hidden;
                        buttonStorage.Visibility = Visibility.Visible;
                        buttonDelivery.Visibility = Visibility.Visible;

                        //buttonDelivery.Visibility = Visibility.Hidden;
                    }
                }
                
                timer = new DispatcherTimer();
                timer.Tick += new EventHandler(timerTick);
                timer.Interval = new TimeSpan(0, 0, 0, 1);
                time = DateTime.Now;
                timer.Start();
                labelDate.Content = time.ToLongDateString();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Animation animation = new Animation();
            animation.MoveMainWindow(this);
        }

        private void titleBar_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch (InvalidOperationException) { }
        }

        private void buttonClose_Click(object sender, RoutedEventArgs e)
        {
            db.CloseConnection();
            timer.Stop();
            Close();
        }

        private void buttonMinimize_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void timerTick(object sender, EventArgs e)
        {
            hello.Visibility = Visibility.Hidden;
            labelTime.Content = DateTime.Now.Subtract(time).ToString(@"hh\:mm\:ss");
        }

        private void GridVisibility(Grid newGrid)
        {
            currGrid.Visibility = Visibility.Hidden;
            newGrid.Visibility = Visibility.Visible;
            currGrid = newGrid;
        }

        private void ButtonStyle(Button button, string style)
        {
            try
            {
                currButton.Style = (Style)button.FindResource(currStyle);
            }
            catch (ArgumentNullException) { }

            button.Style = (Style)button.FindResource(style + "Active");
            currButton = button;
            currStyle = style;
        }

        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName.Contains("ID") || e.PropertyName.Contains("id_t") || e.PropertyName.Contains("id_kid") || e.PropertyName.Contains("id_bab") || e.PropertyName.Contains("id_pa") || e.PropertyName.Contains("id_adm") || e.PropertyName == "id_Schedule" )
                e.Column.Visibility = Visibility.Hidden;

            if (e.PropertyType == typeof(DateTime))
                (e.Column as DataGridTextColumn).Binding.StringFormat = "dd.MM.yyyy";
            
            if (currGrid.Name == "gridBrands" || currGrid.Name == "gridTypes" || currGrid.Name == "gridModels" || currGrid.Name == "gridApplications" || currGrid.Name == "gridStorage")
            {
                e.Column.Width = DataGridLength.Auto;
                //e.Column.Width = (dataGrid.Width - 19) / 8;
                
                if (e.PropertyName == "name_teacher")
                    e.Column.Header = "Имя";
                if (e.PropertyName.Contains("patronymic"))
                    e.Column.Header = "Отчество";
                if (e.PropertyName.Contains("subject"))
                    e.Column.Header = "Предмет";
                if (e.PropertyName.Contains("passport"))
                    e.Column.Header = "Пасспорт";
                if (e.PropertyName.Contains("birthday"))
                    e.Column.Header = "Дата рождения";
                if (e.PropertyName.Contains("phone"))
                    e.Column.Header = "Телефон";
                if (e.PropertyName.Contains("name_tutor"))
                    e.Column.Header = "Имя";
                if (e.PropertyName.Contains("name_baby"))
                    e.Column.Header = "Имя";
                if (e.PropertyName.Contains("name_kid"))
                    e.Column.Header = "Имя";
                if (e.PropertyName == "name_administrator")
                    e.Column.Header = "Имя";
                if (e.PropertyName.Contains("surname"))
                    e.Column.Header = "Фамилия";
                if (e.PropertyName.Contains("group"))
                    e.Column.Header = "Группа";
                if (e.PropertyName.Contains("work"))
                    e.Column.Header = "Должность";
            }
            e.Column.Width = DataGridLength.Auto;

            if (currGrid.Name == "gridRecords")
            {
                if (e.PropertyName.Contains("teacher"))
                    e.Column.Header = "Учитель";
                if (e.PropertyName.Contains("tutor"))
                    e.Column.Header = "Воспитатель";
                if (e.PropertyName.Contains("baby"))
                    e.Column.Header = "Няня";
                if (e.PropertyName.Contains("lesson"))
                    e.Column.Header = " ";
                if (e.PropertyName.Contains("start"))
                {
                    e.Column.Header = "Начало";
                    //(e.Column as DataGridTextColumn).Binding.StringFormat = "hh:mm";
                }
                   
                if (e.PropertyName.Contains("end"))
                    e.Column.Header = "Конец";
                if (e.PropertyName.Contains("subject"))
                    e.Column.Header = "Предмет";
            }
            if (currGrid.Name == "gridDelivery")
            {
                if (e.PropertyName.Contains("surname_mother"))
                    e.Column.Header = "Мать";
                if (e.PropertyName.Contains("surname_father"))
                    e.Column.Header = "Отец";
                if (e.PropertyName.Contains("surname_book"))
                    e.Column.Header = "Бухгалтер";
                if (e.PropertyName.Contains("passport"))
                    e.Column.Header = "Паспорт";
                
                if (e.PropertyName.Contains("phone"))
                    e.Column.Header = "Телефон";
                if (e.PropertyName.Contains("cost"))
                    e.Column.Header = "Сумма к оплате";
            }
        }

        //бренды, пока что через неё подгрузим всю таблицу учителей
        private void buttonBrands_Click(object sender, RoutedEventArgs e)
        {
            grid_main.Visibility = Visibility.Visible ;
            grid_main_person_info.Visibility = Visibility.Hidden;
            grid_main_schedule.Visibility = Visibility.Hidden;
            grid_admin_info.Visibility = Visibility.Hidden;
            grid_main_pay.Visibility = Visibility.Hidden;
            ButtonStyle(buttonBrands, "TopButton");
            GridVisibility(gridBrands);
            db.SelectTable("Teachers",Log_name);
            HideButton(0);
            dataGrid.ItemsSource = dt.DefaultView;
        }

        private void buttonTypes_Click(object sender, RoutedEventArgs e)
        {
            grid_main.Visibility = Visibility.Visible;
            grid_main_person_info.Visibility = Visibility.Hidden;
            grid_main_schedule.Visibility = Visibility.Hidden;
            grid_main_pay.Visibility = Visibility.Hidden;
            ButtonStyle(buttonTypes, "TopButton");
            GridVisibility(gridTypes);
            db.SelectTable("Tutors", Log_name);
            HideButton(0);
            dataGrid.ItemsSource = dt.DefaultView;
        }

        private void buttonModels_Click(object sender, RoutedEventArgs e)
        {
            grid_main.Visibility = Visibility.Visible;
            grid_main_person_info.Visibility = Visibility.Hidden;
            grid_main_schedule.Visibility = Visibility.Hidden;
            grid_main_pay.Visibility = Visibility.Hidden;
            ButtonStyle(buttonModels, "TopButton");
            GridVisibility(gridModels);
            db.SelectTable("Baby", Log_name);
            grid_main_person_info.Visibility = Visibility.Hidden;
            HideButton(0);
            dataGrid.ItemsSource = dt.DefaultView;
        }

        private void buttonDevices_Click(object sender, RoutedEventArgs e)
        {
            ButtonStyle(buttonDevices, "TopButton");
            GridVisibility(gridDevices);
           // db.SelectTable("View_Devices");

            labelCount.Content = db.GetRowsCount("View_Devices").ToString();

            HideButton(0);
        }

        private void buttonClients_Click(object sender, RoutedEventArgs e)
        {
            ButtonStyle(buttonClients, "TopButton");
            GridVisibility(gridClients);
           // db.SelectTable("View_Clients");

            labelCount.Content = db.GetRowsCount("View_Clients").ToString();

            HideButton(0);
        }

        private void buttonStaff_Click(object sender, RoutedEventArgs e)
        {
            ButtonStyle(buttonStaff, "TopButton");
            GridVisibility(gridStaff);
           // db.SelectTable("View_Staff");

            labelCount.Content = db.GetRowsCount("View_Staff").ToString();

            HideButton(0);
        }

        private void buttonSuppliers_Click(object sender, RoutedEventArgs e)
        {
            ButtonStyle(buttonSuppliers, "TopButton");
            GridVisibility(gridSuppliers);
            //db.SelectTable("View_Suppliers");
            grid_main_person_info.Visibility = Visibility.Hidden;
            labelCount.Content = db.GetRowsCount("View_Suppliers").ToString();

            HideButton(0);
        }

        //Личные данные
        private void buttonApplications_Click(object sender, RoutedEventArgs e)
        {
            ButtonStyle(buttonApplications, "LeftMenuButtonApplication");
            GridVisibility(gridApplications);
            grid_main.Visibility = Visibility.Hidden;
            grid_main_schedule.Visibility = Visibility.Hidden;
            grid_main_person_info.Visibility = Visibility.Visible;
            grid_admin_info.Visibility = Visibility.Hidden;
            db.SelectTable("Parents",Log_name);
            string mother_name = db.GetDataTable("Parents").Rows[0][2].ToString() + " " + db.GetDataTable("Parents").Rows[0][3].ToString() + " " + db.GetDataTable("Parents").Rows[0][4].ToString();
            textBoxMother.Text = mother_name;
            string father_name = db.GetDataTable("Parents").Rows[0][7].ToString() + " " + db.GetDataTable("Parents").Rows[0][8].ToString() + " " + db.GetDataTable("Parents").Rows[0][9].ToString();
            textBoxFather.Text = father_name;
            string payment_info = "Отсутсвует";
            Billing.Foreground = Brushes.ForestGreen;
            textBoxDate.Text = "0 руб.";
            if (db.GetDataTable("Parents").Rows[0][11].ToString() == "1")
            {
                Billing.Foreground = Brushes.Red;
                payment_info = "Присутсвует";
            }
            Billing.Content = payment_info;
            db.SelectTable("Kids", Log_name);
            HideButton(0);
            dataGrid_kids.ItemsSource = dt.DefaultView;
            current_kid_id = dt.Rows[0][0].ToString();
            if (payment_info == "Присутсвует")
            {
                db.SelectTable("Rub", Log_name);
                HideButton(0);
                textBoxDate.Text = dt.Rows[0][0].ToString() + " руб.";
            }
        }

        private void buttonRecords_Click(object sender, RoutedEventArgs e)
        {
            grid_admin_info.Visibility = Visibility.Hidden;
            if (!Log_name.Contains("newuser"))
            {
                ButtonStyle(buttonRecords, "LeftMenuButtonRecords");

                grid_main.Visibility = Visibility.Hidden;
                grid_main_schedule.Visibility = Visibility.Visible;
                grid_main_person_info.Visibility = Visibility.Hidden;
                GridVisibility(gridRecords);

                db.SelectTable("Schedule_1", Log_name, current_kid_id);
                HideButton(0);

                Monday.ItemsSource = dt.DefaultView;
                db.SelectTable("Schedule_2", Log_name, current_kid_id);
                HideButton(0);
                Tuesday.ItemsSource = dt.DefaultView;
                db.SelectTable("Schedule_3", Log_name, current_kid_id);
                HideButton(0);
                Wensday.ItemsSource = dt.DefaultView;
                db.SelectTable("Schedule_4", Log_name, current_kid_id);
                HideButton(0);
                Thursday.ItemsSource = dt.DefaultView;
                db.SelectTable("Schedule_5", Log_name, current_kid_id);
                HideButton(0);
                Friday.ItemsSource = dt.DefaultView;
                db.SelectTable("Kids_n", Log_name);
                HideButton(0);
                comboBoxOrdersStaff.Text = dt.Rows[0][0].ToString();
                comboBoxOrdersStaff.ItemsSource = dt.DefaultView;

                comboBoxOrdersStaff.DisplayMemberPath = dt.Columns[0].ToString();
            }
            else
            {
                ButtonStyle(buttonRecords, "LeftMenuButtonRecords");
                grid_main.Visibility = Visibility.Hidden;
                grid_main_schedule.Visibility = Visibility.Visible;
                grid_main_person_info.Visibility = Visibility.Hidden;
                GridVisibility(gridRecords);
                Sched.Content = "Группы";
                gr_sc.Content = "День недели";
                //textBoxCountOrder.Visibility = Visibility.Hidden;
                db.SelectTable("groups_adm", Log_name);
                HideButton(0);
                textBoxDateOrder.Text = dt.Rows[0][4].ToString();


                comboBoxOrdersStaff.ItemsSource = dt.DefaultView;
                comboBoxOrdersStaff.SelectedItem = dt.DefaultView[0];
                comboBoxOrdersStaff.DisplayMemberPath = dt.Columns[1].ToString();
                string gr_id = ((DataRowView)comboBoxOrdersStaff.SelectedItem)[0].ToString();
                comboBoxOrdersStaff.Text = dt.Rows[0][1].ToString();
                db.SelectTable("schedule_adm_1", Log_name, gr_id);
                HideButton(0);

                Monday.ItemsSource = dt.DefaultView;
                db.SelectTable("schedule_adm_2", Log_name, gr_id);
                HideButton(0);
                Tuesday.ItemsSource = dt.DefaultView;
                db.SelectTable("schedule_adm_3", Log_name, gr_id);
                HideButton(0);
                Wensday.ItemsSource = dt.DefaultView;
                db.SelectTable("schedule_adm_4", Log_name, gr_id);
                HideButton(0);
                Thursday.ItemsSource = dt.DefaultView;
                db.SelectTable("schedule_adm_5", Log_name, gr_id);
                HideButton(0);
                Friday.ItemsSource = dt.DefaultView;

                db.SelectTable("tutors_for_subject", current_kid_id);
                HideButton(0);
                comboBoxOrdersStaff_Copy2.ItemsSource = dt.DefaultView;
                comboBoxOrdersStaff_Copy2.SelectedItem = dt.DefaultView[0];
                comboBoxOrdersStaff_Copy2.DisplayMemberPath = dt.Columns[1].ToString();
                comboBoxOrdersStaff_Copy2.Text = dt.Rows[0][1].ToString();

                db.SelectTable("baby_for_subject", current_kid_id);
                HideButton(0);
                comboBoxOrdersStaff_Copy3.ItemsSource = dt.DefaultView;
                comboBoxOrdersStaff_Copy3.SelectedItem = dt.DefaultView[0];
                comboBoxOrdersStaff_Copy3.DisplayMemberPath = dt.Columns[1].ToString();
                comboBoxOrdersStaff_Copy3.Text = dt.Rows[0][1].ToString();
            }
        }

        private void buttonOrders_Click(object sender, RoutedEventArgs e)
        {
            ButtonStyle(buttonOrders, "LeftMenuButtonOrders");
            GridVisibility(gridOrders);
            //db.SelectTable("View_Orders");

            labelCount.Content = db.GetRowsCount("View_Orders").ToString();

            HideButton(0);
        }

        private void buttonDelivery_Click(object sender, RoutedEventArgs e)
        {
            grid_main.Visibility = Visibility.Hidden;
            grid_main_person_info.Visibility = Visibility.Hidden;
            grid_main_schedule.Visibility = Visibility.Hidden;
            grid_admin_info.Visibility = Visibility.Hidden;
            grid_main_pay.Visibility = Visibility.Visible;
            comboBoxDeliverySupplier.SelectedIndex = 0;
            ButtonStyle(buttonDelivery, "LeftMenuButtonDelivery");
            GridVisibility(gridDelivery);
            db.SelectTable("Pay_list", Log_name, "All");
            HideButton(0);
            Parent_pay_table.ItemsSource = dt.DefaultView;
        }

        private void buttonStorage_Click(object sender, RoutedEventArgs e)
        {
            ButtonStyle(buttonStorage, "LeftMenuButtonStorage");
            GridVisibility(gridStorage);
            grid_admin_info.Visibility = Visibility.Visible;

            db.SelectTable("Admin_i",Log_name);
            HideButton(0);
            Admin_i.ItemsSource = dt.DefaultView;



        }

        private void buttonSale_Click(object sender, RoutedEventArgs e)
        {
            ButtonStyle(buttonSale, "LeftMenuButtonSale");
            GridVisibility(gridSales);
           // db.SelectTable("View_Sales");
            labelCount.Content = db.GetRowsCount("View_Sales").ToString();

            HideButton(0);
        }

        private void buttonRecordsOfSales_Click(object sender, RoutedEventArgs e)
        {
            ButtonStyle(buttonRecordsOfSales, "LeftMenuButtonRecordsOfSales");
            GridVisibility(gridRecordsOfSales);
           // db.SelectTable("View_RecordsOfSale");

            labelCount.Content = db.GetRowsCount("View_RecordsOfSale").ToString();

            HideButton(0);
        }

        private void buttonUpdateBrands_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxBrand.Background == Brushes.White)
            {
                if (canSelect)
                {
                    row = dataGrid.SelectedItem as DataRowView;
                    db.UpdateTableBrandsTypes("Brands", textBoxBrand.Text, row[0].ToString());
                    dataGrid.SelectedIndex = 0;
                }
                else
                {
                    if (db.CheckNames("Brand", textBoxBrand.Text) != 1)
                    {
                        db.InsertTableBrandsTypes("Brands", textBoxBrand.Text);

                        int count = db.GetRowsCount("View_Brands");
                        HideButton(count - 1);
                        labelCount.Content = count.ToString();
                    }
                    else
                        MessageBox.Show("Марка с таким названием уже существует!", "Ошибка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
                MessageBox.Show("Некорректный ввод данных! Повторите ввод!", "Ошибка обновления таблицы", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void buttonUpdateTypes_Click(object sender, RoutedEventArgs e)
        {
            //if (textBoxType.Background == Brushes.White)
            //{
            //    if (canSelect)
            //    {
            //        row = dataGrid.SelectedItem as DataRowView;
            //        db.UpdateTableBrandsTypes("TypesOfDevices", textBoxType.Text, row[0].ToString());
            //        dataGrid.SelectedIndex = 0;
            //    }
            //    else
            //    {
            //        if (db.CheckNames("Type", textBoxType.Text) != 1)
            //        {
            //            db.InsertTableBrandsTypes("TypesOfDevices", textBoxType.Text);

            //            int count = db.GetRowsCount("View_TypesOfDevices");
            //            HideButton(count - 1);
            //            labelCount.Content = count.ToString();
            //        }
            //        else
            //            MessageBox.Show("Такой тип уже существует!", "Ошибка данных", MessageBoxButton.OK, MessageBoxImage.Error);
            //    }
            //}
            //else
            //    MessageBox.Show("Некорректный ввод данных! Повторите ввод!", "Ошибка обновления таблицы", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void buttonUpdateModels_Click(object sender, RoutedEventArgs e)
        {
        //    if (comboBoxModelsBrands.Background == Brushes.White && comboBoxModelsTypes.Background == Brushes.White && textBoxModel.Background == Brushes.White)
        //    {
        //        if (canSelect)
        //        {
        //            row = dataGrid.SelectedItem as DataRowView;
        //            db.UpdateTableModels(comboBoxModelsBrands.SelectedValue.ToString(), comboBoxModelsTypes.SelectedValue.ToString(), textBoxModel.Text, row[0].ToString());
        //            dataGrid.SelectedIndex = 0;
        //        }
        //        else
        //        {
        //            if (db.CheckModel(textBoxModel.Text, comboBoxModelsBrands.SelectedValue.ToString(), comboBoxModelsTypes.SelectedValue.ToString()) != 1)
        //            {
        //                db.InsertTableModels(comboBoxModelsBrands.SelectedValue.ToString(), comboBoxModelsTypes.SelectedValue.ToString(), textBoxModel.Text);

        //                int count = db.GetRowsCount("View_Models");
        //                HideButton(count - 1);
        //                labelCount.Content = count.ToString();
        //            }
        //            else
        //                MessageBox.Show("Такая модель уже существует!", "Ошибка данных", MessageBoxButton.OK, MessageBoxImage.Error);
        //        }
        //    }
        //    else
        //        MessageBox.Show("Некорректный ввод данных! Повторите ввод!", "Ошибка обновления таблицы", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void buttonUpdateDevices_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxDevicesModels.Background == Brushes.White && textBoxPrice.Background == Brushes.White)
            {
                if (canSelect)
                {
                    row = dataGrid.SelectedItem as DataRowView;
                    db.UpdateTableDevices(comboBoxDevicesModels.SelectedValue.ToString(), textBoxPrice.Text, row[0].ToString());
                    dataGrid.SelectedIndex = 0;
                }
                else
                {
                    db.InsertTableDevices(comboBoxDevicesModels.SelectedValue.ToString(), textBoxPrice.Text);

                    int count = db.GetRowsCount("View_Devices");
                    HideButton(count - 1);
                    labelCount.Content = count.ToString();
                }
            }
            else
                MessageBox.Show("Некорректный ввод данных! Повторите ввод!", "Ошибка обновления таблицы", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void buttonUpdateClients_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxSurnameClients.Background == Brushes.White && textBoxNameClients.Background == Brushes.White && textBoxPatronymicClients.Background == Brushes.White && textBoxPhoneClients.Background == Brushes.White && textBoxAddresClients.Background == Brushes.White)
            {
                if (canSelect)
                {
                    row = dataGrid.SelectedItem as DataRowView;
                    db.UpdateTableClientsStaff("Clients", textBoxSurnameClients.Text, textBoxNameClients.Text, textBoxPatronymicClients.Text, textBoxPhoneClients.Text, textBoxAddresClients.Text, row[0].ToString());
                    dataGrid.SelectedIndex = 0;
                }
                else
                {
                    if (db.CheckNames("ClientsAddres", textBoxAddresClients.Text) != 1)
                    {
                        db.InsertTableClientsStaff("Clients", textBoxSurnameClients.Text, textBoxNameClients.Text, textBoxPatronymicClients.Text, textBoxPhoneClients.Text, textBoxAddresClients.Text, true);

                        int count = db.GetRowsCount("View_Clients");
                        HideButton(count - 1);
                        labelCount.Content = count.ToString();
                    }
                    else
                        MessageBox.Show("Клиент с таким адресом уже существует!", "Ошибка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
                MessageBox.Show("Некорректный ввод данных! Повторите ввод!", "Ошибка обновления таблицы", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void buttonUpdateStaff_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxSurnameStaff.Background == Brushes.White && textBoxNameStaff.Background == Brushes.White && textBoxPatronymicStaff.Background == Brushes.White && textBoxPhoneStaff.Background == Brushes.White && textBoxAddresStaff.Background == Brushes.White)
            {
                if (canSelect)
                {
                    row = dataGrid.SelectedItem as DataRowView;
                    db.UpdateTableClientsStaff("Staff", textBoxSurnameStaff.Text, textBoxNameStaff.Text, textBoxPatronymicStaff.Text, textBoxPhoneStaff.Text, textBoxAddresStaff.Text, row[0].ToString());
                    dataGrid.SelectedIndex = 0;
                }
                else
                {
                    if (db.CheckNames("StaffAddres", textBoxAddresStaff.Text) != 1)
                    {
                        db.InsertTableClientsStaff("Staff", textBoxSurnameStaff.Text, textBoxNameStaff.Text, textBoxPatronymicStaff.Text, textBoxPhoneStaff.Text, textBoxAddresStaff.Text, true);

                        int count = db.GetRowsCount("View_Staff");
                        HideButton(count - 1);
                        labelCount.Content = count.ToString();
                    }
                    else
                        MessageBox.Show("Сотрудник с таким адресом уже существует!", "Ошибка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
                MessageBox.Show("Некорректный ввод данных! Повторите ввод!", "Ошибка обновления таблицы", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void buttonUpdateSupplier_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxSupplier.Background == Brushes.White && textBoxPhoneSupplier.Background == Brushes.White && textBoxAddresSupplier.Background == Brushes.White)
            {
                if (canSelect)
                {
                    row = dataGrid.SelectedItem as DataRowView;
                    db.UpdateTableSuppliers(textBoxSupplier.Text, textBoxPhoneSupplier.Text, textBoxAddresSupplier.Text, row[0].ToString());
                    dataGrid.SelectedIndex = 0;
                }
                else
                {
                    if (db.CheckNames("Supplier", textBoxSupplier.Text) != 1)
                    {
                        db.InsertTableSuppliers(textBoxSupplier.Text, textBoxPhoneSupplier.Text, textBoxAddresSupplier.Text);

                        int count = db.GetRowsCount("View_Suppliers");
                        HideButton(count - 1);
                        labelCount.Content = count.ToString();
                    }
                    else
                        MessageBox.Show("Поставщик с таким названием уже существует!", "Ошибка данных", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
                MessageBox.Show("Некорректный ввод данных! Повторите ввод!", "Ошибка обновления таблицы", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void buttonUpdateApplication_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxAppClients.Background == Brushes.White && comboBoxAppStaff.Background == Brushes.White && textBoxDate.Background == Brushes.White)
            {
                if (canSelect)
                {
                    row = dataGrid.SelectedItem as DataRowView;
                    db.UpdateTableApplications(comboBoxAppClients.SelectedValue.ToString(), comboBoxAppStaff.SelectedValue.ToString(), textBoxDate.Text, row[0].ToString());
                    dataGrid.SelectedIndex = 0;
                }
                else
                {
                    db.InsertTableApplications(comboBoxAppClients.SelectedValue.ToString(), comboBoxAppStaff.SelectedValue.ToString(), textBoxDate.Text);

                    int count = db.GetRowsCount("View_Applications");
                    HideButton(count - 1);
                    labelCount.Content = count.ToString();
                }
            }
            else
                MessageBox.Show("Некорректный ввод данных! Повторите ввод!", "Ошибка обновления таблицы", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void buttonUpdateRecords_Click(object sender, RoutedEventArgs e)
        {
        //    if (comboBoxRecordsApplications.Background == Brushes.White && comboBoxRecordsDevices.Background == Brushes.White && textBoxCount.Background == Brushes.White)
        //    {
        //        if (canSelect)
        //        {
        //            row = dataGrid.SelectedItem as DataRowView;
        //            db.UpdateRecordsOfApplication(comboBoxRecordsApplications.SelectedValue.ToString(), comboBoxRecordsDevices.SelectedValue.ToString(), textBoxCount.Text, row[0].ToString());
        //            dataGrid.SelectedIndex = 0;
        //        }
        //        else
        //        {
        //            db.InsertTableRecordsOfApplication(comboBoxRecordsApplications.SelectedValue.ToString(), comboBoxRecordsDevices.SelectedValue.ToString(), textBoxCount.Text);

        //            int count = db.GetRowsCount("View_RecordsOfApplication");
        //            HideButton(count - 1);
        //            labelCount.Content = count.ToString();
        //        }
        //    }
        //    else
        //        MessageBox.Show("Некорректный ввод данных! Повторите ввод!", "Ошибка обновления таблицы", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }


        //Выбор ребенка в расписании
        private void buttonUpdateOrders_Click(object sender, RoutedEventArgs e)
        {
            if (!Log_name.Contains("newuser"))
            {
                string kkkid = ((DataRowView)comboBoxOrdersStaff.SelectedItem)[0].ToString();
                db.SelectTable("Select_another", Log_name, kkkid);
                HideButton(0);
                kkkid = dt.Rows[0][0].ToString();
                db.SelectTable("Group_room", kkkid);
                HideButton(0);
                textBoxCountOrder.Text = dt.Rows[0][0].ToString();
                textBoxDateOrder.Text = dt.Rows[0][1].ToString();
                current_kid_id = kkkid;
                db.SelectTable("Schedule_1", Log_name, current_kid_id);
                HideButton(0);

                Monday.ItemsSource = dt.DefaultView;
                db.SelectTable("Schedule_2", Log_name, current_kid_id);
                HideButton(0);
                Tuesday.ItemsSource = dt.DefaultView;
                db.SelectTable("Schedule_3", Log_name, current_kid_id);
                HideButton(0);
                Wensday.ItemsSource = dt.DefaultView;
                db.SelectTable("Schedule_4", Log_name, current_kid_id);
                HideButton(0);
                Thursday.ItemsSource = dt.DefaultView;
                db.SelectTable("Schedule_5", Log_name, current_kid_id);
                HideButton(0);
                Friday.ItemsSource = dt.DefaultView;
                db.SelectTable("Kids_n", Log_name);
                HideButton(0);
            }
            else
            {
                string kkkid = ((DataRowView)comboBoxOrdersStaff.SelectedItem)[0].ToString();
                current_kid_id = kkkid;
                db.SelectTable("schedule_adm_1", Log_name, current_kid_id);
                HideButton(0);

                Monday.ItemsSource = dt.DefaultView;
                db.SelectTable("schedule_adm_2", Log_name, current_kid_id);
                HideButton(0);
                Tuesday.ItemsSource = dt.DefaultView;
                db.SelectTable("schedule_adm_3", Log_name, current_kid_id);
                HideButton(0);
                Wensday.ItemsSource = dt.DefaultView;
                db.SelectTable("schedule_adm_4", Log_name, current_kid_id);
                HideButton(0);
                Thursday.ItemsSource = dt.DefaultView;
                db.SelectTable("schedule_adm_5", Log_name, current_kid_id);
                HideButton(0);
                Friday.ItemsSource = dt.DefaultView;
                db.SelectTable("get_room", current_kid_id);
                HideButton(0);
                textBoxDateOrder.Text = dt.Rows[0][0].ToString();

                db.SelectTable("tutors_for_subject", current_kid_id);
                HideButton(0);
                comboBoxOrdersStaff_Copy2.ItemsSource = dt.DefaultView;
                comboBoxOrdersStaff_Copy2.SelectedItem = dt.DefaultView[0];
                comboBoxOrdersStaff_Copy2.DisplayMemberPath = dt.Columns[1].ToString();
                comboBoxOrdersStaff_Copy2.Text = dt.Rows[0][1].ToString();

                db.SelectTable("baby_for_subject", current_kid_id);
                HideButton(0);
                comboBoxOrdersStaff_Copy3.ItemsSource = dt.DefaultView;
                comboBoxOrdersStaff_Copy3.SelectedItem = dt.DefaultView[0];
                comboBoxOrdersStaff_Copy3.DisplayMemberPath = dt.Columns[1].ToString();
                comboBoxOrdersStaff_Copy3.Text = dt.Rows[0][1].ToString();

            }
        }

        private void buttonUpdateDelivery_Click(object sender, RoutedEventArgs e)
        {
            string view_type_pay = (comboBoxDeliverySupplier.SelectedIndex).ToString();
            if (view_type_pay == "0")
            {
                db.SelectTable("Pay_list", Log_name, "All");
                HideButton(0);
                Parent_pay_table.ItemsSource = dt.DefaultView;
            }
            else
            {
                db.SelectTable("Pay_list", Log_name, "Debtor");
                HideButton(0);
                Parent_pay_table.ItemsSource = dt.DefaultView;
            }
            //if (comboBoxDeliverySupplier.Background == Brushes.White && comboBoxDeliveryOrder.Background == Brushes.White)
            //{
            //    if (!canSelect)
            //    {
            //        db.InsertTableDelivery(comboBoxDeliverySupplier.SelectedValue.ToString(), comboBoxDeliveryOrder.SelectedValue.ToString());

            //        int count = db.GetRowsCount("View_Delivery");
            //        HideButton(count - 1);
            //        labelCount.Content = count.ToString();
            //    }
            //}
            //else
            //    MessageBox.Show("Некорректный ввод данных! Повторите ввод!", "Ошибка обновления таблицы", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void buttonUpdateSales_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxSaleApp.Background == Brushes.White && textBoxDateSale.Background == Brushes.White)
            {
                if (canSelect)
                {
                    row = dataGrid.SelectedItem as DataRowView;
                    db.UpdateSales(comboBoxSaleApp.SelectedValue.ToString(), textBoxDateSale.Text, row[0].ToString());
                    dataGrid.SelectedIndex = 0;
                }
                else
                {
                    db.InsertTableSales(comboBoxSaleApp.SelectedValue.ToString(), textBoxDateSale.Text);

                    int count = db.GetRowsCount("View_Sales");
                    HideButton(count - 1);
                    labelCount.Content = count.ToString();
                }
            }
            else
                MessageBox.Show("Некорректный ввод данных! Повторите ввод!", "Ошибка обновления таблицы", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void buttonUpdateRecordsOfSales_Click(object sender, RoutedEventArgs e)
        {
            if (comboBoxRecordsOfSalesSale.Background == Brushes.White && comboBoxRecordsOfSalesRecords.Background == Brushes.White && textBoxCountRecords.Background == Brushes.White)
            {
                if (canSelect)
                {
                    row = dataGrid.SelectedItem as DataRowView;
                    db.UpdateRecordsOfSale(comboBoxRecordsOfSalesSale.SelectedValue.ToString(), comboBoxRecordsOfSalesRecords.SelectedValue.ToString(), textBoxCountRecords.Text, row[0].ToString());
                    dataGrid.SelectedIndex = 0;
                }
                else
                {
                    db.InsertTableRecordsOfSale(comboBoxRecordsOfSalesSale.SelectedValue.ToString(), comboBoxRecordsOfSalesRecords.SelectedValue.ToString(), textBoxCountRecords.Text);

                    int count = db.GetRowsCount("View_RecordsOfSale");
                    HideButton(count - 1);
                    labelCount.Content = count.ToString();
                }
            }
            else
                MessageBox.Show("Некорректный ввод данных! Повторите ввод!", "Ошибка обновления таблицы", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void dataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //string id_subject = (comboBoxSubjects.SelectedIndex + 1).ToString();
            //if (dataGrid.i)
            //textBoxBrand.Text = dt.Rows[dataGrid.SelectedIndex][1].ToString();
            //string name_t = textBoxBrand_Copy.Text;
            //string patr_t = textBoxBrand_Copy1.Text;
            //string phone_t = textBoxBrand_Copy2.Text;
            //string pass_t = textBoxBrand_Copy3.Text;
            //string date_t = textBoxBrand_Copy4.Text;
        }

        private void buttonNewClient_Click(object sender, RoutedEventArgs e)
        {
            NewClientWindow form = new NewClientWindow();
            form.ShowDialog();

            if (form.GetStatus())
            {
                db.FillComboBox(comboBoxAppClients, "View_Clients", "ID_client", "-ФИО");

                comboBoxAppClients.SelectedValue = db.GetID("Clients", "ID_client");
            }
        }

        private void HideButton(int row)
        {
            canSelect = true;
            if (currGrid.Name == "gridBrands")
                dt = db.GetDataTable("Teachers");
            else
                dt = db.GetDataTable("Kids");
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
          
            if (currGrid.Name == "gridBrands")
            {
                //textBoxBrand.Clear();
                string id_subject = (comboBoxSubjects.SelectedIndex + 1).ToString();
                string surn_t = textBoxBrand.Text;
                string name_t = textBoxBrand_Copy.Text;
                string patr_t = textBoxBrand_Copy1.Text;
                string phone_t = textBoxBrand_Copy2.Text;
                string pass_t = textBoxBrand_Copy3.Text;
                string date_t = textBoxBrand_Copy4.Text;
                string com = "INSERT INTO android_test.teachers (surname_teacher,name_teacher,patronymic_teacher,passport,phone,birthday,id_subject) VALUES  ( '" + surn_t +"' , '" + name_t + "' , '" + patr_t + "' , " + pass_t + " , '" + phone_t + "' , '" + date_t + "' , " + id_subject + " )";
                //textBoxBrand_Copy.Text = dt.Rows[2][0].ToString();
                db.AddTeacher(com);
            }
                
            if (currGrid.Name == "gridTypes")
            {
                string surn_t = textBoxTutor.Text;
                string name_t = textBoxBrand_CopyTutor.Text;
                string patr_t = textBoxBrand_Copy1Tutor.Text;
                string phone_t = textBoxBrand_Copy2Tutor.Text;
                string pass_t = textBoxBrand_Copy3Tutor.Text;
                string date_t = textBoxBrand_Copy4Tutor.Text;
                string com = "INSERT INTO android_test.tutors (surname_tutor,name_tutor,patronymic_tutor,passport,phone,birthday) VALUES  ( '" + surn_t + "' , '" + name_t + "' , '" + patr_t + "' , " + pass_t + " , '" + phone_t + "' , '" + date_t + "' )";
                //textBoxBrand_Copy.Text = dt.Rows[2][0].ToString();
                db.AddTeacher(com);
            }

            if (currGrid.Name == "gridModels")
            {
                string surn_t = textBoxBaby.Text;
                string name_t = textBoxBrand_CopyBaby.Text;
                string patr_t = textBoxBrand_Copy1Baby.Text;
                string phone_t = textBoxBrand_Copy2Baby.Text;
                string pass_t = textBoxBrand_Copy3Baby.Text;
                string date_t = textBoxBrand_Copy4Baby.Text;
                string com = "INSERT INTO android_test.babysitters (surname_babysitter,name_babysitter,patronymic_babysitter,passport,phone,birthday) VALUES  ( '" + surn_t + "' , '" + name_t + "' , '" + patr_t + "' , " + pass_t + " , '" + phone_t + "' , '" + date_t + "' )";
                //textBoxBrand_Copy.Text = dt.Rows[2][0].ToString();
                db.AddTeacher(com);
            }
            else if (currGrid.Name == "gridDevices")
            {
                
            }
            else if (currGrid.Name == "gridClients")
            {
                
            }
            else if (currGrid.Name == "gridStaff")
            {
                
            }
            else if (currGrid.Name == "gridSuppliers")
            {
               
            }
            else if (currGrid.Name == "gridApplications")
            {
                comboBoxAppStaff.IsEnabled = true;
                comboBoxAppClients.IsEnabled = true;
                textBoxDate.IsReadOnly = true;
                //buttonNewClient.IsEnabled = true;
                buttonUpdateApplication.IsEnabled = true;

                comboBoxAppClients.Text = "";
                comboBoxAppStaff.Text = "";
            }
            else if (currGrid.Name == "gridRecords")
            {
                bool er_data = false;
                string less_id = textBoxCountOrder_Copy.Text;
                if (less_id == "1" || less_id == "2" || less_id == "3")
                {
                    er_data = true;
                }
                string wee = textBoxCountOrder.Text;
                if (wee == "1" || wee == "2" || wee == "3" || wee == "4" || wee == "5") ;
                {
                    er_data = true;
                }
                if (er_data)
                {
                    string subj_id = (comboBoxOrdersStaff_Copy.SelectedIndex + 1).ToString();
                    db.SelectTable("teacher_for_subject", subj_id);
                    HideButton(0);
                    string gr_id = ((DataRowView)comboBoxOrdersStaff.SelectedItem)[0].ToString();
                    string teach_t = ((DataRowView)comboBoxOrdersStaff_Copy1.SelectedItem)[0].ToString();
                    string tutor_t = ((DataRowView)comboBoxOrdersStaff_Copy2.SelectedItem)[0].ToString();
                    string baby_t = ((DataRowView)comboBoxOrdersStaff_Copy3.SelectedItem)[0].ToString();
                    
                    string com = "INSERT INTO android_test.schedule (group_id, day_of_week, lesson_id, subject_id, teacher_id, tutor_id, babysitter_id) VALUES  ( '" + gr_id + "' , '" + wee + "' , '" + less_id + "' , " + subj_id + " , '" + teach_t + "'  , '" +  tutor_t + "' , '"  + baby_t + "' )";
                    db.AddTeacher(com);

                }
                else
                    MessageBox.Show("Введены неккоректные данные");
                

            }
            else if (currGrid.Name == "gridOrders")
            {

            }
            else if (currGrid.Name == "gridDelivery")
            {
                
            }
            else if (currGrid.Name == "gridSales")
            {
                
            }
            else if (currGrid.Name == "gridRecordsOfSales")
            {
                
            }
        }

        private void buttonCancel_Click(object sender, RoutedEventArgs e)
        {
            HideButton(0);
        }

        private void buttonDelete_Click(object sender, RoutedEventArgs e)
        {
            if (currGrid.Name == "gridBrands")
            {
                string id_delete_t = ((DataRowView)dataGrid.SelectedItem)[0].ToString();
                string com = "DELETE FROM android_test.teachers WHERE id_teacher = " + id_delete_t;
                db.AddTeacher(com);

            }
            else if (currGrid.Name == "gridTypes")
            {
                string id_delete_t = ((DataRowView)dataGrid.SelectedItem)[0].ToString();
                string com = "DELETE FROM android_test.tutors WHERE id_tutor = " + id_delete_t;
                db.AddTeacher(com);
            }
            else if (currGrid.Name == "gridModels")
            {
                string id_delete_t = ((DataRowView)dataGrid.SelectedItem)[0].ToString();
                string com = "DELETE FROM android_test.babysitters WHERE id_babysitter = " + id_delete_t;
                db.AddTeacher(com);
            }
            else if (currGrid.Name == "gridDevices")
            {
                row = dataGrid.SelectedItem as DataRowView;
                db.DeleteFromTable("Devices", dataGrid.Columns[0].Header.ToString(), row[0].ToString());
                HideButton(0);

                labelCount.Content = db.GetRowsCount("View_Devices").ToString();
            }
            else if (currGrid.Name == "gridClients")
            {
                row = dataGrid.SelectedItem as DataRowView;
                db.DeleteFromTable("Clients", dataGrid.Columns[0].Header.ToString(), row[0].ToString());
                HideButton(0);

                labelCount.Content = db.GetRowsCount("View_Clients").ToString();
            }
            else if (currGrid.Name == "gridStaff")
            {
                row = dataGrid.SelectedItem as DataRowView;
                db.DeleteFromTable("Staff", dataGrid.Columns[0].Header.ToString(), row[0].ToString());
                HideButton(0);

                labelCount.Content = db.GetRowsCount("View_Staff").ToString();
            }
            else if (currGrid.Name == "gridSuppliers")
            {
                row = dataGrid.SelectedItem as DataRowView;
                db.DeleteFromTable("Suppliers", dataGrid.Columns[0].Header.ToString(), row[0].ToString());
                HideButton(0);

                labelCount.Content = db.GetRowsCount("View_Suppliers").ToString();
            }
            else if (currGrid.Name == "gridApplications")
            {
                row = dataGrid.SelectedItem as DataRowView;
                db.DeleteFromTable("Applications", dataGrid.Columns[0].Header.ToString(), row[0].ToString());
                HideButton(0);

                labelCount.Content = db.GetRowsCount("View_Applications").ToString();
            }
            else if (currGrid.Name == "gridRecords")
            {
                string id_s = ((DataRowView)Tuesday.SelectedItem)[0].ToString();
                string com = "DELETE FROM android_test.schedule WHERE id_Schedule = " + id_s;
                db.AddTeacher(com);
            }
            else if (currGrid.Name == "gridOrders")
            {
                row = dataGrid.SelectedItem as DataRowView;
                db.DeleteFromTable("Orders", dataGrid.Columns[0].Header.ToString(), row[0].ToString());
                HideButton(0);

                labelCount.Content = db.GetRowsCount("View_Orders").ToString();
            }
            else if (currGrid.Name == "gridDelivery")
            {
                row = dataGrid.SelectedItem as DataRowView;
                db.DeleteFromTable("Delivery", dataGrid.Columns[0].Header.ToString(), row[0].ToString());
                HideButton(0);

                labelCount.Content = db.GetRowsCount("View_Delivery").ToString();
            }
            else if (currGrid.Name == "gridSales")
            {
                row = dataGrid.SelectedItem as DataRowView;
                db.DeleteFromTable("Sales", dataGrid.Columns[0].Header.ToString(), row[0].ToString());
                HideButton(0);

                labelCount.Content = db.GetRowsCount("View_Sales").ToString();
            }
            else if (currGrid.Name == "gridRecordsOfSales")
            {
                row = dataGrid.SelectedItem as DataRowView;
                db.DeleteRecordOfSale(row[0].ToString());
                HideButton(0);

                labelCount.Content = db.GetRowsCount("View_RecordsOfSale").ToString();
            }
        }

        private void PreviewTextInputCheck(object sender, TextCompositionEventArgs e)
        {
            if (!Checks.TextInput(sender as TextBox, e))
                e.Handled = true;
        }

        private void PreviewKeyDownCheck(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
                e.Handled = true;
        }

        private void TextChangedCheck(object sender, TextChangedEventArgs e)
        {
            Checks.TextChanged(sender as TextBox, countDevices);
        }

        private void SelectionChangedCheck(object sender, SelectionChangedEventArgs e)
        {
            //if ((sender as ComboBox).Name == "comboBoxRecordsDevices")
            //{
            //    try
            //    {
            //        countDevices = db.GetCountDevicesInStorage((e.AddedItems[0] as DataRowView).Row["ID_device"].ToString());

            //        int count;
            //        if (textBoxCount.Text == "")
            //            count = 0;
            //        else count = Convert.ToInt32(textBoxCount.Text);

            //        if (count > countDevices || count == 0)
            //            textBoxCount.Background = new SolidColorBrush(Color.FromRgb(255, 197, 197));
            //        else
            //            textBoxCount.Background = Brushes.White;
            //    }
            //    catch (IndexOutOfRangeException) { }

            //    labelCountDevices.Content = "Количество (доступно " + countDevices.ToString() + " шт.)";
            //}

            //    if ((sender as ComboBox).Name == "comboBoxRecordsOfSalesSale")
            //    {
            //        try
            //        {
            //            int fk = db.GetFkApplication((e.AddedItems[0] as DataRowView).Row["ID_sale"].ToString());

            //            db.FillComboBoxWithCondition(comboBoxRecordsOfSalesRecords, fk.ToString());
            //            db.SelectValueForComboBox(comboBoxRecordsOfSalesRecords, "FK_record_app");
            //        }
            //        catch (IndexOutOfRangeException) { }
            //    }

            //    if ((sender as ComboBox).Name == "comboBoxRecordsOfSalesRecords")
            //    {
            //        try
            //        {
            //            countDevices = db.GetCountDevicesInRecordsOfApplication((e.AddedItems[0] as DataRowView).Row["ID_record"].ToString());
            //        }
            //        catch (IndexOutOfRangeException) { }

            //        labelCountDevicesInRecords.Content = "Количество (доступно " + countDevices.ToString() + " шт.)";
            //    }

            //    Checks.SelectionChanged(sender as ComboBox, e);
        }

        private void textBoxFilter_GotFocus(object sender, RoutedEventArgs e)
            {
                if (textBoxFilter.Text == "Поиск...")
                    textBoxFilter.Text = "";
            }

        private void textBoxFilter_LostFocus(object sender, RoutedEventArgs e)
            {
                if (textBoxFilter.Text == "")
                    textBoxFilter.Text = "Поиск...";
            }

        private void Search(string value)
            {
                DataTable newDT = dt.Clone();
                newDT.Clear();

                foreach (DataRow row in dt.Rows)
                {
                    foreach (DataColumn column in dt.Columns)
                    {
                        try
                        {
                            if (column.DataType == typeof(string) && !column.ColumnName.Contains("-") && !column.ColumnName.Contains("_"))
                            {
                                if (row.Field<string>(column.ColumnName).ToLower().Contains(value.ToLower()))
                                {
                                    newDT.ImportRow(row);
                                    break;
                                }
                            }

                            if (column.DataType == typeof(DateTime) && !column.ColumnName.Contains("-") && !column.ColumnName.Contains("_"))
                            {
                                if (row.Field<DateTime>(column.ColumnName) > DateTime.Parse(value + " 00:00:00") && row.Field<DateTime>(column.ColumnName) < DateTime.Parse(value + " 23:59:59"))
                                {
                                    newDT.ImportRow(row);
                                    break;
                                }
                            }

                            if (column.DataType == typeof(int) && !column.ColumnName.Contains("-") && !column.ColumnName.Contains("_"))
                            {
                                if (value.StartsWith("="))
                                {
                                    if (row.Field<int>(column.ColumnName) == int.Parse(value.Substring(1)))
                                    {
                                        newDT.ImportRow(row);
                                        break;
                                    }
                                }
                                else if (value.StartsWith(">"))
                                {
                                    if (row.Field<int>(column.ColumnName) > int.Parse(value.Substring(1)))
                                    {
                                        newDT.ImportRow(row);
                                        break;
                                    }
                                }
                                else if (value.StartsWith("<"))
                                {
                                    if (row.Field<int>(column.ColumnName) < int.Parse(value.Substring(1)))
                                    {
                                        newDT.ImportRow(row);
                                        break;
                                    }
                                }
                                else
                                {
                                    if (row.Field<int>(column.ColumnName).ToString().Contains(value))
                                    {
                                        newDT.ImportRow(row);
                                        break;
                                    }
                                }
                            }
                        }
                        catch (Exception) { }
                    }
                }

                dataGrid.ItemsSource = newDT.DefaultView;
                dataGrid.SelectedIndex = 0;
        }


        private void buttonReset_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = dt.DefaultView;
            dataGrid.SelectedIndex = 0;

            textBoxFilter.Text = "Поиск...";
        }

        private void textBoxFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                if (textBoxFilter.Text == "" || textBoxFilter.Text == "Поиск..." || (textBoxFilter.Text.StartsWith(">") && textBoxFilter.Text.Length == 1) || (textBoxFilter.Text.StartsWith("<") && textBoxFilter.Text.Length == 1) || (textBoxFilter.Text.StartsWith("=") && textBoxFilter.Text.Length == 1))
                {
                    dataGrid.ItemsSource = dt.DefaultView;
                    dataGrid.SelectedIndex = 0;
                }
                else
                    Search(textBoxFilter.Text);
            }
            catch (Exception) { }
        }

        private void buttonReport_Click(object sender, RoutedEventArgs e)
        {
            if (currGrid.Name == "gridBrands")
            {
                string id_delete_t = ((DataRowView)dataGrid.SelectedItem)[0].ToString();
                string surn_t = ((DataRowView)dataGrid.SelectedItem)[1].ToString();
                string name_t = ((DataRowView)dataGrid.SelectedItem)[2].ToString();
                string patr_t = ((DataRowView)dataGrid.SelectedItem)[3].ToString();
                string pass_t = ((DataRowView)dataGrid.SelectedItem)[4].ToString();
                string phone_t = ((DataRowView)dataGrid.SelectedItem)[5].ToString();
                string dat_t = ((DataRowView)dataGrid.SelectedItem)[6].ToString();
                string date_t = dat_t[6].ToString() + dat_t[7].ToString() + dat_t[8].ToString() + dat_t[9].ToString() + "-" + dat_t[3].ToString() + dat_t[4].ToString() + "-" + dat_t[0].ToString() + dat_t[1].ToString();
                string i_S = Subject_and_id(((DataRowView)dataGrid.SelectedItem)[7].ToString()).ToString();
                //string com = "INSERT INTO android_test.teachers (surname_teacher,name_teacher,patronymic_teacher,passport,phone,birthday,id_subject) VALUES  ( '" + surn_t + "' , '" + name_t + "' , '" + patr_t + "' , " + pass_t + " , '" + phone_t + "' , '" + date_t + "' , " + id_subject + " )";

                string com = "UPDATE android_test.teachers t SET t.surname_teacher = '" + surn_t + "', t.name_teacher = '" + name_t + "', t.patronymic_teacher = '" + patr_t + "', t.passport = '" + pass_t + "', t.phone = '" + phone_t + "', t.birthday = '" + date_t + "' WHERE t.id_teacher = " + id_delete_t;
                //textBoxBrand.Text = com;
                db.AddTeacher(com);

            }

            if (currGrid.Name == "gridTypes")
            {
                string id_delete_t = ((DataRowView)dataGrid.SelectedItem)[0].ToString();
                string surn_t = ((DataRowView)dataGrid.SelectedItem)[1].ToString();
                string name_t = ((DataRowView)dataGrid.SelectedItem)[2].ToString();
                string patr_t = ((DataRowView)dataGrid.SelectedItem)[3].ToString();
                string pass_t = ((DataRowView)dataGrid.SelectedItem)[4].ToString();
                string phone_t = ((DataRowView)dataGrid.SelectedItem)[5].ToString();
                string dat_t = ((DataRowView)dataGrid.SelectedItem)[6].ToString();
                string date_t = dat_t[6].ToString() + dat_t[7].ToString() + dat_t[8].ToString() + dat_t[9].ToString() + "-" + dat_t[3].ToString() + dat_t[4].ToString() + "-" + dat_t[0].ToString() + dat_t[1].ToString();
                //string i_S = Subject_and_id(((DataRowView)dataGrid.SelectedItem)[7].ToString()).ToString();
                //string com = "INSERT INTO android_test.teachers (surname_teacher,name_teacher,patronymic_teacher,passport,phone,birthday,id_subject) VALUES  ( '" + surn_t + "' , '" + name_t + "' , '" + patr_t + "' , " + pass_t + " , '" + phone_t + "' , '" + date_t + "' , " + id_subject + " )";

                string com = "UPDATE android_test.tutors t SET t.surname_tutor = '" + surn_t + "', t.name_tutor = '" + name_t + "', t.patronymic_tutor = '" + patr_t + "', t.passport = '" + pass_t + "', t.phone = '" + phone_t + "', t.birthday = '" + date_t + "' WHERE t.id_tutor = " + id_delete_t;
                //textBoxBrand.Text = com;
                db.AddTeacher(com);

            }

            if (currGrid.Name == "gridModels")
            {
                string id_delete_t = ((DataRowView)dataGrid.SelectedItem)[0].ToString();
                string surn_t = ((DataRowView)dataGrid.SelectedItem)[1].ToString();
                string name_t = ((DataRowView)dataGrid.SelectedItem)[2].ToString();
                string patr_t = ((DataRowView)dataGrid.SelectedItem)[3].ToString();
                string pass_t = ((DataRowView)dataGrid.SelectedItem)[4].ToString();
                string phone_t = ((DataRowView)dataGrid.SelectedItem)[5].ToString();
                string dat_t = ((DataRowView)dataGrid.SelectedItem)[6].ToString();
                string date_t = dat_t[6].ToString() + dat_t[7].ToString() + dat_t[8].ToString() + dat_t[9].ToString() + "-" + dat_t[3].ToString() + dat_t[4].ToString() + "-" + dat_t[0].ToString() + dat_t[1].ToString();
                //string i_S = Subject_and_id(((DataRowView)dataGrid.SelectedItem)[7].ToString()).ToString();
                //string com = "INSERT INTO android_test.teachers (surname_teacher,name_teacher,patronymic_teacher,passport,phone,birthday,id_subject) VALUES  ( '" + surn_t + "' , '" + name_t + "' , '" + patr_t + "' , " + pass_t + " , '" + phone_t + "' , '" + date_t + "' , " + id_subject + " )";

                string com = "UPDATE android_test.babysitters t SET t.surname_babysitter = '" + surn_t + "', t.name_babysitter = '" + name_t + "', t.patronymic_babysitter = '" + patr_t + "', t.passport = '" + pass_t + "', t.phone = '" + phone_t + "', t.birthday = '" + date_t + "' WHERE t.id_babysitter = " + id_delete_t;
                //textBoxBrand.Text = com;
                db.AddTeacher(com);

            }
        }

        private int Subject_and_id(string sub)
        {
            int i_s = 0;
            if (sub == "Арифметика")
                i_s = 1;
            if (sub == "ИЗО")
                i_s = 2;
            if (sub == "Физкультура")
                i_s = 3;
            if (sub == "Музыка")
                i_s = 4;
            if (sub == "Чтение")
                i_s = 5;
            if (sub == "Занятие с логопедом")
                i_s = 6;
            if (sub == "Занятие с психологом")
                i_s = 7;
            return i_s;
               
        }

        private void buttonEditKid_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Search_teachers(object sender, SelectionChangedEventArgs e)
        {
            string subj_id = (comboBoxOrdersStaff_Copy.SelectedIndex + 1).ToString();
            db.SelectTable("teacher_for_subject", subj_id);
            HideButton(0);
            comboBoxOrdersStaff_Copy1.ItemsSource = dt.DefaultView;
            comboBoxOrdersStaff_Copy1.SelectedItem = dt.DefaultView[0];
            comboBoxOrdersStaff_Copy1.DisplayMemberPath = dt.Columns[1].ToString();
            comboBoxOrdersStaff_Copy1.Text = dt.Rows[0][1].ToString();

        }

        private void buttonUpdateDeliverySet_Click(object sender, RoutedEventArgs e)
        {
            string new_cost = textBoxCost.Text;
            string parent_id = ((DataRowView)Parent_pay_table.SelectedItem)[0].ToString();
            //textBoxCost.Text = parent_id;
            string com = "UPDATE android_test.recipe_of_payment t SET t.cost = '" + new_cost + "' WHERE t.parent_id = " + parent_id;
            //textBoxBrand_Copy.Text = dt.Rows[2][0].ToString();
            db.AddTeacher(com);
            string view_type_pay = (comboBoxDeliverySupplier.SelectedIndex).ToString();
            if (view_type_pay == "0")
            {
                db.SelectTable("Pay_list", Log_name, "All");
                HideButton(0);
                Parent_pay_table.ItemsSource = dt.DefaultView;
            }
            else
            {
                db.SelectTable("Pay_list", Log_name, "Debtor");
                HideButton(0);
                Parent_pay_table.ItemsSource = dt.DefaultView;
            }
        }
    }
}