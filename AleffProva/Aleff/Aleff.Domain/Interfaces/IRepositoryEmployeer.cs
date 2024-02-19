using Aleff.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Aleff.Domain.Interfaces
{
  public interface IRepositoryEmployeer
  {
    Task<List<Employeer>> GetEmployeesAsync();
  }
}
