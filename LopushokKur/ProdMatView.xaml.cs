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

            LViewProdMat.ItemsSource = LopushBase.GetContext().ProductMaterial.ToList();
        }

        private void UpdateData()
        {
            var currentData = LopushBase.GetContext().ProductMaterial.ToList();

            if (CmbBoxFilt.SelectedIndex > 0)
                currentData = currentData.Where(p => p.Product.ProductType.Title == (CmbBoxFilt.SelectedValue as ProductType).Title.ToString()).ToList();

            if (TxtBoxFilt.Text != "Введите для поиска")
                currentData = currentData.Where(p => p.Product.Title.ToLower().Contains(TxtBoxFilt.Text.ToLower()) || p.Material.Title.ToLower().Contains(TxtBoxFilt.Text)).ToList();

            switch (CmbBoxSort.SelectedIndex)
            {
                case 0:
                    {
                        LViewProdMat.ItemsSource = currentData.ToList();
                        break;
                    }
                case 1:
                    {
                        LViewProdMat.ItemsSource = currentData.OrderBy(p => p.Product.Title).ToList();
                        break;
                    }
                case 2:
                    {
                        LViewProdMat.ItemsSource = currentData.OrderBy(p => p.Material.Cost).ToList();
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
            UpdateData();
        }

        private void CmbBoxFilt_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
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
    }
}
