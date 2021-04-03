using ElevenNote.Models.CategoryNote;
using ElevenNote.Services;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ElevenNote.WebAPI.Controllers
{
    [Authorize]
    public class CategoryNoteController : ApiController
    {
        [HttpPost]
        public IHttpActionResult PostCategoryNote(CategoryNoteCreate model)
        {
            var service = CreateService();
            if (!service.CreateCategoryNote(model))
                return InternalServerError();

            return Ok($"Note {model.NoteId} was added to category {model.CategoryId}");
        }

        [HttpDelete]
        public IHttpActionResult DeleteCategoryNote([FromUri]int noteId, [FromUri] int categoryId)
        {
            var service = CreateService();
            if (service.DeleteCategoryNote(noteId, categoryId))
                return Ok();

            return InternalServerError();
        }


        private CategoryNoteService CreateService()
        {
            Guid userId = Guid.Parse(User.Identity.GetUserId());
            var service = new CategoryNoteService(userId);
            return service;
        }
    }
}
