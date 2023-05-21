using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Telegram.Bot;
using PLANTILLA.ICARO.Controllers;
using PLANTILLA.ICARO.Models;
using PLANTILLA.ICARO.Repositories;

namespace PLANTILLA.ICARO.Clases
{
    public class TelegramPoster
    {
        public string token;
        public string tokenOEP;
        //public string [] chatIDVYP;

        readonly ILogger<TelegramPoster> _logger;


        RepositoryTokens repo_codes;

        public TelegramPoster(RepositoryTokens repo_codes, ILogger<TelegramPoster> logger)
        {
            this._logger = logger;
            this.repo_codes = repo_codes;
            this.token = repo_codes.GetCode("TelegramTokenBot");
            this.tokenOEP = repo_codes.GetCode("TelegramTokenOEPBot");
        }
        public async Task<bool> SendMessage(OfertaModelPub oferta)
        {
            try
            {
                List<string> chats = await GetChatIds();

                _logger.LogInformation("OFERTA: " + oferta.url);

                foreach (var chat in chats)
                {
                    _logger.LogInformation("Enviando a " + chat);
                    //Si el chat es el de VYP
                    if(chat== "-1001597984102")
                    {
                        await SendToChat(oferta, token, chat);
                    }
                    else
                    {
                        await SendToChat(oferta, tokenOEP, chat);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task SendToChat(OfertaModelPub oferta, string token, string id)
        {

            try
            {
                if (oferta.url.Contains(" PM"))
                {
                    oferta.url = oferta.url.Replace(" PM", "+PM");
                }
                if (oferta.url.Contains(" "))
                {
                    oferta.url = oferta.url.Replace(" ", "%20");
                }

                Uri m = new Uri(oferta.url);
                TelegramBotClient cli = new TelegramBotClient(token);

                await cli.SendTextMessageAsync(id, oferta.titulo + " \n" + m.AbsoluteUri + "-");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }

        public bool SendMessageNoticia(NoticiaModelPub noticia)
        {

            SendToChatNoticia(noticia, token, "-1001597984102"); //ID GRUPO VYP
            return true;
        }
        public void SendToChatNoticia(NoticiaModelPub noticia, string token, string id)
        {

            try
            {
                TelegramBotClient cli = new TelegramBotClient(token);
                cli.SendTextMessageAsync(id, noticia.url);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }
        }
        public async Task<List<string>> GetChatIds()
        {
            //TelegramBotClient cli = new TelegramBotClient(token);
            List<string> chatids = new List<string>();
            //Telegram.Bot.Types.Update[] updates = await cli.GetUpdatesAsync();
            //foreach (var update in updates)
            //{
            //    try
            //    {
            //        if (!chatids.Contains(update.Message.Chat.Id.ToString()))
            //        {
            //            chatids.Add(update.Message.Chat.Id.ToString());
            //            _logger.LogInformation("CHAT ID -" + update.Message.Chat.Id.ToString());
            //        }
            //        //-1001597984102


            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
            chatids.Add("-1001597984102"); //VYP
            chatids.Add("-1001512656427"); //OFERTAS SEGURIDAD PRIVADA


            return chatids;
        }
    }
}
