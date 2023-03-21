using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
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
using System.Windows.Threading;

namespace YP_MDK_01_session_one.pages
{
    /// <summary>
    /// Логика взаимодействия для PageAuto.xaml
    /// </summary>
    public partial class PageAuto : Page
    {

        DispatcherTimer timer = new DispatcherTimer();
        int time = 10;
        string str = String.Empty;
        public PageAuto()
        {
            InitializeComponent();
            tb_login.Focus();
        }

        public PageAuto(int k)
        {
            InitializeComponent();
            tb_login.Focus();
            CAPTCHA();
            spCaptcha.Visibility = Visibility.Visible;
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            time--;
            tb_time.Text = "Вы сможете зайти через " + time + " секунд";
            if (time < 0)
            {
                timer.Stop();
                bt_auto.IsEnabled = true;
                tb_time.Visibility = Visibility.Collapsed;
                ClassFrame.lframe.Navigate(new PageAuto(2));
            }
        }
        private void bt_guest_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.lframe.Navigate(new pages.ShowProduct());
        }

        private void tbCaptcha_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (tbCaptcha.Text.Length == 4)
            {
                if (tbCaptcha.Text == str)
                {
                    MessageBox.Show("Успешно!", "Подтверждение");
                    bt_auto.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Капча введена неверно!", "Ошибка");
                    bt_auto.IsEnabled = false;
                    tb_login.Text = "";
                    pb_password.Password = "";
                    tbCaptcha.Text = "";
                    tb_login.IsEnabled = false;
                    pb_password.IsEnabled = false;
                    tbCaptcha.IsEnabled = false;
                    timer.Interval = new TimeSpan(0, 0, 1);
                    timer.Tick += new EventHandler(Timer_Tick);
                    timer.Start();
                    tb_time.Visibility = Visibility.Visible;
                    tbCaptcha.Visibility = Visibility.Visible;
                }
            }
        }

        public void CAPTCHA()
        {
            CCaptcha.Children.Clear();
            Random random = new Random();
            string sym = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string[] ch = new string[4];
            for (int i = 0; i < 10; i++)
            {
                SolidColorBrush solidColor = new SolidColorBrush(Color.FromRgb((byte)random.Next(256), (byte)random.Next(256), (byte)random.Next(256)));
                Line l = new Line()
                {
                    X1 = random.Next((int)CCaptcha.Width),
                    Y1 = random.Next((int)CCaptcha.Height),
                    X2 = random.Next((int)CCaptcha.Width),
                    Y2 = random.Next((int)CCaptcha.Height),
                    Stroke = solidColor
                };
                CCaptcha.Children.Add(l);
            }
            for (int i = 0; i < 4; i++)
            {
                ch[i] = Convert.ToString(sym[random.Next(sym.Length)]);
                str += ch[i];
            }
            TextBlock txt1 = new TextBlock()
            {
                Text = (string)ch[0].ToString(),
                TextDecorations = TextDecorations.Strikethrough,
                FontSize = random.Next(18, 22),
                FontFamily = new FontFamily("Comic Sans MS"),
                FontWeight = FontWeights.Bold,
                FontStyle = FontStyles.Italic,
                Margin = new Thickness(10),
                Padding = new Thickness(15)
            };
            CCaptcha.Children.Add(txt1);
            TextBlock txt = new TextBlock()
            {
                Text = (string)ch[1].ToString(),
                FontSize = random.Next(14, 18),
                FontFamily = new FontFamily("Comic Sans MS"),
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(17),
                Padding = new Thickness(23)
            };
            CCaptcha.Children.Add(txt);
            TextBlock txt2 = new TextBlock()
            {
                Text = (string)ch[2].ToString(),
                TextDecorations = TextDecorations.Strikethrough,
                FontSize = random.Next(14, 18),
                FontFamily = new FontFamily("Comic Sans MS"),
                FontWeight = FontWeights.Bold,
                FontStyle = FontStyles.Italic,
                Margin = new Thickness(10),
                Padding = new Thickness(35)
            };
            CCaptcha.Children.Add(txt2);
            TextBlock txt3 = new TextBlock()
            {
                Text = (string)ch[3].ToString(),
                FontSize = random.Next(14, 18),
                FontFamily = new FontFamily("Comic Sans MS"),
                FontWeight = FontWeights.Bold,
                Margin = new Thickness(20),
                Padding = new Thickness(35)
            };
            CCaptcha.Children.Add(txt3);
        }
        private void bt_auto_Click(object sender, RoutedEventArgs e)
        {
            if (tb_login.Text == "" || pb_password.Password == "")
            {
                MessageBox.Show("Не все обязательные поля заполнены!");
            }
            else
            {
                Users user = ClassBase.BD.Users.FirstOrDefault(z => z.UserLogin == tb_login.Text && z.UserPassword == pb_password.Password);
                if (user == null)
                {
                    MessageBox.Show("Вы ввели данные неверно! Повторите вход!");
                    bt_auto.IsEnabled = false;
                    tb_login.Text = "";
                    pb_password.Password = "";
                    CAPTCHA();
                    spCaptcha.Visibility = Visibility.Visible;
                }
                else
                {
                    switch (user.UserRole)
                    {
                        case 1: //клиент
                            ClassFrame.lframe.Navigate(new ShowProduct(user));
                            break;
                        case 2: //администратор
                            ClassFrame.lframe.Navigate(new ShowProduct(user));
                            break;
                        case 3: //менеджер
                            ClassFrame.lframe.Navigate(new ShowProduct(user));
                            break;
                    }
                }
            }
        }
    }
}
