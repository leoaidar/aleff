using Aleff.Domain.DTO;
using Aleff.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aleff.Domain.Interfaces
{
  public interface IServiceLogAcesso : IService<LogAcesso,int>
  {
    List<LogAcesso> GetAllByUser(int userId);
    List<DTOLogAcessoResponse> DomainToDTOResponse(List<LogAcesso> logsRequest);
    List<DTOLogAcessoUserResponse> DomainToDTOUserResponse(List<LogAcesso> logsRequest);
  }
}
