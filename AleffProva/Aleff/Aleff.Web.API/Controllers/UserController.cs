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

namespace Aleff.Web.API.Controllers
{
  /// <summary>
  /// Usuario Endpoint.
  /// </summary>
  public class UserController : ApiController
  {

    private IService<Usuario, int> _service;
    private IServiceLogAcesso _serviceLogAcesso;


    // GET api/<controller>
    /// <summary>
    /// Construtor do servi○o de usuario
    /// </summary>
    public UserController(IService<Usuario, int> service, IServiceLogAcesso serviceLogAcesso)
    {
      _service = service;
      _serviceLogAcesso = serviceLogAcesso;
    }


    // GET api/<controller>
    /// <summary>
    /// Realiza a busca geral
    /// </summary>
    /// <returns>Usuario[]</returns>
    /// <response code="200"></response>
    /// <response code="400"></response>
    /// <response code="404"></response>
    /// <response code="500"></response>
    [HttpGet]
    [ResponseType(typeof(List<Usuario>))]
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
        var users = _service.GetAll();
        var json = JsonConvert.SerializeObject(users, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        users = JsonConvert.DeserializeObject<List<Usuario>>(json);
        if (users == null || users.Count == 0)
        {
          return NotFound();
        }
        return Ok(users);
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
    /// <returns>Usuario</returns>
    /// <response code="200"></response>
    /// <response code="400"></response>
    /// <response code="404"></response>
    /// <response code="500"></response>
    [HttpGet]
    [ResponseType(typeof(Usuario))]
    public IHttpActionResult Get(int id)
    {
      string uri = HttpContext.Current.Request.Url.AbsoluteUri;
      GenerateLogRequest(uri, id);

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      try
      {
        var user = _service.GetById(id);
        var json = JsonConvert.SerializeObject(user, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        user = JsonConvert.DeserializeObject<Usuario>(json);
        if (user == null)
        {
          return NotFound();
        }
        return Ok(user);
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
    /// <param name="usuario"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="400"></response>
    /// <response code="500"></response>
    [HttpPost]
    public IHttpActionResult Post([FromBody] DTOUsuario usuario)
    {
      string uri = HttpContext.Current.Request.Url.AbsoluteUri;
      GenerateLogRequest(uri);

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      var user = new Usuario
      {
        IsAdmin = usuario.IsAdmin,
        Login = usuario.Login,
        Nome = usuario.Nome,
        Senha = usuario.Senha
      };

      if (!Usuario.IsValidPassword(user))
      {
        return BadRequest("Senha nao atende os requisitos");
      }

      try
      {
        _service.Save(user);
        return Ok();
      }
      catch (Exception ex)
      {
        return InternalServerError(ex);
      }
    }

    // PUT api/<controller>/5
    /// <summary>
    /// Realiza a alteracao por id.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="usuario"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="400"></response>
    /// <response code="404"></response>
    /// <response code="500"></response>
    [HttpPut]
    public IHttpActionResult Put(int id, [FromBody] DTOUsuario usuario)
    {
      string uri = HttpContext.Current.Request.Url.AbsoluteUri;
      GenerateLogRequest(uri,id);

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      try
      {
        var user = new Usuario
        {
          Id = id,
          IsAdmin = usuario.IsAdmin,
          Login = usuario.Login,
          Nome = usuario.Nome,
          Senha = usuario.Senha
        };
        _service.Update(user);
        return Ok();
      }
      catch (DomainException ex)
      {
        if(ex.Message.Equals("404"))
          return NotFound();

        return InternalServerError(ex);
      }      
      catch (Exception ex)
      {
        return InternalServerError(ex);
      }
    }


    // DELETE api/<controller>/5
    /// <summary>
    /// Realiza a alteracao por id.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    /// <response code="200"></response>
    /// <response code="400"></response>
    /// <response code="404"></response>
    /// <response code="500"></response>
    [HttpDelete]
    public IHttpActionResult Delete(int id)
    {
      string uri = HttpContext.Current.Request.Url.AbsoluteUri;
      GenerateLogRequest(uri,id);

      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      try
      {
        _service.DeleteById(id);
        return Ok();
      }
      catch (DomainException ex)
      {
        if (ex.Message.Equals("404"))
          return NotFound();

        return InternalServerError(ex);
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