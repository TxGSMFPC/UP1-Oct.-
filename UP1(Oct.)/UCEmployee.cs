using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace UP1_Oct._
{
    public partial class UCEmployee : UserControl
    {
        WorkDB db = new WorkDB();
        public UCEmployee()
        {
            InitializeComponent();
            UpdateUC();
        }

        private void UpdateUC()
        {
            DGEmp.DataSource = db.GetFullEmp();
            dateTimePicker1.MaxDate = DateTime.Today;
            comboBox3.DataSource = new[] { new { Name = "Active" }, new { Name = "Vacation" } };
            comboBox3.DisplayMember = "Name";
            comboBox3.ValueMember = "Name";
            comboBox2.DataSource = db.GetDepID();
            comboBox2.ValueMember = "department_id";
            comboBox2.DisplayMember = "name";
            button3.Hide();
        }

        public void Redact(bool redact)
        {
            // 1. Скрываем/показываем нужные кнопки сохранения
            button1.Visible = !redact;
            button3.Visible = redact;

            if (redact)
            {
                // Проверяем, выбрана ли вообще строка в таблице DGEmp, чтобы избежать краша
                if (DGEmp.CurrentRow != null && DGEmp.CurrentRow.DataBoundItem != null)
                {
                    DGEmp.Enabled = false;
                    // Извлекаем текущую строку из таблицы в формате DataRowView
                    var row = (System.Data.DataRowView)DGEmp.CurrentRow.DataBoundItem;

                    string lastName = row["last_name"].ToString().Trim();
                    string firstName = row["first_name"].ToString().Trim();
                    string middleName = row["middle_name"].ToString().Trim();

                    textBox1.Text = $"{lastName} {firstName} {middleName}".Replace("  ", " ").Trim();
                    textBox2.Text = row["email"].ToString();
                    maskedTextBox1.Text = row["phone"].ToString();

                    // Конвертируем значение даты в формат DateTime для календаря
                    dateTimePicker1.Value = Convert.ToDateTime(row["hire_date"]);

                    // Устанавливаем ID
                    comboBox2.SelectedValue = row["department_id"];
                    comboBox3.SelectedValue = row["status"];
                }
                else
                {
                    MessageBox.Show("Пожалуйста, сначала выберите строку в таблице сотрудников.");
                }
            }
            else
            {
                textBox1.Clear();
                textBox2.Clear();
                maskedTextBox1.Clear();
                button1.Visible = !redact;
                button3.Visible = redact;
            }
        }


        private void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateUC();
        }

        private void btn_AddEmp_Click(object sender, EventArgs e)
        {

            // 1. Валидация ФИО (textBox1)
            string fioInput = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(fioInput))
            {
                MessageBox.Show("Заполните поле ФИО!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }

            string[] fioParts = fioInput.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (fioParts.Length < 2)
            {
                MessageBox.Show("Введите как минимум Фамилию и Имя через пробел!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }

            string lastName = fioParts[0];
            string firstName = fioParts[1];
            string middleName = "";

            if (fioParts.Length > 2)
            {
                middleName = string.Join(" ", fioParts, 2, fioParts.Length - 2);
            }

            // 2. Валидация Email (textBox2)
            string emailInput = textBox2.Text.Trim();
            if (string.IsNullOrWhiteSpace(emailInput))
            {
                MessageBox.Show("Заполните поле Email!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return;
            }

            // проверка структуры Email на наличие '@' и '.'
            if (!emailInput.Contains("@") || !emailInput.Contains("."))
            {
                MessageBox.Show("Введите корректный адрес электронной почты!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return;
            }

            // 3. Валидация Телефона (maskedTextBox1)
            if (!maskedTextBox1.MaskCompleted)
            {
                MessageBox.Show("Введите номер телефона полностью!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                maskedTextBox1.Focus();
                return;
            }

            // 4. Валидация Даты найма (dateTimePicker1)
            // Запрет нанимать сотрудника в далёком будущем
            if (dateTimePicker1.Value > DateTime.Today.AddDays(30))
            {
                MessageBox.Show("Дата найма не может быть более чем на месяц в будущем!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Запрет даты из прошлого века
            if (dateTimePicker1.Value.Year < 2000)
            {
                MessageBox.Show("Указана некорректная дата найма (раньше 2000 года)!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 5. Валидация Специальности (comboBox2)
            if (comboBox2.SelectedValue == null || Convert.ToInt32(comboBox2.SelectedValue) <= 0)
            {
                MessageBox.Show("Выберите специальность из списка!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox2.Focus();
                return;
            }

            // 6. Валидация Статуса (comboBox3)
            if (string.IsNullOrWhiteSpace(comboBox3.Text) || comboBox3.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите статус сотрудника из списка!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox3.Focus();
                return;
            }

            // Отправка проверенных данных в БД
            db.AddEmp(
                firstName,
                lastName,
                middleName,
                emailInput,
                maskedTextBox1.Text,
                dateTimePicker1.Value,
                Convert.ToInt32(comboBox2.SelectedValue),
                comboBox3.Text
            );

            UpdateUC();
        }

        private void btnDel_Click(object sender, EventArgs e)
        {
            if (DGEmp.CurrentRow == null || DGEmp.CurrentRow.Index < 0) return;

            var selectedRow = DGEmp.CurrentRow;

            if (selectedRow.Cells["employee_id"].Value == null) return;
            int id = Convert.ToInt32(selectedRow.Cells["employee_id"].Value);

            var confirm = MessageBox.Show("Удалить сотрудника?", "Подтверждение", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (confirm == DialogResult.Yes)
            {
                db.DelEmp(id);
                UpdateUC();
            }
        }

        private void btn_Edit_Click(object sender, EventArgs e)
        {
            Redact(true);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // 1. Валидация ФИО (textBox1)
            string fioInput = textBox1.Text.Trim();
            if (string.IsNullOrWhiteSpace(fioInput))
            {
                MessageBox.Show("Заполните поле ФИО!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }

            string[] fioParts = fioInput.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (fioParts.Length < 2)
            {
                MessageBox.Show("Введите как минимум Фамилию и Имя через пробел!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox1.Focus();
                return;
            }

            string lastName = fioParts[0];
            string firstName = fioParts[1];
            string middleName = "";

            if (fioParts.Length > 2)
            {
                middleName = string.Join(" ", fioParts, 2, fioParts.Length - 2);
            }

            // 2. Валидация Email (textBox2)
            string emailInput = textBox2.Text.Trim();
            if (string.IsNullOrWhiteSpace(emailInput))
            {
                MessageBox.Show("Заполните поле Email!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return;
            }

            // проверка структуры Email на наличие '@' и '.'
            if (!emailInput.Contains("@") || !emailInput.Contains("."))
            {
                MessageBox.Show("Введите корректный адрес электронной почты!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                textBox2.Focus();
                return;
            }

            // 3. Валидация Телефона (maskedTextBox1)
            if (!maskedTextBox1.MaskCompleted)
            {
                MessageBox.Show("Введите номер телефона полностью!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                maskedTextBox1.Focus();
                return;
            }

            // 4. Валидация Даты найма (dateTimePicker1)
            // Запрет нанимать сотрудника в далёком будущем
            if (dateTimePicker1.Value > DateTime.Today.AddDays(30))
            {
                MessageBox.Show("Дата найма не может быть более чем на месяц в будущем!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            // Запрет даты из прошлого века
            if (dateTimePicker1.Value.Year < 2000)
            {
                MessageBox.Show("Указана некорректная дата найма (раньше 2000 года)!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 5. Валидация Специальности (comboBox2)
            if (comboBox2.SelectedValue == null || Convert.ToInt32(comboBox2.SelectedValue) <= 0)
            {
                MessageBox.Show("Выберите специальность из списка!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox2.Focus();
                return;
            }

            // 6. Валидация Статуса (comboBox3)
            if (string.IsNullOrWhiteSpace(comboBox3.Text) || comboBox3.SelectedIndex == -1)
            {
                MessageBox.Show("Выберите статус сотрудника из списка!", "Ошибка валидации", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                comboBox3.Focus();
                return;
            }
            var row = (System.Data.DataRowView)DGEmp.CurrentRow.DataBoundItem;
            DGEmp.Enabled = false;
            int id = Convert.ToInt32(row["employee_id"]);

            // Отправка проверенных данных в БД
            db.EditEmp(
                id,
                firstName,
                lastName,
                middleName,
                emailInput,
                maskedTextBox1.Text,
                dateTimePicker1.Value,
                Convert.ToInt32(comboBox2.SelectedValue),
                comboBox3.Text
            );
            Redact(false);
            DGEmp.Enabled = true;
            UpdateUC();
        }
    }
}
