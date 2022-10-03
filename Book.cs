using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Runtime.CompilerServices;

namespace BookCat
{
    //Класс Книга -- основной класс модели
    class Book : INotifyPropertyChanged
    {
        protected int id = -1;//Уникальный идентификатор
        protected string name;//Название книги
        protected int publicationYear;//Год издания
        protected long isbn;//ISBN
        protected string annotation;//Краткое содержание
        protected Image cover;//Изображение обложки
        protected Person author;//Автор

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
        public Image Cover
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
