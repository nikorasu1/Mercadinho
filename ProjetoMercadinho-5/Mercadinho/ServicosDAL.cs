using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace Mercadinho
{
    internal class ServicosDAL
    {
        const string arqListaProd = @"C:\TEMP\LISTAPROD.TXT";
        const string arqNumCupom = @"C:\TEMP\NUMCUPOM.TXT";

        public static List<String> ObterTabelaProdutos() 
        {
            List<String> listaTemp = new List<string>();

            StreamReader sr = new StreamReader(arqListaProd);
            try
            {
                while (sr.Peek() != -1)
                {
                    string linha = sr.ReadLine();
                    listaTemp.Add(linha);
                }
            }

            catch (Exception ex) { }

            finally 
            { 
                sr.Close(); 
            }

            //Verificando a lista

            /*foreach(string a in listaTemp)
            {
                Console.WriteLine(a);
            }*/
            return listaTemp;

        }

        public static int ObterProxNumCupom()
        {
            try
            {
                using (StreamReader sr = new StreamReader(arqNumCupom))
                {
                    if (int.TryParse(sr.ReadLine(), out int num))
                    {
                        return num;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Erro ao obter número do cupom: " + ex.Message + "(RETORNANDO 1 COMO PADRÃO)");
            }
            return 1;
        }

        public static void  AtualizarProxNumCupom(int proximoNumCupom )
        {           
            try 
            {
                using FileStream fs = new FileStream(arqNumCupom, FileMode.OpenOrCreate, FileAccess.Write);
                using StreamWriter sw = new StreamWriter(fs);

                int novoNumCupom = proximoNumCupom + 1;
                sw.Write(novoNumCupom);
            }
            catch (Exception ex) 
            {
                Console.WriteLine("Erro ao atualizar:"+ ex.Message);
            }
              
        }

        public static bool GravarCupom(Cupom cupom) 
        {

            try 
            {
                string nomeArquivo = @"C:\TEMP\CUPOM" + cupom.numeroCupom.ToString().PadLeft(6, '0') + ".CSV";

                using FileStream fs = new FileStream(nomeArquivo, FileMode.Create, FileAccess.Write);
                using StreamWriter sw = new StreamWriter(fs);             

                sw.WriteLine("NÚMERO CUPOM: {0}, DATA: {1}, CPF: {2}", cupom.numeroCupom, cupom.DataEmissaoCupom, cupom.CpfCliente);

                foreach(ItemCupom item in cupom.GetItemCupoms())
                {
                    sw.WriteLine("Código: {0}, Produto: {1}, Quantidade: {2:0.00}, Preço Unitario: R$ {3:0.00}", item.Produto.Codigo, item.Produto.Descricao, item.Quantidade, item.Produto.Preco);
                    //sw.WriteLine("Código: {0}, Produto: {1} Quantidade: {2:0.00}, Unidade: {3}, Preço Unitario: R$ {4:0.00}, Valor Total do Item: {5:0.00}", item.Produto.Codigo, item.Produto.Descricao, item.Quantidade, item.Produto.Tipo, item.Produto.Preco, item.CalcularValorItem());
                }
                //Escrever a quantidade comprada, nao a do produto.           

                return true;
            }
            
            catch (Exception ex) 
            {
                Console.WriteLine("Erro ao gravar:" + ex.Message);
                return false;
            }             
        }        
    }
}
