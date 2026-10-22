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
    public partial class MainForm : Form
    {
        WorkDB db = new WorkDB();
        private bool isRemember = false;
        public MainForm()
        {
            InitializeComponent();
            MainGrid.DataSource = db.GetVacations();
            WorkerCombo.DataSource = db.GetEmp();
            WorkerCombo.DisplayMember = "employee";
            VacancyCombo.DataSource = db.GetDep();
            VacancyCombo.DisplayMember = "name";
        }

        private void btnAuth_Click(object sender, EventArgs e)
        {
            if (isRemember == true)
            {
                FormManagement formManagement = new FormManagement();
                formManagement.ShowDialog();
            }
            else
            {
                FormAuth formAuth = new FormAuth();

                if (formAuth.ShowDialog() == DialogResult.OK)
                {
                    isRemember = true;
                }
            }
        }
    }
}
