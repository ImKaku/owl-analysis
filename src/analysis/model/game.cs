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

        public virtual List<PlayerGame> playerGames{get; set;} = new List<PlayerGame>();

        public PlayerGame FindPlayerGameForPlayer(Player player){
            foreach(var pg in playerGames){
                if(pg.Player.Name.Equals(player.Name)){
                    return pg;
                }
            }

            var playerGame = new PlayerGame(player);
            playerGame.Game = this;
            
            playerGames.Add(playerGame);


            return playerGame;
        }
    }
}