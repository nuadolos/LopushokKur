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
using Microsoft.Win32;

namespace LopushokKur
{
    /// <summary>
    /// Логика взаимодействия для AddEditPage.xaml
    /// </summary>
    public partial class AddEditPage : Page
    {
        string PathProject = System.IO.Path.GetFullPath(System.IO.Path.Combine(AppContext.BaseDirectory, "..\\..\\"));
        private Product pm = new Product();
        public AddEditPage()
        {
            InitializeComponent();

            CmbBox3.ItemsSource = LopushBase.GetContext().ProductType.ToList();

            if (Transition._DataContext != null)
            {
                 pm = (Transition._DataContext as ProductMaterial).Product;

                TxtBox1.Text = pm.ArticleNumber;
                TxtBox2.Text = pm.Title;
                CmbBox3.SelectedIndex = (int)pm.ProductTypeID - 1;
                TxtBox4.Text = pm.Image;
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            pm.ArticleNumber = TxtBox1.Text;
            pm.Title = TxtBox2.Text;
            pm.ProductTypeID = CmbBox3.SelectedIndex;
            pm.Image = TxtBox4.Text;

            LopushBase.GetContext().Product.Add(pm);

            try
            {
                LopushBase.GetContext().SaveChanges();
                MessageBox.Show("Данные сохранены", "Успешно", MessageBoxButton.OK, MessageBoxImage.Information);
                Transition.MainFrame.GoBack();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void LoadBtn_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog loadPhoto = new OpenFileDialog();
            loadPhoto.Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp|All files (*.*)|*.*";

            if (loadPhoto.ShowDialog() == true)
            {
                try
                {
                    if (!File.Exists($"../products/{loadPhoto.SafeFileName}"))
                        File.Copy(loadPhoto.FileName, $"{PathProject}products/{loadPhoto.SafeFileName}");
                    TxtBox4.Text = $"products/{loadPhoto.SafeFileName}";
                }
                catch (Exception)
                {
                    MessageBox.Show("При загрузки изображения возникла ошибка", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void CancelBtn_Click(object sender, RoutedEventArgs e)
        {
            Transition.MainFrame.GoBack();
        }
    }
}
