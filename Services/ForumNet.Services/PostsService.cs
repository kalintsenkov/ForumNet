namespace ForumNet.Services
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using AutoMapper;
    using Contracts;
    using Data;
    using Data.Models;
    using Microsoft.EntityFrameworkCore;

    public class PostsService: IPostsService
    {
        private readonly ForumDbContext db;
        private readonly IMapper mapper;

        public PostsService(ForumDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task Create(string title, string description, string authorId, int categoryId)
        {
            var post = new Post
            {
                Title = title,
                Description = description,
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                AuthorId = authorId,
                CategoryId = categoryId
            };

            await this.db.Posts.AddAsync(post);
            await this.db.SaveChangesAsync();
        }

        public async Task View(int id)
        {
            var post = await this.db.Posts.FirstOrDefaultAsync(p => p.Id == id);

            post.Views++;

            await this.db.SaveChangesAsync();
        }

        public async Task Like(int id)
        {
            var post = await this.db.Posts.FirstOrDefaultAsync(p => p.Id == id);

            post.Likes++;

            await this.db.SaveChangesAsync();
        }

        public async Task Dislike(int id)
        {
            var post = await this.db.Posts.FirstOrDefaultAsync(p => p.Id == id);

            post.Likes--;

            await this.db.SaveChangesAsync();
        }
    }
}