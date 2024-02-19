using Aleff.Domain.Entities;
using Aleff.Domain.Interfaces;
using Aleff.Application.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Aleff.Web.MVC.Controllers
{
  public class HomeController : Controller
  {
    private IService<Usuario, int> _service;

    public HomeController(IService<Usuario, int> service)
    {
      _service = service;
    }

    public ActionResult Index()
    {
      return View();
    }

    public ActionResult About()
    {
      var users = _service.GetAll();
      ViewBag.Message = users.FirstOrDefault()?.Nome;

      return View();
    }

    public ActionResult Contact()
    {

      ViewBag.Message = "Contato";

      return View();
    }
  }
}