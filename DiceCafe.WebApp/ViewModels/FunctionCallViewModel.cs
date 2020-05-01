using System.Collections.Generic;

namespace DiceCafe.WebApp.ViewModels
{
    public class FunctionCallViewModel
    {
        public string Name { get; set; }
        public Dictionary<string, int> Arguments { get; set; }
    }
}