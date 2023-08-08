namespace hl.Nos
{
    public class StringNo : No
    {
        public StringNo(Trecho trecho, string conteudo) : base(trecho)
        {
            Conteudo = conteudo;
        }

        public string Conteudo { get; set; }


        protected override void CompilarImplementa(Ambiente amb, Arquitetura arq)
        {
            int rotulo = amb.RotuloNovo;
            if(amb.CampoParametroVariavel)
            {
                amb.Bits = arq.BitsPonteiro();
            }
            arq.EmiteMudarSegmento(Segmento.DadosInicializados);
            arq.EmiteRotulo(rotulo, amb.Nivel > 0);
            arq.EmiteString(Conteudo);
            arq.EmiteMudarSegmento(Segmento.Codigo);
            arq.EmiteCopiaEnderecoRotuloParaA(amb.Bits, rotulo, amb.Nivel > 0);
        }

    }
}