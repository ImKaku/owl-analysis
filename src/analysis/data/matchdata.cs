using System.Collections.Generic;
using OwlAnalysis.Model;
using OwlAnalysis.Dao;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;

namespace OwlAnalysis.Service
{
    public class MatchData : OwlAnalysisService
    {

        public MatchData(OwlAnalysisContex daoContext) : base(daoContext)
        {

        }

        public List<Match> allMatches()
        {
            return DaoContext.Matches
                .Include(m => m.Stage)
                .Include(m => m.Games)
                    .ThenInclude(g => g.PlayerGames)
                        .ThenInclude(pg => pg.HeroStats)
                            .ThenInclude(pgh => pgh.PlayerStats)
                .Include(m => m.HomeTeam)
                .Include(m => m.AwayTeam)
                .ToList();
        }
    }
}