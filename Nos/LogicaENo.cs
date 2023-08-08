namespace hl.Nos
{
    public class LogicaENo : No
    {
        public LogicaENo(Trecho trecho) : base(trecho)
        {
        }

        protected override void CompilarImplementa(Ambiente amb, Arquitetura arq)
        {
            int nao = amb.RotuloNovo;
            int fim = amb.RotuloNovo;
            PrimeiroAbaixo?.Compilar(amb, arq);
            arq.EmiteDefineB(amb.Bits, 0);
            arq.EmiteComparacaoBComA(amb.Bits);
            arq.EmitePulaSeIgualRotulo(nao, amb.Nivel > 0);
            SegundoAbaixo?.Compilar(amb, arq);
            arq.EmiteDefineB(amb.Bits, 0);
            arq.EmiteComparacaoBComA(amb.Bits);
            arq.EmitePulaSeIgualRotulo(nao, amb.Nivel > 0);
            arq.EmiteDefineA(amb.Bits, 1);
            arq.EmitePulaParaRotulo(fim, amb.Nivel > 0);
            arq.EmiteRotulo(nao, amb.Nivel > 0);
            arq.EmiteDefineA(amb.Bits, 0);
            arq.EmiteRotulo(fim, amb.Nivel > 0);
        }

    }
}