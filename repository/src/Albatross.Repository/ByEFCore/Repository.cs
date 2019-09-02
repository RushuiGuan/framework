﻿using Albatross.Repository.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Albatross.Repository.ByEFCore {
	public class Repository<T> : IRepository<T> where T : class {
		protected CustomDbContext customDbContext;
		protected DbSet<T> dbSet;
		public virtual IQueryable<T> Items => dbSet;
		public IDbSession DbSession => customDbContext;

		public Repository(CustomDbContext dbContext) {
			this.customDbContext = dbContext;
			dbSet = dbContext.Set<T>();
		}

		public virtual T GetItem(params object[] keys) {
			return dbSet.Find(keys);
		}

		public virtual void Add(T t) {
			this.dbSet.Add(t);
		}
		public virtual void AddRange(IEnumerable<T> items) {
			this.dbSet.AddRange(items);
		}

		public virtual void Update(T obj) {
			this.dbSet.Update(obj);
		}
		public virtual void UpdateRange(IEnumerable<T> items) {
			this.dbSet.UpdateRange(items);
		}

		public virtual void Remove(T obj) {
			dbSet.Remove(obj);
		}

		public virtual void RemoveRange(IEnumerable<T> items) {
			this.dbSet.RemoveRange(items);
		}

	}
}