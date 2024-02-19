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
using System.IO;
using System.Web;

namespace Aleff.Web.API.Controllers
{
  /// <summary>
  /// LogAcesso Endpoint.
  /// </summary>
  public class LogRequestController : ApiController
  {

    private IServiceLogAcesso _service;


    // GET api/<controller>
    /// <summary>
    /// Construtor do servi○o de LogAcesso
    /// </summary>
    public LogRequestController(IServiceLogAcesso service)
    {
      _service = service;
    }


    // GET api/<controller>
    /// <summary>
    /// Realiza a busca geral
    /// </summary>
    /// <returns>LogAcesso[]</returns>
    /// <response code="200"></response>
    /// <response code="400"></response>
    /// <response code="404"></response>
    /// <response code="500"></response>
    [HttpGet]
    [ResponseType(typeof(List<DTOLogAcessoResponse>))]
    public IHttpActionResult Get()
    {
      string uri = HttpContext.Current.Request.Url.AbsoluteUri;
      GenerateLogRequest(uri);

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      try
      {
        var logRequests = _service.GetAll();
        if (logRequests == null || logRequests.Count == 0)
        {
          return NotFound();
        }
        var response = _service.DomainToDTOResponse(logRequests);
        var json = JsonConvert.SerializeObject(response, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        response = JsonConvert.DeserializeObject<List<DTOLogAcessoResponse>>(json);
        
        return Ok(response);
      }
      catch (Exception ex)
      {
        return InternalServerError(ex);
      }
    }

    // GET api/<controller>/5
    /// <summary>
    /// Realiza a busca por id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns>LogAcesso</returns>
    /// <response code="200"></response>
    /// <response code="400"></response>
    /// <response code="404"></response>
    /// <response code="500"></response>
    [HttpGet]
    [ResponseType(typeof(LogAcesso))]
    public IHttpActionResult Get(int id)
    {
      string uri = HttpContext.Current.Request.Url.AbsoluteUri;
      GenerateLogRequest(uri);

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      try
      {
        var logRequest = _service.GetById(id);
        if (logRequest == null)
        {
          return NotFound();
        }
        var response = _service.DomainToDTOResponse(new List<LogAcesso>() { logRequest });
        var tuple = response[0];
        var json = JsonConvert.SerializeObject(tuple, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        tuple = JsonConvert.DeserializeObject<DTOLogAcessoResponse>(json);

        return Ok(tuple);
      }
      catch (Exception ex)
      {
        return InternalServerError(ex);
      }
    }

    // GetAllByUser api/<controller>/5
    /// <summary>
    /// Realiza a busca de logs por usuario.
    /// </summary>
    /// <param name="userId"></param>
    /// <returns>LogAcesso[]</returns>
    /// <response code="200"></response>
    /// <response code="400"></response>
    /// <response code="404"></response>
    /// <response code="500"></response>
    [HttpGet]
    [Route("api/LogRequest/ByUser/{userId}")]
    [ResponseType(typeof(List<DTOLogAcessoUserResponse>))]
    public IHttpActionResult GetAllByUser(int userId)
    {
      string uri = HttpContext.Current.Request.Url.AbsoluteUri;
      GenerateLogRequest(uri,userId);

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      try
      {
        var logsRequest = _service.GetAllByUser(userId);
        if (logsRequest == null || logsRequest.Count == 0)
        {
          return NotFound();
        }
        var response = _service.DomainToDTOUserResponse(logsRequest);
        var json = JsonConvert.SerializeObject(response, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        response = JsonConvert.DeserializeObject<List<DTOLogAcessoUserResponse>>(json);

        return Ok(response);
      }
      catch (Exception ex)
      {
        return InternalServerError(ex);
      }
    }

    // POST api/<controller>
    /// <summary>
    /// Realiza o cadastro.
    /// </summary>
    /// <param name="LogAcesso"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="400"></response>
    /// <response code="500"></response>
    [HttpPost]
    public IHttpActionResult Post([FromBody] DTOLogAcesso LogAcesso)
    {
      string uri = HttpContext.Current.Request.Url.AbsoluteUri;
      GenerateLogRequest(uri);

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      try
      {
        var logRequest = new LogAcesso
        {
          Usuario = new Usuario { Id = LogAcesso.UsuarioId },
          DataHoraAcesso = DateTime.Now,
          EnderecoIp = GetIPAddress()
        };
        _service.Save(logRequest);
        return Ok();
      }
      catch (Exception ex)
      {
        return InternalServerError(ex);
      }
    }


    protected string GetIPAddress()
    {
      System.Web.HttpContext context = System.Web.HttpContext.Current;
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

        if(userId != null)
        {
          var log = new LogAcesso
          {
            Usuario = new Usuario { Id = (int)userId },
            DataHoraAcesso = DateTime.Now,
            EnderecoIp = GetIPAddress()
          };

          _service.Save(log);
        }

      }
      catch (Exception ex)
      {
        // nao posso interromper a aplicacao pq nao conseguiu salvar um log
      }
    }


  }
}