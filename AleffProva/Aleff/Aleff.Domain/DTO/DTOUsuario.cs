using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aleff.Domain.DTO
{
  public class DTOUsuario
  {
    public string Nome { get; set; }
    public string Login { get; set; }
    public string Senha { get; set; }
    public bool IsAdmin { get; set; }
  }
}
