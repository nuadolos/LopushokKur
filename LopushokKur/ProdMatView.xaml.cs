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

namespace LopushokKur
{
    /// <summary>
    /// Логика взаимодействия для ProdMatView.xaml
    /// </summary>
    public partial class ProdMatView : Page
    {
        public ProdMatView()
        {
            InitializeComponent();

            LViewProdMat.ItemsSource = LopushBase.GetContext().ProductMaterial.ToList();
        }

        private void TxtBoxFilt_TextChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
