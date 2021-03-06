﻿using Evol.Common;
using Evol.MongoDB.Repository;
using Plunder.Plugin.Repositories;
using Plunder.Storage.MongoDB.Entities;
using System;

namespace Plunder.Storage.MongoDB.Repositories
{
    public class ZhaopinJobRepository : BaseMongoDbRepository<LagouJob, PlunderMongoDBContext>, IRepository<LagouJob>
    {
        public ZhaopinJobRepository(IMongoDbContextProvider mongoDbContextProvider) : base(mongoDbContextProvider)
        {
        }
    }
}
