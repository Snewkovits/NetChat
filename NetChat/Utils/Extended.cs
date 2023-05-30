using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat
{
    static class Extended
    {
        public static List<Message> SortByTime(this List<Message> messages)
        {
            List<Message> result = new List<Message>();
            IOrderedEnumerable<Message> query = 
                from m in messages
                orderby m.When ascending
                select m;
            foreach (Message m in query)
            {
                result.Add(m);
            }
            return result;
        }
    }
}
