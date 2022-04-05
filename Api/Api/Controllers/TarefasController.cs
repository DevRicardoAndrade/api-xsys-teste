using Api.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace Api.Controllers
{
    public class TarefasController : Controller
    {
        Tarefas tarefa;
        [HttpGet] // Define o tipo de request da api
        [Route("api/tarefas")] //Define a rota para acessar o endpoint
        public string Get()
        {
            try
            {
                tarefa = new Tarefas();
                var lista = tarefa.Get();

                return JsonConvert.SerializeObject(lista);
            }
            catch(Exception ex)
            {
                return ex.Message;  
            }
           
        }
        [HttpGet] // Define o tipo de request da api
        [Route("api/tarefas/{id}")] //Define a rota para acessar o endpoint {Parametro esperado na URL}
        public string Get(int id)
        {
            try
            {
                tarefa = new Tarefas();
                var lista = tarefa.Get(id);

                return JsonConvert.SerializeObject(lista);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

        }
        [HttpPost] // Define o tipo de request da api
        [Route("api/tarefas/incluir")] //Define a rota para acessar o endpoint
        public JObject Post([FromBody] Tarefas tarefa)
        {
            try
            {
                this.tarefa = new Tarefas(0, tarefa.Name, tarefa.Data_Conclusao, tarefa.Concluida);
                this.tarefa.Save(this.tarefa);
                var retorno = "{resultado:\"Ok\"}";
                return JObject.Parse(retorno);
            }
            catch (Exception ex)
            {
                var retorno = "{resultado:\"" + ex.Message + "\"}";
                return JObject.Parse(retorno);
            }

        }
        [HttpPut] // Define o tipo de request da api
        [Route("api/tarefas/concluir/{id}")] //Define a rota para acessar o endpoint
        public JObject Put(int id)
        {
            try
            {
                this.tarefa = new Tarefas();
                var t = this.tarefa.Get(id).Find(x=> x.Id.Equals(id));
                t.Concluida = 'S';
                this.tarefa.Save(t);
                var retorno = "{resultado:\"Ok\"}";
                return JObject.Parse(retorno);
            }
            catch (Exception ex)
            {
                var retorno = "{resultado:\"" + ex.Message + "\"}";
                return JObject.Parse(retorno);
            }

        }
    }
}
