using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Aleff.Domain.Entities
{
  public class Usuario
  {
    public int Id { get; set; }
    public string Nome { get; set; }
    public string Login { get; set; }
    public string Senha { get; set; }
    public bool IsAdmin { get; set; }

    public static bool IsValidPassword(Usuario user)
    {
      //- Possuir 10 ou mais caracteres X
      if (user.Senha.Length < 10)
        return false;

      //- Possuir ao menos 1 dígito numérico x
      if (!user.Senha.Any(char.IsDigit))
        return false;

      //- Possuir ao menos 1 letra minúscula x
      if (!user.Senha.Any(char.IsLower))
        return false;

      //- Possuir ao menos 1 letra maiúscula X
      if (!user.Senha.Any(char.IsUpper))
              return false;

      //- Não possuir espaços em branco
      if (user.Senha.Contains(" "))
        return false;

      //- Não possuir caracteres repetidos
      if (user.Senha.Distinct().Count() != user.Senha.Length)
        return false;

      //- Possuir ao menos 1 dos caracteres especiais a seguir: !@#$%^&*()-+
      string specialCh = @"!@#$%^&*()-+" + "\"";
      char[] specialChArray = specialCh.ToCharArray();
      foreach (char ch in specialChArray)
      {
        if (user.Senha.Contains(ch))
          return true;
      }

      return false; 
    }

    public static string EncryptPassword(string password)
    {
      var hash = new StringBuilder();
      using (SHA256 crypt = SHA256.Create())
      {
        byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(password));
        foreach (byte theByte in crypto)
        {
          hash.Append(theByte.ToString("x2"));
        }
      }
      return hash.ToString();
    }

  }
}
