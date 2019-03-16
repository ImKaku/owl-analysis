using System.Collections.Generic;
using OwlAnalysis.Model;
using OwlAnalysis.Dao;
using System.Linq;
using System.Linq.Expressions;  
using Microsoft.EntityFrameworkCore;

namespace OwlAnalysis.Service
{
    public class StatData : OwlAnalysisService
    {

        public StatData(OwlAnalysisContex daoContext) : base(daoContext)
        {

        }

        public List<PlayerHeroStats> allStats()
        {
            return DaoContext.PlayerHeroStats
                .Include(phs => phs.Hero)
                .Include(phs => phs.PlayerStats)
                .Include(phs => phs.PlayerGame)
                    .ThenInclude(pg => pg.Game)
                .Include(phs => phs.PlayerGame)
                    .ThenInclude(pg => pg.Player)
                .ToList();
        }
    }
}