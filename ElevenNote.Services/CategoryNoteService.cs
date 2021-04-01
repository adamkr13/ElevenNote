using ElevenNote.Data;
using ElevenNote.Models.CategoryNote;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class CategoryNoteService
    {
        private readonly Guid _userId;

        public CategoryNoteService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateCategoryNote(CategoryNoteCreate model)
        {


            var entity = new CategoryNote()
            {
                CategoryId = model.CategoryId,
                NoteId = model.NoteId
            };

            using(var ctx = new ApplicationDbContext())
            {
                ctx.CategoryNotes.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

    }
}
