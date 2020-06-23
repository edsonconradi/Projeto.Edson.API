using Newtonsoft.Json.Linq;
using Projeto.Edson.DTO.Input;
using Projeto.Edson.DTO.Output;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Text;
using System.Threading;

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

            Thread.Sleep(900000);

            Processar();
        }

        private void ObterDadosCidade(string cidade)
        {
            try
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

                        var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<ResultadoWaeMap>(readTask.Result);
                                               
                        var temperatura  = obj.main.temp - decimal.Parse("273,15");

                        var dados = new TemperaturaDTO();
                        dados.Cidade = cidade;
                        dados.DataHora = DateTime.Now;
                        dados.Temperatura = Math.Round(temperatura, 2);

                        ListaDados.Add(dados);
                    }
                }

            }
            catch (Exception)
            {
                //Falha na Conexão
            }
        }

        public static string RemoverAcentos(string text)
        {
            text = text.ToLower();
            StringBuilder sbReturn = new StringBuilder();
            var arrayText = text.Normalize(NormalizationForm.FormD).ToCharArray();
            foreach (char letter in arrayText)
            {
                if (CharUnicodeInfo.GetUnicodeCategory(letter) != UnicodeCategory.NonSpacingMark)
                    sbReturn.Append(letter);
            }
            return sbReturn.ToString();
        }
    }
}
