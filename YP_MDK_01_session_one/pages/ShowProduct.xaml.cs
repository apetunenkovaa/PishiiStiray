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
using YP_MDK_01_session_one.clasess;
using YP_MDK_01_session_one.window;

namespace YP_MDK_01_session_one.pages
{
    /// <summary>
    /// Логика взаимодействия для ShowProduct.xaml
    /// </summary>
    public partial class ShowProduct : Page
    {
        List<Product> listFilter;
        Users user;
        List<ClassWorkBasket> basket = new List<ClassWorkBasket>();
       
        public ShowProduct()
        {
            InitializeComponent();
            list_product.ItemsSource = ClassBase.BD.Product.ToList();
            gp_sort.SelectedIndex = 0;
            gp_sale.SelectedIndex = 0;

            Filter();
        }

        public ShowProduct(Users user)
        {
            InitializeComponent();
            this.user = user;
            list_product.ItemsSource = ClassBase.BD.Product.ToList();
            gp_sort.SelectedIndex = 0;
            gp_sale.SelectedIndex = 0;
            tb_FIO.Text = " " + user.UserSurname + " " + user.UserName + " " + user.UserPatronymic;
            if (user.Role.RoleName == "Администратор" || user.Role.RoleName == "Менеджер")
            {
                btn_zakaz.Visibility = Visibility.Visible;
            }
            Filter();
        }

        public void Filter()
        {
            listFilter = ClassBase.BD.Product.ToList();

            //сортировка
            switch (gp_sort.SelectedIndex)
            {
                case 1:
                    listFilter.Sort((x, y) => x.ProductCost.CompareTo(y.ProductCost));
                    break;
                case 2:
                    listFilter.Sort((x, y) => x.ProductCost.CompareTo(y.ProductCost));
                    listFilter.Reverse();
                    break;
            }

            //фильтр
            switch (gp_sale.SelectedIndex)
            {
                case 1:
                    listFilter = listFilter.Where(x => x.ProductDiscountAmount > 0 && x.ProductDiscountAmount < 9.99).ToList();
                    break;
                case 2:
                    listFilter = listFilter.Where(x => x.ProductDiscountAmount > 10 && x.ProductDiscountAmount < 14.99).ToList();
                    break;
                case 3:
                    listFilter = listFilter.Where(x => x.ProductDiscountAmount > 15).ToList();
                    break;
            }

            //поиск
            if (!string.IsNullOrWhiteSpace(tb_shearch.Text))
            {
                listFilter = listFilter.Where(x => x.ProductName.ToLower().Contains(tb_shearch.Text.ToLower())).ToList(); //поиск по наименованию
            }

            tb_countzap.Text = listFilter.Count.ToString() + " из " + ClassBase.BD.Product.ToList().Count.ToString(); //количество записей

            list_product.ItemsSource = listFilter;
            if (listFilter.Count == 0)
            {
                MessageBox.Show("нет записей");
            }
        }
        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            ClassFrame.lframe.Navigate(new PageAuto());
        }

        private void gp_sort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void gp_sale_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Filter();
        }

        private void btnDelete_Loaded(object sender, RoutedEventArgs e)
        {
            if (user == null)
            {
                return;
            }
            Button btnDel = sender as Button;
            if (user.Role.RoleName == "Администратор" || user.Role.RoleName == "Менеджер")
            {
                btnDel.Visibility = Visibility.Visible;
            }
        }

        private void btn_zakaz_Click(object sender, RoutedEventArgs e)
        {
            WindowOrders windowOrders = new WindowOrders();
            windowOrders.ShowDialog();
        }

        private void btn_order_Click(object sender, RoutedEventArgs e)
        {
            WindowWorkBasket windowWorkBasket = new WindowWorkBasket(basket, user);
            windowWorkBasket.ShowDialog();
            if (basket.Count == 0)
            {
                btn_order.Visibility = Visibility.Collapsed;
            }
        }

        private void tb_shearch_TextChanged(object sender, TextChangedEventArgs e)
        {
            Filter();
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button btn = (Button)sender;
                string index = btn.Uid;
                //if (MessageBox.Show("Вы действительно хотите удалить данный товар?", "Системное сообщение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
                //{
                //    List<OrederProduct> orderProduct = ClassBase.BD.OrederProduct.Where(x => x.OrderProduct == index).ToList();
                //    if (orderProduct.Count == 0)
                //    {
                //        Product product = ClassBase.BD.Product.FirstOrDefault(z => z.ProductArticleNumber == index);
                //        ClassBase.BD.Product.Remove(product);
                //        ClassBase.BD.SaveChanges();
                //        ClassFrame.lframe.Navigate(new ShowProduct(user));
                //    }
                //    else
                //    {
                //        MessageBox.Show("Товар невозможно удалить, потому что он есть в заказе!");
                //    }
                //}
            }
            catch
            {
                MessageBox.Show("Что-то пошло не так..");
            }
        }
    }
}
