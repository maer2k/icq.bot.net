﻿using ICQ.Bot.Requests.Abstractions;
using ICQ.Bot.Types;
using ICQ.Bot.Types.Enums;
using ICQ.Bot.Types.ReplyMarkups;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Net.Http;
using System.Text;

namespace ICQ.Bot.Requests
{
    [JsonObject(MemberSerialization.OptIn, NamingStrategyType = typeof(CamelCaseNamingStrategy))]
    public class SendMessageRequest : RequestBase<MessagesResponse>,
                                      INotifiableMessage,
                                      IReplyMessage,
                                      IReplyMarkupMessage<IReplyMarkup>
    {
        [JsonProperty(Required = Required.Always)]
        public ChatId ChatId { get; }

        [JsonProperty(Required = Required.Always)]
        public string Text { get; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public ParseMode ParseMode { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool DisableWebPagePreview { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public bool DisableNotification { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public int ReplyToMessageId { get; set; }

        [JsonProperty(DefaultValueHandling = DefaultValueHandling.Ignore)]
        public IReplyMarkup ReplyMarkup { get; set; }

        public SendMessageRequest(ChatId chatId, string text)
            : base("/messages/sendText", HttpMethod.Get)
        {
            ChatId = chatId;
            Text = text;
        }

        public override HttpContent ToHttpContent()
        {
            string queryString = string.Format("chatId={0}&text={1}", ChatId, Text);
            if (ReplyMarkup != null)
            {
                string markup = JsonConvert.SerializeObject(ReplyMarkup);
                queryString = string.Format("{0}&inlineKeyboardMarkup={1}", queryString, markup);
            }

            return new StringContent(queryString, Encoding.UTF8);
        }
    }
}
