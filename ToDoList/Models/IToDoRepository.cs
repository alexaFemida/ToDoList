using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDoList.Models
{
   public interface IToDoRepository
   { 
      int AddToDo(ToDo item);
      IEnumerable<ToDo> GetAllToDoS();
      ToDo GetToDo(string toDoID);
      bool UpdateToDo(ToDo item);
      IEnumerable<ToDo> DeleteToDo(int toDoID);
      void ChangeToDoStatus(int todoId, string newStatusId);
      void ChangeToDoStatusForAll(int newStatusId);
      IEnumerable<ToDo> GetToDosByStatusName(string statusName);
   }
}
