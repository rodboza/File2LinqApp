using System;


namespace File2LinqApp.Core
{

    public class LinhaBase
    {

        public File2Linq file2Linq ;
        public string ConsultaValor(int indiceColuna)
        {
            return ConsultaValor (file2Linq.columnNames[indiceColuna].ToString());
        }
            
        public string ConsultaValor(string nomeColuna)
        {
            var valor = this.GetType().GetProperty(nomeColuna).GetValue(this).ToString();
            return valor;
        }

        public void GravaValor (string nomeColuna, string valor)
        {
            this.GetType().GetProperty(nomeColuna).SetValue(this , valor);
        }
    }




}