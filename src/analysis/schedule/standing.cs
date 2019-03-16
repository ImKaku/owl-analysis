using OwlAnalysis.Model;

namespace OwlAnalysis.Schedule
{
    public class Standing
    {
        public Team Team { get; set; }

        public Standing(Team team)
        {
            Team = team;
        }

        public int MapScore()
        {
            int mapScore = 0;

            foreach (Match match in Team.Matches())
            {
                foreach (Game game in match.Games.Values)
                {
                    if (game.Winner == null)
                    {
                        continue;
                    }
                    if (game.Winner.OfficialId == Team.OfficialId)
                    {
                        mapScore++;
                    }
                    else
                    {
                        mapScore--;
                    }
                }
            }

            return mapScore;
        }

        public int MatchWins()
        {
            int matchScore = 0;

            foreach (Match match in Team.Matches())
            {
                if (match.Winner == null)
                {
                    continue;
                }

                if (match.Winner.OfficialId == Team.OfficialId)
                {
                    matchScore++;
                }
            }

            return matchScore;
        }
        public int MatchesPlayed()
        {
            int matchPlayed = 0;

            foreach (Match match in Team.Matches())
            {
                if (match.Winner != null)
                {
                    matchPlayed++;
                }
            }

            return matchPlayed;
        }

        public int MatchLosses(){
            return MatchesPlayed() - MatchWins();
        }
    }


}

