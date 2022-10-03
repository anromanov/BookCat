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

namespace BookCat
{
    /// <summary>
    /// Логика взаимодействия для PersonEditWindow.xaml
    /// </summary>
    public partial class PersonEditWindow : Window
    {
        internal PersonEditWindow(AuthorListVM vm)
        {
            InitializeComponent();
            DataContext = vm;
            if (vm.CloseChildAction == null)
                vm.CloseChildAction = new Action(this.Close);
        }
    }
}
