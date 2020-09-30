using System;

namespace File2LinqApp.Domain
{
    
    public class Dominio
    {
        
        public int Id { get; set; }
        public Processamento Processamento { get; set; }
        public Arquivo ArquivoDominio { get; set; }
        public string ColunaPkDominio { get; set; }
        public string ColunaFKPrincipal { get; set; }
    }
}