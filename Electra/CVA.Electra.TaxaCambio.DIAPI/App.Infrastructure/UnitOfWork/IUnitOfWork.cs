using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using App.Infrastructure.Domain;

namespace App.Infrastructure.UnitOfWork
{
	public interface IUnitOfWork
	{
		void RegisterUpdate(IAggregateRoot aggregateRoot, IUnitOfWorkRepository repository);
		void RegisterInsertion(IAggregateRoot aggregateRoot, IUnitOfWorkRepository repository);
		void RegisterDeletion(IAggregateRoot aggregateRoot, IUnitOfWorkRepository repository);
		void Commit();
	}
}
