using System;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace DL
{
    public partial class ProjectDBContext : DbContext
    {
        public ProjectDBContext()
        {
        }

        public ProjectDBContext(DbContextOptions<ProjectDBContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Gender> Genders { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Rating> Ratings { get; set; }
        public virtual DbSet<SentedMessege> SentedMesseges { get; set; }
        public virtual DbSet<SentedSystemMessege> SentedSystemMesseges { get; set; }
        public virtual DbSet<Status> Statuses { get; set; }
        public virtual DbSet<SystemMessege> SystemMesseges { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserInGroup> UserInGroups { get; set; }
        public virtual DbSet<Weight> Weights { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=SRV2\\PUPILS;Database=ProjectDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Hebrew_CI_AS");

            modelBuilder.Entity<Gender>(entity =>
            {
                entity.ToTable("Gender");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Gender1)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("gender")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("Group");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GenderId).HasColumnName("genderId");

                entity.Property(e => e.GroupName)
                    .IsRequired()
                    .HasMaxLength(15)
                    .HasColumnName("groupName")
                    .IsFixedLength(true);

                entity.Property(e => e.IsOpen).HasColumnName("isOpen");

                entity.Property(e => e.ManagerId).HasColumnName("managerId");

                entity.Property(e => e.MaxAge).HasColumnName("maxAge");

                entity.Property(e => e.MinAge).HasColumnName("minAge");

                entity.Property(e => e.NumOfWeeks).HasColumnName("numOfWeeks");

                entity.Property(e => e.Password)
                    .HasMaxLength(5)
                    .HasColumnName("password")
                    .IsFixedLength(true);

                entity.Property(e => e.StartDate)
                    .HasColumnType("date")
                    .HasColumnName("startDate");

                entity.Property(e => e.Status).HasColumnName("status");

                entity.HasOne(d => d.Gender)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.GenderId)
                    .HasConstraintName("FK_Group_Gender");

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.ManagerId)
                    .HasConstraintName("FK_Group_User");

                entity.HasOne(d => d.StatusNavigation)
                    .WithMany(p => p.Groups)
                    .HasForeignKey(d => d.Status)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Group_Status");
            });

            modelBuilder.Entity<Rating>(entity =>
            {
                entity.ToTable("Rating");

                entity.Property(e => e.RatingId).HasColumnName("RATING_ID");

                entity.Property(e => e.Host)
                    .HasMaxLength(50)
                    .HasColumnName("HOST");

                entity.Property(e => e.Method)
                    .HasMaxLength(10)
                    .HasColumnName("METHOD")
                    .IsFixedLength(true);

                entity.Property(e => e.Path)
                    .HasMaxLength(50)
                    .HasColumnName("PATH");

                entity.Property(e => e.RecordDate)
                    .HasColumnType("datetime")
                    .HasColumnName("Record_Date");

                entity.Property(e => e.Referer)
                    .HasMaxLength(100)
                    .HasColumnName("REFERER");

                entity.Property(e => e.UserAgent).HasColumnName("USER_AGENT");
            });

            modelBuilder.Entity<SentedMessege>(entity =>
            {
                entity.ToTable("SentedMessege");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Content)
                    .IsRequired()
                    .HasMaxLength(300)
                    .HasColumnName("content");

                entity.Property(e => e.GroupId).HasColumnName("groupId");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasColumnName("time");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.SentedMesseges)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SentedMessege_Group");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.SentedMesseges)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SentedMessege_User");
            });

            modelBuilder.Entity<SentedSystemMessege>(entity =>
            {
                entity.ToTable("SentedSystemMessege");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.GroupId).HasColumnName("groupId");

                entity.Property(e => e.MessegeId).HasColumnName("messegeId");

                entity.Property(e => e.Time)
                    .HasColumnType("datetime")
                    .HasColumnName("time");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.SentedSystemMesseges)
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SentedSystemMessege_Group");

                entity.HasOne(d => d.Messege)
                    .WithMany(p => p.SentedSystemMesseges)
                    .HasForeignKey(d => d.MessegeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SentedSystemMessege_SystemMesseges");
            });

            modelBuilder.Entity<Status>(entity =>
            {
                entity.ToTable("Status");

                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("id");

                entity.Property(e => e.Status1)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("status")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<SystemMessege>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.MessegeContent)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("messegeContent")
                    .IsFixedLength(true);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.DateOfBirth)
                    .HasColumnType("date")
                    .HasColumnName("dateOfBirth");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(30)
                    .HasColumnName("email")
                    .IsFixedLength(true);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("firstName")
                    .IsFixedLength(true);

                entity.Property(e => e.Gender).HasColumnName("gender");

                entity.Property(e => e.IdentityNumber)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("identityNumber")
                    .IsFixedLength(true);

                entity.Property(e => e.LastName)
                    .HasMaxLength(10)
                    .HasColumnName("lastName")
                    .IsFixedLength(true);

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasColumnName("password")
                    .IsFixedLength(true);

                entity.Property(e => e.Profile).HasColumnName("profile");

                entity.Property(e => e.Weight).HasColumnName("weight");

                entity.HasOne(d => d.GenderNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Gender)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_Gender");
            });

            modelBuilder.Entity<UserInGroup>(entity =>
            {
                //entity.HasNoKey();

                entity.ToTable("UserInGroup");

                entity.Property(e => e.FirstImage)
                    .HasColumnType("image")
                    .HasColumnName("firstImage");

                entity.Property(e => e.GroupId).HasColumnName("groupId");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.LastImage)
                    .HasColumnType("image")
                    .HasColumnName("lastImage");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Group)
                    .WithMany()
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserInGroup_Group");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserInGroup_User");
            });

            modelBuilder.Entity<Weight>(entity =>
            {
         

                entity.ToTable("Weight");

                entity.Property(e => e.CurrentWeight).HasColumnName("currentWeight");

                entity.Property(e => e.Date)
                    .HasColumnType("date")
                    .HasColumnName("date");

                entity.Property(e => e.GroupId).HasColumnName("groupId");

                entity.Property(e => e.Id)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("id");

                entity.Property(e => e.Kg).HasColumnName("kg");

                entity.Property(e => e.UserId).HasColumnName("userId");

                entity.HasOne(d => d.Group)
                    .WithMany()
                    .HasForeignKey(d => d.GroupId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Weight_Group");

                entity.HasOne(d => d.User)
                    .WithMany()
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Weight_User");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
