using System;
using System.Linq;
using File2LinqApp.Context;
using File2LinqApp.Core;

namespace File2LinqApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ContextDB db = new ContextDB(true);

            
            foreach (var process in db.Processamentos)
            {
                File2Linq arquivoPrincipal = new File2Linq(process.ArquivoPrincipal);
                var dominio = db.Dominios.Find( d => d.Processamento.id == process.id);
                File2Linq arquivoDominio = new File2Linq(dominio.ArquivoDominio);
                
                var join = arquivoPrincipal.rows
                .Join(
                    arquivoDominio.rows, 
                    p => p.ConsultaValor(dominio.ColunaFKPrincipal), 
                    d => d.ConsultaValor(dominio.ColunaPkDominio),
                    (p,d) => new { Principal = p, Dominio = d}
                );

                foreach (var s in join.Take(10))
                {
                    string jsonSaida = "{";
                    string separador = "";

                    foreach (var sd in db.Saidas.Where( s => s.Processamento.id == process.id))
                    {
                        jsonSaida += separador + sd.ColunaSaida + ":" + s.Dominio.ConsultaValor(sd.ColunaOrigem);
                        separador = ";";
                    }

                    jsonSaida += "}";
                                            
                    Console.WriteLine ( jsonSaida);
                }

            }

            
            Console.WriteLine("Good By!");
        }
    }
}
