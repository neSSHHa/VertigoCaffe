using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository.IRepository
{
	public interface IUnitOFWork
	{
		Task SaveAsync();
		public ICardRepsitory Card { get; }
		public ITypeRepository Type { get; }
        public IApplicationUserRepository ApplicationUser { get; }
    }
}
