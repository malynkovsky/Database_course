using System;
using System.Data;
using System.Text.RegularExpressions;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace DB_Store
{
    class Checks
    {
        static SolidColorBrush colorError = new SolidColorBrush(Color.FromRgb(255, 197, 197));

        public static bool TextInput(TextBox tb, TextCompositionEventArgs e)
        {
            bool result = false;
            int code = Convert.ToChar(e.Text);

            if (tb.Name == "textBoxBrand")
            {
                if ((code >= 65 && code <= 90) || (code >= 97 && code <= 122))
                    result = true;
                else
                    result = false;
            }
            else if (tb.Name == "textBoxType" || tb.Name == "textBoxSurnameClients" || tb.Name == "textBoxNameClients" || tb.Name == "textBoxPatronymicClients" || tb.Name == "textBoxSurnameStaff" || tb.Name == "textBoxNameStaff" || tb.Name == "textBoxPatronymicStaff")
            {
                if (code >= 1040 && code <= 1103)
                    result = true;
                else
                    result = false;
            }
            else if (tb.Name == "textBoxModel")
            {
                if ((code >= 65 && code <= 90) || (code >= 97 && code <= 122) || (code >= 48 && code <= 57))
                    result = true;
                else
                    result = false;
            }
            else if (tb.Name == "textBoxPrice" || tb.Name == "textBoxCount" || tb.Name == "textBoxCountOrder" || tb.Name == "textBoxCountRecords")
            {
                if (code >= 48 && code <= 57)
                    result = true;
                else
                    result = false;
            }
            else if (tb.Name == "textBoxPhoneClients" || tb.Name == "textBoxPhoneStaff" || tb.Name == "textBoxPhoneSupplier")
            {
                if ((code >= 48 && code <= 57) || code == 45 || code == 40 || code == 41)
                    result = true;
                else
                    result = false;
            }
            else if (tb.Name == "textBoxAddresClients" || tb.Name == "textBoxAddresStaff" || tb.Name == "textBoxAddresSupplier")
            {
                if ((code >= 1040 && code <= 1103) || (code >= 48 && code <= 57) || code == 45 || code == 44)
                    result = true;
                else
                    result = false;
            }
            else if (tb.Name == "textBoxSupplier")
            {
                if ((code >= 65 && code <= 90) || (code >= 97 && code <= 122) || (code >= 1040 && code <= 1103) || code == 34)
                    result = true;
                else
                    result = false;
            }
            else if (tb.Name == "textBoxDate" || tb.Name == "textBoxDateOrder" || tb.Name == "textBoxDateSale")
            {
                if ((code >= 48 && code <= 57) || code == 46 || code == 58)
                    result = true;
                else
                    result = false;
            }

            return result;
        }

        public static void TextChanged(TextBox tb, int countDevices)
        {
            if (tb.Name == "textBoxBrand")
            {
                if (!Regex.IsMatch(tb.Text, @"^([A-Za-z][\s]?)+[A-Za-z]$"))
                    tb.Background = colorError;
                else
                    tb.Background = Brushes.White;
            }
            else if (tb.Name == "textBoxType")
            {
                if (!Regex.IsMatch(tb.Text, @"^[А-Я]([а-я][\s]?)+[а-я]$"))
                    tb.Background = colorError;
                else
                    tb.Background = Brushes.White;
            }
            else if (tb.Name == "textBoxModel")
            {
                if (!Regex.IsMatch(tb.Text, @"^([A-Za-z\d][\s]?)+[A-Za-z\d]$"))
                    tb.Background = colorError;
                else
                    tb.Background = Brushes.White;
            }
            else if (tb.Name == "textBoxPrice")
            {
                if (!Regex.IsMatch(tb.Text, @"^[\d]+$"))
                    tb.Background = colorError;
                else
                    tb.Background = Brushes.White;
            }
            else if (tb.Name == "textBoxSurnameClients" || tb.Name == "textBoxNameClients" || tb.Name == "textBoxPatronymicClients" || tb.Name == "textBoxSurnameStaff" || tb.Name == "textBoxNameStaff" || tb.Name == "textBoxPatronymicStaff")
            {
                if (!Regex.IsMatch(tb.Text, @"^[А-Я]{1}[а-я]+$"))
                    tb.Background = colorError;
                else
                    tb.Background = Brushes.White;
            }
            else if (tb.Name == "textBoxPhoneClients" || tb.Name == "textBoxPhoneStaff" || tb.Name == "textBoxPhoneSupplier")
            {
                if (!Regex.IsMatch(tb.Text, @"^[(]{1}[\d]{2}[)]{1}[\s]{1}[\d]{3}[-]{1}[\d]{2}[-]{1}[\d]{2}$"))
                    tb.Background = colorError;
                else
                    tb.Background = Brushes.White;
            }
            else if (tb.Name == "textBoxAddresClients" || tb.Name == "textBoxAddresStaff" || tb.Name == "textBoxAddresSupplier")
            {
                if (!Regex.IsMatch(tb.Text, @"^([А-Я]{1}[а-я]+[,]{1}[\s]{1}){2}[\d]{1,2}([-]{1}[\d]{1,3})?$"))
                    tb.Background = colorError;
                else
                    tb.Background = Brushes.White;
            }
            else if (tb.Name == "textBoxSupplier")
            {
                if (!Regex.IsMatch(tb.Text, @"^[А-ЯA-Za-z]{1}[а-яa-z]+$"))
                    tb.Background = colorError;
                else
                    tb.Background = Brushes.White;
            }
            else if (tb.Name == "textBoxDate" || tb.Name == "textBoxDateSale")
            {
                if (!Regex.IsMatch(tb.Text, @"^([\d]{2}[.]{1}){2}[\d]{4}[\s]{1}[\d]{1,2}[:]{1}[\d]{2}[:]{1}[\d]{2}$"))
                    tb.Background = colorError;
                else
                    tb.Background = Brushes.White;
            }
            else if (tb.Name == "textBoxCount" || tb.Name == "textBoxCountRecords")
            {
                int count;
                if (tb.Text == "")
                    count = 0;
                else count = Convert.ToInt32(tb.Text);

                if (count == 0 || count > countDevices)
                    tb.Background = colorError;
                else
                    tb.Background = Brushes.White;
            }
            //else if (tb.Name == "textBoxCountOrder")
            //{
            //    int count;
            //    if (tb.Text == "")
            //        count = 0;
            //    else count = Convert.ToInt32(tb.Text);

            //    if (count == 0)
            //        tb.Background = colorError;
            //    else
            //        tb.Background = Brushes.White;
            //}
        }

        public static void SelectionChanged(ComboBox cb, SelectionChangedEventArgs e)
        {
            string id = "";

            if (cb.Name == "comboBoxModelsTypes")
                id = "ID_type";
            else if (cb.Name == "comboBoxModelsBrands")
                id = "ID_brand";
            else if (cb.Name == "comboBoxDevicesModels")
                id = "ID_model";
            else if (cb.Name == "comboBoxAppClients")
                id = "ID_client";
            else if (cb.Name == "comboBoxAppStaff")
                id = "ID_Staff";
            else if (cb.Name == "comboBoxOrdersStaff")
                id = "ID_Staff";
            else if (cb.Name == "comboBoxOrdersDevices")
                id = "ID_device";
            else if (cb.Name == "comboBoxDeliverySupplier")
                id = "ID_supplier";
            else if (cb.Name == "comboBoxDeliveryOrder")
                id = "ID_order";
            else if (cb.Name == "comboBoxRecordsApplications")
                id = "ID_application";
            else if (cb.Name == "comboBoxRecordsDevices")
                id = "ID_device";
            else if (cb.Name == "comboBoxSaleApp")
                id = "ID_application";
            else if (cb.Name == "comboBoxRecordsOfSalesSale")
                id = "ID_sale";
            else if (cb.Name == "comboBoxRecordsOfSalesRecords")
                id = "ID_record";

            try
            {
                (e.AddedItems[0] as DataRowView).Row[id].ToString();
                cb.Background = Brushes.White;
            }
            catch (Exception)
            {
                cb.Background = colorError;
            }
        }
    }
}