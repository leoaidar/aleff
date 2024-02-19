using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aleff.Domain.Interfaces
{
  public interface IRepository<TEntity, TKey>
          where TEntity : class
  {
    List<TEntity> GetAll();
    TEntity GetById(TKey id);
    void Save(TEntity entity);
    void Update(TEntity entity);
    void Delete(TEntity entity);
    void DeleteById(TKey id);
    void InsertDataGeneric(string table, string values);
    DataTable GetByKeyGeneric(TKey key, string table);
  }
}
