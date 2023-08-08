namespace hl.Nos
{
    public class AsmNo : No
    {
        public AsmNo(Trecho trecho, string asm, bool seletorDeArquitetura) : base(trecho)
        {
            Asm = asm;
            SeletorDeArquitetura = seletorDeArquitetura;
        }

        public bool SeletorDeArquitetura { get; set; }
        public string Asm { get; set; }


        protected override void CompilarImplementa(Ambiente amb, Arquitetura arq)
        {
            if(SeletorDeArquitetura)
            {
                amb.SeletorDeArquitetura = Asm;
                return;
            }
            if
            (
                arq.Nome.ToLower() == amb.SeletorDeArquitetura.ToLower() || 
                arq.NomeCompleto.ToLower() == amb.SeletorDeArquitetura.ToLower()
            )
                arq.Emite(Asm);
            if(amb.SeletorDeArquitetura == "")
            {
                throw new Erro(Trecho, "Arquitetura não selecionada dentro desta função. Use o comando: asm [Arquitetura ou Arquitetura_SistemaOperacional] como no exemplo: asm i86_dos");
            }
        }

    }
}