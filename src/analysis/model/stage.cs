using System.Collections.Generic;

namespace OwlAnalysis.Model{
    public class Stage{
        public string Id{get; set;}

        public int officialId{get; set;}
        public StageEnum StageEmum{get; set;}

        public virtual List<Match> Matches{get; set;} = new List<Match>();

        public Stage(){}

        public Stage(StageEnum stageEnum){
            this.StageEmum = stageEnum;
        }

        public Match findMatch(int matchNr){
            foreach(var match in Matches){
                if(match.MatchNumber == matchNr){
                    return match;
                }
            }

            return null;
        }
    }
}