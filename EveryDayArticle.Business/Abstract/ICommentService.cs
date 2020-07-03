using EveryDayArticle.DataAccess.Concreate;
using EveryDayArticle.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayArticle.Business.Abstract
{
    public interface ICommentService:IService<Comment>,IValidate<Comment>
    {
        int GetCommentCountById(int id);

        BaseResponse<List<Comment>> GetCommentsById(int Id); 
    }
}
