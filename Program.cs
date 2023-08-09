using hl.Arquiteturas;

namespace hl
{
    class Program
    {
        const int Version = 0;
        const int SubVersion = 1;
        const int Revision = 1;

        static void Main(string[] args)
        {
            Saida? saida= null;
            string saidaObj = "";
            string saidaAsm = "";
            Ambiente? ambiente= null;
            List<string> fontes = new List<string>();
            Arquitetura? arquitetura = null;
            bool arq8086 = false;
            bool arq386 = false;
            bool arqx64 = false;
            bool arqz80 = false;
            bool asmApenas = false;
            SistemaOperacional sistemaOperacional = SistemaOperacional.Padrao;

            if(args.Length == 0)
            {
                args = new string[]{"-h"};
            }

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i].Equals("-o"))
                {
                    i++;
                    saidaObj = args[i];
                    saidaAsm = saidaObj.EndsWith(".asm") ? saidaObj : saidaObj + ".tmp";
                    if(saidaObj.EndsWith(".asm")) asmApenas = true;
                    saida = new Saida(saidaAsm);
                }
                else if (args[i].Equals("-8086") || args[i].Equals("-i86") || args[i].Equals("-m16"))
                {
                    arq8086 = true;
                }
                else if (args[i].Equals("-386") || args[i].Equals("-i386") || args[i].Equals("-m32"))
                {
                    arq386 = true;
                }
                else if (args[i].Equals("-86-64") || args[i].Equals("-x64") || args[i].Equals("-m64"))
                {
                    arqx64 = true;
                }
                else if (args[i].Equals("-z80") || args[i].Equals("-m8"))
                {
                    arqz80 = true;
                }
                else if(args[i].Equals("-fudeba") || args[i].Equals("-fudebaso"))
                {
                    sistemaOperacional = SistemaOperacional.fudebaSO;
                }
                else if(args[i].Equals("-macos"))
                {
                    sistemaOperacional = SistemaOperacional.macOS;
                }
                else if(args[i].Equals("-win"))
                {
                    sistemaOperacional = SistemaOperacional.Windows;
                }
                else if(args[i].Equals("-linux"))
                {
                    sistemaOperacional = SistemaOperacional.Linux;
                }
                else if(args[i].Equals("-dos"))
                {
                    sistemaOperacional = SistemaOperacional.DOS;
                }
                else if(args[i].Equals("-cpm"))
                {
                    sistemaOperacional = SistemaOperacional.CPM;
                }
                else if(args[i].Equals("-bios"))
                {
                    sistemaOperacional = SistemaOperacional.BIOS;
                }
                else if(args[i].Equals("-asm"))
                {
                    asmApenas = true;
                }
                else if(args[i].Equals("-h") || args[i].Equals("--help") || args[i].Equals("/?") || args[i].Equals("/h") || args[i].Equals("/help"))
                {
                    Console.WriteLine($"Compilador HL v{Version}.{SubVersion} R{Revision}");
                    Console.WriteLine("Copyright (c) 2023, Humberto Costa dos Santos Junior");
                    Console.WriteLine();
                    Console.WriteLine("Modo de uso: hl [destino] [sistema operacional] [opções] [arquivos .hl ...]");
                    Console.WriteLine();
                    Console.WriteLine("Opções:");
                    Console.WriteLine(" -o [arq.o|arq.asm]   = Saida do compilador");
                    Console.WriteLine(" -asm                 = Força apenas o código assembly");
                    Console.WriteLine();
                    Console.WriteLine("Sistemas Operacionais:");
                    Console.WriteLine(" -win                 = Windows");
                    Console.WriteLine(" -linux               = Linux");
                    Console.WriteLine(" -macos               = macOS");
                    Console.WriteLine(" -dos                 = DOS");
                    Console.WriteLine(" -cpm                 = CPM/86");
                    Console.WriteLine(" -bios                = PC BIOS API (Bootloader)");
                    Console.WriteLine(" -fudeba              = fudebaSO");
                    Console.WriteLine();
                    Console.WriteLine("Destinos:");
                    Console.WriteLine(" -m8                  = Destino Z80");
                    Console.WriteLine(" -z80                 ");
                    Console.WriteLine(" -m16                 = Destino 8086/8088");
                    Console.WriteLine(" -i86                 ");
                    Console.WriteLine(" -m32                 = Destino 386");
                    Console.WriteLine(" -i386                ");
                    Console.WriteLine(" -m64                 = Destino x86-64");
                    Console.WriteLine(" -x64                 ");
                }
                else
                {
                    fontes.Add(args[i]);
                }
            }

            if(saida != null  && fontes.Count > 0)
            {
                /*
                try
                {
                */
                if(arq8086) arquitetura = new Arq8086(saida, sistemaOperacional);
                if(arq386) arquitetura = new Arq386(saida, sistemaOperacional);
                if(arqx64) arquitetura = new ArqX64(saida, sistemaOperacional);
                if(arqz80) arquitetura = new ArqZ80(saida, sistemaOperacional);
                if(arquitetura == null)
                {
                    Console.Error.WriteLine("Arquitetura de destino não selecionada");
                    return;
                }
                ambiente = new Ambiente(arquitetura, saida);
                arquitetura.EmiteCabecalho();
                foreach (var item in fontes)
                {
                    ambiente.Adicionar(item);
                }
                ambiente.Compilar();
                arquitetura.EmiteRodape();
                saida.Fechar();
                if(!asmApenas)
                {
                    arquitetura.ChamarAsmLink(saidaAsm, saidaObj);
                    if(saidaAsm.EndsWith(".tmp"))
                    {
                        File.Delete(saidaAsm);
                    }
                }
                /*
                }
                catch(Erro erro)
                {
                    Console.WriteLine(erro.Message);
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                */
            }
        }
    }
}