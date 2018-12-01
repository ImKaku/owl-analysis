using System.Collections.Generic;

namespace OwlAnalysis.Model{
    public class PlayerGame{
        public string Id{get; set;}

        public virtual Player Player{get; set;}

        public virtual Game Game{get; set;}

        public virtual List<PlayerHeroStats> HeroStats{get; set;} = new List<PlayerHeroStats>();

        public PlayerGame(){

        }

        public PlayerGame(Player player){
            this.Player = player;
            this.Player.PlayerGames.Add(this);
        }
    }
}