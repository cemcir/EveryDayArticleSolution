using EveryDayArticle.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EveryDayArticle.DataAccess.Abstract
{
    public interface ICommentRepository : IRepository<Comment>
    {
        int GetCommentCountById(int Id);

        List<Comment> GetCommentsById(int Id);
    }
}
