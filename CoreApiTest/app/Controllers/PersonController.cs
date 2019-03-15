using app.Dal;
using app.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;

namespace app.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase
    {
        private readonly dbManager _dbManager;

        //Original code, works OOTB with the proper environment variable 
        //    ControllerSettings__DbConfig__DbConnectionString: mongodb+srv://odedia:O4JLBjqF5CR0f@cluster0-lozjx.gcp.mongodb.net/sampledb
        public PersonController(IOptions<ControllerSettings> settings)
        {
           _dbManager = new dbManager(settings?.Value.DbConfig);
        }

        //PCF OSBAPI Code
        // public PersonController(IOptions<ControllerSettings> settings, IConfiguration conf)
        // {
        //     var uri = conf["vcap:services:user-provided:0:credentials:url"];
        //     var db = new DB_Config();
        //     db.DbConnectionString = uri;
        //     db.DbName = "sampledb";

        //     _dbManager = new dbManager(db);
        // }

        // GET api/Person
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            try
            {
                var list = _dbManager.GetPersonsList();

                return Ok(list);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error - " + ex.Message);
            }
        }

        // GET api/Person/5
        [HttpGet("{id}")]
        public ActionResult Get(string id)
        {
            try
            {
                var person = _dbManager.GetPerson(id);

                if (person != null)
                {
                    return Ok(person);
                }

                return StatusCode(400, "Person not Found");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error - " + ex.Message);
            }
        }

        // POST api/Person
        [HttpPost]
        public void Post([FromBody] Person person)
        {
            _dbManager.InsertNewPerson(person);
        }

        // PUT api/Person/5
        [HttpPut("{id}")]
        public ActionResult Put(string id, [FromBody] Person person)
        {
            try
            {
                if (id != person.ID)
                {
                    return StatusCode(400, "Error - ID not match, ID can not be change");
                }

                _dbManager.UpdatePerson(person);
                return Ok("Done");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Error - " + ex.Message);
            }
        }

        // DELETE api/Person/5
        [HttpDelete("{id}")]
        public void Delete(string id)
        {
            _dbManager.DeletePerson(id);
        }
    }
}
