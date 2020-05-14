using System.Collections.Generic;

namespace DiceCafe.WebApp.ViewModels
{
    public class DiscordMessageViewModel
    {
        public string UserName { get; set; }
        public List<DiscordEmbedViewModel> Embeds { get; set; }
    }
}