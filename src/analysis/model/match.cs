using System.Collections.Generic;
using System;

namespace OwlAnalysis.Model
{
    public class Match
    {
        public string Id { get; set; }

        public virtual Stage Stage { get; set; }

        public int MatchNumber { get; set; }

        public DateTime Start { get; set; }

        public DateTime End { get; set; }

        public int WeekNumber { get; set; }

        public virtual Dictionary<int, Game> Games { get; set; } = new Dictionary<int, Game>();

        public virtual Team HomeTeam { get; set; }

        public virtual Team AwayTeam { get; set; }

        public virtual Team Winner { get; set; }

        public Match()
        {

        }

        public Match(Stage stage)
        {
            this.Stage = stage;
            this.Stage.Matches.Add(this);
        }

        public Game FindOrCreateGame(int gameNr, Map map)
        {
            foreach (var k in Games)
            {
                if (k.Value.GameNumber == gameNr && k.Value.Map == map)
                {
                    return k.Value;
                }
            }

            Game game = new Game(this);

            game.GameNumber = gameNr;
            game.Map = map;

            Games.Add(game.GameNumber, game);

            return game;
        }

        public int HomeScore()
        {
            if (HomeTeam == null)
            {
                return 0;
            }

            int score = 0;

            foreach (Game game in Games.Values)
            {
                if (game.Winner != null && game.Winner.OfficialId == HomeTeam.OfficialId)
                {
                    score++;
                }
            }

            return score;
        }

        public int AwayScore()
        {
            if (AwayTeam == null)
            {
                return 0;
            }

            int score = 0;

            foreach (Game game in Games.Values)
            {
                if (game.Winner != null && game.Winner.OfficialId == AwayTeam.OfficialId)
                {
                    score++;
                }
            }

            return score;
        }

        public Match SetHomeTeam(Team homeTeam)
        {
            if (HomeTeam != null)
            {
                throw new ArgumentException("Home team already set");
            }

            HomeTeam = homeTeam;
            HomeTeam.HomeMatches.Add(this);

            return this;
        }

        public Match SetAwayTeam(Team awayTeam)
        {
            if (AwayTeam != null)
            {
                throw new ArgumentException("Away team already set");
            }

            AwayTeam = awayTeam;
            awayTeam.AwayMatches.Add(this);

            return this;
        }
    }

}