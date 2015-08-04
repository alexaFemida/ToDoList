using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using ToDoList.Models;

namespace ToDoList.Controllers
{
   public class ToDosController : ApiController
   {
      IToDoRepository _repository = new ToDoRepository();

      public IEnumerable<ToDo> GetAllTodos()
      {
         return _repository.GetAllToDoS();
      }

      [HttpGet]
      public IEnumerable<ToDo> ToDosByStatusName(string id)
      {
         return _repository.GetToDosByStatusName(id);
      }

      public HttpResponseMessage ChangeStatus(JObject jsonData)
      {
         dynamic json = jsonData;
         int itemId = json.itemId;
         string statusId = json.statusId.ToString();
         if (statusId == "1")
            _repository.ChangeToDoStatus(itemId, "2");
         if (statusId == "0")
            _repository.ChangeToDoStatus(itemId, "1");

         return Request.CreateResponse(HttpStatusCode.OK);
      }

      [HttpGet]
      public HttpResponseMessage ChangeStatusForAll(int newStatusId)
      {
         _repository.ChangeToDoStatusForAll(newStatusId);

         return Request.CreateResponse(HttpStatusCode.OK);
      }

      [HttpPost]
      public int Post(ToDo newTodo)
      {
         return _repository.AddToDo(newTodo);

      }

      [HttpGet]
      public IEnumerable<ToDo> Delete(int id)
      {
         return _repository.DeleteToDo(id);
      }
   }
}
