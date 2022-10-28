﻿using HuangD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HuangD.Commands
{
    public class CommandChangePlayCountry : ICommand
    {
        public string key => "ChangePlayCountry";

        public string help => "Change play Country";

        public int minArgCount => 1;

        public int maxArgCount => 1;

        public List<Action<ICountry>> OnChangedListeners = new List<Action<ICountry>>();

        public void Exec(ISession session, string[] argvs)
        {
            var targetCountryName = argvs[0];

            var country = session.countries.Single(x => x.name == targetCountryName);
            session.playerCountry = country;

            foreach (var listener in OnChangedListeners)
            {
                listener(session.playerCountry);
            }
        }
    }
}