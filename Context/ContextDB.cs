using System;
using System.Collections.Generic;
using File2LinqApp.Domain;
using System.Linq;

namespace File2LinqApp.Context
{
    public class ContextDB
    {

        public List<Arquivo> Arquivos { get; set; }
        public List<Dominio> Dominios { get; set; }
        public List<Processamento> Processamentos { get; set; }
        public List<Saida> Saidas { get; set; }


        public ContextDB ()
        {


        }
        public ContextDB (bool Inicializa):this()
        {
            if (Inicializa)
            {
                Inicializar();
            }
        }

        private void Inicializar()
        {

            Arquivos = new List<Arquivo>();
            Dominios = new List<Dominio>();
            Saidas = new List<Saida>();
            Processamentos = new List<Processamento>();

            var arqPrinc = new Arquivo { Id = 1, Nome = "ArquivoPrincipal", Caminho = @".\files\Equities_TradeInformationFile_20180625_1.txt"};
            var arqDomin = new Arquivo { Id = 2, Nome = "ArquivoDominio", Caminho = @".\files\Equities_Rodboza.txt"} ;
            Arquivos.Add( arqPrinc );
            Arquivos.Add( arqDomin );
            
            var proc = new Processamento{ id = 1, Nome="Teste Processamento", ArquivoPrincipal = arqPrinc };
            Processamentos.Add (proc);

            Dominios.Add (new Dominio {Id = 1, Processamento = proc, ArquivoDominio = arqDomin, ColunaFKPrincipal = "TckrSymb", ColunaPkDominio = "TckrSymb" });

            Saidas.Add( new Saida {Id = Saidas.Count, Processamento = proc, ArquivoOrigem = arqPrinc, ColunaOrigem = "TckrSymb", ColunaSaida = "Ativo"});
            Saidas.Add( new Saida {Id = Saidas.Count, Processamento = proc, ArquivoOrigem = arqPrinc, ColunaOrigem = "RptDt", ColunaSaida = "Data"});
            Saidas.Add( new Saida {Id = Saidas.Count, Processamento = proc, ArquivoOrigem = arqPrinc, ColunaOrigem = "ISIN", ColunaSaida = "Codigo"});
            Saidas.Add( new Saida {Id = Saidas.Count, Processamento = proc, ArquivoOrigem = arqDomin, ColunaOrigem = "NovaColunaRodboza", ColunaSaida = "ColunaDominio"});

        }
    }
}