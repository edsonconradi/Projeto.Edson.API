using Newtonsoft.Json.Linq;
using Projeto.Edson.DTO.Input;
using Projeto.Edson.DTO.Output;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Projeto.Edson.Services
{
    public class Consult
    {
        public static List<TemperaturaDTO> ListaDados = new List<TemperaturaDTO>();

        public void IniciarServico()
        {
            Thread tr = new Thread(Processar);
            tr.Start();
        }

        public void Processar()
        {
            ObterDadosCidade("Florianópolis");
            ObterDadosCidade("São Paulo");
            ObterDadosCidade("Rio de Janeiro");           

            Thread.Sleep(120000);

            Processar();
        }   

        private void ObterDadosCidade(string cidade)
        {
            using (HttpClient client = new HttpClient())
            {
                var responseTask = client.GetAsync("http://api.openweathermap.org/data/2.5/weather?q=" + cidade + "&appid=637b0fba6f8c59e3026f47b8b5457ab6");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();

                    dynamic dadosRetorno = JObject.Parse(readTask.Result);
                    var temperatura = dadosRetorno.main.temp.Value - 273.15;

                    var dados = new TemperaturaDTO();
                    dados.Cidade = cidade;
                    dados.DataHora = DateTime.Now;
                    dados.Temperatura = Math.Round(temperatura, 2);

                    ListaDados.Add(dados);
                }
            }
        }
    }
}
