using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ToDoList.Models
{
   public class ToDo
   {
      public int Id { get; set; }
      public string Name { get; set; }
      public string StatusName { get; set; }
      public int Level { get; set; }
      public DateTime CreateDate { get; set; }
   }
}