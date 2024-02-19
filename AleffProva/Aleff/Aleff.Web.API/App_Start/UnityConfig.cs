using Aleff.Application.Services;
using Aleff.Data.AgentService.Dummy;
using Aleff.Data.Context;
using Aleff.Data.Repositories;
using Aleff.Domain.Entities;
using Aleff.Domain.Interfaces;
using Microsoft.Practices.Unity;
using System.Net.Http;
using System.Web.Http;
using Unity.WebApi;

namespace Aleff.Web.API
{
  public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			    var container = new UnityContainer();
          container.RegisterType<IDatabaseContext, DatabaseContext>();
          var agentService = new AgentService(new HttpClient());
          container.RegisterInstance<AgentService>(agentService);
          container.RegisterType<IAgentService, AgentService>();
          container.RegisterType<IRepository<Usuario, int>, UsuarioRepository>();
          container.RegisterType<IRepository<LogAcesso, int>, LogAcessoRepository>();
          container.RegisterType<IRepositoryLogAcesso, LogAcessoRepository>();
          container.RegisterType<IRepositoryEmployeer, EmployeerRepository>();
          container.RegisterType<IService<Usuario, int>, UsuarioService>();
          container.RegisterType<IService<LogAcesso, int>, LogAcessoService>();
          container.RegisterType<IServiceLogAcesso, LogAcessoService>();
          container.RegisterType<IServiceEmployeer, EmployeerService>();
          GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}