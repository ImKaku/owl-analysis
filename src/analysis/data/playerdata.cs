using System.Collections.Generic;
using OwlAnalysis.Model;
using OwlAnalysis.Dao;
using System.Linq;
using System.Linq.Expressions;  
using Microsoft.EntityFrameworkCore;

namespace OwlAnalysis.Service
{
    public class PlayerData : OwlAnalysisService
    {

        public PlayerData(OwlAnalysisContex daoContext) : base(daoContext)
        {

        }

        public Player FindPlayerByName(string name)
        {
            return DaoContext.Players
                    .Where(p => p.Name == name)
                    .FirstOrDefault<Player>();
        }
    }
}