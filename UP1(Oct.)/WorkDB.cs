using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace UP1_Oct._
{
    internal class WorkDB
    {
        string connectionString = "Server=localhost;Port=3306;Database=company_vacations;Uid=root;Pwd=root;";
        //кщще
        public DataTable GetEmp()
        {
            DataTable dt = new DataTable();
            string query = "SELECT CONCAT(last_name, ' ', first_name, ' ', middle_name, '') AS employee FROM employee";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при получении сотрудников: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return dt; 
        }

        public DataTable GetFullEmp()
        {
            DataTable dt = new DataTable();
            string query = "SELECT employee_id, first_name, last_name, middle_name, email, phone, hire_date, department_id, status, CONCAT(first_name, ' ', last_name, ' ', middle_name, ' ') AS full_name FROM employee";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при получении сотрудников: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return dt;
        }

        public DataTable GetDep()
        {
            DataTable dt = new DataTable();
            string query = "SELECT name FROM department";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при получении должностей: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return dt;
        }

        public DataTable GetDepID()
        {
            DataTable dt = new DataTable();
            string query = "SELECT department_id, name FROM department";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при получении должностей: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return dt;
        }

        public DataTable GetDeps()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM department";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при получении должностей: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return dt;
        }

        public DataTable GetVacations()
        {
            DataTable dataTable = new DataTable();

            string query = @"
        SELECT 
            CONCAT(e.last_name, ' ', e.first_name, ' ', IFNULL(e.middle_name, '')) AS 'Сотрудник',
            d.name AS 'Отдел',
            CONCAT(DATE_FORMAT(v.start_date, '%d.%m.%Y'), ' - ', DATE_FORMAT(v.end_date, '%d.%m.%Y')) AS 'Период отпуска',
            v.vacation_type AS 'Тип отпуска',
            v.status AS 'Статус',
            DATE_FORMAT(v.created_at, '%d.%m.%Y %H:%i') AS 'Дата создания'
        FROM vacation v
        INNER JOIN employee e ON v.employee_id = e.employee_id
        INNER JOIN department d ON e.department_id = d.department_id;";


            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                    {
                        adapter.Fill(dataTable);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при получении отпусков: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return dataTable;
        }

        public void AddEmp(string fname, string lname, string mname, string email, string phone, DateTime hdate, int dep_id, string status)
        {
            string query = @"INSERT INTO employee (first_name, last_name, middle_name, email, phone, hire_date, department_id, status) 
                        VALUES (@fname, @lname, @mname, @email, @phone, @hdate, @dep_id, @status)";
            using (MySqlConnection connection = new MySqlConnection(this.connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@fname", fname);
                    command.Parameters.AddWithValue("@lname", lname);
                    command.Parameters.AddWithValue("@mname", string.IsNullOrEmpty(mname) ? (object)DBNull.Value : mname);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@phone", phone);
                    command.Parameters.AddWithValue("@hdate", hdate);
                    command.Parameters.AddWithValue("@dep_id", dep_id);
                    command.Parameters.AddWithValue("@status", status);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при добавлении в БД: " + ex.Message);
                    }
                }
            }
        }

        public void EditEmp(int id, string fname, string lname, string mname, string email, string phone, DateTime hdate, int dep_id, string status)
        {

            string query = @"UPDATE employee 
                     SET first_name = @fname, 
                         last_name = @lname, 
                         middle_name = @mname, 
                         email = @email, 
                         phone = @phone, 
                         hire_date = @hdate, 
                         department_id = @dep_id, 
                         status = @status 
                     WHERE employee_id = @id";

            using (MySqlConnection connection = new MySqlConnection(this.connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@id", id);


                    command.Parameters.AddWithValue("@fname", fname);
                    command.Parameters.AddWithValue("@lname", lname);
                    command.Parameters.AddWithValue("@mname", string.IsNullOrEmpty(mname) ? (object)DBNull.Value : mname);
                    command.Parameters.AddWithValue("@email", email);
                    command.Parameters.AddWithValue("@phone", phone);
                    command.Parameters.AddWithValue("@hdate", hdate);
                    command.Parameters.AddWithValue("@dep_id", dep_id);
                    command.Parameters.AddWithValue("@status", status);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Данные сотрудника успешно обновлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Сотрудник для обновления не найден.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при обновлении в БД: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public void DelEmp(int id)
        {
            string query = "DELETE FROM employee WHERE employee_id = @id";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Сотрудник успешно удален.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Сотрудник с таким ID не найден в базе данных.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении сотрудника: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        public void AddDep(string name, string desc)
        {
            string query = "INSERT INTO department (name, description) VALUES (@name, @desc)";
            using (MySqlConnection connection = new MySqlConnection(this.connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@desc", desc);

                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при добавлении в БД: " + ex.Message);
                    }
                }
            }
        }

        public void DelDep(int id)
        {
            string query = "DELETE FROM department WHERE department_id = @id";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Специальность успешно удалена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Специальность с таким ID не найдена в базе данных.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении специальности: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void EditDep(int id, string name, string desc)
        {
            string query = "UPDATE department SET name = @name, description = @desc WHERE department_id = @id";
            using (MySqlConnection connection = new MySqlConnection(this.connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@id", id);


                    command.Parameters.AddWithValue("@name", name);
                    command.Parameters.AddWithValue("@desc", desc);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Данные отдела успешно обновлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Отдел для обновления не найден.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при обновлении в БД: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public void AddVacation(int employeeId, DateTime startDate, DateTime endDate, string vacationType, string status)
        {

            string query = "INSERT INTO vacation (employee_id, start_date, end_date, vacation_type, status, created_at) " +
                           "VALUES (@employeeId, @startDate, @endDate, @vacationType, @status, NOW())";

            using (MySqlConnection connection = new MySqlConnection(this.connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {

                    command.Parameters.AddWithValue("@employeeId", employeeId);
                    command.Parameters.AddWithValue("@startDate", startDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@endDate", endDate.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@vacationType", vacationType);
                    command.Parameters.AddWithValue("@status", status);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Отпуск успешно добавлен.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при добавлении отпуска в БД: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        public DataTable GetVacationsWithDepartments()
        {
            DataTable dataTable = new DataTable();
            string query = @"
        SELECT 
            v.employee_id,
            CONCAT(e.last_name, ' ', e.first_name) AS employee_name,
            e.department_id,
            d.name AS department_name,
            v.start_date,
            v.end_date
        FROM vacation v
        INNER JOIN employee e ON v.employee_id = e.employee_id
        INNER JOIN department d ON e.department_id = d.department_id";

            using (MySqlConnection connection = new MySqlConnection(this.connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    try
                    {
                        connection.Open();
                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dataTable);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при получении аналитики отпусков: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            return dataTable;
        }

        public void DelVac(int id)
        {
            string query = "DELETE FROM vacation WHERE vacation_id = @id";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);

                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Запись об отпуске успешно удалена.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Запись об отпуске с таким ID не найдена в базе данных.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при удалении записи об отпуске: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void EditVac(int id, int employee_id, DateTime start, DateTime end, string type, string status)
        {
            string query = "UPDATE vacation SET employee_id = @employee_id, start_date = @start, end_date = @end, vacation_type = @type, status = @status WHERE vacation_id = @id";

            using (MySqlConnection connection = new MySqlConnection(this.connectionString))
            {
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    command.Parameters.AddWithValue("@employee_id", employee_id);
                    command.Parameters.AddWithValue("@start", start.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@end", end.ToString("yyyy-MM-dd"));
                    command.Parameters.AddWithValue("@type", type);
                    command.Parameters.AddWithValue("@status", status);

                    try
                    {
                        connection.Open();
                        int rowsAffected = command.ExecuteNonQuery();

                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Данные отпуска успешно обновлены.", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show("Отпуск для обновления не найден.", "Предупреждение", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Ошибка при обновлении отпуска в БД: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        public DataTable GetFullVac()
        {
            DataTable dt = new DataTable();
            string query = "SELECT * FROM vacation";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(query, connection))
                    {
                        adapter.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при получении списка отпусков: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return dt;
        }

        public DataTable SortMain(string type)
        {
            DataTable dt = new DataTable();

            string query = @"SELECT 
                        CONCAT(e.last_name, ' ', e.first_name, ' ', IFNULL(e.middle_name, '')) AS 'Сотрудник',
                        d.name AS 'Отдел',
                        CONCAT(DATE_FORMAT(v.start_date, '%d.%m.%Y'), ' - ', DATE_FORMAT(v.end_date, '%d.%m.%Y')) AS 'Период отпуска',
                        v.vacation_type AS 'Тип отпуска',
                        v.status AS 'Статус',
                        DATE_FORMAT(v.created_at, '%d.%m.%Y %H:%i') AS 'Дата создания'
                     FROM vacation v 
                     INNER JOIN employee e ON v.employee_id = e.employee_id 
                     INNER JOIN department d ON e.department_id = d.department_id
                     WHERE v.vacation_type = @type;";

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    using (MySqlCommand command = new MySqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@type", type);

                        using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                        {
                            adapter.Fill(dt);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка при получении отпусков: " + ex.Message, "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            return dt;
        }

    }
}
