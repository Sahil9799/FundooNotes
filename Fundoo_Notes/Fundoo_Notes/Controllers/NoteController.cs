using BusinessLayer.Interfaces;
using DatabasLayer.Note.DatabaseLayer.Note;
using DatabasLayer.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Services;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fundoo_Notes.Controllers


{
    [ApiController]
    [Route("[controller]")]
    public class NoteController : ControllerBase
    {
        INoteBL noteBL;
        FundooContext fundooContext;

        public NoteController(INoteBL noteBL, FundooContext fundooContext)
        {
            this.noteBL = noteBL;
            this.fundooContext = fundooContext;
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
    }
}