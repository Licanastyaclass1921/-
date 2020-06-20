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
using System.Data.SqlClient;
using System.Data;
using System.Configuration;

namespace Atelie
{
    /// <summary>
    /// Логика взаимодействия для Ателье.xaml
    /// </summary>
    public partial class Ателье : Window
    {

        bool redact = false;
        public Ателье()
        {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) //действие по загрузке формы
        {
            DB db = new DB();
            DB db2 = new DB();
            DB db3 = new DB();
            try
            {

                using (db.GetConnect())// создание подключения
                {
                    db.ConnectOpen();//открытие подключения
                    DataTable data = new DataTable();//создание таблицы данных, в которую заполним информацию об услугах
                    SqlDataAdapter adapter = new SqlDataAdapter("Select * from uslugi where status = 0", db.GetConnect()); //запрос на услуги для таблицы услуг
                    adapter.Fill(data);//заполнение таблицы данных информацией об услугах
                    uslygi.ItemsSource = data.DefaultView;//заполнение таблицы информацией
                                                          //далее анологично
                }
                using (db2.GetConnect())
                {
                    db2.ConnectOpen();
                    DataTable data = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter("select * from klient where status = 0", db2.GetConnect());
                    adapter.Fill(data);
                    klient.ItemsSource = data.DefaultView;
                }
                using (db3.GetConnect())
                {
                    db3.ConnectOpen();
                    DataTable data = new DataTable();
                    SqlDataAdapter adapter = new SqlDataAdapter("select * from View_1 where status = 0", db3.GetConnect()); // здесь загрузка из View (представления)
                    adapter.Fill(data);
                    zakaza.ItemsSource = data.DefaultView;
                }
            }
            catch
            {
                MessageBox.Show("Введите корректные данные");
                throw;
            }
        }

        private void Ok_Click_2(object sender, RoutedEventArgs e)
        {
            DB db = new DB();
            DB db2 = new DB();
            if (redact) //редактирование услуг
            {
                using (db.GetConnect())
                {
                    db.ConnectOpen();
                    var command = new SqlCommand("Update uslugi set Nazvanie='" + nazvUsl.Text + "',Hena ='" + hena.Text + "' where kod= " + ((DataRowView)uslygi.SelectedItem)[0], db.GetConnect());// запрос на обновление имени и стоимости услуги где ид равен первому столбцу выделенной строки
                    command.ExecuteNonQuery();//выполнение запроса
                }

                using (db2.GetConnect())
                {
                    db2.ConnectOpen();
                    var dataAdapter = new SqlDataAdapter("Select * from uslugi where status = 0", db2.GetConnect());//обновление таблицы на форме
                    var data = new DataTable();
                    dataAdapter.Fill(data);
                    uslygi.ItemsSource = data.DefaultView;
                }
            }

            else
            { //Новая услуга
                using (db.GetConnect())
                {
                    db.ConnectOpen();
                    var command = new SqlCommand("insert into uslugi(Nazvanie,Hena,status)values('" + nazvUsl.Text + "','" + hena.Text + "',0)", db.GetConnect());// запрос на добавление услуги с указанием названия и стоимости
                    command.ExecuteNonQuery();
                }

                using (db2.GetConnect())
                {
                    db2.ConnectOpen();
                    var dataAdapter = new SqlDataAdapter("Select * from uslugi where status = 0", db2.GetConnect());//обновление таблицы на форме
                    var data = new DataTable();
                    dataAdapter.Fill(data);
                    uslygi.ItemsSource = data.DefaultView;
                }
            }


            nazvUsl.IsEnabled = false; //отчистка и выключение текстбоксов и кнопок услуг
            nazvUsl.Text = "";
            hena.IsEnabled = false;
            hena.Text = "";
            Ok.IsEnabled = false;
            Otmena.IsEnabled = false;
            redact = false;
        }

        private void Novusl_Click(object sender, RoutedEventArgs e) //включение текстбоксов названия и цены услуги, включение режима для добавления (isEdit = false)
        {
            nazvUsl.IsEnabled = true;
            hena.IsEnabled = true;
            Ok.IsEnabled = true;
            Otmena.IsEnabled = true;
            redact = false;
        }

