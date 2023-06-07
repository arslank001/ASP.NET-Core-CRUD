using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CRUDCoreApplication.Data;
using Microsoft.AspNetCore.Mvc;
using CRUDCoreApplication.Models;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using CRUDCoreApplication.Models.ViewModel;

namespace CRUDCoreApplication.Controllers
{
    [Authorize]
    public class EmployeeController : Controller
    {
        private readonly ApplicationContext context;

        public EmployeeController(ApplicationContext context)
        {
            this.context = context;
        }

        public IActionResult Index()
        {
            var GetData = context.Employee.ToList();  
            return View(GetData);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(EmployeeModel EmpModel)
        {
            if (ModelState.IsValid)
            {
                var Emp = new EmployeeModel()
                {
                    Name = EmpModel.Name,
                    City = EmpModel.City,
                    State = EmpModel.State,
                    Salary = EmpModel.Salary
                };
                context.Employee.Add(Emp);
                context.SaveChanges();
                TempData["SuccessMsg"] = "Record has been added successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["ErrorMsg"] = "Please fill all fields first!";
                return View();
            }       
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var emp = context.Employee.SingleOrDefault(e => e.EmployeeID == id);
            var result = new EmployeeModel()
            {
                Name = emp.Name,
                City = emp.City,
                State = emp.State,
                Salary = emp.Salary
            };
            return View(result);
        }

        [HttpGet]
        public IActionResult RegisteredUsers()
        {
            List<SignupViewModel> viewModelList = new List<SignupViewModel>();
            var GetUsers = context.Users.ToList();
            foreach (var Users in GetUsers)
            {
                SignupViewModel viewModel = new SignupViewModel
                {
                    UserName = Users.UserName,
                    Email = Users.Email,
                    Password = DecryptPassword(Users.Password),
                    Mobile = Users.Mobile,
                    IsActive = Users.IsActive,
                    CreatedOn = Users.CreatedOn,
                    CreatedBy = Users.CreatedBy
                };
                viewModelList.Add(viewModel);
            }
            return View(viewModelList);
        }
        public static string DecryptPassword(string Password)
        {
            if (string.IsNullOrEmpty(Password))
            {
                return null;
            }
            else
            {
                byte[] EncryptedPassword = Convert.FromBase64String(Password);
                string DecryptedPassword = ASCIIEncoding.ASCII.GetString(EncryptedPassword);
                return DecryptedPassword;
            }
        }
        [HttpPost]
        public IActionResult Edit(EmployeeModel EmpModel, int id)
        {
            var emp = new EmployeeModel()
            {
                EmployeeID = id,
                Name = EmpModel.Name,
                City = EmpModel.City,
                State = EmpModel.State,
                Salary = EmpModel.Salary
            };
            context.Employee.Update(emp);
            context.SaveChanges();
            TempData["SuccessMsg"] = "Record has been updated successfully!";
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var Emp = context.Employee.SingleOrDefault(e => e.EmployeeID == id);
            context.Employee.Remove(Emp);
            context.SaveChanges();
            TempData["DeleteMsg"] = "Record has been deleted successfully!";
            return RedirectToAction("Index");
        }

        public IActionResult Testing()
        {
            return View();
        }
    }
}
