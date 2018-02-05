using MaratonaBots.Formulario;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;
using Microsoft.Bot.Connector;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace MaratonaBots
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {
            //trabalho com idiomas / locale
            var currentUICulture = Thread.CurrentThread.CurrentUICulture;
            var currentCulture = Thread.CurrentThread.CurrentCulture;


            if (activity.Type == ActivityTypes.Message)
            {
                //await Conversation.SendAsync(activity, () => new Dialogs.CotacaoDialog());
                await SendConversation(activity);
            }
            else if (activity.Type == ActivityTypes.ConversationUpdate)
            {
                //toda vez que tiver uma atualização de conversar , podemos verificar se um membro foi adicionado e se esse membro não é o próprio bot
                if (activity.MembersAdded != null && activity.MembersAdded.Any())
                {
                    foreach (var member in activity.MembersAdded)
                    {
                        //o cara que ta entrando é de fato um cara nova e não um membro que já estava lá
                        if (member.Id != activity.Recipient.Id)
                        {
                            await SendConversation(activity);
                        }
                    }
                }
            }
            else
            {
                HandleSystemMessage(activity);
            }
            var response = Request.CreateResponse(HttpStatusCode.OK);
            return response;
        }

        private async Task SendConversation(Activity activity)
        {
            /*
             Quando um membro novo entrar, starta o formulario
             Toda mensagem que receber, gerencia também.
             O proprio form flow guarda o status da conversação, não precisa se preocupar com a memória do que já foi preenchido ou não
             
             */
            await Conversation.SendAsync(activity, () => Chain.From(() => FormDialog.FromForm(() => Pedido.BuildForm(), FormOptions.PromptFieldsWithValues)));
        }

        /// <summary>
        /// Tratamos cada tipo de mensagem aqui
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private Activity HandleSystemMessage(Activity message)
        {
            if (message.Type == ActivityTypes.DeleteUserData)
            {
                // Implement user deletion here
                // If we handle user deletion, return a real message
            }
            else if (message.Type == ActivityTypes.ConversationUpdate)
            {
                // Handle conversation state changes, like members being added and removed
                // Use Activity.MembersAdded and Activity.MembersRemoved and Activity.Action for info
                // Not available in all channels
            }
            else if (message.Type == ActivityTypes.ContactRelationUpdate)
            {
                // Handle add/remove from contact lists
                // Activity.From + Activity.Action represent what happened
            }
            else if (message.Type == ActivityTypes.Typing)
            {
                // Handle knowing tha the user is typing
            }
            else if (message.Type == ActivityTypes.Ping)
            {
            }

            return null;
        }
    }
}