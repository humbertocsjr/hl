namespace hl.Nos
{
    public class EnquantoNo : No
    {
        public EnquantoNo(Trecho trecho, No condicao, No conteudo) : base(trecho)
        {
            Condicao = condicao;
            Abaixo.Add(conteudo);
        }

        public No Condicao {get;set;}


        protected override void CompilarImplementa(Ambiente amb, Arquitetura arq)
        {
            int loopFim = amb.RotuloNovo;
            int loopInicio = amb.RotuloNovo;
            arq.EmiteRotulo(loopInicio, amb.Nivel > 0);

            Bits antBits = amb.Bits;
            bool antSinal = amb.Sinal;
            bool antParVar = amb.CampoParametroVariavel;
            amb.Sinal = false;
            amb.Bits = arq.BitsPadrao();
            amb.CampoParametroVariavel = false;
            if(Condicao is ComparacaoNo cmp)
            {
                cmp.RotuloDestinoNao = loopFim;
                cmp.Compilar(amb, arq);
            }
            else
            {
                int loopSim = amb.RotuloNovo;
                Condicao.Compilar(amb, arq);
                arq.EmiteDefineB(amb.Bits, 0);
                arq.EmiteComparacaoBComA(amb.Bits);
                arq.EmitePulaSeDiferenteRotulo(loopSim, amb.Nivel > 0);
                arq.EmitePulaParaRotulo(loopFim, amb.Nivel > 0);
                arq.EmiteRotulo(loopSim, amb.Nivel > 0 );
            }
            amb.Bits = antBits;
            amb.Sinal = antSinal;
            amb.CampoParametroVariavel = antParVar;

            PrimeiroAbaixo?.Compilar(amb, arq);
            arq.EmitePulaParaRotulo(loopInicio, amb.Nivel > 0);
            arq.EmiteRotulo(loopFim, amb.Nivel > 0);
        }

    }
}