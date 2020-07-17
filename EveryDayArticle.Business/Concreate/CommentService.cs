using EveryDayArticle.Business.Abstract;
using EveryDayArticle.DataAccess.Abstract;
using EveryDayArticle.DataAccess.Concreate;
using EveryDayArticle.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayArticle.Business.Concreate
{
    public class CommentService: Service<Comment>, ICommentService
    {
        public Message message { get; set; }

        public CommentService(IUnitOfWork unitOfWork, IRepository<Comment> repository, Message message) : base(unitOfWork, repository) {
            this.message = message;
        }

        public bool Validate(Comment entity)
        {
            throw new NotImplementedException();
        }

        public int GetCommentCountById(int id)
        {
            try {
                int commentCount = _unitOfWork.Comments.GetCommentCountById(id);
                return commentCount;
            }
            catch (Exception) {
                return 0;
            }
        }

        public BaseResponse<List<Comment>> GetCommentsById(int Id)
        {
            try {
                var comments = _unitOfWork.Comments.GetCommentsById(Id);
                return new BaseResponse<List<Comment>>(comments);
            }
            catch (Exception) {
                return new BaseResponse<List<Comment>>(new MessageCode().ErrorCreated);
            }
        }

        public BaseResponse<List<Comment>> GetCommentsByUserId(string userId,int Id)
        {
            try {
                var comments = _unitOfWork.Comments.GetCommentsByUserId(userId,Id);
                return new BaseResponse<List<Comment>>(comments);
            }
            catch (Exception) {
                return new BaseResponse<List<Comment>>(new MessageCode().ErrorCreated);
            }
        }
    }
}
