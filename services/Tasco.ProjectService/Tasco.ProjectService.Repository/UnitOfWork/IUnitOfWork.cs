﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tasco.ProjectService.Repository.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        Task SaveChangesAsync();
    }
}
