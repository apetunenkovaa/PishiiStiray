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
using YP_MDK_01_session_one.clasess;

namespace YP_MDK_01_session_one.window
{
    /// <summary>
    /// Логика взаимодействия для WindowWorkBasket.xaml
    /// </summary>
    public partial class WindowWorkBasket : Window
    {
        double sum; 
        double sumDiscount; 
        double sD;
        List<ClassWorkBasket> basket;
        Users user;
        public WindowWorkBasket(List<ClassWorkBasket>, Users users)
        {
            InitializeComponent();
            this.basket = basket;
            this.user = user;
            ListProd.ItemsSource = basket;
            List<PickupPoint> pickupPoints = ClassBase.BD.PickupPoint.ToList();
            foreach (PickupPoint pickup in pickupPoints)
            {
                cbPickPoint.Items.Add(pickup.PostCode + ", " + pickup.City.CityName + ", " + pickup.Street + ", " + pickup.NumberHome);
            }
            cbPickPoint.SelectedIndex = 0;
            if (user != null)
            {
                tb_FIO.Text = " " + user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
            }
            calculate();
            tbSum.Text = "Сумма заказа: " + string.Format("{0:N2}", sum) + " руб.";
            tbSumDiscount.Text = "Скидка: " + string.Format("{0:N2}", sumDiscount) + "%";
        }

        private void calculate()
        {
            sum = 0; sumDiscount = 0; sD = 0;
            foreach (ClassWorkBasket classWorkBasket in basket)
            {
                sum += classWorkBasket.count * (double)classWorkBasket.product.CostOrders;
                sD += classWorkBasket.count * (double)classWorkBasket.product.ProductCost;
                sumDiscount = 100 - 100 * sum / sD;
            }
        }

        private void btn_Delete_Click(object sender, RoutedEventArgs e)
        {

        }

        private void tb_kolvo_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void tb_kolvo_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {

        }

        private void btn_Back_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btn_Order_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
