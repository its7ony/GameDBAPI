using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameAPI.Service;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GameAPI.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
       UserService userService = new UserService();

        //GET USER//
        // Endpoint GET: https://localhost:44309/api/user/its7ony/123
        [HttpGet("{id}/{pwd}")]
        public string GetUser(string id, string pwd)
        {
            if (userService.makeConnection())
                return JsonConvert.SerializeObject(userService.getUser(id, pwd));
            else return "[]";
        }

        //GET USER//
        //Endpoint GET: https://localhost:44309/api/user
        [HttpGet]
        public string GetTop5()
        {
            if (userService.makeConnection())
                return JsonConvert.SerializeObject(userService.getTop5());
            else return "[]";
        }

        //CREATE USER// 
        //Endpoint POST: https://localhost:44309/api/User
        [HttpPost]
        public string CreateUser([FromBody] User user)
        {
            if (userService.makeConnection())
                return JsonConvert.SerializeObject(userService.createUser(user.Id, user.Pwd));
            else return "[]";
        }

        //UPDATE SCORE// 
        // Endpoint PUT: https://localhost:44309/api/User/its7ony/4000
        [HttpPut("{id}/{score}")]
        public string UpdateScore(string id, int score)
        {
            if (userService.makeConnection())
                return JsonConvert.SerializeObject(userService.updateScore(id,score));
            else return "[]";
        }

        //DELETE USER// 
        // Endpoint DELETE: https://localhost:44309/api/User/its7ony
        [HttpDelete("{id}")]
        public string DeleteUser(string id)
        {
            if (userService.makeConnection())
                return JsonConvert.SerializeObject(userService.deleteUser(id));
            else return "[]";
        }
    }
}
