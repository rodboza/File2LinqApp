using System;
using System.Linq;
using File2LinqApp.Context;
using File2LinqApp.Core;
using System.Collections.Generic;

namespace File2LinqApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            ContextDB db = new ContextDB(true);

            
            foreach (var processamento in db.Processamentos)
            {
                File2Linq arquivoPrincipal = new File2Linq(processamento.ArquivoPrincipal);
                File2Linq [] arquivosDominio = db.Dominios
                                                .Where( d => d.Processamento.id == processamento.id )
                                                .Select( d => new File2Linq(d.ArquivoDominio))
                                                .ToArray<File2Linq>();
                
                foreach (var linhaPrincipal  in arquivoPrincipal.rows.Take(500))
                {
                    List<ArquivoLinhas> arquivoLinhas = new List<ArquivoLinhas>();
                    arquivoLinhas.Add(new ArquivoLinhas { NomeArquivo = processamento.ArquivoPrincipal.Nome, Linha = linhaPrincipal} );

                    var arquivoLinhasDominio = db.Dominios
                        .Where( s => s.Processamento.id == processamento.id)
                        .Select( dominio => new ArquivoLinhas { 
                                
                                NomeArquivo = dominio.ArquivoDominio.Nome, 
                                Linha = arquivosDominio
                                        .Single( a => a.className == dominio.ArquivoDominio.Nome)
                                        .rows
                                        .Single(d => d.ConsultaValor(dominio.ColunaPkDominio) == linhaPrincipal.ConsultaValor(dominio.ColunaFKPrincipal))
                                }
                        ).ToArray<ArquivoLinhas>();
                    arquivoLinhas.AddRange(arquivoLinhasDominio);

                    
                    var camposSaida = db.Saidas
                        .Where( s => s.Processamento.id == processamento.id)
                        .Select( saida => {
                            string jsonSaida = 
                                saida.ColunaSaida + ":" + 
                                arquivoLinhas.Single( s => s.NomeArquivo == saida.ArquivoOrigem.Nome).Linha.ConsultaValor(saida.ColunaOrigem);
                            return jsonSaida;
                        } );
                    camposSaida.Prepend("{");
                    camposSaida.Append("}");

                    Console.WriteLine ( string.Join(';', camposSaida ));
                }


            }

            
            Console.WriteLine("Good By!");
        }
    }
}
