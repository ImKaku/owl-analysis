using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;


namespace OwlAnalysis.Model{
    public class Game{
        
        public string Id{get; set;}

        public virtual Match Match{get; set;} 

        public int GameNumber{get; set;}
        
        public Map Map{get; set;}

        public Game(){
        }

        public Game(Match match){
            this.Match = match;
        }

        public virtual List<PlayerGame> PlayerGames{get; set;} = new List<PlayerGame>();

        public PlayerGame FindPlayerGameForPlayer(Player player){
            foreach(var pg in PlayerGames){
                if(pg.Player.Name.Equals(player.Name)){
                    return pg;
                }
            }

            var playerGame = new PlayerGame(player);
            playerGame.Game = this;
            
            PlayerGames.Add(playerGame);


            return playerGame;
        }
    }
}