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

  public class UsuarioRepository : BaseRepository<Usuario, int>
  {
    public UsuarioRepository(IDatabaseContext context)
    {
      base._context = context;
    }

    public override void Delete(Usuario entity)
    {
      using (var conn = _context.GetConnection())
      {
        string sql = "DELETE Usuario Where UsuarioId=@Id";
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
        string sql = "DELETE Usuario Where UsuarioId=@Id";
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


    public override List<Usuario> GetAll()
    {
      string sql = "Select UsuarioId, Nome, Login, Senha, IsAdmin FROM Usuario ORDER BY Nome";
      using (var conn = _context.GetConnection())
      {
        var cmd = new SqlCommand(sql, conn);
        List<Usuario> list = new List<Usuario>();
        Usuario u = null;
        try
        {
          conn.Open();
          using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
          {
            while (reader.Read())
            {
              u = new Usuario();
              u.Id = (int)reader["UsuarioId"];
              u.Nome = reader["Nome"].ToString();
              u.Login = reader["Login"].ToString();
              u.Senha = reader["Senha"].ToString();
              u.IsAdmin = (bool)reader["IsAdmin"];
              list.Add(u);
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

    public Usuario GetById2(int id)
    {
      using (var conn = _context.GetConnection())
      {
        string sql = "Select UsuarioId, Nome, Login, Senha, IsAdmin FROM Usuario WHERE UsuarioId=@Id";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", id);
        Usuario u = null;
        try
        {
          conn.Open();
          using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
          {
            if (reader.HasRows)
            {
              if (reader.Read())
              {
                u = new Usuario();
                u.Id = (int)reader["UsuarioId"];
                u.Nome = reader["Nome"].ToString();
                u.Login = reader["Login"].ToString();
                u.Senha = reader["Senha"].ToString();
                u.IsAdmin = (bool)reader["IsAdmin"];
              }
            }
          }
        }
        catch (Exception e)
        {
          throw e;
        }
        return u;
      }
    }

    public override Usuario GetById(int id)
    {
      Usuario u = null;
      try
      {
        var reader = base.GetByKeyGeneric(id, nameof(Usuario));
        if (reader.Rows.Count > 0)
        {
          var row = reader.Rows[0];
          u = new Usuario();
          u.Id = (int)row["UsuarioId"];
          u.Nome = row["Nome"].ToString();
          u.Login = row["Login"].ToString();
          u.Senha = row["Senha"].ToString();
          u.IsAdmin = (bool)row["IsAdmin"];
        }
      }
      catch (Exception e)
      {
        throw e;
      }

      return u;
    }

    public void Save2(Usuario entity)
    {
      using (var conn = _context.GetConnection())
      {
        string sql = "INSERT INTO Usuario (Nome, Login, Senha, IsAdmin) VALUES (@Nome, @Login, @Senha, @IsAdmin)";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Nome", entity.Nome);
        cmd.Parameters.AddWithValue("@Login", entity.Login);
        cmd.Parameters.AddWithValue("@Senha", entity.Senha);
        cmd.Parameters.AddWithValue("@IsAdmin", entity.IsAdmin);
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
    public override void Save(Usuario entity)
    {
      try
      {
        base.InsertDataGeneric(nameof(Usuario), $"'{entity.Nome}','{entity.Login}','{entity.Senha}',{Convert.ToByte(entity.IsAdmin)}");
      }
      catch (Exception e)
      {
        throw e;
      }
    }

    public override void Update(Usuario entity)
    {
      using (var conn = _context.GetConnection())
      {
        string sql = "UPDATE Usuario SET Nome=@Nome, Login=@Login, Senha=@Senha, IsAdmin=@IsAdmin Where UsuarioId=@Id";
        SqlCommand cmd = new SqlCommand(sql, conn);
        cmd.Parameters.AddWithValue("@Id", entity.Id);
        cmd.Parameters.AddWithValue("@Nome", entity.Nome);
        cmd.Parameters.AddWithValue("@Login", entity.Login);
        cmd.Parameters.AddWithValue("@Senha", entity.Senha);
        cmd.Parameters.AddWithValue("@IsAdmin", entity.IsAdmin);
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

