using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DotNetOpenAuth.InfoCard;
using InfraEstrutura;
using InfraEstrutura.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebMVC.Models;
using WebMVC.Models.CustomerComment;

namespace WebMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly ICustomerService _db;
        public AccountController(ICustomerService db)
        {
            _db = db;
        }
        // GET: AccountController
        [Route("/login")]
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [Route("/login")]
        public async Task<ActionResult> Login(LoginViewModel item)
        {
            var r = _db.SelectCustomer(item.UserName, item.Password);
            if (r != null) //Sessão -  Scoped
            {
                //Vamos criar a CLAIM e Registar o HttpContext reclamando Identity
                // ou seja, existe algue que diz ser alguem e nós vamos verificar se tem fundamento
                var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
                identity.AddClaim(new Claim(
                    ClaimTypes.Name, item.UserName));
                identity.AddClaim(new Claim(ClaimTypes.Country, r.Country));
                identity.AddClaim(new Claim("Tema", r.Country));
                foreach (var cr in r.CustomerRoles)
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role, cr.RoleName));
                }
                DateTime dt = DateTime.UtcNow.AddSeconds(10);
                var principal = new ClaimsPrincipal(identity);  //EMEA
                var properties = new AuthenticationProperties {IsPersistent=item.RememberMe};

                await this.HttpContext.SignInAsync(principal, properties).ConfigureAwait(false);
                
                return Redirect("/");

            }
            ModelState.AddModelError("", "Credenciais Inválidas!");
            return View();
        }
        // Get - Token gerado no servidor
        public ActionResult Register()
        {
            return View();
        }
        // POst + Token 
        [HttpPost]
        public ActionResult Register(RegisterViewModel item)
        {
            //AutoMapper
            var r = _db.RegisterNewCustomer(new Customer
            { CustomerId= Guid.NewGuid(), Name = item.Name, NIF = item.NIF, UserName = item.UserName, Country=item.Country, Email=item.Email, PassHash=item.Password });

            if (r == 1)
            {
                // O registo foi inserido com sucesso
                return Redirect("/");
            }
            ModelState.AddModelError("" , "Não foi possivel criar o registo.Campos Duplicados" );

            return View(item);
        }

        // GET: AccountController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AccountController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AccountController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: AccountController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: AccountController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
