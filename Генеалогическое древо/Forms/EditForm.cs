using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Генеалогическое_древо
{
    public partial class EditForm : Form
    {
        // Объект редактируемого человека
        Human _human;

        string _firstName;      // Введённое имя
        string _secondName;     // Введённая фамилия
        string _thirdName;      // Введённое отчество
        DateTime _birthDate;    // Введённая дата рождения

        // Конструктор формы
        internal EditForm(Human human)
        {
            InitializeComponent();
            _human = human;

            _firstName = _human.FirstName;
            _secondName = _human.SecondName;
            _thirdName = _human.ThirdName;
            _birthDate = _human.BirthDate;
            textBox1.Text = _secondName;
            textBox2.Text = _firstName;
            textBox3.Text = _thirdName;

            textBox4.Text = _birthDate.ToString("dd.MM.yyyy");

            if (_human.Gender) radioButton1.Checked = true;
            else radioButton2.Checked = true;
        }
        private void EditForm_Load(object sender, EventArgs e)
        {

        }

        // Изменение введённого имени
        private void textBox1_Enter(object sender, EventArgs e)
        {
            textBox1.Text = _secondName;
        }

        // Изменение введённой фамилии
        private void textBox2_Enter(object sender, EventArgs e)
        {
            textBox2.Text = _firstName;
        }

        // Изменение введённого отчества
        private void textBox3_Enter(object sender, EventArgs e)
        {
            textBox3.Text = _thirdName;
        }

        // Изменение введённой даты рождения
        private void textBox4_Enter(object sender, EventArgs e)
        {
            textBox4.Text = _birthDate.ToString("dd.MM.yyyy");
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            _secondName = textBox1.Text;
            if (_secondName=="")
                textBox1.Text = _human.SecondName;
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            _firstName = textBox2.Text;
            if (_firstName=="")
                textBox2.Text = _human.FirstName;
        }

        private void textBox3_Leave(object sender, EventArgs e)
        {
            _thirdName = textBox3.Text;
            if (_thirdName == "")
                textBox3.Text = _human.ThirdName;
        }

        // Проверка корректности ввода даты рождения
        private void textBox4_Leave(object sender, EventArgs e)
        {
            DateTime dt;
            if (DateTime.TryParseExact(textBox4.Text, "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                if (dt > DateTime.Now)
                {
                    MessageBox.Show("Введите дату не в будущем!");
                    textBox4.Text = _birthDate.ToString("dd.MM.yyyy");
                    return;
                }
                _birthDate = dt;
                textBox4.Text = dt.ToString("dd.MM.yyyy");
            }
            else
            {
                MessageBox.Show("Введите дату в формате: '" + DateTime.Now.ToString("dd.MM.yyyy") + "'");
                textBox4.Text = _birthDate.ToString("dd.MM.yyyy");
            }
        }

        // Подтверждение ввода
        private void button1_Click(object sender, EventArgs e)
        {
            if (_firstName == "" || _secondName == "")
            {
                MessageBox.Show("Ввведите фамилию и имя!");
                return;
            }
            _human.FirstName = _firstName.Replace('_', ' ');
            _human.SecondName = _secondName.Replace('_', ' ');
            _human.ThirdName = _thirdName.Replace('_', ' ');
            if (_human.Gender != radioButton1.Checked)
            {
                foreach (Human child in _human.Children)
                {
                    if (_human.Gender) child.Father = null;
                    else child.Mother = null;
                }
                _human.Children.Clear();
            }
            _human.Gender = radioButton1.Checked;
            _human.BirthDate = _birthDate;
            Close();
        }
    }
}
