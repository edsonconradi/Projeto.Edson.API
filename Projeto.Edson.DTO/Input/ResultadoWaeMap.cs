using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projeto.Edson.DTO.Input
{

    public class ResultadoWaeMap
    {
        public coord coord { get; set; }
        public List<weather> weather { get; set; }
        public main main { get; set; }
        public long visibility { get; set; }
        public wind wind { get; set; }
        public clouds clouds { get; set; }
        public double dt { get; set; }
        public sys sys { get; set; }
        public long timezone { get; set; }
        public long id { get; set; }
        public string name { get; set; }
        public int cod { get; set; }
    }


    public class coord
    {
        public decimal lon { get; set; }
        public decimal lat { get; set; }
    }

    public class weather
    {
        public int id { get; set; }
        public string main { get; set; }
        public string description { get; set; }
        public string icon { get; set; }
    }

    public class main
    {
        public decimal temp { get; set; }
        public decimal feels_like { get; set; }
        public decimal temp_min { get; set; }
        public decimal temp_max { get; set; }
        public int pressure { get; set; }
        public int humidity { get; set; }
    }

    public class wind
    {
        public string speed { get; set; }
        public string deg { get; set; }
    }

    public class clouds
    {
        public int all { get; set; }
    }

    public class sys
    {
        public int type { get; set; }
        public int id { get; set; }
        public string country { get; set; }
        public long sunrise { get; set; }
        public long sunset { get; set; }
    }
}
