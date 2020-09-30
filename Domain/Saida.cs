using System;

namespace File2LinqApp.Domain
{
    public class Saida
    {
        public int Id { get; set; }
        public Processamento Processamento { get; set; }
        public Arquivo ArquivoOrigem { get; set; }  
        public string ColunaOrigem { get; set; }
        public string ColunaSaida { get; set; }
    }
}