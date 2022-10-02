using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookCat
{
    class BookCatVM : INotifyPropertyChanged
    {
        protected Book selectedBook;
        protected bool editMode = false;
        protected readonly ObservableCollection<Book> bookList;
        public ReadOnlyObservableCollection<Book> BookList { get; }
        public event PropertyChangedEventHandler PropertyChanged;
        public BookCatVM()
        {
            bookList = new ObservableCollection<Book>
            {
                new Book{Name ="Труды", Author = new Person{FirstName = "Иван", MiddleName = "Иванович",
                LastName = "Иванов"}, PublicationYear = 1996, Id = 1, ISBN = 1111111111},
                new Book{Name ="Начало", Author = new Person{FirstName = "Петр", MiddleName = "Петрович",
                LastName = "Петров"}, PublicationYear = 1956, Id = 2, ISBN = 222222222},
                new Book{Name ="Капитал", Author = new Person{FirstName = "Карл", MiddleName = "Гансович",
                LastName = "Маркс"}, PublicationYear = 1973, Id = 3, ISBN = 333333333},
                new Book{Name ="Собрание сочинений", Author = new Person{FirstName = "Сидор", MiddleName = "Сидорович",
                LastName = "Сидоров"}, PublicationYear = 2002, Id = 4, ISBN = 444444444},
                new Book{Name ="Избранное", Author = new Person{FirstName = "Борис", MiddleName = "Борисович",
                LastName = "Борисов"}, PublicationYear = 2021, Id = 5, ISBN = 555555555}
            };
            BookList = new ReadOnlyObservableCollection<Book>(bookList);
        }
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        //=========================Свойства============================================
        public Book SelectedBook
        {
            get => selectedBook;
            set
            {
                selectedBook = value;
                OnPropertyChanged("SelectedBook");
            }
        }
        public bool EditMode
        {
            get => editMode;
            private set
            {
                editMode = value;
                OnPropertyChanged("EditMode");
                OnPropertyChanged("NotEditMode");
            }
        }
        public bool NotEditMode => !editMode;
        //=========================Команды=============================================
        protected RelayCommand newBookCommand; //Создать новую книгу с пустыми полями
        public RelayCommand NewBookCommand
        {
            get
            {
                return newBookCommand ??
                    (newBookCommand = new RelayCommand(obj =>
                    {
                        Book newBook = new Book();
                        EditMode = true;
                        SelectedBook = newBook;
                    }));
            }
        }
        protected RelayCommand editBookCommand;//Редактировать выбранную книгу
        protected RelayCommand saveBookCommand;//Сохранить изменения в книге
        public RelayCommand SaveBookCommand
        {
            get
            {
                return saveBookCommand ??
                    (saveBookCommand = new RelayCommand(obj =>
                    {
                        bookList.Insert(0, selectedBook);
                        EditMode = false;
                    }));
            }
        }
        protected RelayCommand discardChangesCommand;//Отменить все несохраннёные изменения
        protected RelayCommand showAuthorsWindowCommand;//Выбрать автора из списка
        protected RelayCommand showDBWindowCommand;//Показать настройки хранилища
    }
}
