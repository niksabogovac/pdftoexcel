using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace QR_Code
{
    /// <summary>
    /// Startup class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            LoginForm loginForm = new LoginForm();
            DialogResult result = loginForm.ShowDialog();
            while (result != DialogResult.Cancel)
            {
                if (result == DialogResult.OK)
                {
                    MainForm mainForm = new MainForm(loginForm.tbName.Text, loginForm.tbPass.Text);
                    result = mainForm.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Pogrešno ste uneli parametre za logovanje, pokušajte ponovo!");
                    loginForm.ShowDialog();
                }
            }   
        }
    }
}
