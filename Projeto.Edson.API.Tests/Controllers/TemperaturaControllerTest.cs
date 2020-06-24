using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
        public void GetTemperaturaAPITimeOut()
        {
            //Chando servico consulta dados
            var consultaService = new Consult();
            consultaService.IniciarServico();

            //Aguarda o serviço obter ao menos uma amostragem
            int segundosCarregando = 0;
            while (!Consult.Carregado)
            {
                Thread.Sleep(1000);
                segundosCarregando++;

                if (segundosCarregando > 15)
                    Assert.Fail("Demora excessiva ao consultar api.openweathermap.org");
            }

            Assert.IsTrue(segundosCarregando < 15);
        }


        [TestMethod]
        public void GetTemperaturaDadosAPITest()
        {
            //Chando servico consulta dados
            var consultaService = new Consult();
            consultaService.IniciarServico();

            //Aguarda o serviço obter ao menos uma amostragem
            AguardaCarregamentoInicial();

            var controller = new TemperaturaController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var consulta = new CidadeDTO();

            //Cidades separadas por VIRGULA
            consulta.Cidades = "Florianópolis , São Paulo";
            consulta.DataInicial = DateTime.Now.AddDays(-1);
            consulta.DataFinal = DateTime.Now.AddDays(20);

            //Consulta API 
            var responseTask = controller.Get(consulta);

            var readTask = responseTask.Result.Content.ReadAsStringAsync();
            readTask.Wait();         

            var listaRetorno = Newtonsoft.Json.JsonConvert.DeserializeObject<List<TemperaturaDTO>>(readTask.Result);

            //Testando quantidade de retorno
            Assert.AreEqual(2, listaRetorno.Count());

            //Cidade 01
            //Valida Nome
            Assert.AreEqual("Florianópolis", listaRetorno[0].Cidade);

            //Valida Temperatura
            Assert.IsTrue(listaRetorno[0].Temperatura > 0 && listaRetorno[0].Temperatura < 200);

            //Valida Data
            Assert.IsTrue(listaRetorno[0].DataHora < DateTime.Now && listaRetorno[1].DataHora > DateTime.Now.AddDays(-1));

            //Cidade 02
            //Valida Nome
            Assert.AreEqual("São Paulo", listaRetorno[1].Cidade);

            //Valida Temperatura
            Assert.IsTrue(listaRetorno[1].Temperatura > 0 && listaRetorno[0].Temperatura < 200);

            //Valida Data
            Assert.IsTrue(listaRetorno[1].DataHora < DateTime.Now && listaRetorno[1].DataHora > DateTime.Now.AddDays(-1));

        }

        [TestMethod]
        public void GetTemperaturaAPITestSucessHTTP()
        {
            //Chando servico consulta dados
            var consultaService = new Consult();
            consultaService.IniciarServico();

            //Aguarda o serviço obter ao menos uma amostragem
            AguardaCarregamentoInicial();

            var controller = new TemperaturaController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var consulta = new CidadeDTO();

            //Cidades separadas por VIRGULA
            consulta.Cidades = "Florianópolis , São Paulo";
            consulta.DataInicial = DateTime.Now.AddDays(-1);
            consulta.DataFinal = DateTime.Now.AddDays(20);


            //Consulta API 
            var responseTask = controller.Get(consulta);

            var readTask = responseTask.Result.Content.ReadAsStringAsync();
            readTask.Wait();

            var result = responseTask.Result;

            //Valida Retorno HTTP
            Assert.IsTrue(result.IsSuccessStatusCode);

        }        

        [TestMethod]
        public void GetTemperaturaAPITestErrorHTTP()
        {
            //Chando servico consulta dados
            var consultaService = new Consult();
            consultaService.IniciarServico();

            //Aguarda o serviço obter ao menos uma amostragem
            AguardaCarregamentoInicial();

            var controller = new TemperaturaController();
            controller.Request = new HttpRequestMessage();
            controller.Configuration = new HttpConfiguration();

            var consulta = new CidadeDTO();

            //Cidades EMPTY
            consulta.Cidades = "";
            consulta.DataInicial = DateTime.Now.AddDays(-1);
            consulta.DataFinal = DateTime.Now.AddDays(20);


            //Consulta API 
            var responseTask = controller.Get(consulta);

            var readTask = responseTask.Result.Content.ReadAsStringAsync();
            readTask.Wait();

            var result = responseTask.Result;

            //Valida Retorno HTTP          
            Assert.IsTrue(result.StatusCode == HttpStatusCode.NotFound);
            Assert.IsTrue(((ObjectContent)result.Content).Value == "Nome cidade inválido");

        }


        private static void AguardaCarregamentoInicial()
        {
            int segundosCarregando = 0;
            while (!Consult.Carregado)
            {
                Thread.Sleep(1000);
                segundosCarregando++;

                if (segundosCarregando > 15)
                    Assert.Fail("Demora excessiva ao consultar api.openweathermap.org");
            }
        }
    }
}
