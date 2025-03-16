
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mercadinho
{
    internal class ServicosUI
    {
        public static ItemCupom ObterItemCupom(int numCupom, List<Produtos> listaProdutos)
        {
            string codigoInformado;
            double quantidade;
            Produtos produtoEncontrado = null;

            try
            {
                Console.WriteLine("Informe o código EAN13 (ou 'fim' para terminar a compra):");
                codigoInformado = Console.ReadLine();

                if (codigoInformado == "fim")
                {
                    return null;
                }

                if (string.IsNullOrWhiteSpace(codigoInformado))
                {
                    throw new ApplicationException("O código informado não pode ser nulo ou vazio.");
                }

                produtoEncontrado = listaProdutos.Find(x => x.Codigo == codigoInformado);

                if (produtoEncontrado != null)
                {
                    Console.WriteLine($"Produto encontrado: {produtoEncontrado.Descricao}. Informe a quantidade desejada:");
                    do
                    {
                        quantidade = double.Parse(Console.ReadLine());
                        if(quantidade <= 0)
                        {
                            Console.WriteLine("Quantidade menor ou igual a 0! Por favor informe uma quantidade válida.");
                        }
                    }
                    while (quantidade <= 0);

                    return new ItemCupom
                    {
                        Produto = produtoEncontrado,
                        Quantidade = quantidade
                    };
                }
                else
                {
                    Console.WriteLine("Produto não encontrado.");
                }
            }
            catch (ApplicationException ae)
            {
                Console.WriteLine("Erro: " + ae.Message);
            }
            catch (NullReferenceException nrx)
            {
                Console.WriteLine("Erro: Objeto de referência nulo. (" + nrx.Message + ")");
            }
            catch (FormatException fe)
            {
                Console.WriteLine("Erro: Formato inválido. (" + fe.Message + ")");
            }

            return null;
        }

        public static void ImprimirCupom(Cupom cupom)
        {

            //Layout Cupom Fiscal

            Program.Linha();
            Console.WriteLine("|{0,24} {1,56}", "MERCADINHO MANOEL GOMAS", "|");
            Console.WriteLine("|{0,22} {1,58}", "Rua Atônio Manoel 243", "|");

            Program.Linha();
            Console.WriteLine("|{0,6} {1,72} {2,1}", "Data:", cupom.DataEmissaoCupom, "|");
            Program.Linha();

            Console.WriteLine("|{0,81}", "|");
            Console.WriteLine("|{0,45} {1,35}", "CUPOM FISCAL", "|");
            Console.WriteLine("|{0,81}", "|");
            Program.Linha();

            Console.WriteLine("|{0,17} {1,61} {2,1}", "Número do cupom:", cupom.numeroCupom, "|");
            Console.WriteLine("|{0,5} {1,73} {2,1}", "CPF:", cupom.CpfCliente, "|");

            Program.Linha();
            Console.WriteLine("|{0,7} {1,8} {2,7} {3,12} {4,4} {5,3} {6,1} {7,1} {8,6} {9,3}",
                "Código", "|", "Descrição", "|", "Preço", "|", "Quant.Estoque", "|", "Preço Total", "|");
            Program.Linha();

            foreach (ItemCupom item in cupom.ItemCupom)
            {
                Produtos produto = item.Produto;
                double quantidade = item.Quantidade;
                double precoTotal = produto.Preco * quantidade;
                Console.WriteLine("|{0,14} {1,1} {2,-20} {3,1} {4,-7:C} {5,1} {6,13:0.00} {7,1} {8,13:C} {9,1}",
                    produto.Codigo, "|", produto.Descricao, "|", produto.Preco, "|", quantidade, "|", precoTotal, "|");
            }

            Program.Linha();
            Console.WriteLine("|{0,7} {1,71:C} {2,1}", "Total:", cupom.CalcularValorTotal(), "|");
            Program.Linha();
        }
    }
}
