using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace Генеалогическое_древо
{
    public partial class HumanForm : Form
    {
        List<Human> _people;
        Human _human;
        int SelectedIndex = 0;
        internal HumanForm(List<Human> people, int index)
        {
            InitializeComponent();
            _people = people;
            _human = _people[index];
            updateInformation();
        }

        private void updateInformation()
        {
            label1.Text = "Ф: " + _human.SecondName;
            label2.Text = "И: " + _human.FirstName;
            label3.Text = "О: " + _human.ThirdName;
            label4.Text = _human.BirthDate.ToString("dd.MM.yyyy");
            label5.Text = (_human.Gender) ? "пол: мужской" : "пол: женский";

            int age = DateTime.Now.Year - _human.BirthDate.Year;
            if (DateTime.Now.DayOfYear < _human.BirthDate.DayOfYear)
                age--;
            label6.Text = "возраст: " + age + " лет";

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _human.Children.GetRange(0, _human.Children.Count);

            List<Human> parents = new List<Human>();
            parents.Add(_human.Mother);
            parents.Add(_human.Father);
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = parents.GetRange(0, parents.Count);
        }
        private void HumanForm_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Hide();
            EditForm editForm = new EditForm(_human);
            editForm.ShowDialog();
            updateInformation();
            Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            List<Human> newChildren = new List<Human>();
            for (int i = 0; i< _people.Count; i++)
            {
                if (_human.Children.Contains(_people[i]))
                    continue;
                newChildren.Add(_people[i]);
            }
            newChildren.Remove(_human);
            newChildren.Remove(_human.Mother);
            newChildren.Remove(_human.Father);

            PeopleForm newPeople = new PeopleForm(newChildren, true);

            newPeople.ShowDialog();

            if (newPeople._chosen != null)
            {
                _human.Children.Add(newPeople._chosen);
                if (_human.Gender) newPeople._chosen.Father = _human;
                else newPeople._chosen.Mother = _human;
            }
            updateInformation();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (_human.Children.Count == 0)
                return;

            if (_human.Gender) _human.Children[SelectedIndex].Father = null;
            else _human.Children[SelectedIndex].Mother = null;

            _human.Children.RemoveAt(SelectedIndex);
            updateInformation();
        }

        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            SelectedIndex = e.RowIndex;
        }

        private void dataGridView2_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            List<Human> newParent = new List<Human>();
            for (int i = 0; i < _people.Count; i++)
            {
                if (_people[i].Gender == (e.RowIndex == 0)? true : false
                    || _human.Children.Contains(_people[i]))
                    continue;
                newParent.Add(_people[i]);
            }
            newParent.Remove(_human);

            PeopleForm newPeople = new PeopleForm(newParent, true);
            newPeople.ShowDialog();

            if (newPeople._chosen != null)
            {
                newPeople._chosen.Children.Add(_human);
                if (e.RowIndex == 0)
                {
                    if (_human.Mother != null)
                        _human.Mother.Children.Remove(_human);
                    _human.Mother = newPeople._chosen;
                }
                else
                {
                    if (_human.Father != null)
                        _human.Father.Children.Remove(_human);
                    _human.Father = newPeople._chosen;
                }
            }
            updateInformation();
        }
    }
}
