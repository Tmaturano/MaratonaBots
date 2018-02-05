using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace MaratonaBots.Dialogs
{

    [Serializable]
    [LuisModel(modelID: "3f0d4cb9-7227-4c7c-920f-c7903171313e", subscriptionKey: "c19890305e3d45b4a2ca82b9a3127ec4")]
    public class CotacaoDialog : LuisDialog<object>
    {
        //cada método do dialogo, podemos decorar para receber a intenção que o LUIS reconheceu
        //as intenções "None" cairão dentro deste método
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync($"Desculpe, não consegui entender a frase {result.Query}");
        }

        [LuisIntent("Sobre")]
        public async Task Sobre(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Eu sou um BOT e estou sempre aprendendo. Tenha paciência comigo.");
        }

        [LuisIntent("Cumprimento")]
        public async Task Cumprimento(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("Olá, eu sou um BOT que faz cotação de moedas.");
        }

        [LuisIntent("Cotacao")]
        public async Task Cotacao(IDialogContext context, LuisResult result)
        {
            //Pegando todas as entidades que foram passadas na frase e o LUIS reconheceu
            var moedas = result.Entities?.Select(e => e.Entity);            
            var endpoint = $"http://api-cotacoes-maratona-bots.azurewebsites.net/api/Cotacoes/{string.Join(",", moedas.ToArray())}";

            await context.PostAsync("Aguarde um momento enquanto eu obtenho os valores...");

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(endpoint);
                if (!response.IsSuccessStatusCode)
                {
                    await context.PostAsync("Poxa, eu não consegui obter essa informação agora.");
                    return;
                }

                var json = await response.Content.ReadAsStringAsync();
                var resultado = JsonConvert.DeserializeObject<Models.Cotacao[]>(json);

                var cotacoes = resultado.Select(c => $"{c.Nome}: {c.Valor}");
                await context.PostAsync($"Cotações: {string.Join(",", cotacoes.ToArray())}");
            }            
        }
    }
}