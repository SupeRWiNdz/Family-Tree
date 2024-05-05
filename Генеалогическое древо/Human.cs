using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace Генеалогическое_древо
{
    // Класс отдельного человека
    internal class Human
    {
        private DateTime _birthDate;    // Дата рождения
        private string _firstName;      // Имя
        private string _secondName;     // Фамилия
        private string _thirdName;      // Отчество
        private bool _gender;           // Пол
        public int index;               // Индекс (для сохранения в txt файл)

        private Human _mother = null;   // Мать
        private Human _father = null;   // Отец
        private List<Human> _children   // Дети
            = new List<Human>();

        // Конструктор со значениями по умолчанию
        public Human()
        {
            _firstName = "-";
            _secondName = "-";
            _thirdName = "";
            _gender = true;
            _birthDate = new DateTime(2000, 01, 01);
        }
        // Конструктор с фамилией, именем, полом и датой рождения
        public Human(string firstname, string secondname, bool gender, DateTime birthdate)
        {
            _firstName = firstname;
            _secondName = secondname;
            _thirdName = "";
            _gender = gender;
            _birthDate = birthdate;
        }
        public string Name          // Свойство ФИО
        { get { return _secondName + ' ' + _firstName + ' ' + _thirdName; } }
        public string FirstName     // Свойство имени
        {  get { return _firstName; } set { _firstName = value; } }
        public string SecondName    // Свойство фамилии
        {  get { return _secondName; } set { _secondName = value; } }
        public string ThirdName     // Свойство отчества
        {  get { return _thirdName; } set { _thirdName = value; } }
        public bool Gender          // Свойство пола
        {  get { return _gender; } set { _gender = value; } }
        public DateTime BirthDate   // Свойство даты рождения
        { get { return _birthDate; } set { _birthDate = value; } }
        public List<Human> Children // Свойство потомков
        { get { return _children; } set { _children = value; } }
        public Human Mother         // Свойство матери
        { get { return _mother; } set { _mother = value; } }
        public Human Father         // Свойство отца
        { get { return _father; } set { _father = value; } }
    }
}
