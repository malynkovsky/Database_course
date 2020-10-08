using System;
using System.Data;
using System.Data.SqlClient;
using System.Windows.Controls;
using MySql.Data.MySqlClient;

namespace DB_Store
{
    public class DB
    {
        MySqlConnection connection;
        MySqlCommand cmd;
        MySqlDataAdapter da;
        DataTable dt;
        DataGrid dataGrid;
        int rowsCount;

        public DB(DataGrid _dataGrid)
        {
            dataGrid = _dataGrid;
        }

        public void SetConnection(MySqlConnection _connection)
        {
            connection = _connection;
        }

        public void CloseConnection()
        {
            connection.Close();
        }

        public DataTable FillTable(string table, string name_parent)
        {
            string id_parent = name_parent;
            if (table == "Parents")
            {
                cmd = new MySqlCommand("SELECT * FROM parents WHERE id_parent="+id_parent, connection);
            }
            
            if (table == "Admin_i")
            {//id_par = 
                cmd = new MySqlCommand("SELECT * FROM administration WHERE id_administrator=" + id_parent[id_parent.Length-1].ToString(), connection);
            }

            if (table == "Teachers")
                cmd = new MySqlCommand("select id_teacher,surname_teacher,name_teacher,patronymic_teacher,passport,phone,birthday,name_of_subject from teachers join subjects s on teachers.id_subject = s.id_subject", connection);
            if (table == "Kids")
            {//id_par = 
                cmd = new MySqlCommand("select id_kid,surname,name_kid,patronymic,birthday,name_group from kids join kids_and_parents kap on kids.id_kid = kap.kid_id and kap.parent_id = "+ id_parent+ " join group_defining gd on kids.id_kid = gd.kid_id join groups_ g on gd.group_id = g.id_group", connection);
            }
            if (table == "Kids_n")
            {//id_par = 
                cmd = new MySqlCommand("select name_kid from kids join kids_and_parents kap on kids.id_kid = kap.kid_id and kap.parent_id = " + id_parent + " join group_defining gd on kids.id_kid = gd.kid_id join groups_ g on gd.group_id = g.id_group", connection);
            }
            if (table == "Rub")
            {//id_par = 
                cmd = new MySqlCommand("select cost from recipe_of_payment where parent_id = " + id_parent + " and status_of_pay = false", connection);
            }
            if (table == "Tutors")
                cmd = new MySqlCommand("select id_tutor,surname_tutor,name_tutor,patronymic_tutor,passport,phone,birthday from tutors", connection);
            if (table == "Baby")
                cmd = new MySqlCommand("select id_babysitter,surname_babysitter,name_babysitter,patronymic_babysitter,passport,phone,birthday from babysitters", connection);

            if (table == "teacher_for_subject")
            {
                cmd = new MySqlCommand("select id_teacher,surname_teacher from teachers where id_subject = " + id_parent, connection);
            }
            if (table == "tutors_for_subject")
            {
                cmd = new MySqlCommand("select id_tutor,surname_tutor from tutors ", connection);
            }
            if (table == "baby_for_subject")
            {
                cmd = new MySqlCommand("select id_babysitter,surname_babysitter from babysitters ", connection);
            }

            if (table.Contains("groups_adm"))
            {
                cmd = new MySqlCommand("select * from groups_", connection);

            }
            if (table == "get_room")
            {
                cmd = new MySqlCommand("select room from groups_ where id_group = " + id_parent, connection);

            }

            if (table == "Group_room")
            {
                cmd = new MySqlCommand("select name_group, room from groups_ join group_defining gd on groups_.id_group = gd.group_id and gd.kid_id = " + id_parent, connection);
            }
            da = new MySqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
           
            rowsCount = dt.Rows.Count;
            return dt;
        }

