using System.Collections.Generic;

namespace OwlAnalysis.Model
{
    public class Team
    {
            public string Id{get; set;}

            public TeamEnum TeamEnum{get; set;}
       
            public virtual List<Match> AwayMatches{get; set;} = new List<Match>();
            
            public virtual List<Match> HomeMatches{get; set;} = new List<Match>();

            public Team(){

            }

            public Team(TeamEnum teamEnum){
                this.TeamEnum = teamEnum;
            }

            public List<Match> Matches(){
                List<Match> allMatches =  new List<Match>();
                allMatches.AddRange(AwayMatches);
                allMatches.AddRange(HomeMatches);

                return allMatches;
            }
    }
}