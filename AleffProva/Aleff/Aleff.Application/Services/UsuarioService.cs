using Aleff.Data.Repositories;
using Aleff.Domain.Entities;
using Aleff.Domain.Exceptions;
using Aleff.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Aleff.Application.Services
{
  public class UsuarioService : IService<Usuario, int>
  {
    private IRepository<Usuario, int> _repository;

    public UsuarioService(IRepository<Usuario, int> repository)
    {
      _repository = repository;
    }

    public void Delete(Usuario entity)
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

    public List<Usuario> GetAll()
    {
      return _repository.GetAll();
    }

    public Usuario GetById(int id)
    {
      return _repository.GetById(id);
    }

    public void Save(Usuario entity)
    {
      entity.Senha = EncryptPassword(entity.Senha);
      _repository.Save(entity);
    }

    public void Update(Usuario entity)
    {
      var user = _repository.GetById(entity.Id);
      if (user == null)
        throw new DomainException("404");

      _repository.Update(entity);
    }

    private string EncryptPassword(string password)
    {
      var hash = new StringBuilder();
      using (SHA256 crypt = SHA256.Create())
      {
        byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password));
        foreach (byte theByte in crypto)
        {
          hash.Append(theByte.ToString("x2"));
        }
      }
      return hash.ToString();
    }

  }
}
