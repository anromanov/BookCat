using Microsoft.EntityFrameworkCore;
using System;
using System.Windows;

namespace BookCat
{
    
    public partial class AuthorsWindow : Window
    {
        internal AuthorsWindow(DbSet<Person> authorList, Person selAuthor)
        {
            InitializeComponent();
            AuthorListVM alvm = new AuthorListVM(authorList, selAuthor);
            DataContext = alvm;
            if (alvm.CloseAction == null)
                alvm.CloseAction = new Action(this.Close);
        }
    }
}
