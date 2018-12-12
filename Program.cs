using System;
using System.IO;
using System.Reflection;
using System.Text;
using Newtonsoft.Json.Linq;
using OwlAnalysis.Service;
using System.Collections.Generic;
using System.Linq;
using OwlAnalysis.Dao;
using OwlAnalysis.Model;

namespace test
{
    class Program
    {
        
        static void Main(string[] args)
        {
            
            
            importSchedule();
            //importStatData();

            return;
            
            using (var context = new OwlAnalysisContex()){
                PlayerData playerData = new PlayerData(context);
                MatchData matchData = new MatchData(context);
                
                Player muma = playerData.FindPlayerByName("muma");

                Game longestGame = null;
                var longest = 0.0;
                
                foreach(PlayerGame pg in muma.PlayerGames){
                    Console.Write(pg.Game.Match.HomeTeam + " " + pg.Game.Match.AwayTeam);

                    foreach(PlayerHeroStats phs in pg.HeroStats){
                        Console.Write(" - "+ phs.Hero + "(");
                        foreach(Stat stat in phs.PlayerStats){
                            Console.Write(stat.Key + " " + stat.Value + ",");
                        }
                        Console.WriteLine(")");
                    }

                }
            }
        }

        public static void importSchedule(){
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\data\public\owl-schedule-12122018.json");
            String s = File.ReadAllText(path);
            var obj = JObject.Parse(s);

            ScheduleParser parser = new ScheduleParser();

            parser.parse(obj);
        }

        static private void importStatData()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\data\private\data.json");
            String s = File.ReadAllText(path);
            var array = JArray.Parse(s);

            StatParser parser = new StatParser();

            parser.parse(array);

            using (var context = new OwlAnalysisContex())
            {
                var isCreated = context.Database.EnsureCreated();

                Console.WriteLine(isCreated);

                foreach (var stage in parser.stages.Values)
                {
                    context.Stages.Add(stage);
                }

                context.SaveChanges();
            }
        }
    }
}
