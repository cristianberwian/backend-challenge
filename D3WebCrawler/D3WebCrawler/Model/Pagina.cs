using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3WebCrawler.Model
{
    public class Pagina
    {
        public Pagina()
        {
            Componentes = new List<Componente>();
        }

        public string Nome { get; set; }
        public string URL { get; set; }

        public List<Componente> Componentes { get; set; }
    }
}
