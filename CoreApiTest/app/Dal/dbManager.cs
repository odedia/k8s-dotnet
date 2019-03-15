using app.Models;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.Dal
{
    public class dbManager
    {
        const string collectionName = "Persons";
        readonly IMongoCollection<Person> userCollection;
        readonly DB_Config _config;
        

        public dbManager(DB_Config dBConfig)
        {
            _config = dBConfig;
            userCollection = setPersonCollection();
        }


        public List<Person> GetPersonsList()
        {
            //get persons list
            var persons = userCollection.Find(p => true).ToList();

            return persons;
        }
        

        public  Person GetPerson(string id)
        {
            //get specific person by id
            var person = userCollection.Find(p => p.ID == id).FirstOrDefault();

            return person;
        }

        public void InsertNewPerson(Person person)
        {
            if (GetPerson(person.ID) != null)
            {
                throw new Exception($"Person with id {person.ID} all ready exists");
            }

            userCollection.InsertOne(person);
        }

        public void UpdatePerson(Person person)
        {
            if (GetPerson(person.ID) == null)
            {
                throw new Exception($"Person with id {person.ID} not exists");
            }

            DeletePerson(person.ID); //Delete old person
            InsertNewPerson(person); //Insert updated person
        }

        public void DeletePerson(string id)
        {
            if (GetPerson(id) == null)
            {
                throw new Exception($"Person with id {id} not exists");
            }


            userCollection.DeleteOne(p => p.ID == id);
        }

        private IMongoCollection<Person> setPersonCollection()
        {
            // Create a MongoClient object by using the connection string
            var client = new MongoClient(_config.DbConnectionString);

            //Use the MongoClient to access the server
            var database = client.GetDatabase(_config.DbName);

            //get mongodb collection
            var collection = database.GetCollection<Person>(collectionName);

            return collection;
        }
    }
}
