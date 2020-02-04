using Links.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Links.DataLayer
{
    public class LinkService
    {
        IGridFSBucket gridFS;
        IMongoCollection<Link> Links;
        IMongoCollection<CounterOfVisits> Visits;
        public LinkService()
        {
            string connectionString = "mongodb://localhost:27017/Users";
            var connection = new MongoUrlBuilder(connectionString);
            MongoClient client = new MongoClient(connectionString);
            IMongoDatabase database = client.GetDatabase(connection.DatabaseName);
            gridFS = new GridFSBucket(database);
            Links = database.GetCollection<Link>("links");
            Visits = database.GetCollection<CounterOfVisits>("counter");
        }
        //Переход по ссылке
        public async Task<string> ClickOnTheLink(string link)
        {
            var builder = new FilterDefinitionBuilder<Link>();
            var filter = builder.Regex("Hash", new BsonRegularExpression(link));
            var fullLink = await Links.Find(filter).FirstOrDefaultAsync();
            fullLink.NumberOfTransitions++;
            await Links.ReplaceOneAsync(new BsonDocument("Hash", link), fullLink);
            return fullLink.Title;
        }
        //Получение сокращённых ссылок с количеством переходов
        public async Task<Dictionary<string, int>> GetLinks()
        {
            Dictionary<string, int> links = new Dictionary<string, int>();
            var filter = new BsonDocument();
            List<Link> fullLinks = await Links.Find(filter).ToListAsync();
            foreach (var link in fullLinks)
            {
                links.Add(link.AbbreviatedTitle, link.NumberOfTransitions);
            };
            return links;
        }
        //Получение полной ссылки по сокращённой
        public async Task<string> GetLink(string abbreviated)
        {
            CounterOfVisits visits = await Visits.Find(c => c.Id == "5e392e65fc42fe269cb9dc0a").FirstOrDefaultAsync();
            visits.Counter++;
            await Visits.ReplaceOneAsync(new BsonDocument("_id", new ObjectId("5e392e65fc42fe269cb9dc0a")), visits);
            var builder = new FilterDefinitionBuilder<Link>();
            var filter2 = builder.Regex("AbbreviatedTitle", new BsonRegularExpression(abbreviated));
            var link = await Links.Find(filter2).FirstOrDefaultAsync();
            return link.Title;
        }
        //Создание сокращённой ссылки
        public async Task<string> Create(string link)
        {
            string hash = Guid.NewGuid().ToString();
            string url = $"http://localhost:51779/link/{hash}";
            Link newLink = new Link { Title = link, AbbreviatedTitle = url, Hash = hash, NumberOfTransitions = 0 };
            await Links.InsertOneAsync(newLink);
            return url;
        }
    }
}
