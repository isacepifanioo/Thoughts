using Microsoft.EntityFrameworkCore;
using Thoughts.model;
using Thoughts.Model;

namespace Thoughts.Data {
    public class AppDbContext(DbContextOptions<AppDbContext> options): DbContext(options) {
        
        public DbSet<UsuarioModel> Usuario {get; set;} 
        public DbSet<ThoughtsModel> Thoughts {get; set;}
        public DbSet<LikeModel> Likes {get; set;}
        public DbSet<SaveModel> Save {get; set;}
        public DbSet<CommentModel> Comment {get; set;}
    }
}