namespace hl.Nos
{
    public class AtribuicaoNo : No
    {
        public AtribuicaoNo(Trecho trecho, TipoTrecho tipo, string modulo, string nome, bool indice, int pos, bool ponteiro, No? valor = null) : base(trecho)
        {
            Modulo = modulo;
            Nome = nome;
            TipoAtribuicao = tipo;
            Indice = indice;
            Posicao = pos;
            Ponteiro = ponteiro;
            if(valor != null) Abaixo.Add(valor);
        }

        public TipoTrecho TipoAtribuicao { get; set; }

        public string Modulo { get; set; }
        public string Nome { get; set; }
        public bool Indice { get; set; }
        public int Posicao { get; set; }
        public bool Ponteiro { get; set; }


        protected override void CompilarImplementa(Ambiente amb, Arquitetura arq)
        {
            var cons = from v in amb.Locais where v.Variavel == Nome select v;
            if(cons.Any())
            {
                Bits anteriorBits = amb.Bits;
                bool anteriorSinal = amb.Sinal;
                bool anteriorParVar = amb.CampoParametroVariavel;
                amb.Bits = cons.First().CalculaBits(amb, arq);
                amb.Sinal =cons.First().CalculaSinal(amb, arq);
                amb.CampoParametroVariavel = false;
                PrimeiroAbaixo?.Compilar(amb, arq);
                cons.First().Inicializada = true;
                switch(TipoAtribuicao)
                {
                    case TipoTrecho.Atribuicao:
                        arq.EmiteCopiaAParaVar(amb.Bits, true, Nome);
                        break;
                    case TipoTrecho.AtribSoma:
                        arq.EmiteSomaAEmVar(amb.Bits, true, Nome);
                        break;
                    case TipoTrecho.AtribSubtracao:
                        arq.EmiteSubtraiAEmVar(amb.Bits, true, Nome);
                        break;
                    case TipoTrecho.AtribMultiplicacao:
                        arq.EmiteMultiplicaAEmVar(amb.Bits, true, Nome, cons.First().CalculaSinal(amb, arq));
                        break;
                    case TipoTrecho.AtribDivisao:
                        arq.EmiteDivideAEmVar(amb.Bits, true, Nome, cons.First().CalculaSinal(amb, arq));
                        break;
                    default:
                        throw new NotImplementedException();
                }
                
                amb.Bits = anteriorBits;
                amb.Sinal = anteriorSinal;
                amb.CampoParametroVariavel = anteriorParVar;
            }
            else throw new NotImplementedException("Variavel global nao implementada");
        }

    }
}