using DataAccess.Repository;
using DataAccess.Repository.IRepository;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Models;
using Models.ViewModels;
using System.ComponentModel.DataAnnotations;

namespace VertigoCaffe.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class CardController : Controller
	{
		
		private readonly IUnitOFWork _unitOFWork;
		private readonly IWebHostEnvironment _webHost;

		public CardController(IUnitOFWork unitOFWork, IWebHostEnvironment webHost)
		{
			_unitOFWork = unitOFWork;
			_webHost = webHost;
		}

		public IActionResult Index()
		{
			return View(_unitOFWork.Card.GetAll());
		}

		public async Task<IActionResult> Create()
		{
			var k1ID = (await _unitOFWork.Type.FirstOrDefaultAsync(x => x.Name == "Kategorija")).Id;
            var k2ID = (await _unitOFWork.Type.FirstOrDefaultAsync(x => x.Name == "Proizvod")).Id;

            CardVM cardVM = new()
			{
				Card = new(),
				Types = _unitOFWork.Type.GetAll().ToList().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }),
				CategoryTypes = _unitOFWork.Card.GetAll(x =>
														(x.TypeId == k1ID ||
														x.TypeId == k2ID))
														.Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name })
			};

			return View(cardVM);
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Create(Card card, IFormFile image)
		{
			card.Created_at = DateTime.Now;
            card.Updated_at = DateTime.Now;
			

            // checking if model is valid, if not return create view.
            if (ModelState.IsValid && image != null)
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
					card.Image = @"\images\" + newName + extension;
					await _unitOFWork.Card.AddAsync(card);
					await _unitOFWork.SaveAsync();
					return RedirectToAction(nameof(Index));
				}
				catch (Exception e)
				{
					return View();
				}
			}
			else
			{
                var k1ID = (await _unitOFWork.Type.FirstOrDefaultAsync(x => x.Name == "Kategorija")).Id;
                var k2ID = (await _unitOFWork.Type.FirstOrDefaultAsync(x => x.Name == "Proizvod")).Id;

                CardVM cardVM = new()
                {
                    Card = new(),
                    Types = _unitOFWork.Type.GetAll().ToList().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }),
                    CategoryTypes = _unitOFWork.Card.GetAll(x =>
                                                            (x.TypeId == k1ID ||
                                                            x.TypeId == k2ID))
                                                            .Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name })
                };

                return View(cardVM);
            }

			
		}

		public async Task<IActionResult> Update(int id)
		{
            var k1ID = (await _unitOFWork.Type.FirstOrDefaultAsync(x => x.Name == "Kategorija")).Id;
            var k2ID = (await _unitOFWork.Type.FirstOrDefaultAsync(x => x.Name == "Proizvod")).Id;
            var card = await _unitOFWork.Card.FirstOrDefaultAsync(x => x.Id == id);
			if (card != null)
			{
				CardVM cardVM = new()
				{
					Card = card,
					Types = _unitOFWork.Type.GetAll().ToList().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString()}),
                    CategoryTypes = _unitOFWork.Card.GetAll(x =>
                                                           (x.TypeId == k1ID ||
                                                           x.TypeId == k2ID))
                                                            .Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name })
                };

				return View(cardVM);
			}

					// dodaj neku poruku da ne postoji
			return RedirectToAction(nameof(Index));
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		public async Task<IActionResult> Update(Card card, [ValidateNever][CustomizeValidator(Skip = true)] IFormFile image)
		{
            ModelState.Remove("image");
            if (!(ModelState.IsValid))
            {
                var k1ID = (await _unitOFWork.Type.FirstOrDefaultAsync(x => x.Name == "Kategorija")).Id;
                var k2ID = (await _unitOFWork.Type.FirstOrDefaultAsync(x => x.Name == "Proizvod")).Id;
                var cardTemp = await _unitOFWork.Card.FirstOrDefaultAsync(x => x.Id == card.Id);
                if (cardTemp != null)
                {
                    CardVM cardVM = new()
                    {
                        Card = cardTemp,
                        Types = _unitOFWork.Type.GetAll().ToList().Select(x => new SelectListItem() { Text = x.Name, Value = x.Id.ToString() }),
                        CategoryTypes = _unitOFWork.Card.GetAll(x =>
                                                               (x.TypeId == k1ID ||
                                                               x.TypeId == k2ID))
                                                                .Select(x => new SelectListItem() { Value = x.Id.ToString(), Text = x.Name })
                    };

                    return View(cardVM);
                }

                return NotFound();
			}
			else
			{
				if(image != null)
				{
					var root = _webHost.WebRootPath;
					var newName = Guid.NewGuid().ToString();
					var extension = Path.GetExtension(image.FileName);
					var newPath = Path.Combine(root, @"images\");

					var oldImagePath = (await _unitOFWork.Card.FirstOrDefaultAsync(x => x.Id == card.Id)).Image.TrimStart('\\');
					
					//deleting old photo
					if (System.IO.File.Exists(Path.Combine(root, oldImagePath)))
					{
						System.IO.File.Delete(Path.Combine(root, oldImagePath));
					}

					using (FileStream fileStream = new FileStream(Path.Combine(newPath, newName + extension), FileMode.Create))
					{
						await image.CopyToAsync(fileStream);
					}

				}
				card.Updated_at = DateTime.Now;
				await _unitOFWork.Card.updateAsync(card);
				await _unitOFWork.SaveAsync();
                return RedirectToAction(nameof(Index));
            }			
		}

		public async Task<IActionResult> Delete(int id)
		{
			var card = await _unitOFWork.Card.FirstOrDefaultAsync(x => x.Id == id);
			if(card != null)
			{
				// deleting old image
				var root = _webHost.WebRootPath;
				var oldPath = Path.Combine(root, card.Image.TrimStart('\\'));
				if(System.IO.File.Exists(oldPath))
				{
					System.IO.File.Delete(oldPath);
				}
				_unitOFWork.Card.Remove(card);
				await _unitOFWork.SaveAsync();
			}

			return RedirectToAction(nameof(Index));
		}



	}
}
