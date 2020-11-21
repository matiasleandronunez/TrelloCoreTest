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
        private RestClient _client;

        public APIHelper()
        {
            _client = new RestClient(EnvironmentConfig.Instance.Api_uri);
        }

        public string AuthenticationString()
        {
            return $"key={EnvironmentConfig.Instance.ApiKey}&token={EnvironmentConfig.Instance.ApiToken}";
        }

        public string CreateBoard(string name)
        {
            /// <summary>Creates a new board with the name specified. No Predefined lists. Other parameters are harcoded to use defaults (same as in the api sample).</summary>
            /// 
            RestRequest request = new RestRequest($"/1/boards/?name={Uri.EscapeUriString(name)}&defaultLabels=true&defaultLists=false&keepFromSource=none&prefs_permissionLevel=private&prefs_voting=disabled&prefs_comments=members&prefs_invitations=members&prefs_selfJoin=true&prefs_cardCovers=true&prefs_background=blue&prefs_cardAging=regular&{AuthenticationString()}", Method.POST);

            IRestResponse response = _client.Execute(request);

            return GetBoardIdByName(name);
        }

        public List<Board> GetAllBoards()
        {
            /// <summary>Returns a list with all boards accessible to the current user</summary>
            /// 

            RestRequest request = new RestRequest($"/1/members/me/boards?{AuthenticationString()}", Method.GET);

            request.RequestFormat = DataFormat.Json;

            IRestResponse response = _client.Execute(request);

            return JsonConvert.DeserializeObject<List<Board>>(response.Content);
        }

        public string GetBoardIdByName(string name)
        {
            /// <summary>Get a board id searching by name</summary>
            /// 

            return GetAllBoards().Find(b => b.name.Equals(name)).id;
        }

        public void DeleteAllBoards()
        {
            foreach (string id in GetAllBoards().Select(b => b.id).ToList())
            {
                RestRequest request = new RestRequest($"/1/boards/{id}?{AuthenticationString()}", Method.DELETE);
                IRestResponse response = _client.Execute(request);
            }
        }

        public void CreateList(string board_id, string new_list_name)
        {
            RestRequest request = new RestRequest($"/1/lists?name={Uri.EscapeUriString(new_list_name)}&idBoard={board_id}&{AuthenticationString()}", Method.POST);
            IRestResponse response = _client.Execute(request);
        }

        public List<TrelloList> GetAllListsInBoard(string board_name)
        {
            RestRequest request = new RestRequest($"/1/boards/{GetBoardIdByName(board_name)}/lists?{AuthenticationString()}", Method.GET);

            request.RequestFormat = DataFormat.Json;

            IRestResponse response = _client.Execute(request);

            return JsonConvert.DeserializeObject<List<TrelloList>>(response.Content);
        }

        public void CreateCard(string idlist, string cardtitle)
        {
            RestRequest request = new RestRequest($"1/cards?name={Uri.EscapeUriString(cardtitle)}&idList={idlist}&keepFromSource=all&{AuthenticationString()}", Method.POST);
            IRestResponse response = _client.Execute(request);
        }

        public List<Card> GetAllCardsInList(string list_id)
        {
            RestRequest request = new RestRequest($"/1/lists/{list_id}/cards?{AuthenticationString()}", Method.GET);

            request.RequestFormat = DataFormat.Json;

            IRestResponse response = _client.Execute(request);

            return JsonConvert.DeserializeObject<List<Card>>(response.Content);
        }
    }
}
