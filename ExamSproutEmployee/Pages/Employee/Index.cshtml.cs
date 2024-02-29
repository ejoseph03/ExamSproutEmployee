using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Data.SqlClient;

namespace EmployeeSalary.Pages.Employee
{
    public class IndexModel : PageModel
    {
        public List<EmployeeInfo> listEmployees = new List<EmployeeInfo>();

        public void OnGet()
        {
            try
            {
                //CONNECTION SQL
                string connectionString = "Server=KINGEDWARD\\LocalDB;Database=SproutExamDb;User Id=sa;Password=password@1;";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();
                    //JOIN TABLE WITH EMPLOYEE TYPE
                    String sql = "SELECT e.id,FullName,Birthdate,TIN,TypeName FROM Employee e JOIN EmployeeType et ON e.EmployeeTypeId = et.Id WHERE e.isDeleted = '0'";
                    using (SqlCommand command = new SqlCommand(sql, connection))
                    {
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                EmployeeInfo employeeInfo = new EmployeeInfo();
                                employeeInfo.id = "" + reader.GetInt32(0);
                                employeeInfo.fn = reader.GetString(1).ToString();
                                employeeInfo.bdate = reader.GetDateTime(2).ToString();
                                employeeInfo.tin = reader.GetString(3).ToString();
                                employeeInfo.emp_type = reader.GetString(4);

                                listEmployees.Add(employeeInfo);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception:" + ex.ToString());
            }
        }
    }

    public class EmployeeInfo
    {
        public String id;
        public string fn;
        public string bdate;
        public string tin;
        public string emp_type;

    }
}
