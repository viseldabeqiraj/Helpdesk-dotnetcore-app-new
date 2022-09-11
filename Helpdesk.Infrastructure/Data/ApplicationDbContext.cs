using Helpdesk.Infrastructure.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace Helpdesk.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Client> Client { get; set; }
        public DbSet<CommunicationChannel> CommunicationChannel { get; set; }
        public DbSet<Program> Program { get; set; }
        public DbSet<Request> Request { get; set; }
        public DbSet<RequestType> RequestType { get; set; }
        public DbSet<Response> Response { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
    }
}