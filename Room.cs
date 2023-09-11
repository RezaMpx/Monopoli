using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Monopoli
{
    public class Room
    {
        public Guid Id { get; set; }
        public int Numder { get; set; }
        public string RoomName { get; set; }
        public string Password { get; set; }
        public int Count { get; set; }
        public int Coin { get; set; }
        public int PreBuy { get; set; }
        public bool IsStart { get; set; }
        public string Home { get; set; }
    }
}
