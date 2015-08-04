using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ToDoList.Models
{
   public class ToDoRepository : IToDoRepository
   {
      readonly string _connectionString = ConfigurationManager.ConnectionStrings["ToDosConnectionString"].ConnectionString;

      public IEnumerable<ToDo> GetAllToDoS()
      {
         List<ToDo> toDoS = new List<ToDo>();
         string query = string.Format("SELECT ToDo.Id, ToDo.Name, Status.StatusName, ToDo.Level, ToDo.CreateDate" +
                                      " FROM [ToDo]  LEFT JOIN [Status]  ON [ToDo].[StatusId] = [Status].[Id] " +
                                      "where [ToDo].[StatusId] <> 3");

         using (SqlConnection con = new SqlConnection(_connectionString))
         {
            SqlCommand cmd = new SqlCommand(query, con);
            {
               con.Open();
               SqlDataReader reader = cmd.ExecuteReader();
               while (reader.Read())
               {
                  var toDo = new ToDo
                              {
                                 Id = reader.GetInt32(0),
                                 Name = reader.GetString(1),
                                 StatusName = reader.GetString(2),
                                 Level = reader.GetInt32(3),
                                 CreateDate = reader.GetDateTime(4)
                              };
                  toDoS.Add(toDo);
               }
            }
         }
         return toDoS.ToArray();
      }

      public ToDo GetToDo(string toDoId)
      {
         throw new NotImplementedException();
      }

      public int AddToDo(ToDo item)
      {
         string query = "IF NOT EXISTS (select [ToDo].[Name], [ToDo].[StatusId] from [ToDo] where Name = @Name and StatusId <>3)" +
                        "INSERT INTO [ToDo] ([ToDo].[Name], [ToDo].[StatusId], [ToDo].[Level], [ToDo].[CreateDate])  " +
                        "VALUES( @Name, @StatusId, @Level, @CreateDate);";

         using (SqlConnection con = new SqlConnection(_connectionString))
         {
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            cmd.Parameters.Add("@Name", SqlDbType.NVarChar, 50).Value = item.Name;
            cmd.Parameters.Add("@StatusId", SqlDbType.Int, 50).Value = 1;
            cmd.Parameters.Add("@Level", SqlDbType.Int, 50).Value = item.Level;
            cmd.Parameters.Add("@CreateDate", SqlDbType.DateTime, 50).Value = DateTime.Now;
            cmd.Parameters.Add("@Id", SqlDbType.Int).Direction = ParameterDirection.Output;

            cmd.ExecuteNonQuery();
            cmd.Parameters.Clear();
            cmd.CommandText = "SELECT @@IDENTITY";

            int id = Convert.ToInt32(cmd.ExecuteScalar());

            return id;
         }
      }

      public bool UpdateToDo(ToDo item)
      {
         throw new NotImplementedException();
      }

      public IEnumerable<ToDo> GetToDosByStatusName(string statusName)
      {
         List<ToDo> toDoS = new List<ToDo>();

         SqlParameter param = new SqlParameter { ParameterName = "@StatusName", Value = statusName };
         string query = string.Format("SELECT [ToDo].[Id], [ToDo].[Name], [ToDo].[StatusId],  [Status].[StatusName]" +
                                      "FROM [ToDo] LEFT JOIN [Status]  ON [ToDo].[StatusId] = [Status].[Id]" +
                                      "WHERE [Status].[StatusName] = @StatusName;");

         using (SqlConnection con = new SqlConnection(_connectionString))
         {
            SqlCommand cmd = new SqlCommand(query, con);
            con.Open();
            cmd.Parameters.Add(param);
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
               ToDo toDo = new ToDo();
               toDo.Id = reader.GetInt32(0);
               toDo.Name = reader.GetString(1);
               toDo.StatusName = reader.GetString(3);

               toDoS.Add(toDo);
            }
         }
         return toDoS.ToArray();
      }

      public void ChangeToDoStatus(int todoId, string newStatusId)
      {
         string query = string.Format("UPDATE [ToDo] SET [ToDo].StatusId = @NewStatus WHERE [ToDo].Id= @Id;");

         using (SqlConnection con =

                 new SqlConnection(_connectionString))
         {
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
               con.Open();
               cmd.Parameters.Add("@NewStatus", SqlDbType.Int).Value = newStatusId;
               cmd.Parameters.Add("@Id", SqlDbType.Int, 50).Value = todoId;
               cmd.ExecuteNonQuery();
            }
         }
      }
      public void ChangeToDoStatusForAll(int newStatusId)
      {
         int oldStatusId = newStatusId == 1 ? 2 : 1;
         string query = string.Format("UPDATE [ToDo] SET [ToDo].StatusId = @NewStatusId WHERE [ToDo].StatusId= @OldStatusId;");

         using (SqlConnection con =

                 new SqlConnection(_connectionString))
         {
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
               con.Open();
               cmd.Parameters.Add("@OldStatusId", SqlDbType.Int).Value = oldStatusId;
               cmd.Parameters.Add("@NewStatusId", SqlDbType.Int).Value = newStatusId;
               cmd.ExecuteNonQuery();
            }
         }
      }

      public IEnumerable<ToDo> DeleteToDo(int todoId)
      {

         string query = string.Format("UPDATE [ToDo] SET [ToDo].StatusId = 3 WHERE [ToDo].Id= @Id;");

         using (SqlConnection con =

                 new SqlConnection(_connectionString))
         {
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
               con.Open();
               cmd.Parameters.Add("@Id", SqlDbType.Int, 50).Value = todoId;
               cmd.ExecuteNonQuery();

            }
         }
         return GetAllToDoS();
      }
   }
}