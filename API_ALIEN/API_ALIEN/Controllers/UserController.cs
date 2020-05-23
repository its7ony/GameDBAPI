using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using API_ALIEN.Models;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Http.Cors;

namespace API_ALIEN.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [RoutePrefix("api/user")]
    public class UserController : ApiController
    {
        public SqlConnection cnn; 

        //GET CREATE USER//
        [HttpGet]
        [Route("CreateUser/{id}/{pwd}")]
        public HttpResponseMessage CreateUser(string id, string pwd)
        {
            using (SqlConnection cnn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["APIALIEN"].ConnectionString))
            {
                cnn.Open();
                string query = $@"
                INSERT INTO [User] (Id, Pwd, Created, Score)
                VALUES ('{id}', '{pwd}', '" + DateTime.Now.ToShortDateString() + "',0);";

                using (SqlCommand cmd = new SqlCommand(
                    query, cnn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader(
                        CommandBehavior.CloseConnection))
                    {
                        dr.Close();
                        return Login(id, pwd);
                    }
                };
            };
        }

        //GET LOGIN//
        [HttpGet]
        [Route("Login/{id}/{pwd}")]
        public HttpResponseMessage Login(string id, string pwd)
        {

            User user = new User();

            using (SqlConnection cnn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["APIALIEN"].ConnectionString))
            {
                cnn.Open();
                string query = $@"SELECT * FROM [User] WHERE Id = '{id}' AND Pwd = '{pwd}';";

                using (SqlCommand cmd = new SqlCommand(
                    query, cnn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader(
                        CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {

                             user = new User
                            {
                                Id = dr.GetString(0),
                                Pwd = dr.GetString(1),
                                Created = dr.GetString(2),
                                Score = dr.GetInt32(3)

                            };

                           

                        }
                        dr.Close();
                    }
                };
            };

            return Request.CreateResponse(HttpStatusCode.OK, (User)user);
        }

        //GET TOP 5
        [HttpGet]
        [Route("Top5")]
        public HttpResponseMessage Top5()
        {

            List<dynamic> lstTabla = new List<dynamic>();

            dynamic result = new
            {
                Users = lstTabla
            };

            using (SqlConnection cnn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["APIALIEN"].ConnectionString))
            {
                cnn.Open();
                string query = @"SELECT TOP 5
	                        Id,
                            Score
                        FROM
                            [User]
                        ORDER BY 
                            Score DESC;";

                using (SqlCommand cmd = new SqlCommand(
                    query, cnn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader(
                        CommandBehavior.CloseConnection))
                    {
                        while (dr.Read())
                        {
                            dynamic objTabla = new
                            {
                                Id = dr.GetString(0),
                                Score = dr.GetInt32(1)
                            };

                            lstTabla.Add(objTabla);
                        }
                        dr.Close();
                    }
                };
            };

            return Request.CreateResponse(HttpStatusCode.OK, (object)result);
        }

        //PUT UPDATE SCORE
        [HttpPut]
        [Route("UpdateScore/{id}/{score}")]
        public HttpResponseMessage UpdateScore(string id, int score)
        {
            bool response = new bool();

            using (SqlConnection cnn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["APIALIEN"].ConnectionString))
            {
                cnn.Open();

                string query = $@"
                UPDATE [User] SET Score = {score} WHERE Id = '{id}'";

                using (SqlCommand cmd = new SqlCommand(
                    query, cnn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader(
                        CommandBehavior.CloseConnection))
                    {
                        response = true;
                        dr.Close();
                    }
                };
            };

            return Request.CreateResponse(HttpStatusCode.OK, (object)response);
        }

        //DELETE DELETE USER
        [HttpDelete]
        [Route("DeleteUser/{id}")]
        public HttpResponseMessage DeleteUser(string id)
        {
            bool response = new bool();

            using (SqlConnection cnn = new SqlConnection(
                ConfigurationManager.ConnectionStrings["APIALIEN"].ConnectionString))
            {
                cnn.Open();
                string query = $@"
                DELETE [User] WHERE Id = '{id}'";

                using (SqlCommand cmd = new SqlCommand(
                    query, cnn))
                {
                    using (SqlDataReader dr = cmd.ExecuteReader(
                        CommandBehavior.CloseConnection))
                    {
                        response = true;
                        dr.Close();
                    }
                };
            };

            return Request.CreateResponse(HttpStatusCode.OK, (object)response);
        }
    }
}