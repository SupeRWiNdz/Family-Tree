using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Генеалогическое_древо
{
    internal class HumanController
    {
        // Список всех членов дерева семьи
        public List<Human> people;
        // Основная форма
        public PeopleForm peopleForm;

        // Конструктор контроллера
        public HumanController()
        {
            people = new List<Human>();
            peopleForm = new PeopleForm(people, false);
        }
    }
}
