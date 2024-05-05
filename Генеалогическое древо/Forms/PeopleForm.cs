using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Генеалогическое_древо
{
    public partial class PeopleForm : Form
    {
        // Список отображаемых людей
        List<Human> _people;    
        // Выбранный пользователем человек
        internal Human _chosen = null;
        // Выбранный индекс человека
        int SelectedIndex = -1;
        // Режим работы (основная форма - false, локальная форма - true)
        bool _mode;
        // Необходимо ли подтверждение перед закрытием программы
        bool _confirm = false;
        
        // Конструктор формы
        internal PeopleForm(List<Human> people, bool mode)
        {
            InitializeComponent();
            _people = people;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _people.GetRange(0, _people.Count);
            _mode = mode;

            // В случае локального списка, кнопки '+', '-', 'Файл' будут отключены
            if (_mode)
            {
                файлToolStripMenuItem.Enabled = false;
                button1.Enabled = false;
                button2.Enabled = false;
                файлToolStripMenuItem.Visible = false;
                button1.Visible = false;
                button2.Visible = false;
            }
        }

        private void PeopleForm_Load(object sender, EventArgs e)
        {

        }

        // Двойное нажатие на человека в списке
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // Защита от сбоев
            if (e.RowIndex == -1 || _people.Count == 0)
                return;

            // Обработка в соответствии с выбранным режимом
            if (_mode)
            {
                // Локальная форма - выбор человека
                _chosen = _people[e.RowIndex];
                Close();
            }
            else
            {
                // Основная форма - открытие формы с информацией
                // о человеке с возможностью её редактирования
                Hide();
                HumanForm humanForm = new HumanForm(_people, e.RowIndex);
                humanForm.ShowDialog();
                _confirm = true;
                Show();
            }
        }

        // Кнопка '+' - добавление нового человека в общий список
        private void button2_Click(object sender, EventArgs e)
        {
            Human newHuman = new Human();
            EditForm editForm = new EditForm(newHuman);
            editForm.ShowDialog();
            if (newHuman.FirstName != "")
            {
                _people.Add(newHuman);
                _confirm = true;
            }
                dataGridView1.DataSource = null;
            dataGridView1.DataSource = _people;
        }

        // Кнопка '-' - удаление человека из общего списка
        private void button1_Click(object sender, EventArgs e)
        {
            if (_people.Count == 0)
                return;

            foreach (Human child in _people[SelectedIndex].Children)
            {
                if (_people[SelectedIndex].Gender) child.Father = null;
                else child.Mother = null;
            }
            if (_people[SelectedIndex].Mother!=null)
                _people[SelectedIndex].Mother.Children.Remove(_people[SelectedIndex]);
            if (_people[SelectedIndex].Father != null)
                _people[SelectedIndex].Father.Children.Remove(_people[SelectedIndex]);

            _people.RemoveAt(SelectedIndex);

            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _people.GetRange(0, _people.Count);
            _confirm = true;
        }

        // Обновление выбранного индекса после нажатия на человека
        private void dataGridView1_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex == -1)
                return;
            SelectedIndex = e.RowIndex;
        }

        // Сохранение в txt файл
        private void сохранитьКакToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                // Вызов окна сохранения на компьютере
                SaveFileDialog saveFileDialog1 = new SaveFileDialog();
                saveFileDialog1.Filter = "(*.txt)|*.txt";
                saveFileDialog1.Title = "Сохранить как TXT файл";
                saveFileDialog1.ShowDialog();

                if (saveFileDialog1.FileName == "")
                    return;

                System.IO.FileStream fs = (System.IO.FileStream)saveFileDialog1.OpenFile();
                TextWriter txt = new StreamWriter(fs);

                // Присвоение индексов каждому человеку
                for (int i = 0; i < _people.Count; i++)
                    _people[i].index = i;

                // Сохранение информации о каждом человеке
                for (int i = 0; i < _people.Count; i++)
                {
                    txt.Write(_people[i].FirstName);
                    txt.Write('_' + _people[i].SecondName);
                    txt.Write('_' + _people[i].ThirdName);
                    txt.Write('_' + ((_people[i].Gender) ? "1" : "0"));
                    txt.Write('_' + _people[i].BirthDate.ToString("dd_MM_yyyy"));
                    // Сохранение индексов детей в конце строки
                    for (int j = 0; j < _people[i].Children.Count; j++)
                        txt.Write('_' + _people[i].Children[j].index.ToString());
                    txt.Write('\n');
                }

                txt.Close();
                fs.Close();
                _confirm = false;
            }
            catch
            {
                // В случае некорректного файла
                // обрабатывается исключение
                MessageBox.Show("Ошибка записи!");
            }
        }

        // Открытие txt файла
        private void открытьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!confirm())
                return;

            // Данные сохраняются для возможности восстановления
            List<Human> peopleBack = new List<Human>(_people);
            Human chosenBack = _chosen;
            int indexBack = SelectedIndex;

            try
            {
                // Вызов окна для выбора txt файла на компьютере
                OpenFileDialog openFileDialog1 = new OpenFileDialog();
                openFileDialog1.Filter = "(*.txt)|*.txt";
                openFileDialog1.Title = "Открыть TXT файл";

                if (openFileDialog1.ShowDialog() != DialogResult.OK)
                    return;

                var fileStream = openFileDialog1.OpenFile();
                StreamReader reader = new StreamReader(fileStream);
                string fileContent = reader.ReadToEnd();
                reader.Close();

                _people.Clear();
                _chosen = null;
                SelectedIndex = -1;

                // Текст из выбранного файла делится
                // переносом строки и символом '_'
                string[] separatorL = new string[] { "\n" };
                string[] separatorW = new string[] { "_" };
                foreach (string line in fileContent.Split(separatorL, StringSplitOptions.RemoveEmptyEntries))
                {
                    // Для каждой строки создаётся новый человек
                    // с соответствующими этой строке данными
                    Human newHuman = new Human();
                    _people.Add(newHuman);
                    string[] strings = line.Split(separatorW, StringSplitOptions.None);
                    newHuman.FirstName = (strings[0] == "") ? "-" : strings[0];
                    newHuman.SecondName = (strings[1] == "") ? "-" : strings[1];
                    newHuman.ThirdName = strings[2];
                    newHuman.Gender = (strings[3] == "0") ? false : true;

                    string bdate = strings[4] + '.' + strings[5] + '.' + strings[6];

                    DateTime dt;
                    if ((DateTime.TryParseExact(bdate,
                        "dd.MM.yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                        && (dt <= DateTime.Now))
                        newHuman.BirthDate = dt;
                }

                // Вычисляются индексы всех людей
                for (int i = 0; i < _people.Count; i++)
                    _people[i].index = i;

                // Для каждого человека добавляются потомки/родители
                // согласно индексам из файла
                int index = 0;
                foreach (string line in fileContent.Split(separatorL, StringSplitOptions.RemoveEmptyEntries))
                {
                    string[] strings = line.Split(separatorW, StringSplitOptions.None);
                    for (int i = 7; i < strings.Length; i++)
                    {
                        int number;
                        if (int.TryParse(strings[i], out number))
                        {
                            _people[index].Children.Add(_people[number]);
                            if (_people[index].Gender)
                            {
                                if (_people[number].Father != null)
                                    _people[number].Father.Children.Remove(_people[number]);
                                _people[number].Father = _people[index];
                            }
                            else
                            {
                                if (_people[number].Mother != null)
                                    _people[number].Mother.Children.Remove(_people[number]);
                                _people[number].Mother = _people[index];
                            }

                        }
                    }

                    _people[index].Children.Remove(_people[index]);

                    if (_people[index].Children.Contains(_people[index].Mother))
                    {
                        if (_people[index].Gender == true)
                            _people[index].Mother.Father = null;
                        else
                            _people[index].Mother.Mother = null;
                        _people[index].Children.Remove(_people[index].Mother);
                    }
                    if (_people[index].Children.Contains(_people[index].Father))
                    {
                        if (_people[index].Gender == true)
                            _people[index].Father.Father = null;
                        else
                            _people[index].Father.Mother = null;
                        _people[index].Children.Remove(_people[index].Father);
                    }
                    if (_people[index].Mother == _people[index]) _people[index].Mother = null;
                    if (_people[index].Father == _people[index]) _people[index].Father = null;
                    index++;
                }

                dataGridView1.DataSource = null;
                dataGridView1.DataSource = _people.GetRange(0, _people.Count);
                _confirm = false;
            }
            catch
            {
                // В случае ошибки обрабатывается
                // исключение и данные восстанавливаются
                _people = peopleBack;
                _chosen = chosenBack;
                SelectedIndex = indexBack;
                dataGridView1.DataSource = null;
                dataGridView1.DataSource = _people.GetRange(0, _people.Count);
                _confirm = true;
                MessageBox.Show("Ошибка чтения!");
            }
        }

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {

        }

        // Сброс всех данных (удаление общего списка людей)
        private void сбросToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!confirm())
                return;
            _people.Clear();
            _chosen = null;
            SelectedIndex = -1;
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = _people.GetRange(0, _people.Count);
            _confirm = false;
        }

        // Если есть несохранённые данные у пользователя спрашивается подтверждение
        // перед сбросом/открытием нового файла/закрытием программы
        private bool confirm()
        {
            if (_confirm==false)
                return true;

            var confirmResult = MessageBox.Show("Несохранённые данные будут утеряны!", "Вы уверены?",
                                     MessageBoxButtons.YesNo);
            return (confirmResult == DialogResult.Yes)? true : false;
        }

        // Закрытие программы
        private void PeopleForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = (confirm())? false : true;
        }
    }
}
