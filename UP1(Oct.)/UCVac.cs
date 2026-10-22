using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UP1_Oct._
{
    public partial class UCVac : UserControl
    {
        WorkDB db = new WorkDB();
        public UCVac()
        {
            InitializeComponent();
            dataGridView1.DataSource = db.GetVacations();
        }

        private void UpdateUC()
        {
            dataGridView1.DataSource = db.GetVacations();
            //CLEAR() сделать
        }
        private void Redact(bool redact)
        {
            if (redact)
            {
                //button1.Visible = !redact;
                //button3.Visible = redact;

                // вставвитттттьььььььььь ввв поляяяяяяяяяяяяяяяяяяяяяяяяяяяя
            }
            else
            {
                //button1.Visible = redact;
                //button3.Visible = !redact;
                //UpdateUC();
            }
        }
    }
}
