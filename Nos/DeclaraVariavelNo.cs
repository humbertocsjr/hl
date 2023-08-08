namespace hl.Nos
{
    public class DeclaraVariavelNo : No
    {
        public DeclaraVariavelNo(Trecho trecho, string variavel, Tipo tipo, bool lista, int listaQtd, bool ponteiro, No? valorInicial = null) : base(trecho)
        {
            Variavel = variavel;
            Tipo = tipo;
            Lista = lista;
            ListaQtd = listaQtd;
            Ponteiro = ponteiro;
            if(valorInicial != null)Abaixo.Add(valorInicial);
        }


        public string Modulo => Trecho.Fonte.Nome.ToUpper();
        public string Variavel { get; set; } = "";
        public Tipo Tipo { get; set; } = Tipo.Desconhecido;
        public bool Lista { get; set; } = false;
        public int ListaQtd { get; set; } = 0;
        public bool Ponteiro { get; set; } = false;
        public int DesvioPtrBase { get; set; } = 0;
        public bool Inicializada { get; set; } = false;

        public Bits CalculaBits(Ambiente amb, Arquitetura arq)
        {
            return TipoInfo.TipoParaBits(arq, Tipo, Ponteiro);
        }
        public bool CalculaSinal(Ambiente amb, Arquitetura arq)
        {
            switch(Tipo)
            {
                case Tipo.Int8:
                case Tipo.Int16:
                case Tipo.Int32:
                    return true;
                default:
                    return false;
            }
        }

        protected override void CompilarImplementa(Ambiente amb, Arquitetura arq)
        {
            if(amb.Nivel > 0)
            {
                if(amb.Locais.Any(v => v.Variavel == Variavel))
                {
                    throw new Erro(Trecho, "Variavel já declarada anteriormente");
                }
                amb.Locais.Add(this);
                arq.EmiteConstanteNumerica(amb.Nivel > 0, Variavel, DesvioPtrBase);
                if(Abaixo.Any())
                {
                    Bits anteriorBits = amb.Bits;
                    bool anteriorSinal = amb.Sinal;
                    bool anteriorParVar = amb.CampoParametroVariavel;
                    amb.Bits = CalculaBits(amb, arq);
                    amb.Sinal =CalculaSinal(amb, arq);
                    amb.CampoParametroVariavel = false;

                    CompilarAbaixo(amb, arq);

                    arq.EmiteCopiaAParaVar(amb.Bits, true, Variavel);
                    Inicializada = true;
                    amb.Sinal = anteriorSinal;
                    amb.Bits = anteriorBits;
                    amb.CampoParametroVariavel = anteriorParVar;
                }
            }
            else throw new NotImplementedException("Variavel global nao implementada");
        }

    }
}