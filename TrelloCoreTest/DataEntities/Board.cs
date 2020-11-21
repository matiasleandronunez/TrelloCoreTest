﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;


namespace TrelloCoreTest.DataEntities
{
    public class Board
    {
        public string name { get; set; }
        public string desc { get; set; }
        public object descData { get; set; }
        public bool closed { get; set; }
        public object idOrganization { get; set; }
        public object limits { get; set; }
        public object pinned { get; set; }
        public string shortLink { get; set; }
        public List<object> powerUps { get; set; }
        public object dateLastActivity { get; set; }
        public List<object> idTags { get; set; }
        public object datePluginDisable { get; set; }
        public string creationMethod { get; set; }
        public object ixUpdate { get; set; }
        public bool enterpriseOwned { get; set; }
        public string id { get; set; }
        public bool starred { get; set; }
        public string url { get; set; }
        public Prefs prefs { get; set; }
        public bool subscribed { get; set; }
        public LabelNames labelNames { get; set; }
        public string shortUrl { get; set; }
        public object templateGallery { get; set; }
        public List<Membership> memberships { get; set; }

        public class Prefs
        {
            public string permissionLevel { get; set; }
            public bool hideVotes { get; set; }
            public string voting { get; set; }
            public string comments { get; set; }
            public string invitations { get; set; }
            public bool selfJoin { get; set; }
            public bool cardCovers { get; set; }
            public bool isTemplate { get; set; }
            public string cardAging { get; set; }
            public bool calendarFeedEnabled { get; set; }
            public string background { get; set; }
            public object backgroundImage { get; set; }
            public object backgroundImageScaled { get; set; }
            public bool backgroundTile { get; set; }
            public string backgroundBrightness { get; set; }
            public string backgroundColor { get; set; }
            public string backgroundBottomColor { get; set; }
            public string backgroundTopColor { get; set; }
            public bool canBePublic { get; set; }
            public bool canBeEnterprise { get; set; }
            public bool canBeOrg { get; set; }
            public bool canBePrivate { get; set; }
            public bool canInvite { get; set; }
        }

        public class LabelNames
        {
            public string green { get; set; }
            public string yellow { get; set; }
            public string orange { get; set; }
            public string red { get; set; }
            public string purple { get; set; }
            public string blue { get; set; }
            public string sky { get; set; }
            public string lime { get; set; }
            public string pink { get; set; }
            public string black { get; set; }
        }

        public class Membership
        {
            public string id { get; set; }
            public string idMember { get; set; }
            public string memberType { get; set; }
            public bool unconfirmed { get; set; }
            public bool deactivated { get; set; }
        }
    }
}
