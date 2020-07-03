using EveryDayArticle.Business.Abstract;
using EveryDayArticle.DataAccess.Abstract;
using EveryDayArticle.DataAccess.Concreate;
using EveryDayArticle.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayArticle.Business.Concreate
{
    public class LikedService: Service<Liked>, ILikedService
    {
        private Message message;

        public LikedService(IUnitOfWork unitOfWork, IRepository<Liked> repository, Message message) : base(unitOfWork, repository) {

            this.message = message;
        }       

        public BaseResponse<Liked> AddLiked(Liked liked)
        {
            this._unitOfWork.BeginTransaction();
            try {
                this._unitOfWork.Likeds.AddLiked(liked);
                this._unitOfWork.TransactionCommit();
                return new BaseResponse<Liked>(liked);
            }
            catch (Exception) {
                this._unitOfWork.RolBack();
                return new BaseResponse<Liked>(new MessageCode().ErrorCreated);
            }
        }

        public int GetLikedCount(int Id)
        {
            try {
                return _unitOfWork.Likeds.GetLikedCount(Id);
            }
            catch (Exception) {

                throw;
            }
        }
    }
}
