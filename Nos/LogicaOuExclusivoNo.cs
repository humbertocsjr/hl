namespace hl.Nos
{
    public class LogicaOuExclusivoNo: No
    {
        public LogicaOuExclusivoNo(Trecho trecho) : base(trecho)
        {
        }

        protected override void CompilarImplementa(Ambiente amb, Arquitetura arq)
        {
            int psim = amb.RotuloNovo;
            int nao = amb.RotuloNovo;
            int sim = amb.RotuloNovo;
            int fim = amb.RotuloNovo;
            PrimeiroAbaixo?.Compilar(amb, arq);
            arq.EmiteDefineB(amb.Bits, 0);
            arq.EmiteComparacaoBComA(amb.Bits);
            arq.EmitePulaSeDiferenteRotulo(psim, amb.Nivel > 0);
            SegundoAbaixo?.Compilar(amb, arq);
            arq.EmiteDefineB(amb.Bits, 0);
            arq.EmiteComparacaoBComA(amb.Bits);
            arq.EmitePulaSeDiferenteRotulo(sim, amb.Nivel > 0);
            arq.EmitePulaParaRotulo(nao, amb.Nivel > 0);
            arq.EmiteRotulo(psim, amb.Nivel > 0);
            SegundoAbaixo?.Compilar(amb, arq);
            arq.EmiteDefineB(amb.Bits, 0);
            arq.EmiteComparacaoBComA(amb.Bits);
            arq.EmitePulaSeIgualRotulo(sim, amb.Nivel > 0);
            arq.EmiteRotulo(nao, amb.Nivel > 0);
            arq.EmiteDefineA(amb.Bits, 0);
            arq.EmitePulaParaRotulo(fim, amb.Nivel > 0);
            arq.EmiteRotulo(sim, amb.Nivel > 0);
            arq.EmiteDefineA(amb.Bits, 1);
            arq.EmiteRotulo(fim, amb.Nivel > 0);
        }

    }
}