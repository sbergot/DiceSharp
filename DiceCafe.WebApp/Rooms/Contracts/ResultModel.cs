using System;
using DiceCafe.WebApp.Users.Contract;
using DiceScript.Contracts;

namespace DiceCafe.WebApp.Rooms.Contracts
{
    public class ResultModel
    {
        public User User { get; set; }
        public DateTime Created { get; set; }
        public ResultType ResultType { get; set; }
        public Result Result { get; set; }
    }
}