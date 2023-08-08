namespace hl.Nos
{
    public enum TipoComparacao
    {
        Igual,
        Diferente,
        MaiorQue,
        MenorQue,
        MaiorOuIgual,
        MenorOuIgual
    }
    public class ComparacaoNo : No
    {
        public ComparacaoNo(Trecho trecho, TipoComparacao tipo, No primeiro, No segundo) : base(trecho)
        {
            Tipo = tipo;
            Abaixo.Clear();
            Abaixo.Add(primeiro);
            Abaixo.Add(segundo);
        }

        public TipoComparacao Tipo { get; set; }
        public int RotuloDestinoNao {get;set;} = -1;

        protected override void CompilarImplementa(Ambiente amb, Arquitetura arq)
        {
            int sim = amb.RotuloNovo;
            int fim = RotuloDestinoNao >= 0 ? RotuloDestinoNao : amb.RotuloNovo;
            PrimeiroAbaixo?.Compilar(amb, arq);
            arq.EmiteEmpilhaA(amb.Bits);
            SegundoAbaixo?.Compilar(amb, arq);
            arq.EmiteDesempilhaB(amb.Bits);
            arq.EmiteComparacaoBComA(amb.Bits);
            switch (Tipo)
            {
                case TipoComparacao.Igual:
                    arq.EmitePulaSeIgualRotulo(sim, amb.Nivel > 0);
                    break;
                case TipoComparacao.Diferente:
                    arq.EmitePulaSeDiferenteRotulo(sim, amb.Nivel > 0);
                    break;
                case TipoComparacao.MaiorQue:
                    arq.EmitePulaSeMaiorQueRotulo(sim, amb.Nivel > 0, amb.Sinal);
                    break;
                case TipoComparacao.MenorQue:
                    arq.EmitePulaSeMenorQueRotulo(sim, amb.Nivel > 0, amb.Sinal);
                    break;
                case TipoComparacao.MaiorOuIgual:
                    arq.EmitePulaSeMaiorIgualRotulo(sim, amb.Nivel > 0, amb.Sinal);
                    break;
                case TipoComparacao.MenorOuIgual:
                    arq.EmitePulaSeMenorIgualRotulo(sim, amb.Nivel > 0, amb.Sinal);
                    break;
                default:
                    break;
            }
            if(RotuloDestinoNao > 0)
            {
                arq.EmitePulaParaRotulo(fim, amb.Nivel > 0);
                arq.EmiteRotulo(sim, amb.Nivel > 0);
            }
            else
            {
                arq.EmiteDefineA(amb.Bits, 0);
                arq.EmitePulaParaRotulo(fim, amb.Nivel > 0);
                arq.EmiteRotulo(sim, amb.Nivel > 0);
                arq.EmiteDefineA(amb.Bits, 1);
                arq.EmiteRotulo(fim, amb.Nivel > 0);
            }
        }

    }
}