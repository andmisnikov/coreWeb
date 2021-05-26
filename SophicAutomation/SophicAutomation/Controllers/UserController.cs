using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using BL.Dto;
using BL.Interfaces;
using Common;
using DAL.Entities;
using LinqKit;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SophicAutomation.PageHelpers;

namespace SophicAutomation.Controllers
{
    [Authorize]
    public class UserController : Controller
    {
        private readonly ILogger<HomeController> logger;
        private readonly IUserService userService;
        private UserManager<ApplicationUser> userManager;

        public UserController(ILogger<HomeController> logger, IUserService userService, UserManager<ApplicationUser> userManager)
        {
            this.logger = logger;
            this.userService = userService;
            this.userManager = userManager;
        }


        private Expression<Func<UserDto, bool>> BuildExpressionToSearchByFields(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                return item => true;
            }

            var predicate = PredicateBuilder.Or<UserDto>(
                    user => user.Name != null && user.Name.Contains(searchString),
                    user => user.Surname != null && user.Surname.Contains(searchString))
                .Or(user => user.UserName != null && user.UserName.Contains(searchString))
                .Or(user => user.Email != null && user.Email.Contains(searchString))
                .Or(user => user.City != null && user.City.Contains(searchString))
                .Or(user => user.Street != null && user.Street.Contains(searchString)).Or(
                    user => user.Zip != null && user.Zip.Contains(searchString));
            return predicate;
        }

        public FileContentResult DownloadCsv(string currentFilter)
        {
            CsvExport export = new CsvExport { Delimiter = "," };
            var users = this.userService.GetPage( 1, Int32.MaxValue, this.BuildExpressionToSearchByFields(currentFilter), out int count);
            foreach (var user in users)
            {
                export.AddRow();

                export["User Name"] = user.UserName;
                export["Email"] = user.Email;
                export["Name"] = user.Name;
                export["Surname"] = user.Surname; 
                export["City"] = user.City;
                export["Street"] = user.Street;
                export["Zip"] = user.Zip;
                export["Register Date"] = user.RegisterDate.ToUniversalTime();
            }

            var bytes = export.ExportToBytes();

            return File(bytes, "text/csv", "users.csv");
        }

        public async Task<IActionResult> Index(string currentFilter, string searchString, int? pageNumber)
        {
            int pageSize = 5;

            if (searchString != null)
            {
                pageNumber = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            this.ViewData["CurrentFilter"] = searchString;

            var items = this.userService.GetPage(pageNumber ?? 1, pageSize, this.BuildExpressionToSearchByFields(searchString), out int count);

            return this.View(PaginatedList<UserDto>.Create(items, count, pageNumber ?? 1, pageSize));
        }

        [Authorize(Roles = "Administrator")]
        public async Task<IActionResult> Delete(string id)
        {
            var currentUserId = this.userManager.GetUserId(this.User); // Get user id:
            if (currentUserId == id)
            {
                return this.RedirectToAction("Index");
            }

            await this.userService.Delete(id);
            return this.RedirectToAction("Index");
        }

        public IActionResult EditPersonalInfo()
        {
            var currentUserId = this.userManager.GetUserId(this.User);
            var user = this.userService.FindById(currentUserId);
            return this.View("Edit", user);
        }

        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return this.NotFound();
            }

            bool isAdmin = User.IsInRole("Administrator");
            var currentUserId = this.userManager.GetUserId(this.User);
            if (!isAdmin && currentUserId != id)
            {
                return this.Redirect("~/Identity/Account/AccessDenied");
            }

            var user = this.userService.FindById(id);
            if (user == null)
            {
                return this.NotFound();
            }

            return this.View(user);
        }

        // POST: users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("UserName,Email,Name,Surname,Street,Zip,City,RegisterDate,ConcurrencyStamp,Id")] UserDto user)
        {
            if (id != user.Id)
            {
                return this.NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    bool isAdmin = User.IsInRole("Administrator");
                    var currentUserId = this.userManager.GetUserId(this.User);
                    if (!isAdmin && currentUserId != id)
                    {
                        return this.Unauthorized();
                    }

                    await this.userService.UpdatePersonalInfo(user);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!this.userService.Any(id))
                    {
                        return this.NotFound();
                    }

                    throw;
                }

                return RedirectToAction(nameof(Index));
            }

            return this.View(user);
        }

        public async Task<IActionResult> UsersRegisteredPerDay()
        {
            var items = await this.userService.UsersRegisteredPerDay();
            return Json(items);
        }
    }
}