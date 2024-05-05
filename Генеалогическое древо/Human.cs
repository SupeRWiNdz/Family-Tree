using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.AxHost;

namespace Генеалогическое_древо
{
    internal class Human
    {
        private DateTime _birthDate;
        private string _firstName;
        private string _secondName;
        private string _thirdName;
        private bool _gender;
        public int index;

        private Human _mother = null;
        private Human _father = null;
        private List<Human> _children = new List<Human>();

        public Human()
        {
            _firstName = "";
            _secondName = "";
            _thirdName = "";
            _gender = true;
            _birthDate = new DateTime(2000, 01, 01);
        }
        public Human(string firstname, string secondname, bool gender, DateTime birthdate)
        {
            _firstName = firstname;
            _secondName = secondname;
            _thirdName = "";
            _gender = gender;
            _birthDate = birthdate;
        }
        public string Name
        { get { return _secondName + ' ' + _firstName + ' ' + _thirdName; } }
        public string FirstName
        {  get { return _firstName; } set { _firstName = value; } }
        public string SecondName
        {  get { return _secondName; } set { _secondName = value; } }
        public string ThirdName
        {  get { return _thirdName; } set { _thirdName = value; } }
        public bool Gender
        {  get { return _gender; } set { _gender = value; } }
        public DateTime BirthDate
        { get { return _birthDate; } set { _birthDate = value; } }
        public List<Human> Children
        { get { return _children; } set { _children = value; } }
        public Human Mother
        { get { return _mother; } set { _mother = value; } }
        public Human Father
        { get { return _father; } set { _father = value; } }
    }
}
