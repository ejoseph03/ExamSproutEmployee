using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace EmployeeSalary.Pages.Employee
{
    public class calculatesalaryModel : PageModel
    {
        public EmployeeInfo employeeInfo = new EmployeeInfo();
        public string valueregular = "";
        public string valuecontractual = "";
        public string empType;
        public void OnGet()
        {
            String id = Request.Query["id"];

            try
            {
                string connectionString = "Server=KINGEDWARD\\LocalDB;Database=SproutExamDb;User Id=sa;Password=password@1;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sql = "SELECT e.id,FullName,Birthdate,TIN,TypeName FROM Employee e JOIN EmployeeType et ON e.EmployeeTypeId = et.Id WHERE e.id = @id";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                employeeInfo.id = "" + reader.GetInt32(0);
                                employeeInfo.fn = reader.GetString(1).ToString();
                                employeeInfo.bdate = reader.GetDateTime(2).ToString("yyyy-MM-dd");
                                employeeInfo.tin = reader.GetString(3).ToString();
                                employeeInfo.emp_type = reader.GetString(4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }

        public void OnPost()
        {
            String id = Request.Query["id"];

            try
            {
                string connectionString = "Server=KINGEDWARD\\LocalDB;Database=SproutExamDb;User Id=sa;Password=password@1;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    String sql = "SELECT e.id,FullName,Birthdate,TIN,TypeName FROM Employee e JOIN EmployeeType et ON e.EmployeeTypeId = et.Id WHERE e.id = @id and isDeleted = '0'";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@id", id);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {

                                employeeInfo.id = "" + reader.GetInt32(0);
                                employeeInfo.fn = reader.GetString(1).ToString();
                                employeeInfo.bdate = reader.GetDateTime(2).ToString("yyyy-MM-dd");
                                employeeInfo.tin = reader.GetString(3).ToString();
                                employeeInfo.emp_type = reader.GetString(4);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }

            string empT = Request.Form["emp_type"];

            if (empT == "Regular")
            {
                string salary = Request.Form["salary"];
                string tax = "0.12";
                string noAbsent = Request.Form["aDays"];
                decimal calculate1;
                decimal totalAbsent = Convert.ToDecimal(noAbsent) + 21;
                calculate1 = Convert.ToDecimal(salary) - (Convert.ToDecimal(salary) / totalAbsent) - (Convert.ToDecimal(salary) * Convert.ToDecimal(tax));

                valueregular = "Your Net Income : " + Math.Round(calculate1, 2).ToString();

            }
            else
            {
                string wDays = Request.Form["wDays"];
                decimal calculate1 = 500 * Convert.ToDecimal(wDays);
                valuecontractual = "Your Net Income : " + Math.Round(calculate1, 2).ToString();
            }
            
        }
    }
}