        public DataTable FillTable(string table, string name_parent, string kid)
        {
            string id_parent = name_parent;
            if (table.Contains("Schedule"))
            {
                cmd = new MySqlCommand("select group_id from group_defining join kids_and_parents kap on group_defining.kid_id = kap.kid_id where parent_id = " + id_parent + " and kap.kid_id = " + kid , connection);
                da = new MySqlDataAdapter(cmd);
                dt = new DataTable();
                da.Fill(dt);
                string id_group = dt.Rows[0][0].ToString();
                cmd = new MySqlCommand("select lesson_id,start_time,end_time,name_of_subject,surname_teacher,name_teacher,patronymic_teacher,surname_tutor,name_tutor,patronymic_tutor,surname_babysitter,name_babysitter,patronymic_babysitter from schedule join subjects s on schedule.subject_id = s.id_subject join teachers t on schedule.teacher_id = t.id_teacher join tutors t2 on schedule.tutor_id = t2.id_tutor join babysitters b on schedule.babysitter_id = b.id_babysitter join lesson_period lp on schedule.lesson_id = lp.id_lesson where day_of_week = " + table[table.Length - 1].ToString() + " and group_id = " + id_group, connection);
            }
            if (table == "Select_another")
            {
                cmd = new MySqlCommand("select id_kid from kids join kids_and_parents kap on kids.id_kid = kap.kid_id and kap.parent_id = " + id_parent + " join group_defining gd on kids.id_kid = gd.kid_id join groups_ g on gd.group_id = g.id_group and name_kid = '" + kid+"'", connection);

            }
            if (table.Contains("schedule_adm"))
            {
                string id_group = kid;
                cmd = new MySqlCommand("select id_Schedule, start_time,end_time,name_of_subject,surname_teacher,name_teacher,patronymic_teacher,surname_tutor,name_tutor,patronymic_tutor,surname_babysitter,name_babysitter,patronymic_babysitter from schedule join subjects s on schedule.subject_id = s.id_subject join teachers t on schedule.teacher_id = t.id_teacher join tutors t2 on schedule.tutor_id = t2.id_tutor join babysitters b on schedule.babysitter_id = b.id_babysitter join lesson_period lp on schedule.lesson_id = lp.id_lesson where day_of_week = " + table[table.Length - 1].ToString() + " and group_id = " + id_group, connection);

            }
            if (table.Contains("Pay_list"))
            {
                if (kid == "All")
                {
                    cmd = new MySqlCommand("select id_parent, passport_mother, surname_mother, name_mother, patronymic_mother, phone_mother, passport_father, surname_father, name_father, patronymic_father, phone_father, surname_bookkeeper, name_bookkeeper,patronymic_bookkeeper, cost from parents join recipe_of_payment rop on parents.id_parent = rop.parent_id join bookkeeping b on rop.bookkeeper_id = b.id_bookkeeper", connection);

                }
                else
                {
                    cmd = new MySqlCommand("select id_parent, passport_mother, surname_mother, name_mother, patronymic_mother, phone_mother, passport_father, surname_father, name_father, patronymic_father, phone_father, surname_bookkeeper, name_bookkeeper,patronymic_bookkeeper, cost from parents join recipe_of_payment rop on parents.id_parent = rop.parent_id join bookkeeping b on rop.bookkeeper_id = b.id_bookkeeper where status_of_pay = false", connection);

                }
            }


            da = new MySqlDataAdapter(cmd);
            dt = new DataTable();
            da.Fill(dt);
            if (table.Contains("Schedule"))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string n = dt.Rows[i][4].ToString() + " " + dt.Rows[i][5].ToString()[0].ToString() + ". " + dt.Rows[i][6].ToString()[0].ToString() + ".";
                    dt.Rows[i][4] = n;
                    n = dt.Rows[i][7].ToString() + " " + dt.Rows[i][8].ToString()[0].ToString() + ". " + dt.Rows[i][9].ToString()[0].ToString() + ".";
                    dt.Rows[i][7] = n;
                    n = dt.Rows[i][10].ToString() + " " + dt.Rows[i][11].ToString()[0].ToString() + ". " + dt.Rows[i][12].ToString()[0].ToString() + ".";
                    dt.Rows[i][10] = n;


                }
                dt.Columns.Remove("name_teacher");
                dt.Columns.Remove("patronymic_teacher");
                dt.Columns.Remove("name_tutor");
                dt.Columns.Remove("patronymic_tutor");
                dt.Columns.Remove("name_babysitter");
                dt.Columns.Remove("patronymic_babysitter");
            }
            if (table.Contains("schedule"))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string n = dt.Rows[i][4].ToString() + " " + dt.Rows[i][5].ToString()[0].ToString() + ". " + dt.Rows[i][6].ToString()[0].ToString() + ".";
                    dt.Rows[i][4] = n;
                    n = dt.Rows[i][7].ToString() + " " + dt.Rows[i][8].ToString()[0].ToString() + ". " + dt.Rows[i][9].ToString()[0].ToString() + ".";
                    dt.Rows[i][7] = n;
                    n = dt.Rows[i][10].ToString() + " " + dt.Rows[i][11].ToString()[0].ToString() + ". " + dt.Rows[i][12].ToString()[0].ToString() + ".";
                    dt.Rows[i][10] = n;


                }
                dt.Columns.Remove("name_teacher");
                dt.Columns.Remove("patronymic_teacher");
                dt.Columns.Remove("name_tutor");
                dt.Columns.Remove("patronymic_tutor");
                dt.Columns.Remove("name_babysitter");
                dt.Columns.Remove("patronymic_babysitter");
            }


            if (table.Contains("Pay_list"))
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string n = dt.Rows[i][1].ToString() + " " + dt.Rows[i][2].ToString()[0].ToString() + ". " + dt.Rows[i][3].ToString()[0].ToString() + ".";
                    dt.Rows[i][1] = n;
                    n = dt.Rows[i][6].ToString() + " " + dt.Rows[i][7].ToString()[0].ToString() + ". " + dt.Rows[i][8].ToString()[0].ToString() + ".";
                    dt.Rows[i][6] = n;
                    n = dt.Rows[i][10].ToString() + " " + dt.Rows[i][11].ToString()[0].ToString() + ". " + dt.Rows[i][12].ToString()[0].ToString() + ".";
                    dt.Rows[i][10] = n;


                }
                dt.Columns.Remove("name_mother");
                dt.Columns.Remove("patronymic_mother");
                dt.Columns.Remove("name_father");
                dt.Columns.Remove("patronymic_father");
                dt.Columns.Remove("name_bookkeeper");
                dt.Columns.Remove("patronymic_bookkeeper");
            }
            return dt;
        }

        public void SelectTable(string table, string user_name)
        {
            dt = FillTable(table, user_name);
        }
        public void SelectTable(string table, string user_name, string kid)
        {
            dt = FillTable(table, user_name, kid);
        }

        public void FillComboBox(ComboBox cb, string table, string id, string field)
        {
            if (table == "View_Orders")
                dt = FillTable(table, "View_Orders");
            else if (table == "View_Applications")
                dt = FillTable(table, "View_Applications");
            else
                dt = FillTable(table, null);

            cb.ItemsSource = dt.DefaultView;
            cb.SelectedValuePath = dt.Columns[id].ToString();
            cb.DisplayMemberPath = dt.Columns[field].ToString();
        }

        public void FillComboBoxWithCondition(ComboBox cb, string fk)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM View_RecordsOfApplication WHERE FK_application='" + fk + "' AND Is_deleted='0' AND [-Inactive]='0'", connection);
            MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            
            cb.ItemsSource = dt.DefaultView;
            cb.SelectedValuePath = dt.Columns["ID_record"].ToString();
            cb.DisplayMemberPath = dt.Columns["-Устройство (Количество)"].ToString();
        }

        public void SelectValueForComboBox(ComboBox cb, string columnFK)
        {
            try
            {
                DataRowView row = dataGrid.SelectedItem as DataRowView;
                cb.SelectedValue = row[columnFK].ToString();
            }
            catch (NullReferenceException) { }
        }

        public string GetID(string table, string columnID)
        {
            dt = FillTable(table, null);

            return dt.Rows[rowsCount - 1][columnID].ToString();
        }

        public int GetRowsCount(string table)
        {
            dt = FillTable(table, null);

            return dt.Rows.Count;
        }

        public DataTable GetDataTable(string table)
        {

            //MySqlCommand cmd = new MySqlCommand("SELECT * FROM " + table, connection);
            //MySqlDataAdapter da = new MySqlDataAdapter(cmd);
            //DataTable dt = new DataTable();
            //da.Fill(dt);

            return dt;
        }

        public int CheckNames(string table, string name)
        {
            cmd = connection.CreateCommand();
            cmd.CommandText = "Check_" + table;
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", name);
            var returnParameter = cmd.Parameters.Add("@Code", MySqlDbType.Int64);
            returnParameter.Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();

            return Convert.ToInt32(returnParameter.Value);
        }

        public int CheckModel(string name, string FKbrand, string FKtype)
        {
            cmd = connection.CreateCommand();
            cmd.CommandText = "Check_Model";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@Name", name);
            cmd.Parameters.AddWithValue("@FK_brand", Convert.ToInt32(FKbrand));
            cmd.Parameters.AddWithValue("@FK_type", Convert.ToInt32(FKtype));
            var returnParameter = cmd.Parameters.Add("@Code", MySqlDbType.Int64);////////
            returnParameter.Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();

            return Convert.ToInt32(returnParameter.Value);
        }

        public void UpdateTableBrandsTypes(string table, string textFromTB, string id)
        {
            string columnID;
            if (table == "Brands")
                columnID = "brand";
            else
                columnID = "type";

            cmd = new MySqlCommand("UPDATE " + table + " SET Name='" + textFromTB + "' WHERE ID_" + columnID + "='" + id + "'", connection);
            cmd.ExecuteNonQuery();

           // SelectTable("View_" + table);
        }

        public void AddTeacher(string query_text)
        {
            
            cmd = new MySqlCommand(query_text, connection);
            cmd.ExecuteNonQuery();

            // SelectTable("View_" + table);
        }

        public void UpdateTableModels(string idFromCB1, string idFromCB2, string textFromTB, string id)
        {
            cmd = new MySqlCommand("UPDATE Models SET FK_brand='" + idFromCB1 + "', FK_type='" + idFromCB2 + "', Name='" + textFromTB + "' WHERE ID_model='" + id + "'", connection);
            cmd.ExecuteNonQuery();

           // SelectTable("View_Models");
        }

        public void UpdateTableDevices(string idFromCB, string textFromTB, string id)
        {
            cmd = new MySqlCommand("UPDATE Devices SET FK_model='" + idFromCB + "', Price='" + textFromTB + "' WHERE ID_device='" + id + "'", connection);
            cmd.ExecuteNonQuery();

            //SelectTable("View_Devices");
        }

        public void UpdateTableClientsStaff(string table, string textFromTB1, string textFromTB2, string textFromTB3, string textFromTB4, string textFromTB5, string id)
        {
            string columnID;
            if (table == "Clients")
                columnID = "client";
            else
                columnID = "staff";

            cmd = new MySqlCommand("UPDATE " + table + " SET Surname='" + textFromTB1 + "', Name='" + textFromTB2 + "', Patronymic='" + textFromTB3 + "', Phone='" + textFromTB4 + "', Addres='" + textFromTB5 + "' WHERE ID_" + columnID + "='" + id + "'", connection);
            cmd.ExecuteNonQuery();

            //SelectTable("View_" + table);
        }

        public void UpdateTableSuppliers(string textFromTB1, string textFromTB2, string textFromTB3, string id)
        {
            cmd = new MySqlCommand("UPDATE Suppliers SET Name='" + textFromTB1 + "', Phone='" + textFromTB2 + "', Addres='" + textFromTB3 + "' WHERE ID_supplier='" + id + "'", connection);
            cmd.ExecuteNonQuery();

           // SelectTable("View_Suppliers");
        }

        public void UpdateTableApplications(string idFromCB1, string idFromCB2, string d, string id)
        {
            cmd = new MySqlCommand("UPDATE Applications SET FK_client='" + idFromCB1 + "', FK_staff='" + idFromCB2 + "', Date_application='" + d + "' WHERE ID_application='" + id + "'", connection);
            cmd.ExecuteNonQuery();

           // SelectTable("View_Applications");
        }

        public void UpdateRecordsOfApplication(string idFromCB1, string idFromCB2, string textFromTB, string id)
        {
            cmd = connection.CreateCommand();
            cmd.CommandText = "Update_RecordOfApplication";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FK_application", Convert.ToInt32(idFromCB1));
            cmd.Parameters.AddWithValue("@FK_device", Convert.ToInt32(idFromCB2));
            cmd.Parameters.AddWithValue("@Count_devices", Convert.ToInt32(textFromTB));
            cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(id));
            cmd.ExecuteNonQuery();

            //SelectTable("View_RecordsOfApplication");
        }

        public int GetCountDevicesInStorage(string FK_device)
        {
            cmd = connection.CreateCommand();
            cmd.CommandText = "Get_CountDevicesInStorage";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FK_device", Convert.ToInt32(FK_device));
            var returnParameter = cmd.Parameters.Add("@Count", MySqlDbType.Int64);
            returnParameter.Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();

            return Convert.ToInt32(returnParameter.Value);
        }

        public void UpdateOrders(string idFromCB1, string idFromCB2, string textFromTB1, string textFromTB2, string id)
        {
            cmd = new MySqlCommand("UPDATE Orders SET FK_staff='" + idFromCB1 + "', FK_device='" + idFromCB2 + "', Count_devices='" + textFromTB1 + "', Date_order='" + textFromTB2 + "' WHERE ID_order='" + id + "'", connection);
            cmd.ExecuteNonQuery();

           // SelectTable("View_Orders");
        }

        public void UpdateSales(string idFromCB, string textFromTB, string id)
        {
            cmd = new MySqlCommand("UPDATE Sales SET FK_application='" + idFromCB + "', Date_sale='" + textFromTB + "' WHERE ID_sale='" + id + "'", connection);
            cmd.ExecuteNonQuery();

           // SelectTable("View_Sales");
        }

        public void UpdateRecordsOfSale(string idFromCB1, string idFromCB2, string textFromTB, string id)
        {
            cmd = connection.CreateCommand();
            cmd.CommandText = "Update_RecordOfSale";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FK_sale", Convert.ToInt32(idFromCB1));
            cmd.Parameters.AddWithValue("@FK_record_app", Convert.ToInt32(idFromCB2));
            cmd.Parameters.AddWithValue("@Count_devices", Convert.ToInt32(textFromTB));
            cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(id));
            cmd.ExecuteNonQuery();

           //SelectTable("View_RecordsOfSale");
        }

        public int GetFkApplication(string FK_sale)
        {
            cmd = connection.CreateCommand();
            cmd.CommandText = "Get_FKapplication";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FK_sale", Convert.ToInt32(FK_sale));
            var returnParameter = cmd.Parameters.Add("@FK_application", MySqlDbType.Int64);
            returnParameter.Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();

            return Convert.ToInt32(returnParameter.Value);
        }

        public int GetCountDevicesInRecordsOfApplication(string FK_record)
        {
            cmd = connection.CreateCommand();
            cmd.CommandText = "Get_CountDevicesInRecordsOfApplication";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FK_record", Convert.ToInt32(FK_record));
            var returnParameter = cmd.Parameters.Add("@Count", MySqlDbType.Int64);
            returnParameter.Direction = ParameterDirection.ReturnValue;
            cmd.ExecuteNonQuery();

            return Convert.ToInt32(returnParameter.Value);
        }

        public void InsertTableBrandsTypes(string table, string textFromTB)
        {
            cmd = new MySqlCommand("INSERT INTO " + table + " (Name) VALUES ('" + textFromTB + "')", connection);
            cmd.ExecuteNonQuery();

           // SelectTable("View_" + table);
        }

        public void InsertTableModels(string idFromCB1, string idFromCB2, string textFromTB)
        {
            cmd = new MySqlCommand("INSERT INTO Models (FK_brand, FK_type, Name) VALUES ('" + idFromCB1 + "', '" + idFromCB2 + "', '" + textFromTB + "')", connection);
            cmd.ExecuteNonQuery();

            //SelectTable("View_Models");
        }

        public void InsertTableDevices(string idFromCB, string textFromTB)
        {
            cmd = new MySqlCommand("INSERT INTO Devices (FK_model, Price) VALUES ('" + idFromCB + "', '" + textFromTB + "')", connection);
            cmd.ExecuteNonQuery();

            //SelectTable("View_Devices");
        }

        public void InsertTableClientsStaff(string table, string textFromTB1, string textFromTB2, string textFromTB3, string textFromTB4, string textFromTB5, bool select)
        {
            cmd = new MySqlCommand("INSERT INTO " + table + " (Surname, Name, Patronymic, Phone, Addres) VALUES ('" + textFromTB1 + "', '" + textFromTB2 + "', '" + textFromTB3 + "', '" + textFromTB4 + "', '" + textFromTB5 + "')", connection);
            cmd.ExecuteNonQuery();

            //if (select)
            //    SelectTable("View_" + table);

            
        }

        public void InsertTableSuppliers(string textFromTB1, string textFromTB2, string textFromTB3)
        {
            cmd = new MySqlCommand("INSERT INTO Suppliers (Name, Phone, Addres) VALUES ('" + textFromTB1 + "', '" + textFromTB2 + "', '" + textFromTB3 + "')", connection);
            cmd.ExecuteNonQuery();

           //SelectTable("View_Suppliers");
        }

        public void InsertTableApplications(string idFromCB1, string idFromCB2, string textFromTB)
        {
            cmd = new MySqlCommand("INSERT INTO Applications (FK_client, FK_staff, Date_application, Status_application) VALUES ('" + idFromCB1 + "', '" + idFromCB2 + "', '" + textFromTB + "', 'Оформляется')", connection);
            cmd.ExecuteNonQuery();

           // SelectTable("View_Applications");
        }

        public void InsertTableRecordsOfApplication(string idFromCB1, string idFromCB2, string textFromTB)
        {
            cmd = connection.CreateCommand();
            cmd.CommandText = "Add_RecordOfApplication";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FK_application", Convert.ToInt32(idFromCB1));
            cmd.Parameters.AddWithValue("@FK_device", Convert.ToInt32(idFromCB2));
            cmd.Parameters.AddWithValue("@Count_devices", Convert.ToInt32(textFromTB));
            cmd.ExecuteNonQuery();

            //SelectTable("View_RecordsOfApplication");
        }

        public void InsertTableOrders(string idFromCB1, string idFromCB2, string textFromTB1, string textFromTB2)
        {
            cmd = new MySqlCommand("INSERT INTO Orders (FK_staff, FK_device, Count_devices, Date_order, Status_order) VALUES ('" + idFromCB1 + "', '" + idFromCB2 + "', '" + textFromTB1 + "', '" + textFromTB2 + "', 'Открыт')", connection);
            cmd.ExecuteNonQuery();

            //SelectTable("View_Orders");
        }

        public void InsertTableDelivery(string idFromCB1, string idFromCB2)
        {
            cmd = connection.CreateCommand();
            cmd.CommandText = "Add_Delivery";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FK_supplier", Convert.ToInt32(idFromCB1));
            cmd.Parameters.AddWithValue("@FK_order", Convert.ToInt32(idFromCB2));
            cmd.ExecuteNonQuery();

           // SelectTable("View_Delivery");
        }

        public void InsertTableSales(string idFromCB, string textFromTB)
        {
            cmd = new MySqlCommand("INSERT INTO Sales (FK_application, Date_sale) VALUES ('" + idFromCB + "', '" + textFromTB + "')", connection);
            cmd.ExecuteNonQuery();

            cmd = new MySqlCommand("UPDATE Applications SET Status_application='Утверждён' WHERE ID_application='" + idFromCB + "'", connection);
            cmd.ExecuteNonQuery();

            //SelectTable("View_Sales");
        }

        public void InsertTableRecordsOfSale(string idFromCB1, string idFromCB2, string textFromTB)
        {
            cmd = connection.CreateCommand();
            cmd.CommandText = "Add_RecordOfSale";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FK_sale", Convert.ToInt32(idFromCB1));
            cmd.Parameters.AddWithValue("@FK_record_app", Convert.ToInt32(idFromCB2));
            cmd.Parameters.AddWithValue("@Count_devices", Convert.ToInt32(textFromTB));
            cmd.ExecuteNonQuery();

            //SelectTable("View_RecordsOfSale");
        }

        public void DeleteFromTable(string table, string idColumn, string id)
        {
            cmd = new MySqlCommand("UPDATE " + table + " SET Is_deleted='1' WHERE " + idColumn + "='" + id + "'", connection);
            cmd.ExecuteNonQuery();

            //SelectTable("View_" + table);
        }

        public void DeleteRecordOfApplication(string id)
        {
            cmd = connection.CreateCommand();
            cmd.CommandText = "Delete_RecordOfApplication";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(id));
            cmd.ExecuteNonQuery();

            //SelectTable("View_RecordsOfApplication");
        }

        public void DeleteRecordOfSale(string id)
        {
            cmd = connection.CreateCommand();
            cmd.CommandText = "Delete_RecordOfSale";
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@ID", Convert.ToInt32(id));
            cmd.ExecuteNonQuery();

           // SelectTable("View_RecordsOfSale");
        }
    }
}