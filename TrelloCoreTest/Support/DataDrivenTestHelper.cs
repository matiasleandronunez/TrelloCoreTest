using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TrelloCoreTest.Support
{
    public sealed class DataDrivenTestHelper
    {
        string board1ToCreate;
        List<string> board1ListsToCreate;
        string cardToCreate;
        string apiCardToCreate;

        DataDrivenTestHelper()
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase).Replace(Path.Combine("bin","Debug","netcoreapp3.1").ToString(), "");

            try
            {
                var dobj = JsonConvert.DeserializeObject<dynamic>(
                    System.IO.File.ReadAllText(new Uri(Path.Combine(path, "testdata.json")).LocalPath));

                board1ToCreate = dobj.TESTDATA.Preconditions.Board1ToCreate.ToString();
                board1ListsToCreate = JsonConvert.DeserializeObject<List<string>>(dobj.TESTDATA.Preconditions.Board1ListsToCreate.ToString());
                cardToCreate = dobj.TESTDATA.TrelloTestFeature.CreateANewCardInABoard.CardName.ToString();
                apiCardToCreate = dobj.TESTDATA.APITestFeature.CreateANewCardInABoardThroughTheAPI.CardName.ToString();
            }
            catch (JsonReaderException e)
            {
                throw new Exception(message: "testdata.json is either absent, missing values, empty or bad formated.", innerException: e);
            }
        }

        private static readonly object padlock = new object();
        private static DataDrivenTestHelper instance = null;
        public static DataDrivenTestHelper Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new DataDrivenTestHelper();
                    }
                    return instance;
                }
            }
        }

        public string Board1ToCreate { get => board1ToCreate; set => board1ToCreate = value; }
        public List<string> Board1ListsToCreate { get => board1ListsToCreate; set => board1ListsToCreate = value; }
        public string CardToCreate { get => cardToCreate; set => cardToCreate = value; }
        public string APICardToCreate { get => apiCardToCreate; set => apiCardToCreate = value; }
    }
}
