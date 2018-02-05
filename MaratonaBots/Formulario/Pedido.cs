using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using System;

namespace MaratonaBots.Formulario
{
    [Serializable]
    [Template(TemplateUsage.NotUnderstood, "Desculpe não entendi \"{0}\".")]
    public class Pedido
    {
        public Salgadinhos Salgadinhos { get; set; }
        public Bebidas Bebidas { get; set; }
        public TipoEntrega TipoEntrega { get; set; }
        public CPFNaNota CPFNaNota { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Endereco { get; set; }


        public static IForm<Pedido> BuildForm()
        {
            //configuração para o bot entender como ele vai montar esse formulario

            var form = new FormBuilder<Pedido>();

            //o padrão de apresentação dos itens na tela, prioriza botões.
            //se deixar em aberto, o usuário pode digitar varias coisas e o fluxo pode acabar se perdendo
            form.Configuration.DefaultPrompt.ChoiceStyle = ChoiceStyleOptions.Buttons;
            form.Configuration.Yes = new string[] { "sim", "yes", "s", "y", "yep" };
            form.Configuration.No = new string[] { "não", "nao", "no", "not", "n", "nop" };

            //mensagem que vai ser exibida quando entrar nesse fluxo
            form.Message("Olá, seja bem vindo. Será um prazer atender você.");
            form.OnCompletion(async (context, pedido) =>
            {
                //salvar na base de dados 
                //gerar pedido
                //integrar com servico Xpto

                await context.PostAsync("Seu pedido número 123456 foi gerado e em instantes será entregue.");
            });

            return form.Build();
        }
    }


    [Describe("Tipo de Entrega")]
    public enum TipoEntrega
    {
        [Terms("Retirar no Local", "Passo ai", "eu pego", "Retiro ai")]
        [Describe("Retirar no Local")]
        RetirarNoLocal = 1,

        [Terms("Motoboy","motoca", "cachorro loco", "cachorro louco", "entrega em casa")]
        [Describe("Motoboy")]
        Motoboy
    }

    [Describe("Salgados")]
    public enum Salgadinhos
    {
        [Terms("Esfirra", "isfirra", "Esfira", "isfira")]
        [Describe("Esfirra")]
        Esfirra = 1,

        [Terms("Kibe", "Quibe", "k", "q")]
        [Describe("Quibe")]
        Quibe,

        [Terms("Coxinha", "cochinha", "coxa", "c")]
        [Describe("Coxinha")]
        Coxinha
    }

    [Describe("Bebida")]
    public enum Bebidas
    {
        [Terms("Água", "Agua", "h2o", "a")] //sinônimos
        [Describe("Água")] // texto que vai aparecer escrito nas opções
        Agua = 1,

        [Terms("Refrigerante", "Refri", "r")] //sinônimos
        [Describe("Refrigerante")]
        Refrigerante,

        [Terms("Suco", "s")] //sinônimos
        [Describe("Suco")]
        Suco
    }

    [Describe("CPF na Nota")]
    public enum CPFNaNota
    {
        [Terms("Sim", "yes", "yep")] //sinônimos
        [Describe("Sim")]
        Sim = 1,

        [Terms("Não", "nao", "no", "nop")] //sinônimos
        [Describe("Não")]
        Nao
    }
}