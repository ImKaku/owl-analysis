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

            //import(parser());

            using (var context = new OwlAnalysisContex()){
                PlayerData playerData = new PlayerData(context);
                Player gido = playerData.FindPlayerByName("gido");

                Console.WriteLine(gido.Name);
                
                foreach(PlayerGame pg in gido.PlayerGames){
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

        static private JsonParser parser(){
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\data\data.json");
            String s = File.ReadAllText(path);
            var array = JArray.Parse(s);

            JsonParser parser = new JsonParser();

            parser.parse(array);

            return parser;
        }

        static private void import(JsonParser parser)
        {
            
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
