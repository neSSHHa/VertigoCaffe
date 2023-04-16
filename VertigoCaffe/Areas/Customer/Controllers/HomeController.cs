using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using Models.Models;
using Models.ViewModels;
using NuGet.Packaging.Signing;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Security.Claims;
using System.Text.RegularExpressions;
using System.Web.WebPages;
using Utility;
using VertigoCaffe.Models;
using Validator = System.ComponentModel.DataAnnotations.Validator;

namespace VertigoCaffe.Areas.Customer.Controllers
{
    [Authorize]
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOFWork _unitOfWork;
        private readonly IWebHostEnvironment _webHost;
		private readonly UserManager<IdentityUser> _userManager;

		public HomeController(ILogger<HomeController> logger, IUnitOFWork unitOFWork, IWebHostEnvironment webHost, UserManager<IdentityUser> userMenager)
        {
            _logger = logger;
            _unitOfWork = unitOFWork;
            _webHost = webHost;
			_userManager = userMenager;
        }
        
        public async Task<IActionResult> Index()
        {
			var k1ID = (await _unitOfWork.Type.FirstOrDefaultAsync(x => x.Name == "Kategorija")).Id;
			var k2ID = (await _unitOfWork.Type.FirstOrDefaultAsync(x => x.Name == "Proizvod")).Id;
			

			var claims = (ClaimsIdentity)User.Identity;
            var claim = claims.FindFirst(ClaimTypes.NameIdentifier);
           
            ApplicationUser currentUser = await _unitOfWork.ApplicationUser.FirstOrDefaultAsync(x => x.Id == claim.Value, tracked: false);
            List<ApplicationUser> applicationUsers = _unitOfWork.ApplicationUser.GetAll(x => x.Id != claim.Value, tracked: false).ToList();
            List<Card> cards = _unitOfWork.Card.GetAll(included:"Parent,Type").ToList();

            ControlsVM controlsVM = new()
            {
                CurrentUser = currentUser,
                Users = applicationUsers,
                cards = cards,
                newCard = new(),
				Types = _unitOfWork.Type.GetAll().ToList().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }),
				CategoryTypes = _unitOfWork.Card.GetAll(x =>
													   (x.TypeId == k1ID ||
													   x.TypeId == k2ID))
															.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name }),
				Roles = new List<SelectListItem> { new SelectListItem() { Text = StaticDetails.ROLE_MODERATOR}, new SelectListItem() { Text = StaticDetails.ROLE_ADMIN } }
			};
			foreach(var item in controlsVM.Users)
			{
				var userRole = (await _userManager.GetRolesAsync(item)).FirstOrDefault();
				item.Role = userRole;
			}

            return View(controlsVM);
        }

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update([FromForm] ControlsVM controlsVM, [FromForm][ValidateNever][CustomizeValidator(Skip = true)] IFormFile image)
		{
			var cardToValidate = controlsVM.cards[controlsVM.EditedCardId]; // Get the card to validate using the EditedCardId
			var validationResults = new List<ValidationResult>();
			var validationContext = new ValidationContext(cardToValidate, null, null);
			bool isValid = Validator.TryValidateObject(cardToValidate, validationContext, validationResults, true);

			ModelState.ClearValidationState(nameof(Card));
			if (!isValid)
			{
			

				return RedirectToAction(nameof(Index));
			}
			else
			{
				if (image != null)
				{
					var root = _webHost.WebRootPath;
					var newName = Guid.NewGuid().ToString();
					var extension = Path.GetExtension(image.FileName);
					var newPath = Path.Combine(root, @"images\");

					var oldImagePath = (await _unitOfWork.Card.FirstOrDefaultAsync(x => x.Id == controlsVM.cards[controlsVM.EditedCardId].Id)).Image.TrimStart('\\');

					//deleting old photo
					if (System.IO.File.Exists(Path.Combine(root, oldImagePath)))
					{
						System.IO.File.Delete(Path.Combine(root, oldImagePath));
					}

					using (FileStream fileStream = new FileStream(Path.Combine(newPath, newName + extension), FileMode.Create))
					{
						await image.CopyToAsync(fileStream);
					}
					controlsVM.cards[controlsVM.EditedCardId].Image = @"\images\" + newName + extension;
				}
				
				controlsVM.cards[controlsVM.EditedCardId].Updated_at = DateTime.Now;
				await _unitOfWork.Card.updateAsync(controlsVM.cards[controlsVM.EditedCardId]);
				await _unitOfWork.SaveAsync();
				return RedirectToAction(nameof(Index));
			}
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> UpdateUser([FromForm] ControlsVM controlsVM, [FromForm][ValidateNever][CustomizeValidator(Skip = true)] IFormFile image)
		{
			var userToValidate = controlsVM.Users[controlsVM.EditedUserIndex]; // Get the card to validate using the EditedCardId
			var validationResults = new List<ValidationResult>();
			var validationContext = new ValidationContext(userToValidate, null, null);
			bool isValid = Validator.TryValidateObject(userToValidate, validationContext, validationResults, true);

			controlsVM.Role = controlsVM.Users[controlsVM.EditedUserIndex].Role;

			ModelState.ClearValidationState(nameof(ApplicationUser));
			if (!isValid)
			{
				return RedirectToAction(nameof(Index));
			}
			else
			{
				if (!controlsVM.Password.IsEmpty())
				{
					string passwordPater = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)(?=.*[#$^+=!*()@%&]).{8,}$";
					if(!Regex.IsMatch(controlsVM.Password, passwordPater, RegexOptions.None))
					{
						return RedirectToAction(nameof(Index));
					}
				}
				var olduser2 = (await _unitOfWork.ApplicationUser.FirstOrDefaultAsync(x => x.Id == controlsVM.Users[controlsVM.EditedUserIndex].Id, tracked: false));
				if (image != null)
				{
					var root = _webHost.WebRootPath;
					var newName = Guid.NewGuid().ToString();
					var extension = Path.GetExtension(image.FileName);
					var newPath = Path.Combine(root, @"images\");

					var oldImagePath = olduser2.Image.TrimStart('\\');

					//deleting old photo
					if (System.IO.File.Exists(Path.Combine(root, oldImagePath)))
					{
						System.IO.File.Delete(Path.Combine(root, oldImagePath));
					}

					using (FileStream fileStream = new FileStream(Path.Combine(newPath, newName + extension), FileMode.Create))
					{
						await image.CopyToAsync(fileStream);
					}
					olduser2.Image = @"\images\" + newName + extension;
				}

				//var oldUser = await _userManager.FindByIdAsync(controlsVM.Users[controlsVM.EditedUserIndex].Id);
				
				if (olduser2 != null)
				{
					string userRole = (await _userManager.GetRolesAsync(olduser2)).FirstOrDefault();
					// do something with userRole
					if (userRole != controlsVM.Role)
					{
						await _userManager.RemoveFromRoleAsync(olduser2, userRole);
						await _userManager.AddToRoleAsync(olduser2, controlsVM.Role);
					}
					if (!controlsVM.Password.IsEmpty())
					{
						var token = await _userManager.GeneratePasswordResetTokenAsync(olduser2);
						await _userManager.ResetPasswordAsync(olduser2, token,controlsVM.Password);
					}
					if(controlsVM.Users[controlsVM.EditedUserIndex].UserName != olduser2.UserName)
					{
						await _userManager.SetUserNameAsync(olduser2, controlsVM.Users[controlsVM.EditedUserIndex].UserName);
					}
					
				}
				olduser2.PhoneNumber = controlsVM.Users[controlsVM.EditedUserIndex].PhoneNumber;
				olduser2.Email = controlsVM.Users[controlsVM.EditedUserIndex].Email;
				_unitOfWork.ApplicationUser.DetachAllApplicationUsers();
				_unitOfWork.ApplicationUser.Update(olduser2);
				await _unitOfWork.SaveAsync();
				return RedirectToAction(nameof(Index));
			}
		}


		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateUser([FromForm] ControlsVM controlsVM, [FromForm][ValidateNever][CustomizeValidator(Skip = true)] IFormFile image)
		{


			var userToValidate = controlsVM.NewUser; // Get the card to validate using the EditedCardId
			var validationResults = new List<ValidationResult>();
			var validationContext = new ValidationContext(userToValidate, null, null);
			bool isValid = Validator.TryValidateObject(userToValidate, validationContext, validationResults, true);

			// checking if model is valid, if not return create view.
			if (isValid)
			{
				try
				{
					// setting up info for image to be created at wwwrooot/images.
					var root = _webHost.WebRootPath;
					var newName = Guid.NewGuid().ToString();
					var extension = Path.GetExtension(image.FileName);
					var newPath = Path.Combine(root, @"images\");

					// creating FileStream and getting wwwroot/images folder open.
					using (FileStream fileStream = new FileStream(Path.Combine(newPath, newName + extension), FileMode.Create))
					{
						await image.CopyToAsync(fileStream);
					}
					

					_userManager.CreateAsync(new ApplicationUser
					{
						UserName = controlsVM.NewUser.UserName,
						NormalizedUserName = controlsVM.NewUser.UserName,
						Email = controlsVM.NewUser.Email,
						NormalizedEmail = controlsVM.NewUser.Email,
						PhoneNumber = controlsVM.NewUser.PhoneNumber,
						EmailConfirmed = true,
						PhoneNumberConfirmed = true,
						Image = @"\images\" + newName + extension
					}, controlsVM.Password).GetAwaiter().GetResult();
					ApplicationUser user = await _unitOfWork.ApplicationUser.FirstOrDefaultAsync(x => x.Email == controlsVM.NewUser.Email);
					_userManager.AddToRoleAsync(user, controlsVM.Role).GetAwaiter().GetResult();
		
					return RedirectToAction(nameof(Index));
				}
				catch (Exception e)
				{
					return RedirectToAction(nameof(Index));
				}
			}
			// add some error here 
			return RedirectToAction(nameof(Index));
		}




		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> CreateItem([FromForm] ControlsVM controlsVM, [FromForm][ValidateNever][CustomizeValidator(Skip = true)] IFormFile image)
		{
			controlsVM.newCard.Created_at = DateTime.Now;
			controlsVM.newCard.Updated_at = DateTime.Now;

			var cardToValidate = controlsVM.newCard; // Get the card to validate using the EditedCardId
			var validationResults = new List<ValidationResult>();
			var validationContext = new ValidationContext(cardToValidate, null, null);
			bool isValid = Validator.TryValidateObject(cardToValidate, validationContext, validationResults, true);

			// checking if model is valid, if not return create view.
			if (isValid)
			{
				try
				{
					// setting up info for image to be created at wwwrooot/images.
					var root = _webHost.WebRootPath;
					var newName = Guid.NewGuid().ToString();
					var extension = Path.GetExtension(image.FileName);
					var newPath = Path.Combine(root, @"images\");

					// creating FileStream and getting wwwroot/images folder open.
					using (FileStream fileStream = new FileStream(Path.Combine(newPath, newName + extension), FileMode.Create))
					{
						await image.CopyToAsync(fileStream);
					}
					controlsVM.newCard.Image = @"\images\" + newName + extension;
					await _unitOfWork.Card.AddAsync(controlsVM.newCard);
					await _unitOfWork.SaveAsync();
					return RedirectToAction(nameof(Index));
				}
				catch (Exception e)
				{
					return RedirectToAction(nameof(Index));
				}
			}
			// add some error here 
			return RedirectToAction(nameof(Index));
		}


		[ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

		public async Task<IActionResult> Delete(int id)
        {
			var card = await _unitOfWork.Card.FirstOrDefaultAsync(x => x.Id == id);
			if (card != null)
			{
				// deleting old image
				var root = _webHost.WebRootPath;
				var oldPath = Path.Combine(root, card.Image.TrimStart('\\'));
				if (System.IO.File.Exists(oldPath))
				{
					System.IO.File.Delete(oldPath);
                }
				var childCards = _unitOfWork.Card.GetAll(x => x.CardId == id,tracked: false).ToList();
				foreach(var item in childCards)
				{
					item.CardId = null;
					await _unitOfWork.Card.updateAsync(item);
					await _unitOfWork.SaveAsync();
				}
				_unitOfWork.Card.Remove(card);
				await _unitOfWork.SaveAsync();
			}

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> DeleteUser(string id)
		{
			var user = await _unitOfWork.ApplicationUser.FirstOrDefaultAsync(x => x.Id == id);
			if (user != null)
			{
				// deleting old image
				var root = _webHost.WebRootPath;
				var oldPath = Path.Combine(root, user.Image.TrimStart('\\'));
				if (System.IO.File.Exists(oldPath))
				{
					System.IO.File.Delete(oldPath);
				}
				await _userManager.DeleteAsync(user);
			}

			return RedirectToAction(nameof(Index));
		}


		//api to call
		[HttpGet]
		[AllowAnonymous]
        public async Task<IActionResult> Proizvodi(int? id)
        {
            if (id == null || id == 0)
            {
                var mainItems = _unitOfWork.Card.GetAll(x => x.CardId == 0 || x.CardId == null, tracked: false, included: "Type,Parent");
                return Json(mainItems);

            }
            var item = await _unitOfWork.Card.FirstOrDefaultAsync(x => x.Id == id,included:"Type,Parent", tracked: false);
            if(item == null)
            {
                return NotFound();
            }
            if(item.Type.Name == "Proizvod")
            {
				item.Appendices = _unitOfWork.Card.GetAll(x => x.CardId == item.Id).ToList();
				foreach(var addon in item.Appendices)
				{
					addon.Parent = null;
				}
                return Json( item );
            }
            var items = _unitOfWork.Card.GetAll(x => x.CardId == id, included: "Type,Parent",tracked: false).ToList();
			foreach(var card in items)
			{
				card.Appendices = _unitOfWork.Card.GetAll(x => x.CardId == card.Id, tracked: false).ToList();
				foreach (var addon in card.Appendices)
				{
					addon.Parent = null;
					
				}
			}
            return Json(items);


        }

    }
}