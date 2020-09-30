using System;

namespace File2LinqApp.Domain
{
    public class Processamento
    {


        public int id { get; set; }
        public string Nome { get; set; }
        public Arquivo ArquivoPrincipal { get; set; }
    }
}