using Microsoft.AspNetCore.Mvc.Rendering;
using Models.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Models.ViewModels
{
	public class CardVM
	{
		public Card Card { get; set; }
		public IEnumerable<SelectListItem> Types { get; set; }
        public IEnumerable<SelectListItem> CategoryTypes { get; set; }

		

    }
}
