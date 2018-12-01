
using System.Collections.Generic;

namespace OwlAnalysis.Model{
    public class Player{
        public string Id{get; set;}

        public string Name {get; set;}

        public virtual List<PlayerGame> PlayerGames{get; set;} = new List<PlayerGame>();

    }
}