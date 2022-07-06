using DatabasLayer.Note.DatabaseLayer.Note;
using DatabasLayer.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Interfaces;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryLayer.Services
{
    public class NoteRL : INoteRL
    {
        FundooContext fundooContext;

        IConfiguration configuration;

        public NoteRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.configuration = configuration;
        }

        public async Task AddNote(int UserId, NotePostModel notePostModel)
        {
            try
            {
                Note note = new Note();
                note.UserId = UserId;
                note.Title = notePostModel.Title;
                note.Description = notePostModel.Description;
                note.Colour = notePostModel.Colour;
                note.CreatedDate = DateTime.Now;
                note.ModifiedDate = DateTime.Now;
                fundooContext.Add(note);

                await fundooContext.SaveChangesAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task DeleteNote(int UserId, int noteId)
        {
            try
            {
                var note = fundooContext.Notes.Where(x => x.UserId == UserId && x.NoteId == noteId).FirstOrDefault();
                if (note != null)
                {
                    fundooContext.Notes.Remove(note);

                    await fundooContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<List<Note>> GetAllNote(int UserId)
        {
            try
            {
                var note = fundooContext.Notes.Where(u => u.UserId == UserId).FirstOrDefault();
                if (note == null)
                {
                    return null;
                }
                return await fundooContext.Notes.ToListAsync();
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task<Note> GetNote(int UserId, int noteId)
        {
            try
            {
                var note = fundooContext.Notes.Where(u => u.UserId == UserId && u.NoteId == noteId).FirstOrDefault();

                if (note == null)
                {
                    return null;
                }
                return await fundooContext.Notes.FirstOrDefaultAsync(u => u.UserId == UserId && u.NoteId == noteId);
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        public async Task UpdateNote(int UserId, int noteId, NoteUpdateModel noteUpdateModel)
        {
            try
            {
                var note = fundooContext.Notes.Where(x => x.UserId == UserId && x.NoteId == noteId).FirstOrDefault();
                if (note != null)
                {
                    note.Title = noteUpdateModel.Title;
                    note.Description = noteUpdateModel.Description;
                    note.Colour = noteUpdateModel.Colour;
                    note.IsArchive = noteUpdateModel.IsArchive;
                    note.IsPin = noteUpdateModel.IsPin;
                    note.IsReminder = noteUpdateModel.IsReminder;
                    note.IsTrash = noteUpdateModel.IsTrash;
                    await fundooContext.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }
    }
}