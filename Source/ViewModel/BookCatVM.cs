using System;
using System.Collections.ObjectModel;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace BookCat
{
    class BookCatVM : INotifyPropertyChanged
    {
        protected ApplicationContext db = new ApplicationContext();
        protected Book selectedBook;
        protected Book editedBook;//Для сохранения данных редактируемой книги на случай отмены изменений
        protected bool editMode = false;
        protected byte[] emptyBitmap;//Пустой белый JPEG, если у книги нет обложки, чтобы не сыпались исключения при выборе такой книги в списке
        protected readonly ObservableCollection<Book> bookList;
        public ReadOnlyObservableCollection<Book> BookList { get; }
        protected readonly ObservableCollection<Person> authorList;
        public event PropertyChangedEventHandler PropertyChanged;
        public BookCatVM()
        {
            db.Database.EnsureCreated();
            db.AuthorList.ToList();
            db.BookList.ToList();
            bookList = db.BookList.Local.ToObservableCollection();
            authorList = db.AuthorList.Local.ToObservableCollection();
            emptyBitmap = BitmapConvertorTools.UriJpegToByte(new Uri("Image/Empty.jpg", UriKind.RelativeOrAbsolute)); 
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
                if (!editMode)
                {
                    selectedBook = value;
                    OnPropertyChanged("SelectedBook");
                    OnPropertyChanged("CanEditBook");
                }
            }
        }
        public bool EditMode //Режим редактирование: доступны изменения текстовых полей и кнопка сохранения
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
        public bool NotEditMode => !editMode; //Это чтобы не писать пока BoolNegConvertor
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
                        if (newBook.Cover == null)
                            newBook.Cover = emptyBitmap;
                        SelectedBook = newBook;
                        EditMode = true;
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
                        if(selectedBook != null)
                            if (MessageBox.Show("Вы действительно хотите удалить выбранную книгу",
                                "Внимание", MessageBoxButton.YesNo,
                                MessageBoxImage.Warning) == MessageBoxResult.Yes)
                            {
                                db.BookList.Remove(selectedBook);
                                db.SaveChanges();
                                SelectedBook = null;
                            }
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
                        if (selectedBook.Cover == null)
                            selectedBook.Cover = emptyBitmap;
                        OnPropertyChanged("SelectedBook");
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
                            db.BookList.Update(editedBook);
                            db.SaveChanges();
                            SelectedBook = editedBook;
                            editedBook = null;
                        }
                        else
                        {
                            db.BookList.Add(selectedBook);
                            db.SaveChanges();
                        }
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
                         AuthorsWindow aw = new AuthorsWindow(db.AuthorList, selectedBook.Author);
                         aw.ShowDialog(); 
                         Person selctedAuthor = ((AuthorListVM)aw.DataContext).SelectedAuthor;
                         //db.SaveChanges();
                         if (selctedAuthor != null)
                         {
                             SelectedBook.Author = selctedAuthor;
                         }
                         
                     }));
            }
        }
        protected RelayCommand clearDBWindowCommand;//Очистить БД
        public RelayCommand ClearDBWindowCommand
        {
            get
            {
                return clearDBWindowCommand ??
                     (clearDBWindowCommand = new RelayCommand(obj =>
                     {
                         if (MessageBox.Show("Очистить все таблицы данных?",
                                 "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                             if (MessageBox.Show("Все данные будут стерты без возможности восстановления.\n Вы действительно хотите продолжить?",
                                 "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                             {
                                 db.AuthorList.Local.Clear();
                                 db.BookList.Local.Clear();
                                 db.SaveChanges();
                             }
                     }));
            }
        }
        protected RelayCommand fillDBWindowCommand;//Заполнить БД примерами
        public RelayCommand FillDBWindowCommand
        {
            get
            {
                return fillDBWindowCommand ??
                     (fillDBWindowCommand = new RelayCommand(obj =>
                     {
                         if (MessageBox.Show("Очистить все таблицы данных и заполнить примерами?",
                                 "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                             if (MessageBox.Show("Все данные будут стерты без возможности восстановления \n и заменены данными примеров.\n " +
                                 "Вы действительно хотите продолжить?",
                                 "Внимание", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                             {
                                 db.AuthorList.Local.Clear();
                                 db.BookList.Local.Clear();
                                 db.SaveChanges();

                                 db.AuthorList.AddRange(new Person[]
                                 {
                                    new Person { FirstName = "Джек", MiddleName = "", LastName = "Лондон" },
                                    new Person { FirstName = "Роберт", MiddleName = "Льюис", LastName = "Стивенсон" },
                                    new Person { FirstName = "Артур", MiddleName = "Конан", LastName = "Дойл" },
                                    new Person { FirstName = "Ольга", MiddleName = "Александровна", LastName = "Замятина" },
                                    new Person { FirstName = "Даниэль", MiddleName = "", LastName = "Дефо" } 
                                 });

                                 db.BookList.AddRange(new Book[]
                                     {
                                        new Book{Name ="Белый клык", Author = authorList[0], PublicationYear = 2022, ISBN = 9785928734688, 
                                        Cover = BitmapConvertorTools.UriJpegToByte(new Uri("Image/London.jpg", UriKind.RelativeOrAbsolute)),
                                        Annotation = "Белый Клык, полусобака-полуволк, родился на Аляске. Он жил в индейском племени и бегал в упряжке, " +
                                        "в форте Юкон его на потеху публике стравливали с другими собаками. Люди обращались с Белым Клыком жестоко, " +
                                        "и он превратился в свирепого, опасного и мстительного зверя. Во время одного из собачьих боев за него вступился " +
                                        "горный инженер Уидон Скотт; он выкупил полумертвого пса у прежнего хозяина и забрал с собой. " +
                                        "Терпением и добротой Уидону Скотту удалось заслужить доверие Белого Клыка и стать ему другом.\n Для детей 12-14 лет."},

                                        new Book{Name ="Остров Сокровищ", Author = authorList[1], PublicationYear = 2022, ISBN = 9785928734756,
                                        Cover = BitmapConvertorTools.UriJpegToByte(new Uri("Image/Stvenson.jpg", UriKind.RelativeOrAbsolute)),
                                        Annotation = "Джим Хокинс находит в сундуке бывалого моряка карту, которая указывает на настоящий пиратский тайник. " +
                                        "А значит, Джима и его друзей ждут невероятные приключения. Шхуна \"Испаньола\" не медля пустится в плавание к острову Сокровищ. " +
                                        "И чтобы справиться со всеми опасностями, путешественникам придется рисковать, отважно сражаться, идти на хитрость, " +
                                        "не забывая о чести, и иногда полагаться на волю случая.\n Для детей 12-14 лет."},

                                        new Book{Name ="Приключения Шерлока Холмса", Author = authorList[2], PublicationYear = 2019, ISBN = 9785928728366,
                                        Cover = BitmapConvertorTools.UriJpegToByte(new Uri("Image/Doil.jpg", UriKind.RelativeOrAbsolute)),
                                        Annotation = "Пять рассказов Артура Конан-Дойла о Шерлоке Холмсе и докторе Уотсоне снабжены подробным историко-бытовым комментарием. " +
                                        "Читателя ждет погружение в лондонскую жизнь конца XIX века: в комментариях - история и география Британии, " +
                                        "достижения медицины и традиции образования, мода и транспорт рубежа веков, увлекательная история криминалистики: " +
                                        "известные преступники и прославленные сыщики, отпечатки пальцев и следы обуви, грим и маскировка, яды и оружие."},

                                        new Book{Name ="Ничья", Author = authorList[3], PublicationYear = 2022, ISBN =  9785907362796,
                                        Cover = BitmapConvertorTools.UriJpegToByte(new Uri("Image/Zamyatina.jpg", UriKind.RelativeOrAbsolute)),
                                        Annotation = "Когда ты маленький, мир кажется простым и понятным. Так было и в жизни Ксюши: любимые родители и младшая сестрёнка, " +
                                        "лучшая подруга и хорошие оценки в школе. Неужели что-то может измениться? Тринадцатый день рождения приносит с собой сомнения: " +
                                        "а что, если родные всё это время лгали ей… Девочка пересмотрела кипу документов, выучила наизусть мамину соцсеть " +
                                        "с момента своего появления на свет и попыталась выведать у крёстной все тайны. Кажется, Ксюша затеяла игру с родителями, " +
                                        "вот только они об этом даже не догадываются. Будет ли здесь победитель? Или их ждёт Ничья?"},

                                        new Book{Name ="Жизнь и удивительные приключения морехода Робинзона Крузо", Author = authorList[4], PublicationYear = 2022,
                                        Cover = BitmapConvertorTools.UriJpegToByte(new Uri("Image/Defo.jpg", UriKind.RelativeOrAbsolute)), 
                                        ISBN = 9785928727376, Annotation = "Знаменитый роман английского писателя Даниэля Дэфо был написан более 300 лет назад, " +
                                        "но удивительная судьба Робинзона Крузо и сегодня вызывает интерес и сочувствие у читателей. Его предприимчивость, трудолюбие, " +
                                        "вера в собственные силы достойны уважения независимо от смены эпох и нравов.\n Роман печатается в классическом пересказе " +
                                        "Корнея Чуковского с великолепными иллюстрациями Вадима Челака. \n Пересказ с английскогои предисловие Корнея Чуковского.\n" +
                                        "Для детей 7-10 лет."}
                                    });
                                 db.SaveChanges();
                             }
                     }));
            }
        }
        protected RelayCommand addCoverImageCommand;//Загрузить изображение обложки из файла с помощью стандартного диалога
        public RelayCommand AddCoverImageCommand
        {
            get
            {
                return addCoverImageCommand ??
                    (addCoverImageCommand = new RelayCommand(obj =>
                    {
                        var dialog = new Microsoft.Win32.OpenFileDialog();
                        dialog.DefaultExt = ".jpg"; 
                        dialog.Filter = "Изображения (.jpg)|*.jpg"; // Только JPEG

                        if ((bool)dialog.ShowDialog())
                        {
                            SelectedBook.Cover = BitmapConvertorTools.UriJpegToByte(new Uri(dialog.FileName, UriKind.RelativeOrAbsolute));
                        }
                    }));
            }
        }
        protected RelayCommand removeCoverImageCommand;//Удалить изображение обложки
        public RelayCommand RemoveCoverImageCommand
        {
            get
            {
                return removeCoverImageCommand ??
                    (removeCoverImageCommand = new RelayCommand(obj =>
                    {
                        if(selectedBook.Cover != null)
                            if (MessageBox.Show("Вы действительно хотите удалить изображение обложки",
                                "Внимание", MessageBoxButton.YesNo,
                                MessageBoxImage.Warning) == MessageBoxResult.Yes)
                            {
                                SelectedBook.Cover = emptyBitmap;
                            }
                    }));
            }
        }
    }
}
