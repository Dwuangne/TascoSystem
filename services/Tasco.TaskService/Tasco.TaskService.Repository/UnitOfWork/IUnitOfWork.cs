using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tasco.TaskService.Repository.Repositories;

namespace Tasco.TaskService.Repository.UnitOfWork
{
	public interface IUnitOfWork : IGenericRepositoryFactory, IDisposable
	{
		int Commit();

		Task<int> CommitAsync();
	}

	public interface IUnitOfWork<TContext> : IUnitOfWork where TContext : DbContext
	{
		TContext Context { get; }
	}
}
