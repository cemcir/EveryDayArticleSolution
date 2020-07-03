using EveryDayArticle.Business.Concreate;
using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayArticle.Business.Abstract
{
    public interface IValidate<T>
    {
        Message message { get; set; }

        bool Validate(T entity);
    }
}
