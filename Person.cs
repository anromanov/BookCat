using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BookCat
{
    //Класс для реализации списка авторов
    internal class Person : INotifyPropertyChanged
    {
        protected int id = -1;//Уникальный идентификатор
        protected string firstName;//Имя
        protected string middleName;//Отчество
        protected string lastName;//Фамилия

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
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
        public string ShortName => firstName[0] + ". " + middleName[0] + ". " + lastName; 
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