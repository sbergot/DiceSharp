using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DiceCafe.WebApp.Hubs;
using DiceCafe.WebApp.Rooms.Contracts;
using DiceCafe.WebApp.Serialization;
using DiceCafe.WebApp.ViewModels;
using DiceScript.Contracts;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace DiceCafe.WebApp.Rooms
{
    public class ResultPublisher : IResultPublisher
    {
        public ResultPublisher(IHubContext<RoomHub> roomHub, HttpClient httpClient, ILogger<ResultPublisher> logger)
        {
            RoomHub = roomHub;
            HttpClient = httpClient;
            Logger = logger;
        }

        private IHubContext<RoomHub> RoomHub { get; }
        private HttpClient HttpClient { get; }
        private ILogger<ResultPublisher> Logger { get; }

        async public Task Publish(Room room, ResultGroup resultGroup)
        {
            string discordWebHook = room.State.DiscordWebHook;
            if (!string.IsNullOrEmpty(discordWebHook))
            {
                await PublishOnDiscord(discordWebHook, resultGroup);
            }
            room.State.Results.Add(resultGroup);
            await RoomHub.Update(room);
        }

        private async Task PublishOnDiscord(string discordWebHook, ResultGroup resultGroup)
        {
            var resultMarkDown = FormatResultGroup(resultGroup);
            var viewModel = new DiscordMessageViewModel
            {
                UserName = resultGroup.User.Name,
                Embeds = new List<DiscordEmbedViewModel>
                    {
                        new DiscordEmbedViewModel { Description = resultMarkDown }
                    }
            };
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = new LowerCaseNamingPolicy(),
            };
            var serialized = JsonSerializer.Serialize(viewModel, options);
            var response = await HttpClient.PostAsync(
                discordWebHook,
                new StringContent(serialized, Encoding.UTF8, "application/json"));
            Logger.LogInformation(response.StatusCode.ToString());
        }

        private string FormatResultGroup(ResultGroup resultGroup)
        {
            var results = resultGroup.Results
                .Select(r => FormatResult(r.Result))
                .Where(r => r != null);
            return string.Join("\n  ", results);
        }

        private string FormatResult(Result result)
        {
            if (result is RollResult rollResult)
            {
                return FormatRollResult(rollResult);
            }

            if (result is ValueResult valueResult)
            {
                return FormatValueResult(valueResult);
            }

            if (result is DiceResult diceResult)
            {
                return FormatDiceResult(diceResult);
            }

            if (result is PrintResult printResult)
            {
                return printResult.Value;
            }

            throw new InvalidOperationException();
        }

        private static string FormatDiceResult(DiceResult diceResult)
        {
            var dicesfmt = FormatDices(diceResult.Dices);
            var namefmt = FormatName(diceResult.Name);
            var descrfmt = FormatDescription(diceResult.Description);
            return $"{namefmt}{descrfmt} -> {dicesfmt}";
        }

        private static string FormatValueResult(ValueResult valueResult)
        {
            if (valueResult.Name == null) { return null; }
            string result = valueResult.Result.ToString();
            string namefmt = FormatName(valueResult.Name);
            return $"{namefmt}{result}";
        }

        private static string FormatRollResult(RollResult rollResult)
        {
            var dicesfmt = rollResult.Dices.Count > 1 ? $"{FormatDices(rollResult.Dices)} = " : "";
            var namefmt = FormatName(rollResult.Name);
            var descrfmt = FormatDescription(rollResult.Description);
            return $"{namefmt}{descrfmt} -> {dicesfmt}{rollResult.Result}";
        }

        private static string FormatDices(List<Dice> dices)
        {
            return string.Join(' ', dices.Select(d =>
            {
                var result = d.Result.ToString();
                return d.Valid ? result : $"~~{result}~~";
            }));
        }

        private static string FormatName(string name)
        {
            return string.IsNullOrEmpty(name) ? null : $"{name}: ";
        }

        private static string FormatDescription(RollDescription roll)
        {
            var numberfmt = roll.Number > 1 ? roll.Number.ToString() : "";
            var bonusfmt = "";
            if (roll.Bonus > 0)
            {
                bonusfmt = $"+{roll.Bonus}";
            }
            if (roll.Bonus < 0)
            {
                bonusfmt = roll.Bonus.ToString();
            }
            return $"{numberfmt}D{roll.Faces}{bonusfmt}";
        }
    }
}