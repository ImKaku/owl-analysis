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
using Commons.Tools;
using OwlAnalysis.Schedule;

namespace test
{
    class Program
    {

        static void Main(string[] args)
        {


            importSchedule();
            //importStatData();

            //checkStatsHero("");
        }

        public static void checkStatsHero(string hero)
        {
            using (var context = new OwlAnalysisContex())
            {
                var stats = new StatData(context);
                
                foreach (PlayerHeroStats stat in stats.allStats())
                {
                    if (stat.Hero != Hero.hanzo)
                    {
                        continue;
                    }

                    var finals = (double)stat.Stat("final_blows");
                    var deaths = (double)stat.Stat("deaths");
                    var eliminations = (double)stat.Stat("eliminations");
                    var critical_hits = (double)stat.Stat("critical_hits");
                    var damage = (double)stat.Stat("damage");
                    var accuracy_avg = (double)stat.Stat("accuracy_avg");

                    Console.WriteLine(
                            "f" + finals +
                            "d" + deaths +
                            "e" + eliminations +
                            "c" + critical_hits +
                            "d" + damage +
                            "a" + accuracy_avg
                            );
                }
            }
            
        }

        public static void checkAvgKDRatios()
        {
            using (var context = new OwlAnalysisContex())
            {
                var stats = context.PlayerHeroStats;

                foreach (PlayerHeroStats stat in stats)
                {
                    if (!stat.HasStat("final_blows") || !stat.HasStat("deaths"))
                    {
                        continue;
                    }

                    var finals = (int)stat.Stat("final_blows");
                    var deaths = (int)stat.Stat("deaths");
                    var kd = finals - deaths;

                    if (kd > 10)
                    {
                        Console.WriteLine(stat.PlayerGame.Player.Name);
                    }
                }
            }
        }

        public static void checkB2bGames()
        {
            using (var context = new OwlAnalysisContex())
            {
                MatchData matchData = new MatchData(context);
                var matches = matchData.AllMatches();

                Match last = null;
                Team team1 = null;
                Team team2 = null;


                List<Match> sortedMatches = matches.OrderBy(o => o.Start).ToList();

                foreach (Match m in sortedMatches)
                {
                    if (last == null)
                    {
                        last = m;
                        team1 = m.HomeTeam;
                        team2 = m.AwayTeam;
                        continue;
                    }

                    var id1 = m.HomeTeam.Id;
                    var id2 = m.AwayTeam.Id;

                    if ((team1.Id == id1 && team2.Id == id2) || (team1.Id == id2 && team2.Id == id1))
                    {
                        Console.WriteLine(last.Start + " " + m.Start + " " + m.HomeTeam.TeamEnum + " " + m.AwayTeam.TeamEnum);
                    }

                    last = m;
                    team1 = m.HomeTeam;
                    team2 = m.AwayTeam;
                }
            }
        }

        public static void countNumberOfGamesPerWeek()
        {
            using (var context = new OwlAnalysisContex())
            {
                MatchData matchData = new MatchData(context);
                var matches = matchData.AllMatches();


                Dictionary<string, int> matchPlayed = new Dictionary<string, int>();

                Console.WriteLine(matches.Count());

                foreach (Match match in matches)
                {
                    string week = match.Stage.officialId + "" + match.WeekNumber;

                    if (!matchPlayed.ContainsKey(week))
                        matchPlayed[week] = 0;

                    matchPlayed[week]++;
                }

                foreach (KeyValuePair<string, int> play in matchPlayed)
                {
                    Console.WriteLine(play.Key + " " + play.Value);
                }
            }
        }

        public static void countGamesPerTeam()
        {
            using (var context = new OwlAnalysisContex())
            {
                MatchData matchData = new MatchData(context);
                var matches = matchData.AllMatches();

                Dictionary<TeamEnum, int> matchPlayed = new Dictionary<TeamEnum, int>();

                Console.WriteLine(matches.Count());

                foreach (Match match in matches)
                {
                    if (!matchPlayed.ContainsKey(match.HomeTeam.TeamEnum))
                        matchPlayed[match.HomeTeam.TeamEnum] = 0;
                    if (!matchPlayed.ContainsKey(match.AwayTeam.TeamEnum))
                        matchPlayed[match.AwayTeam.TeamEnum] = 0;

                    matchPlayed[match.HomeTeam.TeamEnum]++;
                    matchPlayed[match.AwayTeam.TeamEnum]++;
                }

                foreach (KeyValuePair<TeamEnum, int> play in matchPlayed)
                {
                    Console.WriteLine(play.Key + " " + play.Value);
                }
            }
        }

