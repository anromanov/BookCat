using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookCat
{
    //Класс для реализации списка авторов
    internal class Person : INotifyPropertyChanged
    {
        protected int id;//Уникальный идентификатор
        protected string firstName;//Имя
        protected string middleName;//Отчество
        protected string lastName;//Фамилия

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "") //Для связки с представлением
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(prop));
        }
        //=========================Свойства============================================
        public string FirstName
        {
            get => firstName;
            set
            {
                firstName = value;
                OnPropertyChanged("FirstName");
            }
        }
        public string MiddleName
        {
            get => middleName;
            set
            {
                middleName = value;
                OnPropertyChanged("MiddleName");
            }
        }
        public string LastName
        {
            get => lastName;
            set
            {
                lastName = value;
                OnPropertyChanged("LastName");
            }
        }
        public string ShortName
        {
            get
            {
                string res = "";
                if (firstName != null && firstName.Length > 0)
                    res += firstName[0] + ". ";
                if (middleName != null && middleName.Length > 0)
                    res += middleName[0] + ". ";
                return res + lastName;
            }

        }
        public string FullName => firstName + " " + middleName + " " + lastName;
        public int Id
        {
            get => id;
            set
            {
                id = value;
                OnPropertyChanged("Id");
            }
        }
     }
}