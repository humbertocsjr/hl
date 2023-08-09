namespace hl.Nos
{
    public class ParaCadaNo : No
    {
        public ParaCadaNo(Trecho trecho, string varIndice, string varItem, string varOrigem, No? conteudo) : base(trecho)
        {
            VarIndice = varIndice;
            VarItem = varItem;
            VarOrigem = varOrigem;
            if(conteudo != null) Abaixo.Add(conteudo);
        }

        public string VarIndice { get; set; }
        public string VarItem { get; set; }
        public string VarOrigem { get; set; }

        protected override void CompilarImplementa(Ambiente amb, Arquitetura arq)
        {
            int loopInicio = amb.RotuloNovo;
            int loopSim = amb.RotuloNovo;
            int loopSim2 = amb.RotuloNovo;
            int loopFim = amb.RotuloNovo;
            string modulo = Trecho.Fonte.Nome.ToUpper();
            AtribuicaoNo indiceGrava = new (Trecho, TipoTrecho.Atribuicao, modulo, VarIndice, false, 0, false, new NumeroNo(Trecho, 0));
            indiceGrava.Compilar(amb, arq);

            LeiaVariavelNo indiceLeia = new (Trecho, modulo, VarIndice, false, 0, false);
            var indice = indiceLeia.Variavel(amb, arq);

            LeiaVariavelNo origemLeia = new (Trecho, modulo, VarOrigem, false, 0, false);
            var origem = origemLeia.Variavel(amb, arq);

            arq.EmiteRotulo(loopInicio, amb.Nivel > 0);

            if(origem.Tipo == Tipo.String)
            {
                if(indice.CalculaBits(amb, arq) != Bits.Bits16 || indice.CalculaSinal(amb, arq) == true) throw new Erro(Trecho, "Tipo de indice inválido, deve ser UInt16");
                arq.EmiteCopiaVarParaA(Bits.Bits16, true, indice.Variavel);
                arq.EmiteEmpilhaA(Bits.Bits16);
                arq.EmiteCopiaVarPtrParaA(Bits.Bits16, true, VarOrigem);
                arq.EmiteDesempilhaB(Bits.Bits16);
                arq.EmiteComparacaoBComA(Bits.Bits16);
                arq.EmitePulaSeMenorQueRotulo(loopSim, amb.Nivel > 0, false);
                arq.EmitePulaParaRotulo(loopFim, amb.Nivel > 0);
                arq.EmiteRotulo(loopSim, amb.Nivel > 0);
                arq.EmiteCopiaVarParaA(Bits.Bits16, true, VarIndice);
                arq.EmiteDefineB(Bits.Bits16, 2);
                arq.EmiteSomaBEmA(Bits.Bits16);
                arq.EmiteCopiaAParaB(Bits.Bits16);
                arq.EmiteCopiaVarPtrIndiceEmBParaA(Bits.Bits8, true, VarOrigem);
                arq.EmiteDefineB(Bits.Bits8, 0);
                arq.EmiteComparacaoBComA(Bits.Bits8);
                arq.EmitePulaSeDiferenteRotulo(loopSim2, amb.Nivel > 0);
                arq.EmitePulaParaRotulo(loopFim, amb.Nivel > 0);
                arq.EmiteRotulo(loopSim2, amb.Nivel > 0);
                arq.EmiteCopiaAParaVar(Bits.Bits8, true, VarItem);
                PrimeiroAbaixo?.Compilar(amb, arq);
                indiceGrava = new (Trecho, TipoTrecho.AtribSoma, modulo, VarIndice, false, 0, false, new NumeroNo(Trecho, 1));
                indiceGrava.Compilar(amb, arq);
                arq.EmitePulaParaRotulo(loopInicio, amb.Nivel > 0);
                arq.EmiteRotulo(loopFim, amb.Nivel > 0);

                
                
            }
            else
            {
                throw new Erro(Trecho, "Tipo de origem invalido, tipos aceitos: String");
            }

        }

    }
}