using Aleff.Data.Context;
using Aleff.Domain.Entities;
using Aleff.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Aleff.Data.Repositories
{

  public class EmployeerRepository : IRepositoryEmployeer
  {
    private IAgentService _agentService;

    public EmployeerRepository(IAgentService agentService)
    {
      _agentService = agentService;
    }

    public async Task<List<Employeer>> GetEmployeesAsync()
    {
      List<Employeer> list = new List<Employeer>();
      try
      {
        var retorno = await _agentService.GetEmployeesAsync(new CancellationToken());
        foreach (var emp in retorno.Data)
        {
          var e = new Employeer();
          e.Id = emp.Id;
          e.Name = emp.EmployeeName;
          e.Age = emp.EmployeeAge;
          list.Add(e);
        }
      }
      catch (Exception e)
      {
        throw e;
      }
      return list;
    }
  }

}

