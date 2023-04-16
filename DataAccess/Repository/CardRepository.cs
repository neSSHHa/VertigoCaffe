using DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
	public class CardRepository : Repository<Card>, ICardRepsitory
	{
		private readonly ApplicationDbContext _db;
		public CardRepository(ApplicationDbContext db) : base(db)
		{
			_db = db;
		}

		public async Task updateAsync(Card card)
		{
			var itemFromDb = await _db.Cards.FirstOrDefaultAsync(x => x.Id == card.Id);
			if (itemFromDb != null)
			{
				itemFromDb.Name = card.Name;
				itemFromDb.Description = card.Description;
				itemFromDb.Price= card.Price;
				itemFromDb.Price2 = card.Price;
				itemFromDb.Price3 = card.Price;
				itemFromDb.MorePrices = card.Price;
				itemFromDb.Amount = card.Amount;
				itemFromDb.CardId = card.CardId;
				itemFromDb.TypeId = card.TypeId;
				itemFromDb.Updated_at = card.Updated_at;
				itemFromDb.Created_at = card.Created_at;
				if(card.Image != null && card.Image.Length > 0)
				{
					itemFromDb.Image = card.Image;
				}
				_db.Update(itemFromDb);
			}
			// napravi za sve ono da updata osim slike i tih stvari...
		}
	}
}
