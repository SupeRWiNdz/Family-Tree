using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Генеалогическое_древо
{
    internal class HumanController
    {
        public List<Human> people;
        public PeopleForm peopleForm;
        public HumanController()
        {
            people = new List<Human>();
            peopleForm = new PeopleForm(people, false);
        }
    }
}
