using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aleff.Domain.DTO
{
  public class DTODummyEmployees
  {
    [JsonProperty("status")]
    public string Status { get; set; }

    [JsonProperty("data")]
    public List<EmployeeDummy> Data { get; set; }

    [JsonProperty("message")]
    public string Message { get; set; }

    public class EmployeeDummy
    {

      [JsonProperty("id")]
      public int Id { get; set; }

      [JsonProperty("employee_name")]
      public string EmployeeName { get; set; }

      [JsonProperty("employee_salary")]
      public int EmployeeSalary { get; set; }

      [JsonProperty("employee_age")]
      public int EmployeeAge { get; set; }

      [JsonProperty("profile_image")]
      public string ProfileImage { get; set; }
    }
  }
}
