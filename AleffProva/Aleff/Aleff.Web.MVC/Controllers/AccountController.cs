using System;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Aleff.Web.MVC.Models;
using Aleff.Domain.Interfaces;
using Aleff.Domain.Entities;

namespace Aleff.Web.MVC.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
          private IService<Usuario, int> _service;
          private IServiceLogAcesso _serviceLogAcesso;

        public AccountController(IService<Usuario, int> service, IServiceLogAcesso serviceLogAcesso)
        {
          _service = service;
          _serviceLogAcesso = serviceLogAcesso;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if(string.IsNullOrEmpty(model.Login.Trim()) || string.IsNullOrEmpty(model.Password.Trim()))
            {
              ModelState.AddModelError("", "Tentativa de login inválida.");
              return View(model);
            }

            var result = _service.GetAll().Where(x=>x.Login == model.Login && x.Senha == Usuario.EncryptPassword(model.Password));

            if(result != null && result.Count() == 1 && result.FirstOrDefault().Login == model.Login)
            {
              var log = new LogAcesso
              {
                Usuario = new Usuario { Id = result.FirstOrDefault().Id },
                DataHoraAcesso = DateTime.Now,
                EnderecoIp = GetIPAddress()
              };

              _serviceLogAcesso.Save(log);
             
              return RedirectToLocal(returnUrl);
            }

            ModelState.AddModelError("", "Tentativa de login inválida.");
            return View(model);
        }

        protected string GetIPAddress()
        {
          var context = HttpContext;
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

    //
    // GET: /Account/Register
    [AllowAnonymous]
        public ActionResult Register(RegisterViewModel model)
        {
            
            if (model.IsAdmin == false && model.Login == null && model.Nome == null && model.Password == null && model.ConfirmPassword == null)
            {
              return View();
            }

            if (!ModelState.IsValid)
            {
              ModelState.AddModelError("", "Cadastro inválido.");
              return View(model);
            }

            var user = new Usuario
            {
              IsAdmin = model.IsAdmin,
              Login = model.Login,
              Nome = model.Nome,
              Senha = model.Password
            };

            if (!Usuario.IsValidPassword(user))
            {
              ModelState.AddModelError("", "Senha nao atende os requisitos.");
              return View(model);
            }

            try
            {
              _service.Save(user);
              return View();
            }
            catch (Exception ex)
            {
              ModelState.AddModelError("", "Erro interno da aplicacao");
              return View(model);
            }

        }

        //
        // GET: /Account/ForgotPassword
        [AllowAnonymous]
        public ActionResult ForgotPassword()
        {
            return View();
        }


        //
        // GET: /Account/ForgotPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ForgotPasswordConfirmation()
        {
            return View();
        }

        //
        // GET: /Account/ResetPassword
        [AllowAnonymous]
        public ActionResult ResetPassword(string code)
        {
            return code == null ? View("Error") : View();
        }


        //
        // GET: /Account/ResetPasswordConfirmation
        [AllowAnonymous]
        public ActionResult ResetPasswordConfirmation()
        {
            return View();
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return RedirectToAction("Index", "Home");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
            //    if (_userManager != null)
            //    {
            //        _userManager.Dispose();
            //        _userManager = null;
            //    }

            //    if (_signInManager != null)
            //    {
            //        _signInManager.Dispose();
            //        _signInManager = null;
            //    }
            }

            base.Dispose(disposing);
        }

        #region Auxiliares
        // Usado para proteção XSRF ao adicionar logons externos
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        internal class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri)
                : this(provider, redirectUri, null)
            {
            }

            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                LoginProvider = provider;
                RedirectUri = redirectUri;
                UserId = userId;
            }

            public string LoginProvider { get; set; }
            public string RedirectUri { get; set; }
            public string UserId { get; set; }

            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties { RedirectUri = RedirectUri };
                if (UserId != null)
                {
                    properties.Dictionary[XsrfKey] = UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, LoginProvider);
            }
        }
        #endregion
    }
}