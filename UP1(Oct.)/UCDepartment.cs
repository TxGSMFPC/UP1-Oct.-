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
    public partial class UCDepartment : UserControl
    {
        WorkDB db = new WorkDB();
        public UCDepartment()
        {
            InitializeComponent();
            DGDep.DataSource = db.GetDeps();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateUC();
        }

        private void UpdateUC()
        {
            DGDep.DataSource = db.GetDeps();
            textBox2.Clear();
            textBox1.Clear();
        }

        private void Redact(bool redact)
        {
            if (redact)
            {
                var row = (System.Data.DataRowView)DGDep.CurrentRow.DataBoundItem;
                button1.Visible = !redact;
                button3.Visible = redact;
                textBox1.Text = row["name"].ToString();
                textBox2.Text = row["description"].ToString();
            }
            else
            {
                UpdateUC();
                button1.Visible = redact;
                button3.Visible = !redact;
            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string desc = textBox2.Text;
            db.AddDep(name, desc);
            UpdateUC();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Redact(true);

        }

        private void button3_Click(object sender, EventArgs e)
        {

        }
    }
}
