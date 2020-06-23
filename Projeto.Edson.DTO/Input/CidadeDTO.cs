using System;
using System.ComponentModel.DataAnnotations;


namespace Projeto.Edson.DTO.Input
{
    public class CidadeDTO
    {
        [Required]
        public string Cidades { get; set; }

        [Required]
        public DateTime DataInicial { get; set; }

        [Required]
        public DateTime DataFinal { get; set; }
    }
}