        public static void countGamesPerWeek()
        {
            using (var context = new OwlAnalysisContex())
            {
                MatchData matchData = new MatchData(context);
                var matches = matchData.AllMatches();

                Dictionary<TeamEnum, Dictionary<string, int>> matchPlayed = new Dictionary<TeamEnum, Dictionary<string, int>>();
                Dictionary<TeamEnum, int[]> played = new Dictionary<TeamEnum, int[]>();

                Console.WriteLine(matches.Count());

                foreach (Match match in matches)
                {
                    string week = match.Stage.officialId + "" + match.WeekNumber;

                    if (!matchPlayed.ContainsKey(match.HomeTeam.TeamEnum))
                        matchPlayed[match.HomeTeam.TeamEnum] = new Dictionary<string, int>();
                    if (!matchPlayed.ContainsKey(match.AwayTeam.TeamEnum))
                        matchPlayed[match.AwayTeam.TeamEnum] = new Dictionary<string, int>();
                    if (!matchPlayed[match.HomeTeam.TeamEnum].ContainsKey(week))
                        matchPlayed[match.HomeTeam.TeamEnum][week] = 0;
                    if (!matchPlayed[match.AwayTeam.TeamEnum].ContainsKey(week))
                        matchPlayed[match.AwayTeam.TeamEnum][week] = 0;

                    matchPlayed[match.HomeTeam.TeamEnum][week]++;
                    matchPlayed[match.AwayTeam.TeamEnum][week]++;

                    Console.WriteLine(match.Stage.officialId);
                }
                foreach (KeyValuePair<TeamEnum, Dictionary<string, int>> entry in matchPlayed)
                {
                    if (!played.ContainsKey(entry.Key))
                    {
                        int[] defaultValues = { 0, 0, 0 };
                        played[entry.Key] = defaultValues;
                    }

                    foreach (KeyValuePair<string, int> matchPlay in entry.Value)
                    {
                        int val = matchPlay.Value;
                        played[entry.Key][val]++;
                    }

                    //fill empty
                    foreach (KeyValuePair<string, int> matchPlay in entry.Value)
                    {
                        int val = matchPlay.Value;
                        played[entry.Key][0] = 20 - played[entry.Key][1] - played[entry.Key][2];
                    }

                }



                foreach (KeyValuePair<TeamEnum, int[]> matchPlay in played)
                {
                    Console.WriteLine(matchPlay.Key + " Plays: ");

                    Console.WriteLine(matchPlay.Key + " 0 Games: " + matchPlay.Value[0]);
                    Console.WriteLine(matchPlay.Key + " 1 Games: " + matchPlay.Value[1]);
                    Console.WriteLine(matchPlay.Key + " 2 Games: " + matchPlay.Value[2]);
                }
            }
        }

        public static void importSchedule()
        {
            //string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\data\public\owl-schedule-12122018.json");
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\data\public\owl-schedule-15032019.json");
            
            String s = File.ReadAllText(path);
            var obj = JObject.Parse(s);

            ScheduleParser parser = new ScheduleParser();

            parser.parse(obj);

            foreach(var team in parser.teams.Values){
                var standing = new Standing(team);

                Console.WriteLine(team.TeamEnum + " " + standing.MatchWins() + "-" + standing.MatchLosses() + ":" + standing.MapScore());
            }

            List<Match> matches = parser.stages[0].Matches;

            new Schedule().StrengthOfSchedule(matches);

            using (var context = new OwlAnalysisContex())
            {
                //var isCreated = context.Database.EnsureCreated();

                //Console.WriteLine(isCreated);

                foreach (var stage in parser.stages.Values)
                {
                 //   context.Stages.Add(stage);
                }

                //context.SaveChanges();
            }
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
