using System;
using System.Collections.Generic;
using System.Text;

namespace EveryDayArticle.Business.Concreate
{
    public class Message
    {
        public string Title { get; set; }

        public List<string> Messages = new List<string>();

        public string Content { get; set; }

        public string Css { get; set; }

        public string Entity { get; set; }
    }
}
