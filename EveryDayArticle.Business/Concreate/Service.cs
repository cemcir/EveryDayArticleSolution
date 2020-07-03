using EveryDayArticle.Business.Abstract;
using EveryDayArticle.DataAccess.Abstract;
using EveryDayArticle.DataAccess.Concreate;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace EveryDayArticle.Business.Concreate
{
    public class Service<TEntity> : IService<TEntity> where TEntity : class
    {
        public readonly IUnitOfWork _unitOfWork;
        private readonly IRepository<TEntity> _repository;

        public Service(IUnitOfWork unitOfwork, IRepository<TEntity> repository)
        {
            this._unitOfWork = unitOfwork;
            this._repository = repository;
        }

        public BaseResponse<TEntity> Add(TEntity entity) {
            try {
                _repository.Add(entity);
                _unitOfWork.Commit();
                return new BaseResponse<TEntity>(entity);
            }
            catch (Exception) {
                return new BaseResponse<TEntity>(new MessageCode().ErrorCreated);
            }
        }

        public BaseResponse<IEnumerable<TEntity>> AddRange(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public BaseResponse<List<TEntity>> GetAll()
        {
            try {
                List<TEntity> entity = this._repository.GetAll();
                if (entity.Count > 0) {
                    return new BaseResponse<List<TEntity>>(entity);
                }
                return new BaseResponse<List<TEntity>>(new MessageCode().NoEntity);
            }
            catch (Exception) {
                return new BaseResponse<List<TEntity>>(new MessageCode().ErrorCreated);
            }
        }

        public BaseResponse<TEntity> GetById(int id) {
            try {
                var entity = this._repository.GetById(id);
                if (entity != null) {
                    return new BaseResponse<TEntity>(entity);
                }
                return new BaseResponse<TEntity>(new MessageCode().NoEntity);
            }
            catch (Exception) {
                return new BaseResponse<TEntity>(new MessageCode().ErrorCreated);
            }
        }

        public bool isEmpty(string statement)
        {
            if (string.IsNullOrEmpty(statement)) {
                return true;
            }
            return false;
        }

        public BaseResponse<TEntity> Remove(TEntity entity)
        {
            try
            {
                this._repository.Remove(entity);
                this._unitOfWork.Commit();
                return new BaseResponse<TEntity>(null);
            }
            catch (Exception)
            {
                return new BaseResponse<TEntity>(new MessageCode().ErrorCreated);
                throw;
            }
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            throw new NotImplementedException();
        }

        public BaseResponse<TEntity> SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var entity = this._repository.SingleOrDefault(predicate);
                if (entity != null)
                {
                    return new BaseResponse<TEntity>(entity);
                }
                return new BaseResponse<TEntity>(new MessageCode().NoEntity);
            }
            catch (Exception)
            {
                return new BaseResponse<TEntity>(new MessageCode().ErrorCreated);
            }
        }

        public BaseResponse<TEntity> Update(TEntity entity) {
            try {
                this._repository.Update(entity);
                this._unitOfWork.Commit();
                return new BaseResponse<TEntity>(entity);
            }
            catch (Exception) {
                return new BaseResponse<TEntity>(new MessageCode().ErrorCreated);
            }
        }

        public BaseResponse<IEnumerable<TEntity>> Where(Expression<Func<TEntity, bool>> predicate)
        {
            throw new NotImplementedException();
        }

    }
}
