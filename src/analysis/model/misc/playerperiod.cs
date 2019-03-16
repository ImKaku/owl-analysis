using System.Collections.Generic;
using System;

namespace OwlAnalysis.Model
{
    public class PlayerPeriod
    {
        public string Id { get; set; }

        public DateTime start;

        public DateTime end;

        public Player player;
    }
}