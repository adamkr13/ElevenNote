using ElevenNote.Data;
using ElevenNote.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElevenNote.Services
{
    public class CategoryService
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
                OwnerId = _userId,
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
                return new CategoryDetail { CategoryId = entity.CategoryId, CategoryName = entity.CategoryName, Description = entity.Description, CreatedUtc = entity.CreatedUtc, ModifiedUtc = entity.ModifiedUtc };
            }
        }

        public bool UpdateCategory(CategoryEdit model)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Categories.Single(e => e.CategoryId == model.CategoryId);

                entity.CategoryName = model.CategoryName;
                entity.Description = model.Description;
                entity.ModifiedUtc = DateTimeOffset.UtcNow;

                return ctx.SaveChanges() > 0;
            }
        }
        public bool DeleteCategory(int categoryId)
        {
            using (var ctx = new ApplicationDbContext())
            {
                var entity = ctx.Categories.Single(e => e.CategoryId == categoryId && e.OwnerId == _userId);

                ctx.Categories.Remove(entity);

                return ctx.SaveChanges() == 1;
            }
        }
    }
}
