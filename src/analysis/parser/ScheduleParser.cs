using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using OwlAnalysis.Model;
using System.Globalization;

namespace OwlAnalysis.Service
{
    public class ScheduleParser
    {
        public Dictionary<int, Stage> stages { get; set; } = new Dictionary<int, Stage>();

        public Dictionary<string, Player> players { get; set; } = new Dictionary<string, Player>();

        public Dictionary<TeamEnum, Team> teams { get; set; } = new Dictionary<TeamEnum, Team>();

        public ScheduleParser()
        {

        }

        public void parse(JObject jObject)
        {
            parseData((JObject)jObject["data"]);
        }

        public void parseData(JObject jObject)
        {
            var id = (int)jObject["id"]; // Also the year
            var startDate = (string)jObject["start-date"];
            var endDate = (string)jObject["end-date"];
            var matchNr = (int)jObject["seriesId"];

            parseStages((JArray)jObject["stages"]);
        }

        public void parseStages(JArray jArray)
        {
            foreach (JObject jObject in jArray.Children<JObject>())
            {
                parseStage(jObject);
            }
        }

        public void parseStage(JObject jObject)
        {

            bool enabled = (bool)jObject["enabled"];
            int stageId = (int)jObject["id"];
            JArray matches = (JArray)jObject["matches"];

            if (!stages.ContainsKey(stageId))
            {
                stages[stageId] = new Stage();
                stages[stageId].officialId = stageId;
            }

            Stage stage = stages[stageId];

            foreach (JObject matchObj in matches.Children<JObject>())
            {
                Match match = parseMatch(stage, matchObj);
            }
        }


        public Match parseMatch(Stage stage, JObject matchJObj)
        {
            int id = (int)matchJObj["id"];

            Match match = stage.findMatch(id);

            if (match == null)
            {
                match = new Match(stage);
                match.MatchNumber = id;
            }

            JArray competitors = (JArray)matchJObj["competitors"];
            var competitorsList = competitors.Children<JObject>();

            if (competitorsList.Count() != 2)
            {
                Console.WriteLine("Error");
            }

            match.HomeTeam = parseMatchCompetitor(competitorsList.ElementAt(0));
            match.AwayTeam = parseMatchCompetitor(competitorsList.ElementAt(1));

            match.start = DateTime.Parse((string)matchJObj["startDate"]);
            match.end = DateTime.Parse((string)matchJObj["endDate"]);

            Console.WriteLine(match.Stage.officialId + " " + GetIso8601WeekOfYear(match.start) + " " + match.HomeTeam.TeamEnum + " " + match.AwayTeam.TeamEnum);

            return match;
        }

        public Team parseMatchCompetitor(JObject competitor)
        {
            Team team = parseTeam((string)competitor["abbreviatedName"]);

            return team;
        }

        public Team parseTeam(string team)
        {
            TeamEnum teamEnum = parseTeamEnum(team);

            if (!teams.ContainsKey(teamEnum))
            {
                teams[teamEnum] = new Team(teamEnum);
            }

            return teams[teamEnum];
        }

        public static int GetIso8601WeekOfYear(DateTime time)
        {
            // Seriously cheat.  If its Monday, Tuesday or Wednesday, then it'll 
            // be the same week# as whatever Thursday, Friday or Saturday are,
            // and we always get those right
            DayOfWeek day = CultureInfo.InvariantCulture.Calendar.GetDayOfWeek(time);
            if (day >= DayOfWeek.Monday && day <= DayOfWeek.Wednesday)
            {
                time = time.AddDays(3);
            }

            // Return the week of our adjusted day
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }
        public TeamEnum parseTeamEnum(string team)
        {

            switch (team)
            {
                case "FLA":
                    return TeamEnum.mayham;
                case "SFS":
                    return TeamEnum.shock;
                case "VAL":
                    return TeamEnum.valient;
                case "SHD":
                    return TeamEnum.dragons;
                case "SEO":
                    return TeamEnum.dynasty;
                case "LDN":
                    return TeamEnum.spitfire;
                case "GLA":
                    return TeamEnum.gladiators;
                case "HOU":
                    return TeamEnum.outlaws;
                case "DAL":
                    return TeamEnum.fuel;
                case "BOS":
                    return TeamEnum.uprising;
                case "NYE":
                    return TeamEnum.excelsior;
                case "PHI":
                    return TeamEnum.fusion;
                case "HZS":
                    return TeamEnum.spark;
                case "GZC":
                    return TeamEnum.charge;
                case "CDH":
                    return TeamEnum.hunters;
                case "WAS":
                    return TeamEnum.justice;
                case "ATL":
                    return TeamEnum.reign;
                case "VAN":
                    return TeamEnum.titans;
                case "TOR":
                    return TeamEnum.defiant;
                case "PAR":
                    return TeamEnum.eternal;

            }

            throw new System.Exception("Not supported team exception " + team);
        }
    }
}