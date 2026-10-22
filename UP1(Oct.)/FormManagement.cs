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
    public partial class FormManagement : Form
    {
        public FormManagement()
        {
            InitializeComponent();
        }

        private void btnAddEmp_Click(object sender, EventArgs e)
        {
            UCEmployee ucemp = new UCEmployee();
            panel1.Controls.Clear();
            panel1.Controls.Add(ucemp);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            UCDepartment ucdep = new UCDepartment();
            panel1.Controls.Clear();
            panel1.Controls.Add(ucdep);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            UCVac ucvac = new UCVac();
            panel1.Controls.Clear();
            panel1.Controls.Add(ucvac);
        }
    }
}
