using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Models
{
	public class Card
	{
		[Key]
		public int Id { get; set; }

		public string Name { get; set; }

		[ValidateNever]
		public string Description { get; set; } = "";

		public double Price { get; set; }
		public double Price2 { get; set; }
		public double Price3 { get; set; }
		public double Price4 { get; set; }
		public double MorePrices { get; set; }

		[ValidateNever]
		public string Image { get; set; }

		[Required]
		public int Amount { get; set; }

        [ValidateNever]
        public int? CardId { get; set; }
		[ForeignKey("CardId")]
		[ValidateNever]
		public Card Parent { get; set; }

        [ValidateNever]
        public int TypeId { get; set; }
		[ForeignKey("TypeId")]
		[ValidateNever]
		public Type Type { get; set; }

        [ValidateNever]
        public DateTime Created_at { get; set; }
        [ValidateNever]
        public DateTime Updated_at { get; set; }

		[ValidateNever]
		[NotMapped]
		public List<Card>? Appendices { get; set; }


	}
}
