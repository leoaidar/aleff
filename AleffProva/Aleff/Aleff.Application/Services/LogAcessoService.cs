using Aleff.Data.Repositories;
using Aleff.Domain.DTO;
using Aleff.Domain.Entities;
using Aleff.Domain.Exceptions;
using Aleff.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aleff.Application.Services
{
  public class LogAcessoService : IServiceLogAcesso
  {
    private IRepositoryLogAcesso _repository;

    public LogAcessoService(IRepositoryLogAcesso repository)
    {
      _repository = repository;
    }

    public void Delete(LogAcesso entity)
    {
      throw new NotImplementedException();
    }

    public void DeleteById(int id)
    {
      var user = _repository.GetById(id);
      if (user == null)
        throw new DomainException("404");

      _repository.DeleteById(id);
    }

    public List<LogAcesso> GetAll()
    {
      return _repository.GetAll();
    }
    
    public List<LogAcesso> GetAllByUser(int userId)
    {
      return _repository.GetAllByUser(userId);
    }

    public LogAcesso GetById(int id)
    {
      return _repository.GetById(id);
    }

    public void Save(LogAcesso entity)
    {
      _repository.Save(entity);
    }

    public void Update(LogAcesso entity)
    {
      var user = _repository.GetById(entity.Id);
      if (user == null)
        throw new DomainException("404");

      _repository.Update(entity);
    }
    
    public List<DTOLogAcessoResponse> DomainToDTOResponse(List<LogAcesso> logsRequest)
    {
      List<DTOLogAcessoResponse> dtosResponse = new List<DTOLogAcessoResponse>();
      if (logsRequest != null && logsRequest.Count > 0)
      {
        foreach (var log in logsRequest)
        {
          var dto = new DTOLogAcessoResponse
          {
            DataHoraAcesso = log.DataHoraAcesso,
            EnderecoIp = log.EnderecoIp,
            Id = log.Id,
            UsuarioId = log.Usuario.Id,
            UsuarioNome = log.Usuario.Nome
          };
          dtosResponse.Add(dto);
        }
      }
      return dtosResponse;
    }

    public List<DTOLogAcessoUserResponse> DomainToDTOUserResponse(List<LogAcesso> logsRequest)
    {
      var modified = logsRequest.Select(x =>
      {
        return new DTOLogAcessoUserResponse
        {
          hora = x.DataHoraAcesso.ToString().Substring(11, 2) + ":00",
          quantidade = 0
        };
      }).ToList();
      var grouped = modified
                      .GroupBy(u => u.hora)
                      .Select(grp => grp.ToList())
                      .ToList();

      var counting = new List<DTOLogAcessoUserResponse>();

      foreach (var group in grouped)
      {
        counting.Add(new DTOLogAcessoUserResponse
        {
          hora = group.First().hora,
          quantidade = group.Count()
        });
      }
      return counting;
    }



  }
}
