using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Deserializers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrelloCoreTest.DataEntities;
using System.Web;
using System.Text.RegularExpressions;

namespace TrelloCoreTest.Support
{
    public class APIHelper
    {
        private RestClient client;

        public APIHelper()
        {
            client = new RestClient(EnvironmentConfig.Instance.Api_uri);
        }

        public string AuthenticationString()
        {
            return $"key={EnvironmentConfig.Instance.ApiKey}&token={EnvironmentConfig.Instance.ApiToken}";
        }

        /// <summary>Creates a new board with the name specified. No Predefined lists. Other parameters are harcoded to use defaults (same as in the api sample).</summary>
        /// 
        public string CreateBoard(string name)
        {
            var request = new RestRequest($"/1/boards/?name={Uri.EscapeUriString(name)}&defaultLabels=true&defaultLists=false&keepFromSource=none&prefs_permissionLevel=private&prefs_voting=disabled&prefs_comments=members&prefs_invitations=members&prefs_selfJoin=true&prefs_cardCovers=true&prefs_background=blue&prefs_cardAging=regular&{AuthenticationString()}", Method.POST);

            var response = client.Execute(request);

            return GetBoardIdByName(name);
        }

        /// <summary>Returns a list with all boards accessible to the current user</summary>
        /// 
        public List<Board> GetAllBoards()
        {
            var request = new RestRequest($"/1/members/me/boards?{AuthenticationString()}",
                Method.GET)
                { 
                    RequestFormat = DataFormat.Json
                };

            var response = client.Execute(request);

            return JsonConvert.DeserializeObject<List<Board>>(response.Content);
        }

        /// <summary>Get a board id searching by name</summary>
        /// 
        public string GetBoardIdByName(string name)
        {
            return GetAllBoards().Find(b => b.name.Equals(name)).id;
        }

        public void DeleteAllBoards()
        {
            foreach (string id in GetAllBoards().Select(b => b.id).ToList())
            {
                var request = new RestRequest($"/1/boards/{id}?{AuthenticationString()}", Method.DELETE);
                var response = client.Execute(request);
            }
        }

        public void CreateList(string boardId, string newListName)
        {
            var request = new RestRequest($"/1/lists?name={Uri.EscapeUriString(newListName)}&idBoard={boardId}&{AuthenticationString()}", Method.POST);
            var response = client.Execute(request);
        }

        public List<TrelloList> GetAllListsInBoard(string boardName)
        {
            var request = new RestRequest($"/1/boards/{GetBoardIdByName(boardName)}/lists?{AuthenticationString()}", 
                Method.GET)
                {
                    RequestFormat = DataFormat.Json
                };

            var response = client.Execute(request);

            return JsonConvert.DeserializeObject<List<TrelloList>>(response.Content);
        }

        public void CreateCard(string idList, string cardTitle)
        {
            var request = new RestRequest($"1/cards?name={Uri.EscapeUriString(cardTitle)}&idList={idList}&keepFromSource=all&{AuthenticationString()}", Method.POST);
            var response = client.Execute(request);
        }

        public List<Card> GetAllCardsInList(string listId)
        {
            var request = new RestRequest($"/1/lists/{listId}/cards?{AuthenticationString()}",
                Method.GET)
                {
                    RequestFormat = DataFormat.Json
                };

            var response = client.Execute(request);

            return JsonConvert.DeserializeObject<List<Card>>(response.Content);
        }
    }
}
