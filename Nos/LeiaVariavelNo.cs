namespace hl.Nos
{
    public class LeiaVariavelNo : No
    {
        public LeiaVariavelNo(Trecho trecho, string modulo, string nome, bool lista, int listaPos, bool ponteiro) : base(trecho)
        {
            Modulo = modulo;
            Nome = nome;
            Lista = lista;
            ListaPosicao = listaPos;
            Ponteiro = ponteiro;
        }
        public bool Ponteiro { get; set; }
        public bool Lista { get; set; }
        public int ListaPosicao { get; set; }
        public string Modulo { get; set; }
        public string Nome { get; set; }

        public DeclaraVariavelNo Variavel(Ambiente amb, Arquitetura arq)
        {
            var cons = from v in amb.Locais where v.Modulo == Modulo && v.Variavel == Nome select v;
            if(!cons.Any()) throw new Erro(Trecho, $"Variavel '{Nome}' do módulo {Modulo} não encontrada.");
            var variavel = cons.First();
            return variavel;
        }


        protected override void CompilarImplementa(Ambiente amb, Arquitetura arq)
        {
            var variavel = Variavel(amb, arq);
            if(!variavel.Inicializada) throw new Erro(Trecho, $"Variavel '{Nome}' do módulo {Modulo} não foi inicializada antes de ser lida");
            if(Ponteiro)
            {
                if(!variavel.Ponteiro)
                {
                    throw new Erro(Trecho, $"Variavel '{Nome}' do módulo '{Modulo}' não encontrada.");
                }
                else
                {
                    arq.EmiteCopiaVarPtrParaA(arq.BitsTipo(variavel.Tipo), true, Nome);
                }
            }
            else
            {
                arq.EmiteCopiaVarParaA(variavel.CalculaBits(amb, arq), true, Nome);
            }
        }

    }
}