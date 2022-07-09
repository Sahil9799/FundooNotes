using BusinessLayer.Interfaces;
using DatabasLayer.Note;
using DatabasLayer.Note.DatabaseLayer.Note;
using DatabasLayer.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepositoryLayer.Services;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fundoo_Notes.Controllers


{

    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase
    {
        INoteBL noteBL;
        FundooContext fundooContext;
        private readonly IDistributedCache distributedCache;
        private readonly IMemoryCache memoryCache;

        public NoteController(INoteBL noteBL, FundooContext fundooContext, IDistributedCache distributedCache, IMemoryCache memoryCache)
        {
            this.noteBL = noteBL;
            this.fundooContext = fundooContext;
            this.distributedCache = distributedCache;
            this.memoryCache = memoryCache;
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> AddNote(NotePostModel notePostModel)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                await this.noteBL.AddNote(userId, notePostModel);
                return this.Ok(new { success = true, message = "Note Added Sucessfully" });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult> GetAllNote()
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == userId);
                if (note == null)
                {
                    return this.BadRequest(new { success = false, message = "Your Note doesn't exist" });
                }
                List<Note> list = new List<Note>();
                list = await this.noteBL.GetAllNote(userId);

                return this.Ok(new { sucsess = true, message = "Getting your all note successfully", data = list });

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpDelete("{NoteId}")]
        public async Task<ActionResult> DeleteNote(int NoteId)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == userId && u.NoteId == NoteId);
                if (note == null)
                {
                    return this.BadRequest(new { success = false, message = "Sorry..!,Your Note doesn't exist create one" });
                }
                await this.noteBL.DeleteNote(userId, NoteId);

                return this.Ok(new { success = true, message = $"Note Deleted Successfully for the note, {note.Title} " });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpGet("{noteid}")]
        public async Task<ActionResult> GetNote(int noteid)
        {
            try
            {
                var currentUser = HttpContext.User;
                int UserId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == UserId && u.NoteId == noteid);
                if (note == null)
                {
                    return this.BadRequest(new { success = false, message = "Note Doesn't Exists" });
                }

                var note1 = await this.noteBL.GetNote(UserId, noteid);

                return Ok(new { success = true, message = $"Getting your note Successfully for the note, {note.Title} ", data = note1 });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpPut("UpdateNote")]
        public async Task<ActionResult> UpdateNote(int noteId, NoteUpdateModel noteUpdateModel)
        {
            try
            {
                var currentUser = HttpContext.User;
                int UserId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);

                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == UserId && u.NoteId == noteId);
                if (note == null)
                {
                    return this.BadRequest(new { success = false, message = "Sorry..!,Your Note doesn't exist create one" });
                }
                await this.noteBL.UpdateNote(UserId, noteId, noteUpdateModel);

                return this.Ok(new { success = true, message = $"Note Update Successfully for the note, {note.Title} " });

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpPut("ReminderNote")]

        public async Task<ActionResult> ReminderNote(int noteId, NoteReminderModel noteReminderModel)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == userId && u.NoteId == noteId);
                if (note == null)
                {

                    return this.BadRequest(new { success = true, message = "Sorry! Note Doesn't Exist Please Create a Notes" });

                }
                await this.noteBL.ReminderNote(userId, noteId, Convert.ToDateTime(noteReminderModel.Reminder));

                return Ok(new { success = true, message = $"Note Remainder Successfully for the note, {note.Title} " });

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpPut("ArchiveNote")]

        public async Task<ActionResult> ArchiveNote(int NoteId)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == userId && u.NoteId == NoteId);
                if (note == null)
                {

                    return this.BadRequest(new { success = true, message = "Sorry! Note Doesn't Exist Please Create a Notes" });

                }
                await this.noteBL.ArchiveNote(userId, NoteId);

                return Ok(new { success = true, message = $"Note Archive Successfully for the note, {note.Title} " });

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpPut("PinNote")]

        public async Task<ActionResult> PinNote(int NoteId)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == userId && u.NoteId == NoteId);
                if (note == null)
                {

                    return this.BadRequest(new { success = true, message = "Sorry! Note Doesn't Exist Please Create a Notes" });

                }
                await this.noteBL.PinNote(userId, NoteId);

                return Ok(new { success = true, message = $"Note Pinned Successfully for the note, {note.Title} " });

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpPut("TrashNote")]

        public async Task<ActionResult> TrashNote(int NoteId)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == userId && u.NoteId == NoteId);
                if (note == null)
                {

                    return this.BadRequest(new { success = true, message = "Sorry! Note Doesn't Exist Please Create a Notes" });

                }
                await this.noteBL.TrashNote(userId, NoteId);

                return Ok(new { success = true, message = $"Your note trashed Successfully for the note, {note.Title} " });

            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpPut("ColorChnageNote")]

        public async Task<ActionResult> ChangeColorNote(int noteId, ColorChangeModel colorChangeModel)
        {
            try
            {
                var currentUser = HttpContext.User;
                int userId = Convert.ToInt32(currentUser.Claims.FirstOrDefault(c => c.Type == "UserId").Value);
                var note = fundooContext.Notes.FirstOrDefault(u => u.UserId == userId && u.NoteId == noteId);
                if (note == null)
                {

                    return this.BadRequest(new { success = true, message = "Sorry! Note Doesn't Exist Please Create a Notes" });

                }
                await this.noteBL.ChangeColorNote(userId, noteId, colorChangeModel.Colour);

                return Ok(new { success = true, message = $"Note color change Successfully for the note, {note.Title} " });
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        [Authorize]
        [HttpGet("GetAllNoteByRedisCache")]
        public async Task<ActionResult> GetAllNoteByRedis()
        {
            try
            {

                var CacheKey = "NoteList";
                string SerializeNoteList;
                var notelist = new List<Note>();
                var redisnotelist = await distributedCache.GetAsync(CacheKey);
                if (redisnotelist != null)
                {
                    SerializeNoteList = Encoding.UTF8.GetString(redisnotelist);
                    notelist = JsonConvert.DeserializeObject<List<Note>>(SerializeNoteList);
                }
                else
                {
                    var userid = User.Claims.FirstOrDefault(x => x.Type.ToString().Equals("userId", StringComparison.InvariantCultureIgnoreCase));
                    int userId = Int32.Parse(userid.Value);
                    notelist = await this.noteBL.GetAllNote(userId);
                    SerializeNoteList = JsonConvert.SerializeObject(notelist);
                    redisnotelist = Encoding.UTF8.GetBytes(SerializeNoteList);

                    var option = new DistributedCacheEntryOptions().SetSlidingExpiration(TimeSpan.FromMinutes(20)).SetAbsoluteExpiration(TimeSpan.FromHours(6));
                    await distributedCache.SetAsync(CacheKey, redisnotelist, option);
                }
                return this.Ok(new { success = true, message = $"Get Note Successful", data = notelist });

            }
            catch (Exception e)
            {
                throw e;
            }
        }
    }
}