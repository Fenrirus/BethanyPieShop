using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BethanyPieShop.Models
{
    public class PieRepository : IPieRepository
    {
        private readonly AppDbContext _appDbContext;

        public PieRepository(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public IEnumerable<Pie> AllPies => _appDbContext.Pies.Include(c => c.Category);

        public IEnumerable<Pie> PiesOfTheWeek => _appDbContext.Pies.Include(c => c.Category).Where(w => w.IsPieOfTheWeek);

        public Pie GetPieById(int pieId)
        {
            return _appDbContext.Pies.FirstOrDefault(f => f.PieId == pieId);
        }
    }
}