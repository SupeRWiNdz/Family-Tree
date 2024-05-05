using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Генеалогическое_древо
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Создание объекта контроллера
            HumanController humanController = new HumanController();
            // Запуск основной формы
            Application.Run(humanController.peopleForm);
        }
    }
}
