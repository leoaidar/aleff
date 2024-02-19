using Aleff.Data.Repositories;
using Aleff.Domain.Entities;
using Aleff.Domain.Interfaces;
using Aleff.Service.Services;
using Aleff.Web.MVC.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Aleff.Web.MVC.App_Start
{
  public static class Bootstrapper
  {
    public static void Init()
    {
      DependencyInjector.Resolver();
      DependencyInjector.Register<IRepository<Usuario, int>, UsuarioRepository>();
      DependencyInjector.Register<IService<Usuario, int>, UsuarioService>();
      //var controller = DependencyInjector.Retrieve<HomeController>();

      //DependencyInjector.Register<IService, UsuarioService>();

      //container
      //.RegisterType(
      //typeof(IMessageQueue<>),
      //typeof(MessageQueue<>),
      //new InjectionConstructor(storageAccountType,
      //retryPolicyFactoryType, typeof(String)));


      //DependencyInjector.AddExtension<DependencyOfDependencyExtension>();
    }
  }
}