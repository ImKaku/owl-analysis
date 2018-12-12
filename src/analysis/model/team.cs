using System.Collections.Generic;

namespace OwlAnalysis.Model
{
    public class Team
    {
            public string Id{get; set;}

            public TeamEnum TeamEnum{get; set;}

            public List<Match> Matches{get; set;} = new List<Match>();

            public Team(){

            }

            public Team(TeamEnum teamEnum){
                this.TeamEnum = teamEnum;
            }
    }
}