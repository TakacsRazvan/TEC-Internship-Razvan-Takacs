﻿using Internship.Model;
using Internship.ObjectModel;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;

namespace Internship.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PersonsController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var db = new APIDbContext();
            var list = db.Persons.Include(x => x.Salary).Include(x => x.Position)
                   .Select(x => new PersonInformation()
                   {
                       Id = x.Id,
                       Name = x.Name,
                       PositionName = x.Position.Name,
                       Salary = x.Salary.Amount,
                   }).ToList();
            return Ok(list);
        }

        [HttpGet("{Id}")]
        public IActionResult Get(int Id)
        {
            var db = new APIDbContext();
            Person person = db.Persons.FirstOrDefault(x => x.Id == Id);
            if (person == null)
                return NotFound();
            else
                return Ok(person);

        }
        [HttpPost]
        public IActionResult Post(Person person)
        {
            if (ModelState.IsValid)
            {
                var db = new APIDbContext();
                db.Persons.Add(person);
                db.SaveChanges();
                return Created("", person);
            }
            else
                return BadRequest();
        }
        [HttpPut]
        public IActionResult UpdatePerson(Person person)
        {

            if (ModelState.IsValid)
            {
                var db = new APIDbContext();
                Person updateperson = db.Persons.Find(person.Id);
                updateperson.Address = person.Address;
                updateperson.Age = person.Age;
                updateperson.Email = person.Email;
                updateperson.Name = person.Name;
                updateperson.PositionId = person.PositionId;
                updateperson.SalaryId = person.SalaryId;
                updateperson.Surname = person.Surname;
                db.SaveChanges();
                return NoContent();
            }
            else
                return BadRequest();
        }

        [HttpDelete("{Id}")]
        public IActionResult Delete(int Id)
        {
            var db = new APIDbContext();
            Person person = db.Persons.FirstOrDefault(x => x.Id == Id);
            if (person == null)
            {
                return NotFound();
            }
            db.Persons.Remove(person);
            db.SaveChanges();
            return NoContent();
        }
    }
}
