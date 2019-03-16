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
                stages[stageId].officialId = stageId + 1;
            }

            Stage stage = stages[stageId];

            foreach (JObject weekObj in weeks.Children<JObject>())
            {
                int week = (int)weekObj["id"];
                JArray matchesPerWeek = (JArray)weekObj["matches"];

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
                // competitors have not been decided yet 
                return match;
            }

            match.SetHomeTeam(parseMatchCompetitor(competitorsList.ElementAt(0)));
            match.SetAwayTeam(parseMatchCompetitor(competitorsList.ElementAt(1)));

            match.Start = DateTime.Parse((string)matchJObj["startDate"]);
            match.End = DateTime.Parse((string)matchJObj["endDate"]);

            match.WeekNumber = week + 1;

            JArray gamesJArray = (JArray)matchJObj["games"];

            foreach (JObject gameJObject in gamesJArray.Children<JObject>())
            {
                Game game = parseGame(match, gameJObject);
                match.Games.Add(game.GameNumber, game);
            }

            if (matchJObj["winner"] != null)
            {
                JObject winner = (JObject)matchJObj["winner"];
                int winnerId = (int)winner["id"];

                if (winnerId == match.HomeTeam.OfficialId)
                {
                    match.Winner = match.HomeTeam;
                }
                if (winnerId == match.AwayTeam.OfficialId)
                {
                    match.Winner = match.AwayTeam;
                }
            }

            if (matchJObj["wins"] != null)
            {

            }


            //int week = GetIso8601WeekOfYear(match.start);
            String winnerEnum = match.Winner != null ? match.Winner.TeamEnum.ToString() : null;
            int homeScore = match.HomeScore();
            int awayScore = match.AwayScore();

            Console.WriteLine(match.Stage.officialId + " " + match.WeekNumber + " " + match.HomeTeam.TeamEnum + " " + match.AwayTeam.TeamEnum + " | Winner: " + winnerEnum + " | (" + homeScore + "-" + awayScore + ")");

            return match;
        }

        public Game parseGame(Match match, JObject gameJObject)
        {
            Game game = new Game(match);
            game.OfficialId = (int)gameJObject["id"];

            game.GameNumber = (int)gameJObject["number"];

            JArray points = (JArray)gameJObject["points"];

            if (points == null || points.Count() != 2)
            {
                // no game score
                return game;
            }

            game.HomeTeamScore = (int)points.ElementAt(0);
            game.AwayTeamScore = (int)points.ElementAt(1);

            if (game.HomeTeamScore > game.AwayTeamScore){
                game.Winner = match.HomeTeam;
            }
            if (game.HomeTeamScore < game.AwayTeamScore){
                game.Winner = match.AwayTeam;
            }
            if (game.HomeTeamScore == game.AwayTeamScore){
                // draw
            }
            return game;

        }

        public Team parseMatchCompetitor(JObject competitor)
        {
            Team team = parseTeam(competitor);

            return team;
        }

        public Team parseTeam(JObject jOTeam)
        {
            string teamName = (string)jOTeam["abbreviatedName"];

            TeamEnum teamEnum = parseTeamEnum(teamName);

            if (!teams.ContainsKey(teamEnum))
            {
                Team newTeam = new Team(teamEnum);
                newTeam.OfficialId = (int)jOTeam["id"];

                teams[teamEnum] = newTeam;
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