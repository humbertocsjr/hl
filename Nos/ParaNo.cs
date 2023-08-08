namespace hl.Nos
{
    public class ParaNo : No
    {
        public ParaNo(Trecho trecho, string variavel, No valorInicial, No valorFinal, No incremento, No conteudo) : base(trecho)
        {
            Variavel = variavel;
            ValorInicial = valorInicial;
            ValorFinal = valorFinal;
            Incremento = incremento;
            Abaixo.Add(conteudo);
        }

        public string Variavel {get;set;}
        public No ValorInicial {get;set;}
        public No ValorFinal {get;set;}
        public No Incremento {get;set;}


        protected override void CompilarImplementa(Ambiente amb, Arquitetura arq)
        {
            int loopInicio = amb.RotuloNovo;
            int loopSim = amb.RotuloNovo;
            int loopFim = amb.RotuloNovo;
            string modulo = Trecho.Fonte.Nome.ToUpper();
            AtribuicaoNo atrib = new (ValorInicial.Trecho, TipoTrecho.Atribuicao, modulo, Variavel, false, 0, false, ValorInicial);
            atrib.Compilar(amb, arq);

            arq.EmiteRotulo(loopInicio, amb.Nivel > 0);
            LeiaVariavelNo leiaVar = new (Trecho, modulo, Variavel, false, 0, false);
            leiaVar.Compilar(amb, arq);
            bool sinal = leiaVar.Variavel(amb, arq).CalculaSinal(amb, arq);
            Bits bits = leiaVar.Variavel(amb, arq).CalculaBits(amb, arq);
            if(ValorFinal is NumeroNo num)
            {
                arq.EmiteDefineB(bits, num.Numero);
            }
            else
            {
                Bits antBits = amb.Bits;
                bool antSinal = amb.Sinal;
                bool antParVar = amb.CampoParametroVariavel;
                amb.Sinal = sinal;
                amb.Bits = bits;
                amb.CampoParametroVariavel = false;
                arq.EmiteEmpilhaA(amb.Bits);
                ValorFinal.Compilar(amb, arq);
                arq.EmiteCopiaAParaB(amb.Bits);
                arq.EmiteDesempilhaA(amb.Bits);
                amb.Bits = antBits;
                amb.Sinal = antSinal;
                amb.CampoParametroVariavel = antParVar;
            }
            arq.EmiteComparacaoBComA(bits);
            arq.EmitePulaSeMaiorIgualRotulo(loopSim, amb.Nivel > 0, sinal);
            arq.EmitePulaParaRotulo(loopFim, amb.Nivel > 0);
            arq.EmiteRotulo(loopSim, amb.Nivel > 0);

            PrimeiroAbaixo?.Compilar(amb, arq);

            AtribuicaoNo incremento = new (Incremento.Trecho, TipoTrecho.AtribSoma, modulo, Variavel, false, 0, false, Incremento);
            incremento.Compilar(amb, arq);

            arq.EmitePulaParaRotulo(loopInicio, amb.Nivel > 0);
            arq.EmiteRotulo(loopFim, amb.Nivel > 0);
        }

    }
}