using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace TrelloCoreTest.DataEntities
{
    public class Member
    {
        public string id { get; set; }
        public bool activityBlocked { get; set; }
        public object avatarHash { get; set; }
        public object avatarUrl { get; set; }
        public string bio { get; set; }
        public object bioData { get; set; }
        public bool confirmed { get; set; }
        public string fullName { get; set; }
        public object idEnterprise { get; set; }
        public List<object> idEnterprisesDeactivated { get; set; }
        public object idMemberReferrer { get; set; }
        public List<object> idPremOrgsAdmin { get; set; }
        public string initials { get; set; }
        public string memberType { get; set; }
        public NonPublic nonPublic { get; set; }
        public bool nonPublicAvailable { get; set; }
        public List<object> products { get; set; }
        public string url { get; set; }
        public string username { get; set; }
        public string status { get; set; }
        public object aaEmail { get; set; }
        public object aaId { get; set; }
        public string avatarSource { get; set; }
        public object email { get; set; }
        public string gravatarHash { get; set; }
        public List<string> idBoards { get; set; }
        public List<object> idOrganizations { get; set; }
        public List<object> idEnterprisesAdmin { get; set; }
        public Limits limits { get; set; }
        public object loginTypes { get; set; }
        public MarketingOptIn marketingOptIn { get; set; }
        public List<object> messagesDismissed { get; set; }
        public List<string> oneTimeMessagesDismissed { get; set; }
        public Prefs prefs { get; set; }
        public List<object> trophies { get; set; }
        public object uploadedAvatarHash { get; set; }
        public object uploadedAvatarUrl { get; set; }
        public List<object> premiumFeatures { get; set; }
        public bool isAaMastered { get; set; }
        public string ixUpdate { get; set; }
        public object idBoardsPinned { get; set; }

        public class NonPublic
        {
        }

        public class TotalPerMember
        {
            public string status { get; set; }
            public int disableAt { get; set; }
            public int warnAt { get; set; }
        }

        public class Boards
        {
            public TotalPerMember totalPerMember { get; set; }
        }

        public class TotalPerMember2
        {
            public string status { get; set; }
            public int disableAt { get; set; }
            public int warnAt { get; set; }
        }

        public class Orgs
        {
            public TotalPerMember2 totalPerMember { get; set; }
        }

        public class Limits
        {
            public Boards boards { get; set; }
            public Orgs orgs { get; set; }
        }

        public class MarketingOptIn
        {
            public bool optedIn { get; set; }
            public DateTime date { get; set; }
        }

        public class Privacy
        {
            public string fullName { get; set; }
            public string avatar { get; set; }
        }

        public class Prefs
        {
            public Privacy privacy { get; set; }
            public bool sendSummaries { get; set; }
            public int minutesBetweenSummaries { get; set; }
            public int minutesBeforeDeadlineToNotify { get; set; }
            public bool colorBlind { get; set; }
            public string locale { get; set; }
        }
    }
}
