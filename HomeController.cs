using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;

namespace Monopoli
{
    public class HomeController : Controller
    {
        private readonly MonoDB mono;
        public HomeController(MonoDB _)
        {
            mono = _;
        }
        /// <returns>Home Page</returns>
        public IActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Create a room and First user for the Room
        /// </summary>
        /// <param name="room">room name</param>
        /// <param name="password">password for room (Optional)</param>
        /// <param name="name">Name of (First & Admin) Player</param>
        /// <returns>Waiting Page and Room Id in id & Player Id in p</returns>
        [HttpPost]
        public IActionResult Index(string room, string password, string name)
        {
            if (room == null || name == null)
            {
                ViewBag.Error = false;
                return View();
            }
            if (room.Length > 20 || name.Length > 20)
            {
                ViewBag.Error = true;
                return View();
            }

            Room r = new()
            {
                Id = Guid.NewGuid(),
                Password = password,
                RoomName = room,
                Count = 1,
                Numder = (int)(DateTime.Now.Ticks / 1000000),
            };
            mono.Rooms.Add(r);
            Player p = new()
            {
                Id = Guid.NewGuid(),
                Name = name,
                Room = r.Id,
                IsAdmin = true,
            };
            mono.Players.Add(p);
            mono.SaveChanges();
            AppendTurn.Append(p.Id, r.Numder);
            return RedirectToAction("Waiting", new { id = p.Id });
        }
        public IActionResult Signin(string name, string Password)
        {
            Player p = new()
            {
                Name = name,
                Password = Password,
                Id = Guid.NewGuid(),
            };
            mono.Players.Add(p);
            mono.SaveChanges();
            return RedirectToAction("Index");
        }
        /// <param name="id">Room Id in id</param>
        /// <param name="p">Player Id in p</param>
        /// <returns>Waiting Page and <see cref="Player.Player()"/> Model</returns>
        [Route("{controller}/{action}/{id}")]
        public IActionResult Waiting(Guid id)
        {
            var p = mono.Players.FirstOrDefault(m => m.Id == id);
            //Response.Cookies.Append(p.Id + "", p.IsAdmin + "", new() { Expires = DateTime.Now.AddYears(10)});
            return View(model: p);
        }

        /// <summary>
        /// Get List of Players and Room Setting. Then Create Json text from Players List and Start State.
        /// </summary>
        /// <param name="id">Room Number (Id)</param>
        /// <returns>Json with Ajax</returns>
        [HttpPost]
        public IActionResult Player(Guid id)
        {
            var p = mono.Players.Where(m => m.Room == id).ToList();
            if (p.Count == 0)
                return Content("");
            Room r = mono.Rooms.FirstOrDefault(m => m.Id == p[0].Room);
            if (r == null)
                return Content("");
            var pCount = p.Count;
            string json = "{\"p\":[";
            for (int i = 0; i < pCount; i++)
            {
                json += "{\"Name\":\"" + p[i].Name + "\"}" + (i < pCount - 1 ? "," : "");
            }
            json += "],\"IsStart\":" + (r.IsStart + "").ToLower() + "}";
            return Content(json);
        }

        public IActionResult Login(Guid id, string name)
        {
            // id = Room Id
            if (name == null)
            {
                ViewBag.Error = false;
                return View("Index");
            }
            if (name.Length > 20)
            {
                ViewBag.Error = true;
                return View("Index");
            }
            var r = mono.Rooms.FirstOrDefault(m => m.Id == id);
            if (r == null)
                return View("Index");
            else
                r.Count += 1;
            Player p = new()
            {
                Id = Guid.NewGuid(),
                Name = name,
                IsAdmin = false,
                Room = id,
            };
            mono.Players.Add(p);
            mono.SaveChanges();
            AppendTurn.AddPlayer(p.Id, r.Numder);
            return RedirectToAction("Waiting", new { id = p.Id });
        }
        [HttpPost]
        public IActionResult Rooms()
        {
            return Content(System.Text.Json.JsonSerializer.Serialize(mono.Rooms.ToList()));
        }
        [HttpPost]
        public IActionResult Room(Guid id)
        {
            var player = mono.Players.Where(m => m.Room == id).ToList();
            return Content(System.Text.Json.JsonSerializer.Serialize(player));
        }
        public IActionResult Clear()
        {
            var all = mono.Rooms.ToList();
            var all2 = mono.Players.ToList();
            foreach (var item in all)
                mono.Rooms.Remove(item);
            foreach (var item in all2)
                mono.Players.Remove(item);
            mono.SaveChanges();
            return RedirectToAction("Index");
        }
        public IActionResult Game(Guid id)
        {
            // if not admin
            if (id != Guid.Empty)
                return View(model: new Player() { Id = id });
            return RedirectToAction("Index");
        }
        [HttpPost]
        public IActionResult Game(Guid id, int coin, int prebuy)
        {
            // if admin
            if (id != Guid.Empty)
            {
                Player player = mono.Players.FirstOrDefault(m => m.Id == id);
                Room room = mono.Rooms.FirstOrDefault(m => m.Id == player.Room);
                if (string.IsNullOrEmpty(room.Home))
                {
                    room.Coin = coin;
                    room.IsStart = true;
                    room.PreBuy = prebuy;
                    room.Home = HomeCode(prebuy, mono.Players.Where(m => m.Room == id).Count());
                    mono.SaveChanges();
                    return RedirectToAction("Game", new { id });
                }
            }
            return RedirectToAction("Index");
        }



        //** Started **//
        [HttpPost]
        public IActionResult Turn(Guid id, int rn) => Content(Turns.Next(id, rn));
        [HttpPost]
        public IActionResult GetTurn(Guid id, int rn) => Content(Turns.Next2(id, rn));
        /// <param name="v">value</param>
        /// <param name="c">price</param>
        /// <param name="p">position</param>
        [HttpPost]
        public IActionResult Act(Guid id, int rn, string v, int c, int p)
        {
            return Content(Turns.Act(id, rn, v, c, p));
        }
        [HttpPost]
        public IActionResult GetHome(Guid id)
        {
            var r = mono.Rooms.FirstOrDefault(m => m.Id == id);
            return Content(r.Home);
        }


        private string HomeCode(int prebuy, int pCount)
        {
            // create home code
            var code = "";

            return code;
        }
    }
}
