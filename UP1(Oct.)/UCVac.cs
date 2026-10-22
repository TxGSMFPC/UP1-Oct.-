using Google.Protobuf.WellKnownTypes;
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
            dataGridView1.DataSource = db.GetFullVac();
            comboBox1.DataSource = db.GetFullEmp();
            comboBox1.ValueMember = "employee_id";
            comboBox1.DisplayMember = "full_name";
            comboBox3.DataSource = new[] { new { status = "Утверждён" }, new { status = "На рассмотрении" }, new { status = "Отказано" }  };
            comboBox3.DisplayMember = "status";
            comboBox3.ValueMember = "status";
            comboBox2.DataSource = new[] { new { type = "Оплачиваемый" }, new { type = "Неоплачиваемый" } };
            comboBox2.DisplayMember = "type";
            comboBox2.ValueMember = "type";
            button3.Hide();
        }

        private void UpdateUC()
        {
            dataGridView1.DataSource = db.GetFullVac();
            dateTimePicker1.Value = DateTime.Today;
            dateTimePicker3.Value = DateTime.Today;
        }
        private void Redact(bool redact)
        {
            if (redact)
            {
                dataGridView1.Enabled = false;
                button1.Visible = !redact;
                button3.Visible = redact;
            }
            else
            {
                button1.Visible = redact;
                button3.Visible = !redact;
                UpdateUC();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            var row = (System.Data.DataRowView)dataGridView1.CurrentRow.DataBoundItem;
            int employee_id = Convert.ToInt32(comboBox1.SelectedValue);
            int id = Convert.ToInt32(row["vacation_id"]);
            db.EditVac(id, employee_id, dateTimePicker3.Value, dateTimePicker1.Value, comboBox2.Text, comboBox3.Text);
            Redact(false);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var row = (System.Data.DataRowView)dataGridView1.CurrentRow.DataBoundItem;
            int employee_id = Convert.ToInt32(comboBox1.SelectedValue);
            db.AddVacation(employee_id, dateTimePicker3.Value, dateTimePicker1.Value, comboBox2.Text, comboBox3.Text);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Redact(true);
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateUC();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            var row = (System.Data.DataRowView)dataGridView1.CurrentRow.DataBoundItem;
            int id = Convert.ToInt32(row["vacation_id"]);
            db.DelVac(id);
        }
    }
}
