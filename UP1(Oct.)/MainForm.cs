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

        private void button1_Click(object sender, EventArgs e)
        {

            DataTable dt = db.GetVacationsWithDepartments();

            if (dt == null || dt.Rows.Count == 0)
            {
                MessageBox.Show("Нет данных об отпусках для анализа.", "Информация", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var vacations = dt.AsEnumerable().Select(row => new
            {
                EmpId = Convert.ToInt32(row["employee_id"]),
                EmpName = row["employee_name"].ToString(),
                DepId = Convert.ToInt32(row["department_id"]),
                DepName = row["department_name"].ToString(),
                Start = Convert.ToDateTime(row["start_date"]),
                End = Convert.ToDateTime(row["end_date"])
            }).ToList();

            StringBuilder report = new StringBuilder();
            int conflictCount = 0;

            for (int i = 0; i < vacations.Count; i++)
            {
                for (int j = i + 1; j < vacations.Count; j++)
                {
                    var v1 = vacations[i];
                    var v2 = vacations[j];

                    if (v1.DepId == v2.DepId && v1.EmpId != v2.EmpId)
                    {
                        if (v1.Start <= v2.End && v2.Start <= v1.End)
                        {
                            conflictCount++;
                            report.AppendLine($"⚠️ Пересечение #{conflictCount} [Отдел: {v1.DepName}]");
                            report.AppendLine($"  • {v1.EmpName} (ID: {v1.EmpId}): с {v1.Start:dd.MM.yyyy} по {v1.End:dd.MM.yyyy}");
                            report.AppendLine($"  • {v2.EmpName} (ID: {v2.EmpId}): с {v2.Start:dd.MM.yyyy} по {v2.End:dd.MM.yyyy}");
                            report.AppendLine(new string('-', 60));
                        }
                    }
                }
            }

            if (conflictCount > 0)
            {
                MessageBox.Show(report.ToString(), "Внимание! Обнаружены пересечения отпусков", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                MessageBox.Show("Отлично! Накладных расходов не обнаружено. Сотрудники из одного отдела не отдыхают одновременно.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}
