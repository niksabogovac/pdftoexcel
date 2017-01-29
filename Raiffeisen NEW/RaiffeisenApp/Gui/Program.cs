using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gui
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LoginForm loginForm = new LoginForm();
            DialogResult result = loginForm.ShowDialog();


            while (result == DialogResult.Retry)
            {
                loginForm.tbName.Clear();
                loginForm.tbPass.Clear();
                MessageBox.Show("Pogrešno ste uneli parametre za logovanje, pokušajte ponovo!");
                result = loginForm.ShowDialog();
            }

            if (result == DialogResult.OK)
            {
                Kutija5 mainForm = new Kutija5(loginForm.tbPass.Text);
                mainForm.ShowDialog();
            }
        }
    }
}
