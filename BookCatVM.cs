using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BookCat
{
    class BookCatVM : INotifyPropertyChanged
    {
        protected Book selectedBook;
        protected Book editedBook;
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
                OnPropertyChanged("CanEditBook");
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
                OnPropertyChanged("CanEditBook");
            }
        }
        public bool NotEditMode => !editMode;
        public bool CanEditBook => selectedBook != null && !editMode;
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
        protected RelayCommand removeBookCommand;//Удалить выбранную книгу
        public RelayCommand RemoveBookCommand
        {
            get
            {
                return removeBookCommand ??
                    (removeBookCommand = new RelayCommand(obj =>
                    {
                        bookList.Remove(selectedBook);
                        SelectedBook = null;
                    }));
            }
        }
        protected RelayCommand editBookCommand;//Редактировать выбранную книгу
        public RelayCommand EditBookCommand
        {
            get
            {
                return editBookCommand ??
                    (editBookCommand = new RelayCommand(obj =>
                    {
                        editedBook = selectedBook;
                        selectedBook = new Book();
                        selectedBook.CopyFrom(editedBook);
                        EditMode = true;
                    }));
            }
        }
        protected RelayCommand saveBookCommand;//Сохранить изменения в книге
        public RelayCommand SaveBookCommand
        {
            get
            {
                return saveBookCommand ??
                    (saveBookCommand = new RelayCommand(obj =>
                    {
                        if(editedBook != null)
                        {
                            editedBook.CopyFrom(selectedBook);
                            SelectedBook = editedBook;
                            editedBook = null;
                        }
                        else
                            bookList.Insert(0, selectedBook);
                        EditMode = false;
                    }));
            }
        }
        protected RelayCommand discardChangesCommand;//Отменить все несохраннёные изменения
        public RelayCommand DiscardChangesCommand
        {
            get
            {
                return discardChangesCommand ??
                    (discardChangesCommand = new RelayCommand(obj =>
                    {
                        if (editedBook != null)
                        {
                            SelectedBook = editedBook;
                            editedBook = null;
                        }
                        else
                            SelectedBook = null;
                        EditMode = false;
                    }));
            }
        }
        protected RelayCommand showAuthorsWindowCommand;//Выбрать автора из списка
        public RelayCommand ShowAuthorsWindowCommand
        {
            get
            {
                return showAuthorsWindowCommand ??
                     (showAuthorsWindowCommand = new RelayCommand(obj =>
                     {
                         MessageBox.Show("Выбор автора");
                     }));
            }
        }
        protected RelayCommand showDBWindowCommand;//Показать настройки хранилища
        public RelayCommand ShowDBWindowCommand
        {
            get
            {
                return showDBWindowCommand ??
                     (showDBWindowCommand = new RelayCommand(obj =>
                     {
                        MessageBox.Show("Настройки базы данных");
                     }));
            }
        }
    }
}
