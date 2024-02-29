using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace EmployeeSalary.Pages.Employee
{
    public class createModel : PageModel
    {
        public EmployeeInfo employeeInfo = new EmployeeInfo();
        public string errormessage = "";
        public string successMessage = "";


        public enum EmployeeType
        {
            Regular = 0,
            Contractual = 1
        }

        public void OnGet()
        {

        }
        public void OnPost()
        {
            employeeInfo.fn = Request.Form["fn"];
            employeeInfo.bdate = Request.Form["bdate"];
            //employeeInfo.bdate = Convert.ToDateTime(Request.Form["bdate"]);
            employeeInfo.tin = Request.Form["tin"];
            employeeInfo.emp_type = Request.Form["emp_type"];

            if (employeeInfo.fn.Length == 0 || employeeInfo.bdate.ToString() == null || employeeInfo.tin.Length == 0 || employeeInfo.emp_type.Length == 0 )
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
                    String Sqlstring = "INSERT INTO Employee(FullName,Birthdate,TIN,EmployeeTypeId) VALUES(@fn,@bdate,@tin,@emp_type)";

                    using (SqlCommand command = new SqlCommand(Sqlstring, connection))
                    {
                        command.Parameters.AddWithValue("@fn", employeeInfo.fn);
                        command.Parameters.AddWithValue("@bdate", employeeInfo.bdate);
                        command.Parameters.AddWithValue("@tin", employeeInfo.tin);
                        command.Parameters.AddWithValue("@emp_type", employeeInfo.emp_type);

                        command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception ex) 
            {
                errormessage = ex.Message;
                    return;
            }


            successMessage = "New Client Added";

            Response.Redirect("/Employee/Index");


        }

    }
}
