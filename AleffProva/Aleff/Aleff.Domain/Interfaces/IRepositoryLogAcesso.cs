using Aleff.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aleff.Domain.Interfaces
{
  public interface IRepositoryLogAcesso : IRepository<LogAcesso, int>
  {
    List<LogAcesso> GetAllByUser(int userId);
  }
}
