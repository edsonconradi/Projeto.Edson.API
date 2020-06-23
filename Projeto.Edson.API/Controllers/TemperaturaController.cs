﻿using Projeto.Edson.DTO.Input;
using Projeto.Edson.DTO.Output;
using Projeto.Edson.Services;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace Projeto.Edson.API.Controllers
{
    public class TemperaturaController : ApiController
    {
        public async Task<HttpResponseMessage> Get([FromUri] CidadeDTO DTO)
        {
            HttpResponseMessage response;

            if (ModelState.IsValid)
            {
                List<TemperaturaDTO> listaRetorno = new List<TemperaturaDTO>();

                List<string> cidades = new List<string>(DTO.Cidades.Split(','));

                foreach (var cidade in cidades)
                {
                    string nomeCidade = cidade.TrimStart().TrimEnd();

                    listaRetorno.AddRange(Consult.ListaDados.FindAll(x => x.Cidade == nomeCidade && x.DataHora >= DTO.DataInicial && x.DataHora <= DTO.DataFinal));
                }

                listaRetorno = listaRetorno.OrderBy(x => x.DataHora).ToList();

                response = Request.CreateResponse(System.Net.HttpStatusCode.OK, listaRetorno);

            }
            else
            {
                response = Request.CreateResponse(System.Net.HttpStatusCode.NotFound, "Dados da consulta invalidos");
            }

            return response;
        }
    }
}
