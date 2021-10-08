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
        //public bool SortReverseTitle { get; set; }
        //public bool SortReverseCost { get; set; }

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

        private void UpdateData()
        {
            var currentData = LopushBase.GetContext().ProductMaterial.ToList();

            if (CmbBoxFilt.SelectedIndex > 0)
            {
                currentData = currentData.Where(p => p.Product.ProductType.Title == (CmbBoxFilt.SelectedValue as ProductType).Title.ToString()).ToList();

                pvm.CountItems = currentData.Count;
                
                pvm.GetTotalPage();
                pvm.GetIndex();

                if (pvm.CountItems < pvm.StartIndex + pvm.CountRangeItems)
                    pvm.CountRangeItems = CountItemsProdMat - pvm.StartIndex;

                if (pvm.NumberPage == pvm.GetTotalPage())
                {
                    NextPage.Visibility = Visibility.Hidden;
                    pvm.CountRangeItems = 6;
                }
            }

            if (TxtBoxFilt.Text != "Введите для поиска")
            {
                currentData = currentData.Where(p => p.Product.Title.ToLower().Contains(TxtBoxFilt.Text.ToLower()) || p.Material.Title.ToLower().Contains(TxtBoxFilt.Text)).ToList();
            }

            switch (CmbBoxSort.SelectedIndex)
            {
                case 0:
                    {
                        try
                        {
                            LViewProdMat.ItemsSource = currentData.ToList().GetRange(pvm.StartIndex, pvm.CountRangeItems);
                        }
                        catch (Exception)
                        {
                            LViewProdMat.ItemsSource = currentData.ToList();
                        }
                        break;
                    }
                case 1:
                    {
                        try
                        {
                            LViewProdMat.ItemsSource = currentData.OrderBy(p => p.Product.Title).ToList().GetRange(pvm.StartIndex, pvm.CountRangeItems);
                        }
                        catch (Exception)
                        {
                            LViewProdMat.ItemsSource = currentData.OrderBy(p => p.Product.Title).ToList();
                        }
                        break;
                    }
                case 2:
                    {
                        try
                        {
                            LViewProdMat.ItemsSource = currentData.OrderBy(p => p.Material.Cost).ToList().GetRange(pvm.StartIndex, pvm.CountRangeItems);
                        }
                        catch (Exception)
                        {
                            LViewProdMat.ItemsSource = currentData.OrderBy(p => p.Material.Cost).ToList();
                        }
                        break;
                    }
            }
        }

        private void TxtBoxFilt_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (TxtBoxFilt.Text != "Введите для поиска")
                UpdateData();
        }

        private void CmbBoxSort_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //pvm.NumberPage = 1;
            //PreviousPage.Visibility = Visibility.Hidden;
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

            if (pvm.CountItems < pvm.StartIndex + pvm.CountRangeItems)
                pvm.CountRangeItems = CountItemsProdMat - pvm.StartIndex;

            UpdateData();

            if (pvm.NumberPage == pvm.GetTotalPage())
            {
                NextPage.Visibility = Visibility.Hidden;
                pvm.CountRangeItems = 6;
            }

            if (pvm.HasPreviousPage)
                PreviousPage.Visibility = Visibility.Visible;
        }
    }
}
