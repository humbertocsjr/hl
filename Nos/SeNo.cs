namespace hl.Nos
{
    public class SeNo : No
    {
        public SeNo(Trecho trecho, No comparacao, No sim, No? nao) : base(trecho)
        {
            Comparacao = comparacao;
            Abaixo.Add(sim);
            if(nao != null)Abaixo.Add(nao);

        }

        public No Comparacao { get; set; }


        protected override void CompilarImplementa(Ambiente amb, Arquitetura arq)
        {
            int nao = amb.RotuloNovo;
            Bits antBits = amb.Bits;
            bool antSinal = amb.Sinal;
            bool antParVar = amb.CampoParametroVariavel;
            amb.Bits = arq.BitsPadrao();
            amb.Sinal = false;
            amb.CampoParametroVariavel = false;
            if(Comparacao is ComparacaoNo cmp)
            {
                cmp.RotuloDestinoNao = nao;
                cmp.Compilar(amb, arq);
            }
            else
            {
                int sim = amb.RotuloNovo;
                Comparacao.Compilar(amb, arq);
                arq.EmiteDefineB(amb.Bits, 0);
                arq.EmiteComparacaoBComA(amb.Bits);
                arq.EmitePulaSeDiferenteRotulo(sim, amb.Nivel > 0);
                arq.EmitePulaParaRotulo(nao, amb.Nivel > 0);
                arq.EmiteRotulo(sim, amb.Nivel > 0);
            }
            amb.Bits = antBits;
            amb.Sinal = antSinal;
            amb.CampoParametroVariavel = antParVar;
            PrimeiroAbaixo?.Compilar(amb, arq);
            if(SegundoAbaixo != null)
            {
                int fim = amb.RotuloNovo;
                arq.EmitePulaParaRotulo(fim, amb.Nivel > 0);
                arq.EmiteRotulo(nao, amb.Nivel > 0);
                SegundoAbaixo.Compilar(amb, arq);
                arq.EmiteRotulo(fim, amb.Nivel > 0);
            }
            else
            {
                arq.EmiteRotulo(nao, amb.Nivel > 0);
            }
        }

    }
}