﻿using LDAP;
using System.Linq;
using System.Web.Mvc;
using TPGP.Context;
using TPGP.DAL.Interfaces;
using TPGP.Models.Enums;
using TPGP.Models.Jobs;

namespace TPGP.Controllers
{
    public class HomeController : Controller
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;

        public HomeController(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
        }

        public ActionResult Index()
        {
            //juste pour créer la database
            using (var ctx = new TPGPContext())
            {
                var rolesCount = ctx.Roles.Count();
            }
            //juste pour créer la database

            if (Session["username"] != null)
                return View("_AlreadyLoggedIn");

            return View();
        }

        [HttpPost]
        public ActionResult Login(User userModel)
        {
            if (ModelState.IsValid)
            {
                LDAPUser ldapUserDetails = LDAPService.Instance.AuthenticationAndIdentification(userModel.Username, userModel.Password);
                if (ldapUserDetails == null)
                {
                    ModelState.AddModelError(string.Empty, "Wrong username or password.");
                    return View("Index", userModel);
                }

                User user = userRepository.GetBy(u => u.Username == userModel.Username).First();
                if (user == null)
                {
                    User newUser = new User()
                    {
                        Firstname = ldapUserDetails.Firstname,
                        Lastname = ldapUserDetails.Lastname,
                        Username = ldapUserDetails.Username,
                        Email = ldapUserDetails.Email,
                        Role = new Role() { RoleName = Roles.COLLABORATOR, IsAdmin = false }
                    };

                    userRepository.Insert(newUser);
                    userRepository.SaveChanges();

                    Session["username"] = user.Username;
                    Session["role"] = user.Role.RoleName.ToString("g");
                }
                else
                {
                    user.Role = roleRepository.GetById(user.RoleId);

                    Session["username"] = user.Username;
                    Session["role"] = user.Role.RoleName.ToString("g");

                    if (user.Role.RoleName == Roles.ADMIN)
                        return RedirectToAction("Index", "Admin");
                    else if (user.Role.RoleName == Roles.COLLABORATOR)
                        return RedirectToAction("Index", "Portfolio");
                }

                return View("Index");
            }

            return View("Index");
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Logout()
        {
            Session["username"] = null;
            Session["role"] = null;

            return RedirectToAction("Index", "Home");
        }
    }
}