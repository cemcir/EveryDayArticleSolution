using EveryDayArticle.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayArticle.DataAccess.Abstract
{
    public interface ILikedRepository:IRepository<Liked>
    {
        void AddLiked(Liked entity);

        int GetLikedCount(int Id);
    }
}
