using Microsoft.EntityFrameworkCore;
using TaskManager.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

//databaseimde dolaşmak için


namespace TaskManager.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)//constructor that tells you how and where to connect to the database
            : base(options)
        {
            
        }

        //oracle tablelarını temsil eden DbSet'ler
        public DbSet<User> Users { get; set; }
        public DbSet<Domain.Entities.Task> Tasks { get; set; }
        public DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)//c# classlarım table lara maplendikten sonra, yani model oluşturulduktan sonra, ilişkileri açıklamak için kullanılır
        {
            base.OnModelCreating(modelBuilder);//calls default behavior of the model builder

            //primary key leri tanımlıyoruz
            modelBuilder.Entity<User>()
                .HasKey(u => u.usrId);
            
            modelBuilder.Entity<User>()
                .Property(u => u.usrId)
                .ValueGeneratedOnAdd();//auto-increment

            modelBuilder.Entity<User>()
                .Property(u => u.email)
                .IsRequired();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.email)
                .IsUnique();

            //TASK
            modelBuilder.Entity<Domain.Entities.Task>()
                .HasKey(t => t.taskId);

            modelBuilder.Entity<Domain.Entities.Task>()
                .Property(t => t.taskId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Domain.Entities.Task>()
                .Property(t => t.taskName)
                .IsRequired();

            modelBuilder.Entity<Domain.Entities.Task>()
                .HasOne(u => u.User)
                .WithMany(t => t.Tasks)
                .HasForeignKey(t => t.usrId);

            //TAG
            modelBuilder.Entity<Tag>()
                .HasKey(t => t.tagId);

            modelBuilder.Entity<Tag>()
                .Property(t => t.tagId)
                .ValueGeneratedOnAdd();

            modelBuilder.Entity<Tag>()
                .Property(t => t.tagName)
                .IsRequired();

            //user-task
            modelBuilder.Entity<User>()
                .HasMany(t => t.Tasks)
                .WithOne(u => u.User);

            //task-tag
            modelBuilder.Entity<Domain.Entities.Task>()
                .HasMany(t => t.Tags)
                .WithMany(t => t.Tasks)
                .UsingEntity(t => t.ToTable("TaskTag"));


            modelBuilder.Entity<User>()
                .Property(u => u.verified)
                .HasColumnType("NUMBER(1)")//oracle veritabanında boolean tipi yok, bu yüzden 1 bitlik bir sayı olarak saklıyoruz
                .HasDefaultValueSql("0");


        }
    }
}
