using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aleff.Domain.DTO
{
  public class DTOLogAcessoResponse
  {
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string UsuarioNome { get; set; }
    public DateTime DataHoraAcesso { get; set; }
    public string EnderecoIp { get; set; }
  }
}
