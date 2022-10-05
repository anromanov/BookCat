using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace BookCat
{
    internal class AuthorListVM : INotifyPropertyChanged
    {
        protected Person selectedPerson;
        protected Person editedPerson;
        protected bool mustSavePerson;
        protected readonly ObservableCollection<Person> authorList;
        public ReadOnlyObservableCollection<Person> AuthorList { get; }
        public AuthorListVM(DbSet<Person> authors, Person selAuthor)
        {
            authorList = authors.Local.ToObservableCollection();
            AuthorList = new ReadOnlyObservableCollection<Person>(authorList);
            SelectedPerson = selAuthor;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        //=========================Свойства============================================
        public Person SelectedPerson
        {
            get => selectedPerson;
            set
            {
                selectedPerson = value;
                OnPropertyChanged("SelectedPerson");
            }
        }
        public bool IsSave { get; set; }
        public Person EditedPerson => editedPerson;
        //=========================Команды=============================================
        public System.Action CloseAction { get; set; } //Дает возможность закрыть окно из ViewModel, задается в конструкторе окна
        public System.Action CloseChildAction { get; set; } //Дает возможность закрыть дочернее окно из ViewModel, задается в конструкторе окна
        public Person SelectedAuthor { get; set; }
        protected RelayCommand selectAuthorCommand;//Выбрать автора из списка и вернуться в главное окно
        public RelayCommand SelectAuthorCommand
        {
            get
            {
                return selectAuthorCommand ??
                     (selectAuthorCommand = new RelayCommand(obj =>
                     {
                         SelectedAuthor = SelectedPerson;
                         CloseAction();
                     }));
            }
        }
        protected RelayCommand addAuthorCommand;//Добавить автора в список
        public RelayCommand AddAuthorCommand
        {
            get
            {
                return addAuthorCommand ??
                     (addAuthorCommand = new RelayCommand(obj =>
                     {
                         mustSavePerson = false;
                         editedPerson = new Person();
                         PersonEditWindow pew = new PersonEditWindow(this);
                         pew.ShowDialog();
                         if (mustSavePerson)
                         {
                             authorList.Add(editedPerson);
                             
                             SelectedPerson = editedPerson;
                             editedPerson = null;
                         }
                     }));
            }
        }
        protected RelayCommand editAuthorCommand;//Редактировать выбранного автора
        public RelayCommand EditAuthorCommand
        {
            get
            {
                return editAuthorCommand ??
                     (editAuthorCommand = new RelayCommand(obj =>
                     {
                         if(selectedPerson != null)
                         {
                             mustSavePerson = false;
                             editedPerson = new Person 
                                { FirstName = selectedPerson.FirstName, MiddleName = selectedPerson.MiddleName, LastName = selectedPerson.LastName };
                             PersonEditWindow pew = new PersonEditWindow(this);
                             pew.ShowDialog();
                             if (mustSavePerson)
                             {
                                 selectedPerson.FirstName = editedPerson.FirstName;
                                 selectedPerson.MiddleName = editedPerson.MiddleName;
                                 selectedPerson.LastName = editedPerson.LastName;
                                 editedPerson = null;
                             }
                         }

                     }));
            }
        }
        protected RelayCommand removeAuthorCommand;//Удалить автора
        public RelayCommand RemoveAuthorCommand
        {
            get
            {
                return removeAuthorCommand ??
                     (removeAuthorCommand = new RelayCommand(obj =>
                     {
                         if(MessageBox.Show("Вы действительно хотите удалить выбранного автора",
                            "Внимание", MessageBoxButton.YesNo,
                            MessageBoxImage.Warning) == MessageBoxResult.Yes)
                         {
                             authorList.Remove(selectedPerson);
                             if(authorList.Count > 0)
                                SelectedPerson = authorList[0];
                         }
                     }));
            }
        }
        protected RelayCommand saveAuthorCommand;//Сохранить созданного или редактируемого автора
        public RelayCommand SaveAuthorCommand
        {
            get
            {
                return saveAuthorCommand ??
                     (saveAuthorCommand = new RelayCommand(obj =>
                     {
                         mustSavePerson = true;
                         CloseChildAction();
                     }));
            }
        }
    }
}