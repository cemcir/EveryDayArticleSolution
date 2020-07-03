using EveryDayArticle.DataAccess.Concreate;
using EveryDayArticle.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayArticle.Business.Abstract
{
    public interface ILikedService : IService<Liked>
    {
        BaseResponse<Liked> AddLiked(Liked liked);

        int GetLikedCount(int Id);
    }
}
