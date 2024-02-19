using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aleff.Domain.Interfaces
{
  public abstract class BaseRepository<TEntity, TKey> : IRepository<TEntity, TKey>
      where TEntity : class
  {

    protected IDatabaseContext _context;

    public abstract List<TEntity> GetAll();
    public abstract TEntity GetById(TKey id);
    public abstract void Save(TEntity entity);
    public abstract void Update(TEntity entity);
    public abstract void Delete(TEntity entity);
    public abstract void DeleteById(TKey id);
    public void InsertDataGeneric(string table, string values)
    {
      using (var conn = _context.GetConnection())
      {
        SqlCommand cmd = new SqlCommand("SPDML_INSERT", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@TABLE_NAME", table);
        cmd.Parameters.AddWithValue("@INSERT_VALUES", values);
        try
        {
          conn.Open();
          cmd.ExecuteNonQuery();
        }
        catch (Exception e)
        {
          throw e;
        }
        finally
        {
          conn.Close();
        }
      }
    }


    public DataTable GetByKeyGeneric(TKey key, string table)
    {
      using (var conn = _context.GetConnection())
      {
        SqlCommand cmd = new SqlCommand("SPSQL_BYKEY", conn);
        cmd.CommandType = CommandType.StoredProcedure;
        cmd.Parameters.AddWithValue("@TABLE_NAME", table);
        cmd.Parameters.AddWithValue("@VALUE_KEY", key);
        DataTable record = new DataTable();
        try
        {
          conn.Open();
          using (var da = new SqlDataAdapter(cmd))
          {
            da.Fill(record);
          }
        }
        catch (Exception e)
        {
          throw e;
        }
        return record;
      }
    }


  }

}
