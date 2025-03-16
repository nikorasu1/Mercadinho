using System;
using System.Drawing;
using System.Reflection.Emit;
using System.Globalization;

namespace Mercadinho
{
    internal class Program
    {
        // Nome: Carlos Henrique Alves Santos
        // Nome: Nícolas Henrique de Lima Silva
        // Nome: Nicollas Nogueira da Silva

       public static void Linha()
        {
            for (int i = 0; i < 82; i++)
            {
                Console.Write("-");
            }
            Console.WriteLine();
        }

        static void Main(string[] args)
        {         
            string codigo;
            int itemCont = 0;
            int numCupom;
            string CpfCliente = null;
            DateTime data;

            List<string> listaTemp = ServicosDAL.ObterTabelaProdutos();
            List<Produtos> listaProdutos = new List<Produtos>();

            foreach(string linha in listaTemp)
            {
                string[] vetor;
                vetor = linha.Split(',');

                listaProdutos.Add(new Produtos(vetor[0], vetor[1], Convert.ToDouble(vetor[2], CultureInfo.InvariantCulture), Convert.ToDouble(vetor[3], CultureInfo.InvariantCulture), vetor[4]));
            }

            //Tirar as aspas do Codigo EAN13 e dos tipos
            foreach (var produto in listaProdutos)
            {
                produto.Codigo = produto.Codigo.Replace("\"", "");
                produto.Tipo = produto.Tipo.Replace("\"", "");
            }

            Linha();

            Console.WriteLine("|{0,10} {1,5} {2,14} {3,7} {4,7} {5,3} {6,1} {7}", 
                "Código", "|", "Descrição", "|", "Preço", "|", "Quant.Estoque|", "Valor Estoque|");
            Linha();

            foreach (Produtos produto in listaProdutos)
            {
                double valorEstoque = produto.Preco * produto.Qntd;
                Console.WriteLine("|{0,14} {1,1} {2,-20} {3,1} {4,2} {5,6:0.00} {6,1} {7,7:0.00} {8,-4} {9,1} {10, 2} {11, 9:0.00} {12, 1}",
                    produto.Codigo, "|", produto.Descricao, "|", "R$", produto.Preco, "|", produto.Qntd, produto.Tipo, "|", "R$", valorEstoque, "|");
            }
            Linha();

            double totalEstoque = 0;
            foreach (Produtos produto in listaProdutos)
            {
                totalEstoque += produto.ValorEstoque();
            }

            Console.WriteLine("|{0,-61} {1,4} {2,1} {3,9} |", " Total do Estoque:", "|", "R$", totalEstoque);

            Linha();

            Console.WriteLine("Digite algo para limpar a tela:");
            Console.ReadKey();
            Console.Clear();

            //Parte B
        
            numCupom = ServicosDAL.ObterProxNumCupom();
            data = DateTime.Now;

            Console.WriteLine("Informe o CPF do cliente: (XXX.XXX.XXX-XX)");
            CpfCliente = Console.ReadLine();
            Console.Clear();
            //Parte C

            List<ItemCupom> listaItemCupom = new List<ItemCupom>();

            Linha();

            Console.WriteLine("|{0,10} {1,5} {2,14} {3,7} {4,7} {5,3} {6,1} {7}",
                "Código", "|", "Descrição", "|", "Preço", "|", "Quant.Estoque|", "Valor Estoque|");
            Linha();

            foreach (Produtos produto in listaProdutos)
            {
                double valorEstoque = produto.Preco * produto.Qntd;
                Console.WriteLine("|{0,14} {1,1} {2,-20} {3,1} {4,2} {5,6:0.00} {6,1} {7,7:0.00} {8,-4} {9,1} {10, 2} {11, 9:0.00} {12, 1}",
                    produto.Codigo, "|", produto.Descricao, "|", "R$", produto.Preco, "|", produto.Qntd, produto.Tipo, "|", "R$", valorEstoque, "|");
            }
            Linha();

            totalEstoque = 0;
            foreach (Produtos produto in listaProdutos)
            {
                totalEstoque += produto.ValorEstoque();
            }

            Console.WriteLine("|{0,-61} {1,4} {2,1} {3,9} |", " Total do Estoque:", "|", "R$", totalEstoque);

            Linha();
            do
            {
                ItemCupom item = ServicosUI.ObterItemCupom(numCupom, listaProdutos);

                if (item != null)
                {
                    listaItemCupom.Add(item);
                    Console.WriteLine($"Item adicionado: {item.Produto.Descricao}, Quantidade: {item.Quantidade}");
                }
                else
                {
                    Console.WriteLine("Nenhum item foi adicionado ou operação encerrada.");
                    Console.WriteLine("Digite algo para limpar a tela:");
                    break;
                }
            }
            while (listaItemCupom.Count < 20);

            //Parte D

            Cupom cupom = new Cupom(numCupom, data, CpfCliente, listaItemCupom);

            Console.ReadKey();
            Console.Clear();

            //Parte E
            try
            {
                if(ServicosDAL.GravarCupom(cupom) == true)
                {
                    ServicosUI.ImprimirCupom(cupom);
                }

            }

            catch (IOException e)
            {
                Console.WriteLine("Erro: " + e.Message);
            }

            // Parte F
            cupom.FecharCupom();

            Console.WriteLine("Pressione qualquer tecla para sair...");
            Console.ReadKey();
        }
    }
}