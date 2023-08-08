namespace hl.Nos
{
    public class LogicaOuNo: No
    {
        public LogicaOuNo(Trecho trecho) : base(trecho)
        {
        }

        protected override void CompilarImplementa(Ambiente amb, Arquitetura arq)
        {
            int sim = amb.RotuloNovo;
            int fim = amb.RotuloNovo;
            PrimeiroAbaixo?.Compilar(amb, arq);
            arq.EmiteDefineB(amb.Bits, 0);
            arq.EmiteComparacaoBComA(amb.Bits);
            arq.EmitePulaSeDiferenteRotulo(sim, amb.Nivel > 0);
            SegundoAbaixo?.Compilar(amb, arq);
            arq.EmiteDefineB(amb.Bits, 0);
            arq.EmiteComparacaoBComA(amb.Bits);
            arq.EmitePulaSeDiferenteRotulo(sim, amb.Nivel > 0);
            arq.EmiteDefineA(amb.Bits, 0);
            arq.EmitePulaParaRotulo(fim, amb.Nivel > 0);
            arq.EmiteRotulo(sim, amb.Nivel > 0);
            arq.EmiteDefineA(amb.Bits, 1);
            arq.EmiteRotulo(fim, amb.Nivel > 0);
        }

    }
}