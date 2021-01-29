using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    class CategoryService
    {
        private readonly Guid _userId;

        public CategoryService(Guid userId)
        {
            _userId = userId;
        }

        public bool CreateCategory(CategoryCreate model)
        {
            var entity = new Category()
            {
                CategoryName = model.CategoryName,
                Description = model.Description,
                CreatedUtc = DateTimeOffset.Now
            };
            using (var ctx = new ApplicationDbContext())
            {
                ctx.Categories.Add(entity);
                return ctx.SaveChanges() == 1;
            }
        }

        public IEnumerable<CategoryListItem> GetCategories()
        {
            using (var ctx = new ApplicationDbContext())
            {
                var query = ctx.Categories.Select(e => new CategoryListItem
                { CategoryId = e.CategoryId, CategoryName = e.CategoryName });

                return query.ToArray();
            }
        }

        public CategoryDetail GetCategoryById(int id)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Categories.Single(e => e.CategoryId == id);
                return new CategoryDetail { CategoryId = entity.CategoryId, CategoryName = entity.CategoryName };
            }
        }
    }
}
