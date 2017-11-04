using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using bright_ideas.Models;

namespace bright_ideas.Controllers
{
    public class HomeController : Controller
    {
        private Context db;
        public HomeController(Context context)
        {
            db = context;
        }

        // GET: /Home/
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            if(HttpContext.Session.GetInt32("UserID") != null)
                return RedirectToAction("main");
            ViewBag.LogPwErr = TempData["LogPwErr"];
            ViewBag.LogUserErr = TempData["LogUserErr"];
            ViewBag.Title = "Login and Registration";
            return View();
        }

        [HttpPost("register")]
        public IActionResult Register(UserViewModel model)
        {
            User user = db.users.SingleOrDefault( u => u.email == model.email );
            if(ModelState.IsValid && user == null)
            {
                User NewUser = new User {
                    name = model.name,
                    alias = model.alias,
                    email = model.email,
                };
                PasswordHasher<User> Hasher = new PasswordHasher<User>();
                NewUser.password = Hasher.HashPassword(NewUser, model.password);
                db.Add(NewUser);
                db.SaveChanges();
                HttpContext.Session.SetInt32("UserID", db.users.SingleOrDefault( u => u.email == model.email ).userId);
                return RedirectToAction("Main");
            }
            else if(user != null)
                ViewBag.RegUserErr = "User already exists!";
            return View("index");
        }

        [HttpPost("login")]
        public IActionResult Login(string email, string pw)
        {   
            User user = db.users.SingleOrDefault( u => u.email == email );
            if(user != null) 
            {
                var Hasher = new PasswordHasher<User>();
                if(pw == null) pw = "";
                if(Hasher.VerifyHashedPassword(user, user.password, pw) != 0)
                {
                    HttpContext.Session.SetInt32("UserID", user.userId);
                    return RedirectToAction("Main");
                }
                else TempData["LogPwErr"] = "Incorrect password!";
            }
            else TempData["LogUserErr"] = "Username does not exist!";
            return RedirectToAction("index");
        }

        [HttpGet("main")]
        public IActionResult Main()
        {
            if(HttpContext.Session.GetInt32("UserID") == null)
                return RedirectToAction("Index");
            int id = (int)HttpContext.Session.GetInt32("UserID");
            ViewBag.User = db.users.Find(id);
            ViewBag.Ideas = db.posts
                .Include(post => post.User)
                .OrderByDescending(post => post.likecount)
                .ToList();
            ViewBag.Title = "Main Page";
            return View();
        }

        [HttpPost("main")]
        public IActionResult AddPost(PostViewModel model)
        {
            int UserId = (int)HttpContext.Session.GetInt32("UserID");
            User user = db.users.Find(UserId);
            if(ModelState.IsValid)
            {
                Post NewPost = new Post {
                    userId = UserId,
                    User = user,
                    content = model.content,
                };
                db.Add(NewPost);
                db.SaveChanges();
                return RedirectToAction("main");
            }
            return View();
        }

        [HttpGet("user/{id}")]
        public IActionResult showUser(int id)
        {
            ViewBag.id = (int)HttpContext.Session.GetInt32("UserID");
            ViewBag.User = db.users.Find(id);
            var postList = db.posts.Where(post => post.userId == id).ToList();
            ViewBag.PostCount = postList.Count();
            var likeList = db.likes.Where(like => like.userId == id).ToList();
            ViewBag.LikeCount = likeList.Count();
            ViewBag.Title = "User Page";
            return View("user");
        }

        [HttpGet("posts/{id}")]
        public IActionResult showIdea(int id)
        {
            ViewBag.post = db.posts.Find(id);
            ViewBag.author = db.users.Find(ViewBag.post.userId);
            ViewBag.Likes = db.likes
                .Where(like => like.postId == id)
                .Include(like => like.User)
                .ToList();
            ViewBag.Title = "Idea Page";
            return View("idea");
        }

        [HttpGet("posts/likes/{id}")]
        public IActionResult updateLikes(int id)
        {
            Post post = db.posts.Find(id);
            int UserId = (int)HttpContext.Session.GetInt32("UserID");
            User user = db.users.Find(UserId);
            var likeCheck = db.likes
                .Where(like => like.userId == UserId && like.postId == id)
                .ToList();
            if(likeCheck.Count() == 0)
            {
                Like NewLike = new Like {
                userId = UserId,
                User = user,
                postId = id,
                Post = post,
                };
                db.Add(NewLike);
                post.likecount ++;
                db.SaveChanges();
            }
            return RedirectToAction("main");
        }

        [HttpGet("posts/delete/{id}")]
        public IActionResult delete(int id)
        {
            Post post = db.posts.Find(id);
            var likesDelete = db.likes
                .Where(like => like.postId == id);

            foreach (var like in likesDelete)
            {
                db.likes.Remove(like);
            }
            db.posts.Remove(post);
            db.SaveChanges();
            return RedirectToAction("main");
        }

        [HttpGet("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("index");
        }
    }
}
