﻿using BusinessLogic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gui
{
    public partial class ReportDialog : Form
    {
        /// <summary>
        /// Regex for box codes.
        /// </summary>
        private Regex boxCodeRegex = new Regex(@"(RSRFBA)[0-9]{2}C-[0-9]{5}");

        /// <summary>
        /// Regex for date report.
        /// </summary>
        private Regex dateRegex = new Regex(@"[0-9]{1,2}\.[0-9]{1,2}\.[0-9]{4}\.-[0-9]{1,2}\.[0-9]{1,2}\.[0-9]{4}\.");

        private readonly int BOX_CODE_LENGTH = 15;

        public ReportDialog()
        {
            InitializeComponent();
        }

        private void button1Click(object sender, EventArgs e)
        {

            if (rbBoxCode.Checked && !rbOrderNum.Checked && !rbDateTime.Checked)
            {
                if (boxCodeRegex.IsMatch(tbValue.Text) && tbValue.Text.Length == BOX_CODE_LENGTH)
                {
                    if (ReportManager.GenerateReport(tbValue.Text, null))
                    {
                        MessageBox.Show("Uspešno generisan izveštaj!");
                    }
                    else
                    {
                        MessageBox.Show("Neuspešno generisan izveštaj!");
                    }
                }
                else
                {
                    MessageBox.Show("Nije unet dobar kod kutije!");
                }
            }
            else if (!rbBoxCode.Checked && rbOrderNum.Checked && !rbDateTime.Checked)
            {
                int tmp;
                if (Int32.TryParse(tbValue.Text, out tmp))
                {
                    if (ReportManager.GenerateReport(null, tbValue.Text))
                    {
                        MessageBox.Show("Uspešno generisan izveštaj!");
                    }
                    else
                    {
                        MessageBox.Show("Neuspešno generisan izveštaj!");
                    }
                }
                else
                {
                    MessageBox.Show("Nije unet dobar file number!");
                }
            }
            else if (!rbBoxCode.Checked && !rbOrderNum.Checked && rbDateTime.Checked)
            {
                DateTime start = DateTime.MinValue;
                DateTime stop = DateTime.MinValue;

                if (dateRegex.IsMatch(tbValue.Text))
                {
                  
                    try
                    {
                        // date time is in format 
                        // dd.mm.yyyy.-dd.mm.yyyy.
                        string[] items = tbValue.Text.Split(new char[] { '-' });
                        if (!DateTime.TryParse(items[0], out start))
                        {
                            MessageBox.Show("Nije unet dobar datum!");
                            return;
                        }

                        if (!DateTime.TryParse(items[1], out stop))
                        {
                            MessageBox.Show("Nije unet dobar datum!");
                            return;
                        }

                        if (ReportManager.GenerateReport(start, stop))
                        {
                            MessageBox.Show("Uspešno generisan izveštaj!");
                        }
                        else
                        {
                            MessageBox.Show("Neuspešno generisan izveštaj!");
                        }
                    }
                    catch(Exception)
                    {
                        MessageBox.Show("Nije unet dobar datum!");
                    }
                }
                else
                {
                    MessageBox.Show("Nije unet dobar datum!");
                }
            }
            else
            {
                MessageBox.Show("Izaberite tip izveštaja!");
            }
        }
    }
}
