﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plunder.Compoment;
using Plunder.Scheduler;

namespace Plunder.Analyze
{
    public class PageAnalyzer : IProducer
    {
        public IMessage<IEnumerable<Request>> Deliver()
        {
            throw new NotImplementedException();
        }
    }
}