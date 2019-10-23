using D3WebCrawler.Model.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3WebCrawler.Model
{
    public class Componente
    {
        public string Nome { get; set; }
        public string URL { get; set; }
        public TipoComponenteEnum Tipo { get; set; }
    }
}
