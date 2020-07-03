using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayArticle.DataAccess.Abstract
{
    public interface IUnitOfWork
    {
        ICategoryRepository Categories { get; }

        ICommentRepository Comments { get; }

        IArticleRepository Articles { get; }

        ILikedRepository Likeds { get; }

        void BeginTransaction();

        void TransactionCommit();

        void RolBack();

        void Commit();
    }
}
