using Aleff.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aleff.Data.Context
{
  public class DatabaseContext : IDatabaseContext
  {
    private string _connectionString = null;

    public DatabaseContext()
    {
      _connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString;
    }

    public SqlConnection GetConnection()
    {
      var conn = new SqlConnection(_connectionString);
      return conn;
    }

  }
}
