using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace UP1_Oct._
{
    public partial class FormAuth : Form
    {
        MainForm main = new MainForm();
        public FormAuth()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if(txtLogin.Text == "admin" && txtPW.Text == "admin")
            {
                FormManagement formManagement = new FormManagement();
                formManagement.ShowDialog();
                if (checkRemember.Checked)
                {
                    this.DialogResult = DialogResult.OK;
                }
                else
                {
                    this.DialogResult = DialogResult.Yes;
                }
                this.Close();
            }
            else
            {
                MessageBox.Show("Попробуйте снова", "Неверный логин или пароль");
                txtPW.Clear();
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
