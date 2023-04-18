using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using OpenAI.GPT3.Managers;
using OpenAI.GPT3.ObjectModels.RequestModels;
using OpenAI.GPT3.ObjectModels;
using OpenAI.GPT3;

namespace TelegramBotExample
{
    public class MyTelegramBot
    {
        private static readonly string BotToken = "6103461413:AAG2V_8raDVvrxzmkFRs2VD-Qg1xjhK3Ob4";
        private static readonly string OpenAiApiKey = "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";
        private static readonly TelegramBotClient Bot = new TelegramBotClient(BotToken);
        private static readonly OpenAIService openAiService = new OpenAIService(new OpenAiOptions { ApiKey = OpenAiApiKey });

        public static async Task RunBot()
        {
            Bot.OnMessage += Bot_OnMessage;
            Bot.StartReceiving();
            Console.WriteLine("Bot is running...");
            Console.ReadLine();
            Bot.StopReceiving();
        }

        private static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (e.Message.Type == MessageType.Text)
            {
                Console.WriteLine($"Received a text message from {e.Message.Chat.FirstName}: {e.Message.Text}");
                string response = await GetGpt3Response(e.Message.Text);
                await Bot.SendTextMessageAsync(e.Message.Chat.Id, response);
            }
        }

        private static async Task<string> GetGpt3Response(string input)
        {
            var completionResult = await openAiService.ChatCompletion.CreateCompletion(new ChatCompletionCreateRequest
            {
                Messages = new List<ChatMessage>
                {
                    ChatMessage.FromSystem("You are a helpful assistant."),
                    ChatMessage.FromUser(input)
                },
                Model = Models.ChatGpt3_5Turbo,
                MaxTokens = 1000 //optional
            });

            if (completionResult.Successful)
            {
                return completionResult.Choices.First().Message.Content;
            }
            else
            {
                return "Error: Could not generate a response from GPT-3.";
            }
        }
    }
}
