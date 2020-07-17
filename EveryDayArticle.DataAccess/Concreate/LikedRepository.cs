using EveryDayArticle.DataAccess.Abstract;
using EveryDayArticle.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EveryDayArticle.DataAccess.Concreate
{
    public class LikedRepository : Repository<Liked>, ILikedRepository
    {
        private ArticleContext _appDbContext { get => _context as ArticleContext; }

        public LikedRepository(ArticleContext context) : base(context) { }

        public void AddLiked(Liked entity) {
            Liked liked = _appDbContext.Likeds.Where(l => l.ArticleId == entity.ArticleId && l.UserId==entity.UserId).FirstOrDefault();
            if (liked == null) {
                _appDbContext.Likeds.Add(entity);
                _appDbContext.SaveChanges();
            }
            else {
                _appDbContext.Likeds.Remove(liked);
                _appDbContext.SaveChanges();
            }
        }

        public int GetLikedCount(int Id)
        {
            List<Liked> liked = _appDbContext.Likeds.Where(l => l.ArticleId == Id).ToList();
            return liked.Count;
        }
    }
}
