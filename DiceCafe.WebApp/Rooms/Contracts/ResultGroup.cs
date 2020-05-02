using System;
using System.Collections.Generic;
using DiceCafe.WebApp.Users.Contract;

namespace DiceCafe.WebApp.Rooms.Contracts
{
    public class ResultGroup
    {
        public User User { get; set; }
        public DateTime Created { get; set; }
        public List<TaggedResult> Results { get; set; }
    }
}