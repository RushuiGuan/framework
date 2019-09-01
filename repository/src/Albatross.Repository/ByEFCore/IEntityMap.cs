using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Albatross.Repository.ByEFCore {
    
	public interface IEntityMap<T>  where T:class {
		void Map(EntityTypeBuilder<T> builder);
	}
}
