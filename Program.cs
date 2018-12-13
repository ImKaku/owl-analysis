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
namespace test
{
    class Program
    {

        static void Main(string[] args)
        {


            //importSchedule();
            //importStatData();

            using (var context = new OwlAnalysisContex())
            {
                MatchData matchData = new MatchData(context);
                var matches = matchData.AllMatches();

                Dictionary<TeamEnum, Dictionary<int, int>> matchPlayed = new Dictionary<TeamEnum, Dictionary<int, int>>();
                Dictionary<TeamEnum, int[]> played = new Dictionary<TeamEnum, int[]>();

                foreach (Match match in matches)
                {
                    int week = DateTimeHelper.GetIso8601WeekOfYear(match.Start);

                    if (!matchPlayed.ContainsKey(match.HomeTeam.TeamEnum))
                        matchPlayed[match.HomeTeam.TeamEnum] = new Dictionary<int, int>();
                    if (!matchPlayed.ContainsKey(match.AwayTeam.TeamEnum))
                        matchPlayed[match.AwayTeam.TeamEnum] = new Dictionary<int, int>();
                    if (!matchPlayed[match.HomeTeam.TeamEnum].ContainsKey(week))
                        matchPlayed[match.HomeTeam.TeamEnum][week] = 0;
                    if (!matchPlayed[match.AwayTeam.TeamEnum].ContainsKey(week))
                        matchPlayed[match.AwayTeam.TeamEnum][week] = 0;

                    matchPlayed[match.HomeTeam.TeamEnum][week]++;
                    matchPlayed[match.AwayTeam.TeamEnum][week]++;

                    if (match.HomeTeam.TeamEnum == TeamEnum.shock || match.AwayTeam.TeamEnum == TeamEnum.shock)
                    {
                        if (week == 9)
                        {
                            Console.WriteLine(match.HomeTeam.TeamEnum + " " + match.AwayTeam.TeamEnum);
                        }
                    }
                }
                foreach (KeyValuePair<TeamEnum, Dictionary<int, int>> entry in matchPlayed)
                {
                    if (!played.ContainsKey(entry.Key))
                    {
                        int[] defaultValues = { 0, 0, 0, 0 };
                        played[entry.Key] = defaultValues;
                    }

                    foreach (KeyValuePair<int, int> matchPlay in entry.Value)
                    {
                        int val = matchPlay.Value;
                        played[entry.Key][val]++;
                    }
                }

                foreach (KeyValuePair<TeamEnum, int[]> matchPlay in played)
                {
                    Console.WriteLine(matchPlay.Key + " Plays: ");

                    Console.WriteLine(matchPlay.Key + " 0 Games: " + matchPlay.Value[0]);
                    Console.WriteLine(matchPlay.Key + " 1 Games: " + matchPlay.Value[1]);
                    Console.WriteLine(matchPlay.Key + " 2 Games: " + matchPlay.Value[2]);
                    Console.WriteLine(matchPlay.Key + " 3 Games: " + matchPlay.Value[3]);
                }
            }

        }

        public static void importSchedule()
        {
            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), @"..\..\..\data\public\owl-schedule-12122018.json");
            String s = File.ReadAllText(path);
            var obj = JObject.Parse(s);

            ScheduleParser parser = new ScheduleParser();

            parser.parse(obj);

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
