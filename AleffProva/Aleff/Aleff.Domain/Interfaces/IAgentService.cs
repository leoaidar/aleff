using Aleff.Domain.DTO;
using System.Threading;
using System.Threading.Tasks;

namespace Aleff.Domain.Interfaces
{
  public interface IAgentService
  {
    Task<DTODummyEmployees> GetEmployeesAsync(CancellationToken cancellationToken);
  }
}
