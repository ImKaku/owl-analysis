using System.Collections.Generic;

namespace OwlAnalysis.Model
{
    public class PlayerHeroStats
    {
        public string Id { get; set; }

        public virtual PlayerGame PlayerGame { get; set; }

        public virtual Hero Hero { get; set; }

        public virtual List<Stat> PlayerStats { get; set; } = new List<Stat>();

        public PlayerHeroStats()
        {

        }

        public PlayerHeroStats(PlayerGame playerGame, Hero hero)
        {
            this.PlayerGame = playerGame;
            this.Hero = hero;
        }

        public bool HasStat(string key)
        {
            foreach (Stat playerStat in PlayerStats)
            {
                if (playerStat.Key.Equals(key))
                {
                    return true;
                }
            }
            return false;
        }

        public object Stat(string key)
        {
            foreach (Stat playerStat in PlayerStats)
            {
                if (playerStat.Key.Equals(key))
                {
                    return playerStat.Value;
                }
            }

            return null;
        }

        public void AddStat(string key, string value)
        {
            Stat playerStat = new Stat(key, value);

            PlayerStats.Add(playerStat);
        }
    }
}