        private void Otmena_Click(object sender, RoutedEventArgs e) //отчистка и выключение текстбоксов и кнопок услуг
        {
            nazvUsl.IsEnabled = false;
            nazvUsl.Text = "";
            hena.IsEnabled = false;
            hena.Text = "";
            Ok.IsEnabled = false;
            Otmena.IsEnabled = false;
            redact = false;
        }

        private void red_Click(object sender, RoutedEventArgs e) // по нажатию редактировать включаются кнопки и в текстбоксы записывается 
        {
            nazvUsl.IsEnabled = true;
            hena.IsEnabled = true;
            Ok.IsEnabled = true;
            Otmena.IsEnabled = true;
            nazvUsl.Text = ((DataRowView)uslygi.SelectedItem)[1].ToString();
            hena.Text = ((DataRowView)uslygi.SelectedItem)[2].ToString();
            redact = true;
        }

        private void udal_Click_1(object sender, RoutedEventArgs e) // кнопка удалить
        {
            DB db = new DB();
            DB db2 = new DB();
            try
            {
                using (db.GetConnect())
                {
                    db.ConnectOpen();
                    var command = new SqlCommand("Update uslugi set Status = 1 where kod= " + ((DataRowView)uslygi.SelectedItem)[0].ToString(), db.GetConnect());// запрос на удаление услуги, выделенной в таблице
                    command.ExecuteNonQuery();
                }
                using (db2.GetConnect())
                {
                    db2.ConnectOpen();
                    var dataAdapter = new SqlDataAdapter("Select * from uslugi where status = 0", db2.GetConnect());//обновление таблицы на форме
                    var data = new DataTable();
                    dataAdapter.Fill(data);
                    uslygi.ItemsSource = data.DefaultView;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Возникла ошибка"); //При ошибке выскакивает сообщение об ошибке 
                throw;
            }
        }

        bool redact2 = false;
        private void klien_Click(object sender, RoutedEventArgs e) //включение кнопок и текстбоксов клиента для добавления
        {
            FIO.IsEnabled = true;
            Adres.IsEnabled = true;
            Nomer.IsEnabled = true;
            OK.IsEnabled = true;
            Otm.IsEnabled = true;
            redact2 = false;
        }

        private void redak_Click(object sender, RoutedEventArgs e) //включение кнопок и текстбоксов клиента для редактирования
        {
            FIO.IsEnabled = true;
            FIO.Text = ((DataRowView)klient.SelectedItem)[1].ToString();
            Adres.IsEnabled = true;
            Adres.Text = ((DataRowView)klient.SelectedItem)[2].ToString();
            Nomer.IsEnabled = true;
            Nomer.Text = ((DataRowView)klient.SelectedItem)[3].ToString();
            OK.IsEnabled = true;
            Otm.IsEnabled = true;
            redact2 = true;
        }

        private void Otm_Click(object sender, RoutedEventArgs e) //отчистка и выключение кнопок и текстбоксов
        {
            FIO.IsEnabled = false;
            FIO.Text = "";
            Adres.IsEnabled = false;
            Adres.Text = "";
            Nomer.IsEnabled = false;
            Nomer.Text = "";
            OK.IsEnabled = false;
            Otm.IsEnabled = false;
            redact2 = false;
        }

        private void OK_Click(object sender, RoutedEventArgs e) //Нажатоие на ОК
        {
            DB db = new DB();
            DB db2 = new DB();
            if (redact2) //определение режима редактировать/добавить
            {
                using (db.GetConnect())
                {
                    db.ConnectOpen();
                    var command = new SqlCommand("Update klient set FIO='" + FIO.Text + "',Adres ='" + Adres.Text + "',TF = '" + Nomer.Text + "' where ID= " + ((DataRowView)klient.SelectedItem)[0].ToString(), db.GetConnect()); //запрос на редактирование ФИО, адреса и телефона по ИД указанному в таблице в 1 столбике выбранной строки таблицы
                    command.ExecuteNonQuery();
                }
                using (db2.GetConnect())
                {
                    db2.ConnectOpen();
                    var dataAdapter = new SqlDataAdapter("select * from klient where status = 0", db2.GetConnect());//обновление таблицы на форме
                    var data = new DataTable();
                    dataAdapter.Fill(data);
                    klient.ItemsSource = data.DefaultView;
                }
            }
            else
            {
                using (db.GetConnect())
                {
                    db.ConnectOpen();
                    var command = new SqlCommand("insert into klient(FIO,Adres,TF,Status)values('" + FIO.Text + "','" + Adres.Text + "','" + Nomer.Text + "',0)", db.GetConnect());
                    command.ExecuteNonQuery(); //запрос на добавление Клииента
                }
                using (db2.GetConnect())
                {
                    db2.ConnectOpen();
                    var dataAdapter = new SqlDataAdapter("select * from klient where status = 0", db2.GetConnect());//обновление таблицы на форме
                    var data = new DataTable();
                    dataAdapter.Fill(data);
                    klient.ItemsSource = data.DefaultView;
                }
            }

            FIO.IsEnabled = false;
            FIO.Text = "";
            Adres.IsEnabled = false;
            Adres.Text = "";
            Nomer.IsEnabled = false;
            Nomer.Text = "";
            OK.IsEnabled = false;
            Otm.IsEnabled = false;
            redact2 = false;
        }

        private void delit_Click(object sender, RoutedEventArgs e) //запрос на удаление клиента
        {
            DB db = new DB();
            DB db2 = new DB();
            try
            {
                using (db.GetConnect())
                {
                    db.ConnectOpen();
                    var command = new SqlCommand("Update klient set status = 1 where ID= " + ((DataRowView)klient.SelectedItem)[0].ToString(), db.GetConnect());
                    command.ExecuteNonQuery();
                }
                using (db2.GetConnect())
                {
                    db2.ConnectOpen();
                    var dataAdapter = new SqlDataAdapter("select * from klient where status = 0", db2.GetConnect());//обновление таблицы на форме
                    var data = new DataTable();
                    dataAdapter.Fill(data);
                    klient.ItemsSource = data.DefaultView;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Возникла ошибка"); //При ошибке выскакивает сообщение об ошибке
            }

        }

        private void Novzak_Click(object sender, RoutedEventArgs e)//заполнение комбобоксов сущетвующими клиентами и услугами
        {
            DB db = new DB();
            DB db2 = new DB();
            try
            {
                using (db.GetConnect())
                {
                    db.ConnectOpen();
                    var dataAdapter = new SqlDataAdapter("Select Nazvanie from uslugi where status = 0", db.GetConnect());//получение списка услуг
                    var data = new DataTable();
                    dataAdapter.Fill(data);
                    nazvanieuslugi.Items.Clear();//отчистка комбобокса чтобы не повторялись услуги
                    for (int i = 0; i < data.Rows.Count; i++) //Цикл проходит столько раз, сколько услуг есть в таблице
                    {
                        nazvanieuslugi.Items.Add(data.Rows[i][0]);//Добавление услуг в комбобокс
                    }
                }

                using (db2.GetConnect())
                {
                    db2.ConnectOpen();
                    var dataAdapter = new SqlDataAdapter("Select FIO from klient where status = 0", db2.GetConnect());//получение списка клиентов
                    var data = new DataTable();
                    dataAdapter.Fill(data);
                    fioklienta.Items.Clear();//отчистка комбобокса чтобы не повторялись клиенты
                    for (int i = 0; i < data.Rows.Count; i++) //Цикл проходит столько раз, сколько клиентов есть в таблице
                    {
                        fioklienta.Items.Add(data.Rows[i][0]);//цикл для заполнения списка клиентов
                    }
                }
                nazvanieuslugi.IsEnabled = true;
                fioklienta.IsEnabled = true;
                datePicker1.IsEnabled = true;
                ok.IsEnabled = true;
                Otmen.IsEnabled = true;
            }
            catch (Exception)
            {
                MessageBox.Show("Возникла ошибка"); //При ошибке выскакивает сообщение об ошибке
            }
        }
        private void ok_Click_1(object sender, RoutedEventArgs e)//Добавление услуги
        {
            DB db = new DB();
            DB db2 = new DB();
            DB db3 = new DB();
            DB db4 = new DB();
            string kod, ID;

            using (db.GetConnect())
            {
                db.ConnectOpen();
                var data = new DataTable();
                var dataAdapter = new SqlDataAdapter("Select kod from uslugi where Nazvanie ='" + nazvanieuslugi.Text + "'", db.GetConnect()); //поиск ид услуги по названию, выбранному в комбобоксе
                dataAdapter.Fill(data);
                kod = data.Rows[0][0].ToString();//сохранение кода услуги
            }

            using (db2.GetConnect())
            {
                db2.ConnectOpen();
                var data2 = new DataTable();
                var dataAdapter2 = new SqlDataAdapter("Select ID from klient where FIO = '" + fioklienta.Text + "'", db2.GetConnect());//поиск ид клиента по ФИО, выбранному в комбобоксе
                dataAdapter2.Fill(data2);
                ID = data2.Rows[0][0].ToString();// сохранение id клиента
            }

            using (db3.GetConnect())
            {
                db3.ConnectOpen();
                var command = new SqlCommand("insert into zakazy(idklient,koduslugi,Data,OkazYSL,Status)values(" + ID + ",'" + kod + "','" + datePicker1.SelectedDate.Value.ToString("dd'.'MM'.'yyyy") + "',0,0)", db3.GetConnect());
                command.ExecuteNonQuery();
            }

            using (db4.GetConnect())
            {
                db4.ConnectOpen();
                var dataAdapter3 = new SqlDataAdapter("select * from View_1 where status = 0", db4.GetConnect());//обновление таблицы (представления) на форме
                var data3 = new DataTable();
                dataAdapter3.Fill(data3);
                zakaza.ItemsSource = data3.DefaultView;
            }

            nazvanieuslugi.Text = "";
            nazvanieuslugi.IsEnabled = false;
            fioklienta.Text = "";
            fioklienta.IsEnabled = false;
            datePicker1.SelectedDate = DateTime.Now;
            datePicker1.IsEnabled = false;
            ok.IsEnabled = false;
            Otmen.IsEnabled = false;
        }

        private void Otmen_Click(object sender, RoutedEventArgs e) //выключение и отчистка кнопок и комбобоксов
        {
            nazvanieuslugi.Text = "";
            nazvanieuslugi.IsEnabled = false;
            fioklienta.Text = "";
            fioklienta.IsEnabled = false;
            datePicker1.SelectedDate = DateTime.Now;
            datePicker1.IsEnabled = false;
            ok.IsEnabled = false;
            Otmen.IsEnabled = false;
        }

        private void vip_Click(object sender, RoutedEventArgs e)//отметка о выполнении заказа
        {
            DB db = new DB();
            DB db2 = new DB();
            using (db.GetConnect())
            {
                db.ConnectOpen();
                new SqlCommand("update zakazy set OkazYSL = 1 where id=" + ((DataRowView)zakaza.SelectedItem)[0].ToString(), db.GetConnect()).ExecuteNonQuery(); //запрос на изменения статуса услуги на 1 где ид услуги равен выбранному в таблице заказцу
            }

            using (db2.GetConnect())
            {
                db2.ConnectOpen();
                var dataAdapter3 = new SqlDataAdapter("select * from View_1 where status = 0", db2.GetConnect());//обновление таблицы на форме
                var data3 = new DataTable();
                dataAdapter3.Fill(data3);
                zakaza.ItemsSource = data3.DefaultView;
            }
        }

        private void zakaza_SelectionChanged(object sender, SelectionChangedEventArgs e) //поиск клиента по нажатию на заказ
        {
            DB db = new DB();
            if (zakaza.SelectedItem != null)//если в таблице чтото выбрано то
            {
                using (db.GetConnect())
                {
                    db.ConnectOpen();
                    var dataAdapter3 = new SqlDataAdapter("select * from klient where status = 0 and id=" + ((DataRowView)zakaza.SelectedItem)[1].ToString(), db.GetConnect());//ищем клиента по иду, указанному в заказе
                    var data3 = new DataTable();
                    dataAdapter3.Fill(data3);
                    inf.ItemsSource = data3.DefaultView; // и показыввем его в таблице
                }
            }
        }

        private void oki_Click(object sender, RoutedEventArgs e) //поиск заказов по дате

        {
            DB db = new DB();
            using (db.GetConnect())
            {
                db.ConnectOpen();
                var dataAdapter3 = new SqlDataAdapter("select * from View_1 where status = 0 and Data='" + datePicker2.Text + "'", db.GetConnect());//выбираем всё из представления где дата заказа равна выбранной в датапикере
                var data3 = new DataTable();
                dataAdapter3.Fill(data3);
                zapros.ItemsSource = data3.DefaultView;
            }
        }

        private void Zapr_MouseDown(object sender, MouseButtonEventArgs e)
        {

        }

        private void okk_Click(object sender, RoutedEventArgs e)
        {
            DB db = new DB();
            DB db2 = new DB();
            string kod;
            using (db.GetConnect())
            {
                db.ConnectOpen();
                var data = new DataTable();
                var dataAdapter = new SqlDataAdapter("Select kod from uslugi where Nazvanie ='" + viborusl.Text + "'", db.GetConnect()); //поиск кода услуги по названию, выбранному в комбобоксе
                dataAdapter.Fill(data);
                kod = data.Rows[0][0].ToString();
            }
            using (db2.GetConnect())
            {
                db2.ConnectOpen();
                var dataAdapter3 = new SqlDataAdapter("select * from View_1 where status = 0 and koduslugi='" + kod + "'", db2.GetConnect());//Выборак из представления заказов с кодом улуги, выбранным в комбобоксе
                var data3 = new DataTable();
                dataAdapter3.Fill(data3);
                zapros.ItemsSource = data3.DefaultView;
            }
        }

        private void okkk_Click(object sender, RoutedEventArgs e)
        {
            DB db = new DB();
            DB db2 = new DB();
            string ID;
            using (db.GetConnect())
            {
                db.ConnectOpen();
                var data2 = new DataTable();
                var dataAdapter2 = new SqlDataAdapter("Select ID from klient where FIO = '" + viborpo.Text + "'", db.GetConnect());//поиск ид клиента по ФИО, выбранному в комбобоксе
                dataAdapter2.Fill(data2);
                ID = data2.Rows[0][0].ToString();
            }

            using (db2.GetConnect())
            {
                db2.ConnectOpen();
                var dataAdapter3 = new SqlDataAdapter("select * from View_1 where idklient='" + ID + "'", db2.GetConnect());//Выборак из представления заказов с идом клиента, выбранным в комбобоксе
                var data3 = new DataTable();
                dataAdapter3.Fill(data3);
                zapros.ItemsSource = data3.DefaultView;
            }
        }


        private void viborusl_DropDownOpened(object sender, EventArgs e) //заполнение комбобокса услуг, по его открытию 
        {
            DB db = new DB();
            using (db.GetConnect())
            {
                db.ConnectOpen();
                var dataAdapter = new SqlDataAdapter("Select Nazvanie from uslugi where status = 0", db.GetConnect());//получение списка услуг
                var data = new DataTable();
                dataAdapter.Fill(data);
                viborusl.Items.Clear();
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    viborusl.Items.Add(data.Rows[i][0]);//цикл для заполнения списка услуг
                }

            }

        }

        private void viborpo_DropDownOpened(object sender, EventArgs e) //заполнение комбобокса клиентов по его открытию
        {
            DB db = new DB();
            using (db.GetConnect())
            {
                db.ConnectOpen();
                var dataAdapter = new SqlDataAdapter("Select FIO from klient where status = 0", db.GetConnect());//получение списка клиентов
                var data = new DataTable();
                dataAdapter.Fill(data);
                viborpo.Items.Clear();
                for (int i = 0; i < data.Rows.Count; i++)
                {
                    viborpo.Items.Add(data.Rows[i][0]);//цикл для заполнения списка клиентов
                }

            }
        }

        private void ydal_Click(object sender, RoutedEventArgs e)
        {
            DB db2 = new DB();
            try
            {
                using (db2.GetConnect())
                {
                    db2.ConnectOpen();
                    var dataAdapter = new SqlDataAdapter("Select * from uslugi", db2.GetConnect());//обновление таблицы на форме
                    var data = new DataTable();
                    dataAdapter.Fill(data);
                    uslygi.ItemsSource = data.DefaultView;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Возникла ошибка");
            }
        }
    }
}
