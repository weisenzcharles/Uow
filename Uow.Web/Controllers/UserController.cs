using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uow.Core.Domain.Repositories;
using Uow.Core.Domain.Uow;
using Uow.Entities;
using Uow.Services;

namespace Uow.Web.Controllers
{
    public class UserController : Controller
    {
        private readonly IUnitOfWorkAsync _unitOfWork;
        private readonly IUserService _userService;

        public UserController(IUnitOfWorkAsync unitOfWork,IUserService userService)
        {
            _unitOfWork = unitOfWork;
            _userService = userService;
        }


        // GET: User
        public ActionResult Index()
        {
            IList<User> users = new List<User>();
            users = _userService.GetUsers();
            ViewData["users"] = users;
            return View();
        }

        public ActionResult Add()
        {
            User user = new User();
            user.Name = "zhang" + DateTime.Now.Millisecond;
            user.Password = "password" + DateTime.Now.Millisecond;
            _userService.Add(user);
            _unitOfWork.SaveChanges();
            return View();
        }

    }
}