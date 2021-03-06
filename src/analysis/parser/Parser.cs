using System;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using OwlAnalysis.Model;

namespace OwlAnalysis.Service
{
    public class JsonParser
    {
        public Dictionary<StageEnum, Stage> stages { get; set; } = new Dictionary<StageEnum, Stage>();

        public Dictionary<string, Player> players { get; set; } = new Dictionary<string, Player>();

        public JsonParser()
        {
            stages[StageEnum.preseason] = new Stage(StageEnum.preseason);
            stages[StageEnum.stage1] = new Stage(StageEnum.stage1);
            stages[StageEnum.stage2] = new Stage(StageEnum.stage2);
            stages[StageEnum.stage3] = new Stage(StageEnum.stage3);
            stages[StageEnum.stage4] = new Stage(StageEnum.stage4);
            stages[StageEnum.playoffs] = new Stage(StageEnum.playoffs);
        }

        public void parse(JObject jObject)
        {
            Match match = parseMatch(jObject);
            Game game = parseGame(match, jObject);
            PlayerGame playerGame = parsePlayerGame(game, jObject);
            PlayerHeroStats playerHeroStats = parsePlayerHeroStats(playerGame, jObject);
        }

        public void parse(JArray jArray)
        {
            foreach (JObject obj in jArray.Children<JObject>())
            {
                parse(obj);
            }
        }

        public Match parseMatch(JObject jObject)
        {
            StageEnum stageEnum = parseStage((string)jObject["stage"]);
            Stage stage = stages[stageEnum];

            var matchNr = (int)jObject["match"];

            Match match = stages[stageEnum].findMatch(matchNr);

            if (match == null)
            {
                match = new Match(stage);

                match.Stage = stage;
                match.MatchNumber = matchNr;

                match.AwayTeam = parseTeam((string)jObject["opp_team"]);
                match.HomeTeam = parseTeam((string)jObject["team"]);

                stages[stageEnum].Matches.Add(match);
            }

            return match;
        }

        public Game parseGame(Match match, JObject jObject)
        {
            var gameNr = (int)jObject["game"];
            var map = parseMap((string)jObject["map"]);

            Game game = match.FindOrCreateGame(gameNr, map);
            return game;
        }


        public PlayerHeroStats parsePlayerHeroStats(PlayerGame playerGame, JObject jObject)
        {

            Enum.TryParse((string)jObject["hero"], out Hero hero);

            PlayerHeroStats playerHero = new PlayerHeroStats(playerGame, hero);

            List<string> ignoredKeys = new List<string>(){
                "_id", "team", "opp_team", "stage", "game", "hero", "player", "map", "match"
            };

            foreach (var o in jObject.Properties())
            {
                if (!ignoredKeys.Contains(o.Name))
                {
                    playerHero.AddStat(o.Name, o.Value.ToString());
                }

            }

            playerGame.HeroStats.Add(playerHero);

            return playerHero;
        }

        public PlayerGame parsePlayerGame(Game game, JObject jObject)
        {
            var player = parsePlayer(jObject);

            return game.FindPlayerGameForPlayer(player);
        }

        public Player parsePlayer(JObject jObject)
        {
            string playerName = (string)jObject["player"];

            if (!players.ContainsKey(playerName))
            {
                Player player = new Player();
                player.Name = playerName;

                players[playerName] = player;
            }

            return players[playerName];
        }


        public Map parseMap(string map)
        {
            switch (map)
            {
                case "temple-of-anubis":
                    return Map.anubis;
                case "horizon-lunar-colony":
                    return Map.horizon;
                case "kings-row":
                    return Map.kingsrow;
                case "route-66":
                    return Map.route66;
                case "blizzard-world":
                    return Map.blizzardworld;
            }

            return (Map)Enum.Parse(typeof(Map), map, true);
        }

        public StageEnum parseStage(string stage)
        {
            return (StageEnum)Enum.Parse(typeof(StageEnum), stage, true);
        }

        public Team parseTeam(string team)
        {
            switch (team)
            {
                case "Florida Mayhem":
                    return Team.mayham;
                case "San Francisco Shock":
                    return Team.shock;
                case "Los Angeles Valiant":
                    return Team.valient;
                case "Shanghai Dragons":
                    return Team.dragons;
                case "Seoul Dynasty":
                    return Team.dynasty;
                case "London Spitfire":
                    return Team.spitfire;
                case "Los Angeles Gladiators":
                    return Team.gladiators;
                case "Houston Outlaws":
                    return Team.outlaws;
                case "Dallas Fuel":
                    return Team.fuel;
                case "Boston Uprising":
                    return Team.uprising;
                case "New York Excelsior":
                    return Team.excelsior;
                case "Philadelphia Fusion":
                    return Team.fusion;
            }

            throw new System.Exception("Not supported team exception " + team);
        }
    }

}