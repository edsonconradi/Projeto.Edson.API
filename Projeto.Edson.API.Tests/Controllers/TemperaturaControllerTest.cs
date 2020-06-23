using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Projeto.Edson.API;
using Projeto.Edson.API.Controllers;
using Projeto.Edson.DTO.Input;
using Projeto.Edson.DTO.Output;
using Projeto.Edson.Services;

namespace Projeto.Edson.API.Tests.Controllers
{
    [TestClass]
    public class TemperaturaControllerTest
    {
        [TestMethod]
        public void GetCorret()
        {
            //Chando servico consulta dados
            var consultaService = new Consult();
            consultaService.IniciarServico();

            Thread.Sleep(6000);

            var controller = new TemperaturaController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var consulta = new CidadeDTO();
            consulta.Cidades = "Florianópolis , São Paulo";
            consulta.DataInicial = DateTime.Now.AddDays(-1);
            consulta.DataFinal = DateTime.Now.AddDays(20);


            //Consulta API 
            var responseTask = controller.Get(consulta);

            var readTask = responseTask.Result.Content.ReadAsStringAsync();
            readTask.Wait();

            var result = responseTask.Result;

            if (result.IsSuccessStatusCode)
            {
                var listaRetorno = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TemperaturaDTO>>(readTask.Result);

                //Testando retorno
                Assert.AreEqual(2, listaRetorno.Count());
                Assert.AreEqual("Florianópolis", listaRetorno[0].Cidade);
                Assert.AreEqual("São Paulo", listaRetorno[1].Cidade);
            }
            else
            {
                Assert.Fail();
            }
        }
    }
}
