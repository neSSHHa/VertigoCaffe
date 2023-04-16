using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModels
{
    public class ControlsVM
    {
        public ApplicationUser CurrentUser { get; set; }
        public List<Card> cards { get; set; }
        public List<ApplicationUser> Users { get; set; }

		public IEnumerable<SelectListItem> Types { get; set; }
		public IEnumerable<SelectListItem> CategoryTypes { get; set; }
		public IEnumerable<SelectListItem> Roles { get; set; }

		public Card newCard { get; set; }

		public ApplicationUser NewUser { get; set; }

		public string Role { get; set; }

		[DataType(DataType.Password)]
		public string Password { get; set; }

		public int EditedCardId { get; set; }

		public int EditedUserIndex { get; set; }
	}
}
