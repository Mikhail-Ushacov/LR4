using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LR4
{
    internal static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
<<<<<<< HEAD
            Application.Run(new Form3());
            Application.Run(new Form6());
=======
            //Application.Run(new Form1());
            //Application.Run(new Form2());
            //Application.Run(new Form3());
            Application.Run(new Form4());
            //Application.Run(new Form6());
>>>>>>> c26998db75514c4815b30a2800100fb1075acf25
        }
    }
}
