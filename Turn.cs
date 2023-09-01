using Microsoft.AspNetCore.Mvc.ModelBinding;
using Monopoli.Migrations;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace Monopoli
{
    public class Info
    {
        public int Id { get; set; }
        public int Turn { get; set; }
        public int D1 { get; set; }
        public int D2 { get; set; }
        public bool[] State { get; set; }
    }
    public class Turn
    {
        public Guid Player { get; set; }
        public int PlayerTurn { get; set; }
    }
    public class Acts
    {
        public string Value { get; set; }
        public Guid Player { get; set; }
        public int Price { get; set; }
        public int Pos { get; set; }
        public bool[] State { get; set; }

    }
    public class Rooms
    {
        public int RoomNumber { get; set; }
        public int PlayerIndex { get; set; }
        public int PlayerCount { get; set; }
        public List<Acts> Act { get; set; }
        public List<Info> Info { get; set; }
        public List<Turn> Turn { get; set; }
    }
    public static class Turns
    {
        public static string Log { get; set; }
        public static List<Rooms> RoomList { get; set; }
        public static string Next(Guid id, int roomNumber)
        {
            var r = RoomList.FirstOrDefault(m => m.RoomNumber == roomNumber);
            int numId = r.Info.Count + 1;
            int numIndex = numId - 1;
            if (r != null)
            {
                Random random = new();
                var info = new Info
                {
                    Id = numId,
                    Turn = r.PlayerIndex,
                    State = new bool[r.PlayerCount],
                    D1 = random.Next(1, 6),
                    D2 = random.Next(1, 6),
                };
                r.Info.Add(info);

                if (r.Info[numIndex].D1 != r.Info[numIndex].D2)
                {
                    r.PlayerIndex += 1;
                    if (r.PlayerCount <= r.PlayerIndex)
                        r.PlayerIndex = 0;
                }
            }
            return "";
        }
        public static string Next2(Guid id, int roomNumber)
        {
            var r = RoomList.FirstOrDefault(m => m.RoomNumber == roomNumber);
            int numId = r.Info.Count + 1;
            int numIndex = numId - 1;
            bool[] isnull = { false, false };
            var t = r.Turn.FirstOrDefault(m => m.Player == id);
            var i = r.Info.FirstOrDefault(m => m.State[t.PlayerTurn] == false);

            object di = new { };
            if (i != null)
            {
                isnull[0] = true;
                i.State[t.PlayerTurn] = true;
                di = new
                {
                    t = i.Turn,
                    d1 = i.D1,
                    d2 = i.D2,
                };
            }

            var a = r.Act.FirstOrDefault(m => m.State[t.PlayerTurn] == false);
            object na = new { };
            if (a != null)
            {
                a.State[t.PlayerTurn] = true;
                isnull[1] = true;
                na = new
                {
                    v = a.Value,
                    t = a.Player,
                    p = a.Pos,
                    c = a.Price,
                };
            }


            object p = new
            {
                nt = r.PlayerIndex,
                p = r.PlayerCount,
                d = isnull[0],
                di,
                s = isnull[1],
                na,
            };
            return System.Text.Json.JsonSerializer.Serialize(p);
        }
        public static string Act(Guid id, int roomNumber, string value, int price, int pos)
        {
            var r = RoomList.FirstOrDefault(m => m.RoomNumber == roomNumber);
            if (r != null)
            {
                Log = value;
            }
            r?.Act.Add(new()
            {
                Value = value,
                Player = id,
                Pos = pos,
                Price = price,
                State = new bool[r.PlayerCount]
            });
            return "";
        }
    }
    public static class AppendTurn
    {
        public static void Append(Guid id, int roomNumber)
        {
            Turns.RoomList.Add(new Rooms
            {
                PlayerIndex = 0,
                RoomNumber = roomNumber,
                Act = new(),
                Info = new(),
                Turn = new(),
            });
            AddPlayer(id, roomNumber);
        }
        public static void AddPlayer(Guid id, int roomNumber)
        {
            var r = Turns.RoomList.FirstOrDefault(x => x.RoomNumber == roomNumber);
            r.Turn.Add(new()
            {
                Player = id,
                PlayerTurn = r.Turn.Count
            });
        }

        public static void SetCount(int roomNumber, int count)
        {
            var r = Turns.RoomList.FirstOrDefault(x => x.RoomNumber == roomNumber);
            r.PlayerCount = count;
        }
    }
}
