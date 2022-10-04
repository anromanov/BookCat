using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookCat
{
    //Класс Книга -- основной класс модели
    class Book : INotifyPropertyChanged
    {
        protected int id;//Уникальный идентификатор
        protected string name;//Название книги
        protected int publicationYear;//Год издания
        protected long isbn;//ISBN
        protected string annotation;//Краткое содержание
        protected byte[] cover ;//Изображение обложки
        protected Person author;//Автор

        /*public Book()
        {
            System.Windows.Media.Imaging.BitmapImage bi = new System.Windows.Media.Imaging.BitmapImage();
            bi.BeginInit();
            bi.UriSource = new System.Uri("Image/Empty.jpg", System.UriKind.RelativeOrAbsolute);
            bi.DecodePixelWidth = 200;
            bi.EndInit();
            cover = BitmapConvertorTools.BitmapSourceToByte((System.Windows.Media.Imaging.BitmapSource)bi);
        }*/
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        public void CopyFrom(Book book)
        {
            Id = book.Id;
            Name = book.Name;
            PublicationYear = book.PublicationYear;
            ISBN = book.ISBN;
            Annotation = book.Annotation;
            Cover = book.Cover;
            Author = book.Author;
        }
        //=========================Свойства============================================
        public string Name
        {
            get => name;
            set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }
        public string Annotation
        {
            get => annotation;
            set
            {
                annotation = value;
                OnPropertyChanged("Annotation");
            }
        }
        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
        public int PublicationYear
        {
            get => publicationYear;
            set
            {
                publicationYear = value;
                OnPropertyChanged("PublicationYear");
            }
        }
        public long ISBN
        {
            get => isbn;
            set
            {
                isbn = value;
                OnPropertyChanged("ISBN");
            }
        }
        public Person Author
        {
            get => author;
            set
            {
                author = value;
                OnPropertyChanged("Author");
            }
        }
        public byte[] Cover
        {
            get => cover;
            set
            {
                cover = value;
                OnPropertyChanged("Cover");
            }
        }
    }
}
