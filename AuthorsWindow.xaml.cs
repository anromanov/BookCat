using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Логика взаимодействия для AuthorsWindow.xaml
    /// </summary>
    public partial class AuthorsWindow : Window
    {
        internal AuthorsWindow(ObservableCollection<Person> authorList, Person selAuthor)
        {
            InitializeComponent();
            AuthorListVM alvm = new AuthorListVM(authorList, selAuthor);
            DataContext = alvm;
            if (alvm.CloseAction == null)
                alvm.CloseAction = new Action(this.Close);
        }
    }
}
