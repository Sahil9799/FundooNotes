using DatabasLayer.Note.DatabaseLayer.Note;
using DatabasLayer.User;
using RepositoryLayer.Services.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Interfaces
{
    public interface INoteBL
    {
        Task AddNote(int UserId, NotePostModel notePostModel);
        Task<List<Note>> GetAllNote(int UserId);
        Task DeleteNote(int UserId, int noteId);
        Task<Note> GetNote(int UserId, int noteId);
        Task UpdateNote(int UserId, int noteId, NoteUpdateModel noteUpdateModel);
    }
}
