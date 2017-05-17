using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Uow.Core.Domain.Repositories;
using Uow.Entities;
namespace Uow.Web.Controllers
{
    public class HomeController : Controller
    {

        private readonly IRepositoryAsync<User> _repository;

        //public HomeController()
        //{

        //}

        public HomeController(IRepositoryAsync<User> userRepository)
        {
            _repository = userRepository;
        }

        public ActionResult Index()
        {
            List<User> users = new List<User>();
            users = _repository.Queryable(false).ToList();

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}