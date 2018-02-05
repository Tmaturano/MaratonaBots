using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace MaratonaBots.Dialogs
{
    [Serializable]
    public class RootDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }


        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            /*
             As interfaces que compõem IDialogContext:
             * IDialogStack:  Empilhar diálogos, o fluxo de conversação
             * IBotContext: o que traz o conteúdo em si, o contexto que está trabalhando. O BOT não guarda informações , é rest.
             * IBotData: É o gerenciamento de memória, onde conseguimos armazenar informações do usuário (através do UserData), conversa, de tal maneira que lembre do usuário quando ele digitar novamente.
                         O ConversationData são informações daquela conversa. Esse conteúdo fica armazenado na memória de conversa. Saiu da conversa, acabou, pois fica somente em memória.
                         Em UserData, é a identificação do usuário em si (Nome, telefone, endereço por exemplo). Ele fica salvo e mesmo que o usuário saia da conversa, quando ele acessar novamente o BOT vai ter as informações desse usuário.
             
            A Activity tem uma lista de Attachments (fotos, audios, etc)
             
             */

            //Markdown: em negrito
            await context.PostAsync("**Olá, tudo bem?**");

            //cria uma estrutura para que inclua nessa estrutura algum tipo de conteúdo
            var message = activity.CreateReply();

            if (activity.Text.Equals("herocard", StringComparison.InvariantCultureIgnoreCase))
            {
                var heroCard = new HeroCard
                {
                    Title = "Planeta",
                    Subtitle = "Universo",
                    Images = new List<CardImage>
                {
                    new CardImage("https://abrilexame.files.wordpress.com/2017/11/planetaterra.jpg?quality=70&strip=info", "Planeta",
                        new CardAction(ActionTypes.OpenUrl, title: "Microsoft", value: "http://www.microsoft.com"))
                },
                    Buttons = new List<CardAction>
                    {
                        new CardAction
                        {
                            Text = "Botão 1",
                            DisplayText = "Display",
                            Title = "Title",
                            Type = ActionTypes.PostBack,
                            Value = "aqui vai um valor"
                        }
                    }

                };                

                message.Attachments.Add(heroCard.ToAttachment());
            }
            else if (activity.Text.Equals("videocard", StringComparison.InvariantCultureIgnoreCase))
            {
                var videoCard = new VideoCard
                {
                    Title = "Um vídeo qualquer",
                    Subtitle = "Aqui vai um subtítulo",
                    Autostart = true, //comece automaticamente quando for renderizado
                    Autoloop = false,
                    Media = new List<MediaUrl>
                    {
                        new MediaUrl("https://www.youtube.com/watch?v=FG0fTKAqZ5g")
                    }
                };

                message.Attachments.Add(videoCard.ToAttachment());
            }
            else if (activity.Text.Equals("audiocard", StringComparison.InvariantCultureIgnoreCase))
            {
                var attachment = CreateAudioCard();
                message.Attachments.Add(attachment);
            }
            else if (activity.Text.Equals("animationcard", StringComparison.InvariantCultureIgnoreCase))
            {
                var attachment = CreateAnimationCard();
                message.Attachments.Add(attachment);
            }
            else if (activity.Text.Equals("carousel", StringComparison.InvariantCultureIgnoreCase))
            {
                message.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                var audio = CreateAudioCard();
                var animation = CreateAnimationCard();

                message.Attachments.Add(audio);
                message.Attachments.Add(animation);
            }

            await context.PostAsync(message);
            context.Wait(MessageReceivedAsync);
        }

        private Attachment CreateAnimationCard()
        {
            var animationCard = new AnimationCard
            {
                Title = "Um gif fofo",
                Subtitle = "Aqui vai um subtítulo",
                Autostart = true, //comece automaticamente quando for renderizado
                Autoloop = false,
                Media = new List<MediaUrl>
                    {
                        new MediaUrl("https://img.buzzfeed.com/buzzfeed-static/static/enhanced/webdr06/2013/5/31/10/anigif_enhanced-buzz-3734-1370010471-16.gif")
                    },

            };

            return animationCard.ToAttachment();
        }

        private Attachment CreateAudioCard()
        {
            var audiocard = new AudioCard
            {
                Title = "Um áudio revelador",
                Subtitle = "Aqui vai um subtítulo",
                Autostart = true, //comece automaticamente quando for renderizado
                Autoloop = false,
                Media = new List<MediaUrl>
                    {
                        new MediaUrl("http://www.wavlist.com/movies/004/father.wav")
                    },
                Image = new ThumbnailUrl("https://abrilexame.files.wordpress.com/2017/11/planetaterra.jpg?quality=70&strip=info")
            };

            return audiocard.ToAttachment();
        }
    }
}