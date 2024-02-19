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
  public class EmployeerService : IServiceEmployeer
  {
    private IRepositoryEmployeer _repository;

    public EmployeerService(IRepositoryEmployeer repository)
    {
      _repository = repository;
    }

    public async Task<List<Employeer>> GetEmployeesAsync()
    {
      var fullAgeEmployess = await _repository.GetEmployeesAsync();
      var filteredEmployess = fullAgeEmployess.Where(x => x.Age > 30).ToList();
      return filteredEmployess;
    }

  }
}
