using OwlAnalysis.Dao;
using System.Linq;

namespace OwlAnalysis.Service{
    public class OwlAnalysisService{
        public OwlAnalysisContex DaoContext{get;}

        public OwlAnalysisService(OwlAnalysisContex daoContext){
            this.DaoContext = daoContext;
        }
    }
    
}