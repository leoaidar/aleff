using Aleff.Data.Context;
using Aleff.Data.Repositories;
using Aleff.Domain.Entities;
using Aleff.Domain.Interfaces;
using Aleff.Application.Services;
using Aleff.Data.AgentService.Dummy;
using System.Net.Http;

namespace Aleff.CrossCutting
{
  public static class Bootstrapper
  {
    public static void Init()
    {
      DependencyInjector.Register<IDatabaseContext, DatabaseContext>();
      var agentService = new AgentService(new HttpClient());
      DependencyInjector.InjectStub<AgentService>(agentService);
      DependencyInjector.Register<IAgentService, AgentService>();
      DependencyInjector.Register<IRepository<Usuario, int>, UsuarioRepository>();
      DependencyInjector.Register<IRepository<LogAcesso, int>, LogAcessoRepository>();
      DependencyInjector.Register<IRepositoryLogAcesso, LogAcessoRepository>();
      DependencyInjector.Register<IRepositoryEmployeer, EmployeerRepository>();
      DependencyInjector.Register<IService<Usuario, int>, UsuarioService>();
      DependencyInjector.Register<IService<LogAcesso, int>, LogAcessoService>();
      DependencyInjector.Register<IServiceLogAcesso, LogAcessoService>();
      DependencyInjector.Register<IServiceEmployeer, EmployeerService>();
    }
  }

}
