using System;
using Api.Data.Collections;
using Microsoft.Extensions.Configuration;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;

namespace Api.Data
{
    public class MongoDBContext
    {
        public IMongoDatabase Db{ get; set; }

        public void MongoDbContext(IConfiguration configuration)
        {
            try{
                var settings = MongoClientSettings.FromUrl(new MongoUrl(configuration["ConnectionString"]));
                var client = new MongoClient(settings);
                Db = client.GetDatabase(configuration["NomeBanco"]);
                MapClasses();
            }
            catch(Exception ex){
                throw new MongoException("It was not possible to connect to MongoDB",ex);
            }
        }

        public void MapClasses(){
            var conventionPack = new ConventionPack { new CamelCaseElementNameConvention()};
            ConventionRegistry.Register("camelCase", conventionPack, t=>true);

            if(!BsonClassMap.IsClassMapRegistered(typeof(Infectado)))
            {
                BsonClassMap.RegisterClassMap<Infectado>(i=>
                {
                    i.AutoMap(); 
                    i.SetIgnoreExtraElements(true);
                });
            }

        }
    }
}