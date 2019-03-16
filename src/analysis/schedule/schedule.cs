using System;
using System.Collections.Generic;
using OwlAnalysis.Model;

namespace OwlAnalysis.Schedule
{
    public class Schedule
    {
        
        public void StrengthOfSchedule(List<Match> matches){
            Dictionary<TeamEnum, int> rankings = MontesWeek5();

            foreach(TeamEnum teamEnum in rankings.Keys){
                Dictionary<TeamEnum, int> opponents = new Dictionary<TeamEnum, int>();

                int sumOpponents = 0;
                List<Team> opponentsForStage = new List<Team>();

                List<Match> filtered = matches
                    .FindAll(m => m.HomeTeam != null && m.AwayTeam != null)
                    .FindAll(m => m.HomeTeam.TeamEnum == teamEnum || m.AwayTeam.TeamEnum == teamEnum);

                foreach(Match match in filtered){
                    TeamEnum opponent = match.HomeTeam.TeamEnum == teamEnum ? match.AwayTeam.TeamEnum : match.HomeTeam.TeamEnum;

                    sumOpponents += rankings[opponent];
                }
                
                Console.WriteLine(teamEnum + "|" + sumOpponents/7.0);

            }
            
        }


        public Dictionary<TeamEnum, int> MontesWeek5(){
            Dictionary<TeamEnum, int> powerRankings = new Dictionary<TeamEnum, int>();

            powerRankings.Add(TeamEnum.excelsior, 1);
            powerRankings.Add(TeamEnum.titans, 2);
            powerRankings.Add(TeamEnum.shock, 3);
            powerRankings.Add(TeamEnum.defiant, 4);
            powerRankings.Add(TeamEnum.gladiators, 5);
            powerRankings.Add(TeamEnum.fuel, 6);
            powerRankings.Add(TeamEnum.spitfire, 7);
            powerRankings.Add(TeamEnum.charge, 8);
            powerRankings.Add(TeamEnum.reign, 9);
            powerRankings.Add(TeamEnum.spark, 10);
            powerRankings.Add(TeamEnum.dragons, 11);
            powerRankings.Add(TeamEnum.eternal, 12);
            powerRankings.Add(TeamEnum.fusion, 13);
            powerRankings.Add(TeamEnum.uprising, 14);
            powerRankings.Add(TeamEnum.dynasty, 15);
            powerRankings.Add(TeamEnum.hunters, 16);
            powerRankings.Add(TeamEnum.outlaws, 17);
            powerRankings.Add(TeamEnum.mayham, 18);
            powerRankings.Add(TeamEnum.valient, 19);
            powerRankings.Add(TeamEnum.justice, 20);

            return powerRankings;
        }
        
    }
}