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
using LopushokKur.Model;
using System.IO;

namespace LopushokKur
{
    /// <summary>
    /// Логика взаимодействия для ProdMatView.xaml
    /// </summary>
    public partial class ProdMatView : Page
    {

        public List<ProductMaterial> ItemsProdMat { get { return LopushBase.GetContext().ProductMaterial.ToList(); } }
        public int CountItemsProdMat { get { return ItemsProdMat.Count; } }

        public bool CmbBoxSortChanged { get; set; }
        public bool CmbBoxFiltChanged { get; set; }

        PageViewModel pvm = new PageViewModel(1, 0);

        public ProdMatView()
        {
            InitializeComponent();

            var allTypes = LopushBase.GetContext().ProductType.ToList();
            allTypes.Insert(0, new ProductType
            {
                Title = "Все типы"
            });
            CmbBoxFilt.ItemsSource = allTypes;

            CmbBoxFilt.SelectedIndex = 0;
            CmbBoxSort.SelectedIndex = 0;

            pvm.CountItems = CountItemsProdMat;
            pvm.GetIndex();

            LViewProdMat.ItemsSource = ItemsProdMat.GetRange(pvm.StartIndex, pvm.CountRangeItems);
        }

        private void AddProdMatBtn_Click(object sender, RoutedEventArgs e)
        {
            Transition._DataContext = null;
            Transition.MainFrame.Navigate(new AddEditPage());
        }
        
        private void UpdateData()
        {
            var currentData = LopushBase.GetContext().ProductMaterial.ToList();

            if (CmbBoxFilt.SelectedIndex > 0)
            {
                currentData = currentData.Where(p => p.Product.ProductType.Title == (CmbBoxFilt.SelectedValue as ProductType).Title.ToString()).ToList();
            }

            pvm.CountItems = currentData.Count;

            pvm.GetTotalPage();
            pvm.GetIndex();

            if (TxtBoxFilt.Text != "Введите для поиска")
            {
                currentData = currentData.Where(p => p.Product.Title.ToLower().Contains(TxtBoxFilt.Text.ToLower()) || p.Material.Title.ToLower().Contains(TxtBoxFilt.Text)).ToList();

                pvm.CountItems = currentData.Count;

                if (pvm.LessCountRangeItems)
                {
                    NextPage.Visibility = Visibility.Hidden;
                }

                pvm.GetTotalPage();
                pvm.GetIndex();
            }

            switch (CmbBoxSort.SelectedIndex)
            {
                case 0:
                    {
                        for (int i = pvm.CountRangeItems; i > 0; i--)
                        {
                            try
                            {
                                LViewProdMat.ItemsSource = currentData.ToList().GetRange(pvm.StartIndex, i);
                                break;
                            }
                            catch (Exception)
                            {

                            }
                        }
                        break;
                    }
                case 1:
                    {
                        for (int i = pvm.CountRangeItems; i > 0; i--)
                        {
                            try
                            {
                                LViewProdMat.ItemsSource = currentData.OrderBy(p => p.Product.Title).ToList().GetRange(pvm.StartIndex, i);
                                break;
                            }
                            catch (Exception)
                            {

                            }
                        }
                        break;
                    }
                case 2:
                    {
                        for (int i = pvm.CountRangeItems; i > 0; i--)
                        {
                            try
                            {
                                LViewProdMat.ItemsSource = currentData.OrderBy(p => p.Material.Cost).ToList().GetRange(pvm.StartIndex, i);
                                break;
                            }
                            catch (Exception)
                            {

                            }
                        }
                        break;
                    }
            }
        }

        private void TxtBoxFilt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtBoxFilt.Text != "Введите для поиска")
            {
                pvm.NumberPage = 1;
                PreviousPage.Visibility = Visibility.Hidden;
                NextPage.Visibility = Visibility.Visible;
                UpdateData();
            }
        }

        private void CmbBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pvm.NumberPage = 1;
            PreviousPage.Visibility = Visibility.Hidden;
            NextPage.Visibility = Visibility.Visible;
            UpdateData();
        }

        private void CmbBoxFilt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            pvm.NumberPage = 1;
            PreviousPage.Visibility = Visibility.Hidden;
            NextPage.Visibility = Visibility.Visible;
            UpdateData();
        }

        private void TxtBoxFilt_GotFocus(object sender, RoutedEventArgs e)
        {
            TxtBoxFilt.Text = null;
        }

        private void TxtBoxFilt_LostFocus(object sender, RoutedEventArgs e)
        {
            if (TxtBoxFilt.Text == "")
                TxtBoxFilt.Text = "Введите для поиска";
        }

        private void PreviousPage_Click(object sender, RoutedEventArgs e)
        {
            pvm.NumberPage -= 1;
            pvm.GetIndex();
            UpdateData();

            if (!pvm.HasPreviousPage)
                PreviousPage.Visibility = Visibility.Hidden;

            if (pvm.HasNextPage)
                NextPage.Visibility = Visibility.Visible;
        }

        private void NextPage_Click(object sender, RoutedEventArgs e)
        {
            pvm.NumberPage += 1;
            pvm.GetIndex();

            UpdateData();

            if (pvm.NumberPage == pvm.GetTotalPage())
            {
                NextPage.Visibility = Visibility.Hidden;
            }

            if (pvm.HasPreviousPage)
                PreviousPage.Visibility = Visibility.Visible;
        }

        private void LViewProdMat_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (LViewProdMat.SelectedItems.Count == 1)
            {
                Transition._DataContext = LViewProdMat.SelectedItem;
                Transition.MainFrame.Navigate(new AddEditPage());
            }
            else
                MessageBox.Show("Редактировать можно лишь один выделенный элемент", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
