using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Mercadinho
{
    public class Produtos
    {
        public string Codigo { get; set; }
        public string Descricao { get; private set; }
        public double Preco { get; private set; }
        public double Qntd { get; private set; }
        public string Tipo { get; set; }

        public Produtos() { }
        public Produtos(string codigo, string descricao, double preco, double qntd, string tipo)
        {
            Codigo = codigo;
            Descricao = descricao;
            Preco = preco;
            Qntd = qntd;
            Tipo = tipo;
        }

        public double ValorEstoque()
        {
            return Preco * Qntd;
        }

        public override int GetHashCode()
        {
            return Codigo.GetHashCode();
        }

        public override bool Equals(object? obj)
        {
            if(!(obj is Produtos)) { return false; }
            Produtos objProduto = obj as Produtos;
            return Codigo.Equals(objProduto.Codigo);
        }
    }

    public class Cupom
    {
        private int _numeroCupom;
        private DateTime _dataEmissaoCupom = new DateTime();
        private string _cpfCliente;
        private List<ItemCupom> ListaItemCupom;

        public Cupom()
        {
            _numeroCupom = 0;
            _dataEmissaoCupom = DateTime.Now;
            _cpfCliente = "";
            ListaItemCupom = new List<ItemCupom>() { };
        }

        public Cupom(int numCupom, DateTime data, string Cpf, List<ItemCupom> listCupom)
        {
            _numeroCupom = numCupom;
            _dataEmissaoCupom = data;
            _cpfCliente = Cpf;
            ListaItemCupom = listCupom;
        }

        public void FecharCupom()
        {
            ServicosDAL.AtualizarProxNumCupom(_numeroCupom);
            Console.WriteLine("Venda concluída com sucesso!");
        }

        public List<ItemCupom> GetItemCupoms()
        {
            return ListaItemCupom;
        }
        public int numeroCupom {
            get { return _numeroCupom; }
            set
            {
                if (value != 0)
                {
                    _numeroCupom = value;
                }
            }
        }
        public double CalcularValorTotal()
        {
            double total = 0;
            foreach(ItemCupom item in ListaItemCupom)
            {
                total += item.CalcularValorItem();
            }
            return total;
        }

        public DateTime DataEmissaoCupom
        {
            get { return _dataEmissaoCupom; }
            set { _dataEmissaoCupom = value; }
        }

        public string CpfCliente
        {
            get { return _cpfCliente; }
            set { _cpfCliente = value; }
        }

        public List<ItemCupom> ItemCupom
        {
            get { return ListaItemCupom; }
            set { ListaItemCupom = value; }
        }
    }

    public class ItemCupom
    {
        private Produtos _produto;
        private double _quantidade;

        public ItemCupom() {}

        public Produtos Produto
        {
            get { return _produto; }
            set { _produto = value; }
        }

        public double Quantidade
        {
            get { return _quantidade; }
            set
            {
                if (value > 0)
                {
                    _quantidade = value;
                }
            }
        }

        public double CalcularValorItem()
        {
            return this._quantidade * this._produto.Preco;
        }
    }
}

