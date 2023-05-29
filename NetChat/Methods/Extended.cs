using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetChat
{
    static class Extended
    {
        public static List<Message> SortMessages(this List<Message> list) // MEG KELL CSINÁLNI A SORTING-OT
        {
            List<Message> result = new List<Message>();
            Message currentMessage = new Message()
            {
                From = 0,
                To = 0,
                Content = "",
                When = Convert.ToDateTime("9999-01-01")
            };

            int i, j;

            for (i = 0; i < list.Count; i++)
            {
                currentMessage = new Message()
                {
                    From = 0,
                    To = 0,
                    Content = "",
                    When = Convert.ToDateTime("9999-01-01")
                };

                for (j = 0; j < list.Count; j++)
                {
                    if (DateTime.Compare(list[i].When, list[j].When) < 0)
                    {
                        currentMessage = list[j];
                    }
                }
                list.Remove(currentMessage);
                result.Add(currentMessage);
            }

            return result;
        }
    }
}
