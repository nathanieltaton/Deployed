using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.General;
using System.Data;
using TESTIDENTITY.Data;
using TESTIDENTITY.Models;

namespace TESTIDENTITY.Controllers
{
    public class ClientController : Controller
    {
        private readonly ApplicationDbContext _repo;
        private readonly UserManager<IdentityUser> _userManager;
        public ClientController(ApplicationDbContext context,UserManager<IdentityUser> userManager)
        {
            _repo = context;
            _userManager = userManager;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
		public async Task<IActionResult> Index()
        {
            var clients =  await _repo.Clients.ToListAsync();
            return View(clients);
        }

		[Authorize(Roles = "Admin")]
		[HttpGet]
        public IActionResult Add()
        {
            return View();
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> Add(AddClientViewModel clientViewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(clientViewModel);
            }
            var client = new Client()
            {
                Id = Guid.NewGuid(),
                FirstName = clientViewModel.FirstName,
                LastName = clientViewModel.LastName,
                PhoneNumber = clientViewModel.PhoneNumber,
                EmailAddress = clientViewModel.EmailAddress,
                Address = clientViewModel.Address,
                Notes = clientViewModel.Notes,
                Birthday = clientViewModel.Birthday,
                IsAdmin = clientViewModel.IsAdmin,
            };

            await AssignUserRole(clientViewModel.EmailAddress,clientViewModel.IsAdmin);
            await _repo.Clients.AddAsync(client);
            await _repo.SaveChangesAsync();

            return RedirectToAction("Index");
        }
		[Authorize(Roles = "Admin")]
		[HttpGet]
		public async Task<IActionResult> View(Guid id)
        {
            var client = await _repo.Clients.FirstOrDefaultAsync(x => x.Id == id);

            if (client != null)
            {
                var viewModel = new UpdateClientViewModel()
                {
                    Id = client.Id,
                    FirstName = client.FirstName,
                    LastName = client.LastName,
                    PhoneNumber = client.PhoneNumber,
                    EmailAddress = client.EmailAddress,
                    Address = client.Address,
                    Notes = client.Notes,
                    Birthday = client.Birthday,
                    IsAdmin= client.IsAdmin,
                };
                return await Task.Run(() =>View("View",viewModel));
            }
            return RedirectToAction("Index");   
           
        }
		[Authorize(Roles = "Admin")]
		[HttpPost]
		public async Task<IActionResult> View(UpdateClientViewModel viewModel)
        {
            var client = await _repo.Clients.FindAsync(viewModel.Id);

            if (client != null)
            {
                client.FirstName = viewModel.FirstName;
                client.LastName = viewModel.LastName;
                client.PhoneNumber = viewModel.PhoneNumber;
                client.EmailAddress = viewModel.EmailAddress;
                client.Address = viewModel.Address;
                client.Notes = viewModel.Notes;
                client.Birthday = viewModel.Birthday;
                client.IsAdmin = viewModel.IsAdmin;

				await Task.Run( () => EditUserRole(client.EmailAddress, client.IsAdmin));
				await _repo.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return RedirectToAction("Index");
        }

		[Authorize(Roles = "Admin")]
		[HttpPost]
        public async Task<IActionResult> Delete(UpdateClientViewModel model)
        {
            var client = await _repo.Clients.FindAsync(model.Id);

            if(client != null)
            {
                _repo.Clients.Remove(client);
                await _repo.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> ClientProfile(string id)
        {
            var clientProfile = await _repo.Clients.FirstOrDefaultAsync(e=>e.EmailAddress == id);
            var user = User.IsInRole("Admin");
            user = User.IsInRole("Client");
            if (clientProfile != null)
            {
                return View("ClientProfile",clientProfile);
            }
            return RedirectToAction("NoProfile");
        }

        [HttpGet]
        public IActionResult NoProfile()
        {
            return View();
        }

        public async Task AssignUserRole(string email, bool role)
        {
            var userEmail = await _repo.Users.FirstOrDefaultAsync(x => x.Email == email);
            if(userEmail != null)
            {
				var roles = await _repo.Roles.FirstOrDefaultAsync(r => r.Name == (role ? "Admin" : "Client"));
				var newRole = new IdentityUserRole<string>
				{
					UserId = userEmail.Id,
					RoleId = roles.Id
				};
				await _repo.UserRoles.AddAsync(newRole);
            }
            else
            {
                string password = "Password01!";
                var user = new IdentityUser()
                {
                    UserName = email,
                    Email = email,
                };
                await _userManager.CreateAsync(user,password);
                await _userManager.AddToRoleAsync(user, (role ? "Admin" : "Client"));
            }
        }

        public async Task EditUserRole(string email, bool role)
        {
            //var userId = await _userManager.FindByEmailAsync(email);
            var userId = await _repo.Users.FirstOrDefaultAsync(x=>x.Email == email);

			var userEmail = await _repo.UserRoles.FirstOrDefaultAsync(x => x.UserId == userId.Id);
            var roles = await _repo.Roles.FirstOrDefaultAsync(r=> r.Name == (role ? "Admin" : "Client"));
			if (userEmail != null)
            {

                userEmail.RoleId = roles.Id;
            }
            else
            {
                var newRole = new IdentityUserRole<string>
                {
                    UserId = userId.Id,
                    RoleId = roles.Id
                };
               await _repo.UserRoles.AddAsync(newRole);
            }
			await _repo.SaveChangesAsync();
		}
    }
}
