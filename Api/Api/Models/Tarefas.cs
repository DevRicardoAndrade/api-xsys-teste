using System;
using System.Collections.Generic;
using System.Data.SqlClient;
namespace Api.Models
{
    public class Tarefas
    {
        SqlConnection connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Tarefas;Integrated Security=True;Pooling=False");

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime? Data_Conclusao { get; set; }
        public char Concluida { get; set; }
        public Tarefas() { }
        public Tarefas(int id, string name, DateTime? data_conclusao, char concluida = 'N')
        {
            this.Id = id;   
            this.Name = name;   
            this.Data_Conclusao = data_conclusao;
            this.Concluida = concluida;

        }
        public bool Save(Tarefas tarefa)
        {
            try
            {
                var data = Convert.ToDateTime(tarefa.Data_Conclusao);
                connection.Open();
                var sql = "";
                if (tarefa.Id > 0)
                    sql = "UPDATE Tarefas SET Name = @Name, Data_Conclusao = @Data_Conclusao, Concluida = @Concluida WHERE Id = @Id";
                else
                    sql = "INSERT INTO Tarefas (Name, Data_Conclusao, Concluida) VALUES (@Name, @Data_Conclusao, @Concluida)";
                using(SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    if(tarefa.Id > 0)
                        cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = tarefa.Id;
                    cmd.Parameters.Add("@Name", System.Data.SqlDbType.VarChar).Value = tarefa.Name;
                    cmd.Parameters.Add("@Data_Conclusao", System.Data.SqlDbType.Date).Value = data.ToString("yyyy-MM-dd");
                    cmd.Parameters.Add("@Concluida", System.Data.SqlDbType.Char).Value = tarefa.Concluida;
                    int linhas = cmd.ExecuteNonQuery();
                    connection.Close();
                    if (linhas > 0)
                        return true;
                    else
                        return false;   
                }
            }
            catch (Exception)
            {
                connection.Close();
                return false;
            }
        }
        public List<Tarefas> Get(int id = 0)
        {
            try
            {
                connection.Open();
                var lista = new List<Tarefas>();
                var sql = "";
                if (id > 0)
                    sql = "SELECT Id, Name, Data_Conclusao, Concluida FROM Tarefas WHERE Id = @Id";
                else
                    sql = "SELECT Id, Name, Data_Conclusao, Concluida FROM Tarefas WHERE Concluida = 'N' ORDER BY Data_Conclusao";
                using(SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    if (id > 0)
                        cmd.Parameters.Add("@Id", System.Data.SqlDbType.Int).Value = id;
                    using(var rd = cmd.ExecuteReader())
                    {
                        while (rd.Read())
                        {
                            lista.Add(new Tarefas(Convert.ToInt32(rd[0].ToString()),
                                rd[1].ToString(),
                                Convert.ToDateTime(rd[2].ToString()),
                                Convert.ToChar(rd[3].ToString())));
                        }
                        rd.Close();
                        connection.Close();
                        return lista;
                    }
                }
            }
            catch (Exception)
            {
                connection.Close();
                return null;
            }
        }
    }
}
