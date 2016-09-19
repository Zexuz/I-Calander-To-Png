using Discord;


namespace ICalendarToPng {

    public class DiscordBot {

        public DiscordBot() {
            var client = new DiscordClient();

            client.MessageReceived += async (s, e) => {
                if (e.Message.IsAuthor || !e.Message.Text.Equals("!image")) return;

                await e.Channel.SendMessage("Here is the schema!");
                await e.Channel.SendMessage("http://i.imgur.com/2ibF7NU.jpg");
                await e.Channel.SendFile("http://imgur.com/gallery/BE5GN");
            };


            client.ExecuteAndWait(async () => {
                //Connect to the Discord server using our email and password
                await client.Connect("MjI3MTU2NTk4MDQzMTgxMDU3.CsCfSA.CA_kvqX9oJMKw_KbfV7dY0dUKgk", TokenType.Bot);

                //If we are not a member of any server, use our invite code (made beforehand in the official Discord Client)
            });
        }

    }

}
