﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plunder.Download
{
    public struct UserAgent
    {
        public string Agent { get; }

        public UserAgent(string agent)
        {
            Agent = agent;
        }
    }
}
