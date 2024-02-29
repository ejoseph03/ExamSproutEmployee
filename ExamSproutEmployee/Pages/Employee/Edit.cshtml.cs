using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;
using System.Globalization;

namespace EmployeeSalary.Pages.Employee
{
    public class EditModel : PageModel
    {

        public EmployeeInfo employeeInfo = new EmployeeInfo();
        public string errormessage = "";
        public string successMessage = "";


        public void OnGet()
        {
            String id = Request.Query["id"];
            //RETRIEVED DATA
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
            employeeInfo.id = Request.Form["id"];
            employeeInfo.fn = Request.Form["fn"];
            employeeInfo.bdate = Request.Form["bdate"];
            employeeInfo.tin = Request.Form["tin"];
            employeeInfo.emp_type = Request.Form["emp_type"];

            if (employeeInfo.fn.Length == 0 || employeeInfo.bdate.ToString().Length == 0 || employeeInfo.tin.Length == 0 || employeeInfo.emp_type.Length == 0)
            {
                errormessage = "All Fields are required";
                return;
            }
            //INSERT TO DATABASE
            try
            {
                string connectionString = "Server=KINGEDWARD\\LocalDB;Database=SproutExamDb;User Id=sa;Password=password@1;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    String Sqlstring = "UPDATE Employee SET FullName = @fn, Birthdate= @bdate, TIN = @tin, EmployeeTypeId = @emp_type WHERE id = @id";

                    using (SqlCommand command = new SqlCommand(Sqlstring, connection))
                    {

                        command.Parameters.AddWithValue("@fn", employeeInfo.fn);
                        command.Parameters.AddWithValue("@bdate", employeeInfo.bdate);
                        command.Parameters.AddWithValue("@tin", employeeInfo.tin);
                        command.Parameters.AddWithValue("@emp_type", employeeInfo.emp_type);
                        command.Parameters.AddWithValue("@id", employeeInfo.id);

                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex)
            {
                errormessage = ex.Message;
                return;
            }

            Response.Redirect("/Employee/Index");
        }
    }
}
