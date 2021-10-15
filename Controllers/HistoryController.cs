using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TodayApi.Models;

namespace TodayApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HistoryController : ControllerBase
    {

        private readonly IConfiguration _configuration;
        public HistoryController (IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpGet]
        public JsonResult Get()
        {
            string query = @"
                            select searchId, city from
                            dbo.history
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("HistoryAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon =new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using(SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult(table);
        }

        [HttpPost]
        public JsonResult Post(History h)
        {
            string query = @"
                            insert into dbo.history
                            values (@city)
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("HistoryAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@city", h.city);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Post Correcto");
        }

        [HttpPut]
        public JsonResult Put(History h)
        {
            string query = @"
                            update dbo.history
                            set city=@city
                            where searchId=@searchId
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("HistoryAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@searchId", h.searchId);
                    myCommand.Parameters.AddWithValue("@city", h.city);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Put Correcto");
        }

        [HttpDelete("{id}")]
        public JsonResult Delete(int id)
        {
            string query = @"
                            delete from dbo.history
                            where searchId=@searchId
                            ";
            DataTable table = new DataTable();
            string sqlDataSource = _configuration.GetConnectionString("HistoryAppCon");
            SqlDataReader myReader;
            using (SqlConnection myCon = new SqlConnection(sqlDataSource))
            {
                myCon.Open();
                using (SqlCommand myCommand = new SqlCommand(query, myCon))
                {
                    myCommand.Parameters.AddWithValue("@searchId", id);
                    myReader = myCommand.ExecuteReader();
                    table.Load(myReader);
                    myReader.Close();
                    myCon.Close();
                }
            }

            return new JsonResult("Delete correcto");
        }

    }
}
