using hl.Nos;

namespace hl
{
    public class Ambiente
    {
        int _rotulo = 0;
        public string SeletorDeArquitetura {get;set;} = "";
        public Bits Bits { get; set; } = Bits.Nenhum;
        public bool Sinal { get; set; } = false;
        public bool CampoParametroVariavel {get;set;} = false;
        public Bits BitsRetorno { get; set; } = Bits.Nenhum;
        public int Nivel { get; set; } = 0;
        public int RotuloNovo => _rotulo++;
        public int RotuloFimFunc { get; set; } = 0;
        public List<Fonte> Fontes { get; set; } = new List<Fonte>();
        public Arquitetura Arquitetura { get; set; }
        public Saida Saida { get; set; }

        public List<DeclaraVariavelNo> Locais = new ();

        public List<FuncaoNo> Funcoes { get; set; } = new ();

        public List<FuncaoNo> FuncoesDeclaradas { get; set; } = new ();
        public List<FuncaoNo> FuncoesUsadas { get; set; } = new ();

        public Ambiente(Arquitetura arq, Saida saida)
        {
            Arquitetura = arq;
            Saida = saida;
        }

        public void Adicionar(string endereco)
        {
            Fonte fonte = new(endereco);
            Fontes.Add(fonte);
            Sintaxe.Processar(this, fonte);
        }

        public void Compilar()
        {
            foreach (var item in Funcoes.Where(f => f.Nome == "_START" || f.Nome == "MAIN"))
            {
                item.BuscarReferencias(this, Arquitetura);
            }
            foreach (var item in FuncoesUsadas.OrderBy(f => f.Nome == "_START" ? 0 : f.Nome == "MAIN" ? 1 : 2))
            {
                item.Otimiza(this, Arquitetura).Compilar(this, Arquitetura);
            }
        }

    }
}