using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class NoteService
    {
        private readonly Guid _userId;

        public NoteService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateNote(NoteCreate model)
        {
            var entity =
                new Note()
                {
                    OwnerId = _userId,
                    Title = model.Title,
                    Content = model.Content,
                    CreatedUtc = DateTimeOffset.Now
                };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Notes.Add(entity);
                return ctx.SaveChanges() == 0;
            }
        }
        public IEnumerable<NoteListItem> GetNotes()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Notes
                        .Where(e => e.OwnerId == _userId)
                        .Select(
                            e =>
                                new NoteListItem
                                {
                                    NoteId = e.NoteId,
                                    Title = e.Title,
                                    CreatedUtc = e.CreatedUtc,
                                    CategoryIds = e.Categories.Select(n => n.Category.CategoryId).ToList(),
                                    CategoryNames = e.Categories.Select(n=>n.Category.CategoryName).ToList()
                                });
                return query.ToArray();
            }
        }
        public IEnumerable<NoteListItem> GetNotesByCategoryId(int categoryId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query =
                    ctx
                        .Notes
                        .Where(e => e.OwnerId == _userId && e.CategoryId == categoryId)
                        .Select(
                            e =>
                                new NoteListItem
                                {
                                    NoteId = e.NoteId,
                                    Title = e.Title,
                                    CreatedUtc = e.CreatedUtc,
                                    CategoryId = e.CategoryId,
                                    CategoryName = e.Category.CategoryName
                                });
                return query.ToArray();
            }
        }
        public NoteDetail GetNoteById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Notes
                        .Single(e => e.NoteId == id && e.OwnerId == _userId);
                if (entity.CategoryId == null)
                {
                    return
                    new NoteDetail
                    {
                        NoteId = entity.NoteId,
                        Title = entity.Title,
                        Content = entity.Content,
                        CategoryName = null,
                        CreatedUtc = entity.CreatedUtc,
                        ModifiedUtc = entity.ModifiedUtc
                    };
                }


                else
                {
                    return
                    new NoteDetail
                    {
                        NoteId = entity.NoteId,
                        Title = entity.Title,
                        Content = entity.Content,
                        CreatedUtc = entity.CreatedUtc,
                        ModifiedUtc = entity.ModifiedUtc,
                        CategoryId = entity.CategoryId,
                        Category = new CategoryListItem() { CategoryId = entity.Category.CategoryId, CategoryName = entity.Category.CategoryName }
                    };
                }
            }
        }

        public bool UpdateNote(NoteEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity =
                    ctx
                        .Notes
                        .Single(e => e.NoteId == model.NoteId && e.OwnerId == _userId);

                entity.Title = model.Title;
                entity.Content = model.Content;
                entity.CategoryId = model.CategoryId;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() > 0;
            }
        }

        public bool DeleteNote(int noteId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Notes.Single(e => e.NoteId == noteId && e.OwnerId == _userId);

                ctx.Notes.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
        public void NullCategory(int Id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Notes.Where(e => e.CategoryId == Id);

                foreach (var note in entity)
                    note.CategoryId = null;

                //var notes = GetNotesByCategory(Id);

                //foreach (var note in notes)
                //    note.CategoryId = null;

                var test = (ctx.SaveChanges() > 0);
            }
        }
    }
}
