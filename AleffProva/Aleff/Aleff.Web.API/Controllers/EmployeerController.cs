using Aleff.Domain.Entities;
using Aleff.Domain.Interfaces;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Description;
using System.Net;
using Newtonsoft.Json;
using System;
using Aleff.Domain.Exceptions;
using Aleff.Domain.DTO;
using System.Web;
using System.IO;
using System.Threading.Tasks;

namespace Aleff.Web.API.Controllers
{
  /// <summary>
  /// Usuario Endpoint.
  /// </summary>
  public class EmployeerController : ApiController
  {

    private IServiceEmployeer _service;
    private IServiceLogAcesso _serviceLogAcesso;


    // GET api/<controller>
    /// <summary>
    /// Construtor 
    /// </summary>
    public EmployeerController(IServiceEmployeer service, IServiceLogAcesso serviceLogAcesso)
    {
      _service = service;
      _serviceLogAcesso = serviceLogAcesso;
    }


    // GET api/<controller>
    /// <summary>
    /// Realiza a busca geral
    /// </summary>
    /// <returns>Employeer[]</returns>
    /// <response code="200"></response>
    /// <response code="400"></response>
    /// <response code="404"></response>
    /// <response code="500"></response>
    [HttpGet]
    [ResponseType(typeof(List<Employeer>))]
    public async Task<IHttpActionResult> Get()
    {
      string uri = HttpContext.Current.Request.Url.AbsoluteUri;
      GenerateLogRequest(uri);
      
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      try
      {
        var emps = await _service.GetEmployeesAsync();
        var json = JsonConvert.SerializeObject(emps, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        emps = JsonConvert.DeserializeObject<List<Employeer>>(json);
        if (emps == null || emps.Count == 0)
        {
          return NotFound();
        }
        return Ok(emps);
      }
      catch (Exception ex)
      {
        return InternalServerError(ex);
      }
    }

    protected string GetIPAddress()
    {
      var context = HttpContext.Current;
      string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

      if (!string.IsNullOrEmpty(ipAddress))
      {
        string[] addresses = ipAddress.Split(',');
        if (addresses.Length != 0)
        {
          return addresses[0];
        }
      }

      return context.Request.ServerVariables["REMOTE_ADDR"];
    }

    protected void GenerateLogRequest(string uri, int? userId = null)
    {
      try
      {

        string logsPath = AppDomain.CurrentDomain.BaseDirectory + "\\Logs";
        if (!Directory.Exists(logsPath))
          Directory.CreateDirectory(logsPath);

        var today = DateTime.Today;
        var filePath = logsPath + "\\" + today.ToString("yyyy-MM-dd");
        StreamWriter file = null;
        if (!File.Exists(filePath))
          file = File.CreateText(filePath);
        else
          file = File.AppendText(filePath);

        var logRequest = uri + "," + today.ToString("yyyy-MM-dd") + "," + DateTime.Now.ToString("HH:mm:ss");
        file.WriteLine(logRequest);
        file.Close();

        if (userId != null)
        {
          var log = new LogAcesso
          {
            Usuario = new Usuario { Id = (int)userId },
            DataHoraAcesso = DateTime.Now,
            EnderecoIp = GetIPAddress()
          };

          _serviceLogAcesso.Save(log);
        }

      }
      catch (Exception ex)
      {
        // nao posso interromper a aplicacao pq nao conseguiu salvar um log
      }
    }





  }
}