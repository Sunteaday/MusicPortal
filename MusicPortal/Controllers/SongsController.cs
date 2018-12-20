using MusicPortal.Infrastructure;
using MusicPortal.Models;
using MusicPortal.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace MusicPortal.Controllers
{
    public class SongsController : Controller
    {
        protected MusicPortalDbContext db = new MusicPortalDbContext();

        public async Task<ActionResult> All()
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(await db.Songs.ToListAsync());
        }


        // GET: Songs
        public async Task<ActionResult> Index()
        {
            IEnumerable<Song> songs = await db.Songs.ToListAsync();

            return View(songs.Select(song => song.ToViewModel()));
        }


        // GET: Songs/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Song song = await db.Songs.FindAsync(id);
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song.ToViewModel());
        }

        // GET: Songs/Create
        public async Task<ActionResult> Create()
        {
            if (!Session.UserHasRole("Authorized"))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            return View(new SongVM()
            {
                Genres = (await db.Genres.ToListAsync()).Select(g => g.ToViewModel())
            });
        }

        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CreateConfirm([Bind(Include = "Name,File,Genres")]SongVM songVM)
        {
            if (!Session.UserHasRole("Authorized"))
                return new HttpStatusCodeResult(HttpStatusCode.Forbidden);

            if (await db.Songs
                        .Where(s => s.Name == songVM.Name)
                        .FirstOrDefaultAsync() != null)
            {
                ModelState.AddModelError(String.Empty, "This song is exist");

                return View(songVM);
            }

            string filename;

            string mappedSongsFolderPath = Server.MapPath(AppConstants.SONGS_FOLDER_VIRTUAL_PATH);
            string savePath;

            // check if random filename exist
            do
            {
                filename = $"{Guid.NewGuid()}.mp3";
                savePath = Path.Combine(mappedSongsFolderPath, filename);
            } while (System.IO.File.Exists(savePath));

            using (FileStream f = System.IO.File.Create(savePath, 4096, FileOptions.Asynchronous))
            {
                await songVM.File.InputStream.CopyToAsync(f);
            }

            Song newSong = new Song()
            {
                Name = songVM.Name,
                FileName = filename,
                Genres = await Task.Run(() =>
                {
                    // Transform selected genres to Domain model
                    return songVM
                                 .Genres
                                 .Where(genre => genre.IsSelected)
                                 .Select(async selectedGenre => await db.Genres.FindAsync(selectedGenre.Id))
                                 .Select(t => t.Result)
                                 .ToList();
                })
            };
            db.Songs.Add(newSong);

            await db.SaveChangesAsync();

            return Redirect("~/");
        }


        // GET: Songs/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            Song song = await db.Songs.FindAsync(id);
            if (song == null)
            {
                return HttpNotFound();
            }

            var editVM = new EditSongVM
            {
                Id = song.Id,
                Name = song.Name,
                Genres = (await db.Genres.ToListAsync()).Select(genre =>
                    new SelectedGenre()
                    {
                        Id = genre.Id,
                        IsSelected = song.Genres.FirstOrDefault(g => g.Id == genre.Id) != null,
                        Name = genre.Name
                    })
            };
            return View(editVM);
        }

        // POST: Songs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Name,File,Genres")] EditSongVM songVM)
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            if (ModelState.IsValid)
            {
                Song song = await db.Songs.FindAsync(songVM.Id);
                song.Name = songVM.Name;

                song.Genres.Clear();

                await Task.Run(() =>
                {
                    songVM
                        .Genres
                        .Where(genre => genre.IsSelected)
                        .Select(async genre => await db.Genres.FindAsync(genre.Id))
                        .Select(t => t.Result)
                        .ToList()
                        .ForEach(genre => song.Genres.Add(genre));
                });

                if (songVM.File != null)
                {
                    string songFilePath = Server.MapPath(song.GetVirtualFilePath());

                    using (FileStream songFileStream = new FileStream(songFilePath, FileMode.Truncate, FileAccess.Write, FileShare.None, 4096, true))
                    {
                        await songVM.File.InputStream.CopyToAsync(songFileStream);
                    }
                }
                await db.SaveChangesAsync();

                return RedirectToAction("All");
            }

            ModelState.AddModelError(String.Empty, "Bad request");
            return View(songVM);
        }

        // GET: Songs/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Song song = await db.Songs.FindAsync(id);
            if (song == null)
            {
                return HttpNotFound();
            }
            return View(song);
        }

        [ActionName("Song")]
        public async Task<ActionResult> GetSong(string name)
        {
            if (name == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Song song = await db.Songs.FirstOrDefaultAsync(s => s.Name == name);
            if (song == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            return View(song.ToViewModel());
        }

        [ActionName("GetSongFile")]
        public async Task<ActionResult> GetSongFileById(string name)
        {
            if (name == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Song song = await db.Songs.Where(s => s.Name == name).FirstOrDefaultAsync();

            if (song == null)
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            string filePath = Server.MapPath(song.GetVirtualFilePath());

            return File(System.IO.File.Open(filePath, FileMode.Open), MimeMapping.GetMimeMapping(filePath)/*, Path.GetFileName(filePath)*/);
        }

        // POST: Songs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            if (!Session.UserHasRole("Admin"))
                return new HttpStatusCodeResult(HttpStatusCode.NotFound);

            Song song = await db.Songs.FindAsync(id);
            db.Songs.Remove(song);

            string filePath = Server.MapPath(song.GetVirtualFilePath());
            System.IO.File.Delete(filePath);

            await db.SaveChangesAsync();

            return RedirectToAction("All");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
