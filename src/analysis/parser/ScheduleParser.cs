using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using OwlAnalysis.Model;
using System.Globalization;
using Commons.Tools;

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
            JArray weeks = (JArray)jObject["weeks"];

            if (!stages.ContainsKey(stageId))
            {
                stages[stageId] = new Stage();
                stages[stageId].officialId = stageId+1;
            }

            Stage stage = stages[stageId];

            foreach (JObject weekObj in weeks.Children<JObject>())
            {
                int week = (int)weekObj["id"];
                JArray matchesPerWeek = (JArray) weekObj["matches"];

                foreach (JObject matchObj in matchesPerWeek.Children<JObject>())
                {
                    Match match = parseMatch(stage, week, matchObj);

                }
            }

        }


        public Match parseMatch(Stage stage, int week, JObject matchJObj)
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

            match.Start = DateTime.Parse((string)matchJObj["startDate"]);
            match.End = DateTime.Parse((string)matchJObj["endDate"]);

            match.WeekNumber = week+1;

            //int week = GetIso8601WeekOfYear(match.start);

            Console.WriteLine(match.Stage.officialId + " " + match.WeekNumber + " " + match.HomeTeam.TeamEnum + " " + match.AwayTeam.TeamEnum);

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