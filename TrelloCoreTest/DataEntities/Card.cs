using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TrelloCoreTest.DataEntities
{
    public class Badges
    {
        public int votes { get; set; }
        public bool viewingMemberVoted { get; set; }
        public bool subscribed { get; set; }
        public string fogbugz { get; set; }
        public int checkItems { get; set; }
        public int checkItemsChecked { get; set; }
        public int comments { get; set; }
        public int attachments { get; set; }
        public bool description { get; set; }
        public object due { get; set; }
        public bool dueComplete { get; set; }
    }

    public class Card
    {
        public string id { get; set; }
        public string name { get; set; }
        public Badges badges { get; set; }
        public List<object> labels { get; set; }
    }
}
