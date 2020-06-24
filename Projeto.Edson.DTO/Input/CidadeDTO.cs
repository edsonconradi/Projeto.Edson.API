using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace Projeto.Edson.DTO.Input
{
    public class CidadeDTO
    {
        [Required]
        [Description("Cidades a serem consultadas separadas por ',' virgula.")]
        public string Cidades { get; set; }

        [Required]
        [Description("Data inicial da consulta")]
        public DateTime DataInicial { get; set; }

        [Required]
        [Description("Data final da consulta")]
        public DateTime DataFinal { get; set; }
    }
}
