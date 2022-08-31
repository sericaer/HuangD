using HuangD.Interfaces;
using System;
using System.Collections;
using System.Collections.Generic;

namespace HuangD.Sessions
{
    public partial class Session : ISession
    {
        public IMap map { get; set; }
    }
}