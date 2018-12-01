using System.Collections.Generic;

namespace OwlAnalysis.Model{
    public class Match{
        public string Id{get; set;}

        public virtual Stage Stage{get; set;}

        public int MatchNumber{get; set;}

        public virtual List<Game> Games{get; set;} = new List<Game>();

        public Team HomeTeam{get; set;}

        public Team AwayTeam{get; set;}

        public Match(){

        }

        public Match(Stage stage){
            this.Stage = stage;
        }

        public Game FindOrCreateGame(int gameNr, Map map){
            foreach(var g in Games){
                if(g.GameNumber == gameNr && g.Map == map){
                    return g;
                }
            }

            Game game = new Game(this);

            game.GameNumber = gameNr;
            game.Map = map;

            Games.Add(game);

            return game;
        }
    }
}