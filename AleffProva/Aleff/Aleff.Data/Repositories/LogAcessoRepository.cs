using Aleff.Data.Context;
using Aleff.Domain.Entities;
using Aleff.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aleff.Data.Repositories
{

  public class LogAcessoRepository : BaseRepository<LogAcesso, int>, IRepositoryLogAcesso
  {
    private IRepository<Usuario, int> _repositoryUser;

    public LogAcessoRepository(IDatabaseContext context, IRepository<Usuario, int> repositoryUser)
    {
      base._context = context;
      _repositoryUser = repositoryUser;
    }

    public override void Delete(LogAcesso entity)
    {
      using (var conn = _context.GetConnection())
      {
        string sql = "DELETE LogAcesso Where LogAcessoId=@Id";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", entity.Id);
        try
        {
          conn.Open();
          cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
          throw e;
        }
      }
    }

    public override void DeleteById(int id)
    {
      using (var conn = _context.GetConnection())
      {
        string sql = "DELETE LogAcesso Where LogAcessoId=@Id";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);
        try
        {
          conn.Open();
          cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
          throw e;
        }
      }
    }


    public override List<LogAcesso> GetAll()
    {
      string sql = "Select LogAcessoId, UsuarioId, DataHoraAcesso, EnderecoIp FROM LogAcesso ORDER BY DataHoraAcesso";
      using (var conn = _context.GetConnection())
      {
        var cmd = new SqlCommand(sql, conn);
        List<LogAcesso> list = new List<LogAcesso>();
        LogAcesso l = null;
        try
        {
          conn.Open();
          using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
          {
            while (reader.Read())
            {
              l = new LogAcesso();
              l.Id = (int)reader["LogAcessoId"];
              l.DataHoraAcesso = DateTime.Parse(reader["DataHoraAcesso"].ToString());
              l.EnderecoIp = reader["EnderecoIp"].ToString();
              var u = _repositoryUser.GetById((int)reader["UsuarioId"]);
              l.Usuario = u;
              list.Add(l);
            }
          }
        }
        catch (Exception e)
        {
          throw e;
        }
        return list;
      }
    }

    public List<LogAcesso> GetAllByUser(int userId)
    {
      string sql = "Select LogAcessoId, UsuarioId, DataHoraAcesso, EnderecoIp FROM LogAcesso WHERE UsuarioId=@UsuarioId ORDER BY DataHoraAcesso";
      using (var conn = _context.GetConnection())
      {
        var cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@UsuarioId", userId);
        List<LogAcesso> list = new List<LogAcesso>();
        LogAcesso l = null;
        try
        {
          conn.Open();
          using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
          {
            while (reader.Read())
            {
              l = new LogAcesso();
              l.Id = (int)reader["LogAcessoId"];
              l.DataHoraAcesso = DateTime.Parse(reader["DataHoraAcesso"].ToString());
              l.EnderecoIp = reader["EnderecoIp"].ToString();
              var u = _repositoryUser.GetById((int)reader["UsuarioId"]);
              l.Usuario = u;
              list.Add(l);
            }
          }
        }
        catch (Exception e)
        {
          throw e;
        }
        return list;
      }
    }

    public override LogAcesso GetById(int id)
    {
      LogAcesso l = null;
      try
      {
        var reader = base.GetByKeyGeneric(id, nameof(LogAcesso));
        if (reader.Rows.Count > 0)
        {
          var row = reader.Rows[0];
          l = new LogAcesso();
          l.Id = (int)row["LogAcessoId"];
          l.DataHoraAcesso = DateTime.Parse(row["DataHoraAcesso"].ToString());
          l.EnderecoIp = row["EnderecoIp"].ToString();
          var u = _repositoryUser.GetById((int)row["UsuarioId"]);
          l.Usuario = u;
        }
      }
      catch (Exception e)
      {
        throw e;
      }

      return l;
    }


    public override void Save(LogAcesso entity)
    {
      try
      {
        var dataHoraAcessoFormatted = entity.DataHoraAcesso.ToString("yyyy-MM-dd HH:mm:ss");
        base.InsertDataGeneric(nameof(LogAcesso), $"'{entity.Usuario.Id}','{dataHoraAcessoFormatted}','{entity.EnderecoIp}'");
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public override void Update(LogAcesso entity)
    {
      using (var conn = _context.GetConnection())
      {
        string sql = "UPDATE LogAcesso SET UsuarioId=@UsuarioId, DataHoraAcesso=@DataHoraAcesso, EnderecoIp=@EnderecoIp Where LogAcessoId=@Id";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", entity.Id);
        cmd.Parameters.AddWithValue("@UsuarioId", entity.Usuario.Id);
        cmd.Parameters.AddWithValue("@DataHoraAcesso", entity.DataHoraAcesso);
        cmd.Parameters.AddWithValue("@EnderecoIp", entity.EnderecoIp);
        try
        {
          conn.Open();
          cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
          throw e;
        }
      }
    }
  }

}

