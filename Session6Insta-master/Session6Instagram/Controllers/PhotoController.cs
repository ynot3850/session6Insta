using Session6Instagram.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Session6Instagram.Controllers
{
    public class PhotoController : Controller
    {
        public ActionResult UploadPhoto()
        {
            InstagramDbContext database = new InstagramDbContext();
            var users = database.Users.ToList();
            return View(users);
        }

        [HttpPost]
        public ActionResult SavePhoto(HttpPostedFileBase photo, int userId, string caption)
        {
            InstagramDbContext database = new InstagramDbContext();
            Photo temp_photo = new Photo();
            temp_photo.Picture = photo.FileName;
            temp_photo.PictureData = new byte[photo.ContentLength];
            photo.InputStream.Read(temp_photo.PictureData, 0, photo.ContentLength);
            temp_photo.Caption = caption;
            temp_photo.PhotoUser = database.Users.Find(userId);
            temp_photo.Date = DateTime.Now;
            database.Photos.Add(temp_photo);
            database.SaveChanges();

            return RedirectToAction("Feed");


        }
    
          



        public ActionResult Feed()
        {
            var database = new InstagramDbContext();
            ViewBag.currentUser = database.Users.Find(1);
            return View(database.Photos.ToList());
        }

        public ActionResult GetPicture()
        {
            var dir = Server.MapPath("/Images/");
            var path = Path.Combine(dir, "test.jpg"); //validate the path for security or use other means to generate the path.

            Bitmap image = new Bitmap(path);
            // image = new Bitmap(image, new Size(250, 250)); (got awful resize)

            for (int i = 0; i < image.Height; i++)
            {
                for (int j = 0; j < image.Width; j++)
                {
                   
                    Color temp = image.GetPixel(j, i);
                    temp = Color.FromArgb(temp.A, temp.R / 2, temp.G, temp.B);
                    image.SetPixel(j, i, temp);
                    
                }
            }
            

            var memstream = new MemoryStream();
            image.Save(memstream,ImageFormat.Jpeg);
            memstream.Position = 0;

            //FileStream stream = new FileStream(path, FileMode.Open);
            FileStreamResult result = new FileStreamResult(memstream, "image/jpg");



            return result;
        }

        public ActionResult GetImage(int id)
        {
            var database = new InstagramDbContext();
            var photo = database.Photos.Find(id);
            var stream = new MemoryStream(photo.PictureData);

            var bitmap = new Bitmap(stream);
            for (int i = 0; i < bitmap.Height; i++)
            {
                for (int j = 0; j < bitmap.Width; j++)
                {
                    var temp_pixel = bitmap.GetPixel(j, i);
                   // int average = (temp_pixel.R + temp_pixel.B + temp_pixel.G) / 3;
                   // bitmap.SetPixel(j, i, Color.FromArgb(temp_pixel.R / 2, temp_pixel.G, temp_pixel.B));
                }
            }
            var newstream = new MemoryStream();
            bitmap.Save(newstream, ImageFormat.Jpeg);
            newstream.Position = 0;

            return new FileStreamResult(newstream, "image/jpg");
        }

        public ActionResult PressLike(int userId, int photoId)
        {
            var database = new InstagramDbContext();
            var user = database.Users.Find(userId);
            var photo = database.Photos.Find(photoId);
            var like = database.Likes.Where(x => x.User.Id == user.Id && x.Photo.Id == photo.Id).FirstOrDefault();
            var Liked = database.Likes;
            if (like == null)
            { 
         
                like = new Like();
                like.Photo = photo;
                like.User = user;
                database.Likes.Add(like);
                
                
                database.SaveChanges();
            }
            else
            {
                database.Likes.Remove(like);
                database.SaveChanges();
            }

            return RedirectToAction("feed");
        }



    }
}