using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aleff.Domain.Entities
{
  public class LogAcesso
  {
    public int Id { get; set; }
    public Usuario Usuario { get; set; }
    public DateTime DataHoraAcesso { get; set; }
    public string EnderecoIp { get; set; }
  }
}
