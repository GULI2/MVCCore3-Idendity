using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace JNGH_IDENDITY.Models
{
    public partial class YXCloudCenterContext : DbContext
    {
        public YXCloudCenterContext()
        {
        }

        public YXCloudCenterContext(DbContextOptions<YXCloudCenterContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AspNetRoleClaims> AspNetRoleClaims { get; set; }
        public virtual DbSet<AspNetRoles> AspNetRoles { get; set; }
        public virtual DbSet<AspNetUserClaims> AspNetUserClaims { get; set; }
        public virtual DbSet<AspNetUserLogins> AspNetUserLogins { get; set; }
        public virtual DbSet<AspNetUserRoles> AspNetUserRoles { get; set; }
        public virtual DbSet<AspNetUserTokens> AspNetUserTokens { get; set; }
        public virtual DbSet<AspNetUsers> AspNetUsers { get; set; }
        public virtual DbSet<SysCompanies> SysCompanies { get; set; }
        public virtual DbSet<SysDepartments> SysDepartments { get; set; }
        public virtual DbSet<SysEmpPositions> SysEmpPositions { get; set; }
        public virtual DbSet<SysEmployees> SysEmployees { get; set; }
        public virtual DbSet<SysMenus> SysMenus { get; set; }
        public virtual DbSet<SysPositions> SysPositions { get; set; }
        public virtual DbSet<ViewEmployees> ViewEmployees { get; set; }
        public virtual DbSet<ViewUser> ViewUser { get; set; }
        public virtual DbSet<ViewOrganizationTree> ViewOrganizationTree { get; set; }
        public object Configuration { get; internal set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=10.115.5.32;Database=YXCloudCenter;Trusted_Connection=True;");
               // optionsBuilder.UseSqlServer("Server=JNNB-IT-GL\\MSSQLSERVER2;Database=IdentityDB;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AspNetRoleClaims>(entity =>
            {
                entity.HasIndex(e => e.RoleId);

                entity.Property(e => e.RoleId).IsRequired();

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetRoleClaims)
                    .HasForeignKey(d => d.RoleId);
            });

            modelBuilder.Entity<AspNetRoles>(entity =>
            {
                entity.HasIndex(e => e.NormalizedName)
                    .HasName("RoleNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedName] IS NOT NULL)");

                entity.Property(e => e.Name).HasMaxLength(256);

                entity.Property(e => e.NormalizedName).HasMaxLength(256);
            });

            modelBuilder.Entity<AspNetUserClaims>(entity =>
            {
                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserClaims)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserLogins>(entity =>
            {
                entity.HasKey(e => new { e.LoginProvider, e.ProviderKey });

                entity.HasIndex(e => e.UserId);

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.ProviderKey).HasMaxLength(128);

                entity.Property(e => e.UserId).IsRequired();

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserLogins)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserRoles>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.RoleId });

                entity.HasIndex(e => e.RoleId);

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.RoleId);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserRoles)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUserTokens>(entity =>
            {
                entity.HasKey(e => new { e.UserId, e.LoginProvider, e.Name });

                entity.Property(e => e.LoginProvider).HasMaxLength(128);

                entity.Property(e => e.Name).HasMaxLength(128);

                entity.HasOne(d => d.User)
                    .WithMany(p => p.AspNetUserTokens)
                    .HasForeignKey(d => d.UserId);
            });

            modelBuilder.Entity<AspNetUsers>(entity =>
            {
                entity.HasIndex(e => e.NormalizedEmail)
                    .HasName("EmailIndex");

                entity.HasIndex(e => e.NormalizedUserName)
                    .HasName("UserNameIndex")
                    .IsUnique()
                    .HasFilter("([NormalizedUserName] IS NOT NULL)");

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.NormalizedEmail).HasMaxLength(256);

                entity.Property(e => e.NormalizedUserName).HasMaxLength(256);

                entity.Property(e => e.UserName).HasMaxLength(256);
            });

         
            modelBuilder.Entity<SysCompanies>(entity =>
            {
                entity.HasKey(e => e.CompId)
                    .HasName("PK_SYS_COMPANIES");

                entity.ToTable("Sys_Companies");

                entity.Property(e => e.CompId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Address).HasMaxLength(100);

                entity.Property(e => e.CompInfo).HasMaxLength(1000);

                entity.Property(e => e.CompLevel)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.Fax)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.FullName).HasMaxLength(50);

                entity.Property(e => e.ParentId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PostCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks).HasMaxLength(500);

                entity.Property(e => e.ShortName).HasMaxLength(50);

                entity.Property(e => e.Telephone)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.Website)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SysDepartments>(entity =>
            {
                entity.HasKey(e => new { e.CompId, e.DeptId })
                    .HasName("PK_SYS_DEPARTMENTS");

                entity.ToTable("Sys_Departments");

                entity.Property(e => e.CompId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeptId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeptLevel)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.DeptName).HasMaxLength(100);

                entity.Property(e => e.ParentId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks).HasMaxLength(200);
            });

            modelBuilder.Entity<SysEmpPositions>(entity =>
            {
                entity.HasKey(e => new { e.EmpId, e.CompId, e.PostId, e.DeptId })
                    .HasName("PK_SYS_EMP_POSITIONS");

                entity.ToTable("Sys_Emp_Positions");

                entity.Property(e => e.EmpId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.CompId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PostId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeptId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks).HasMaxLength(200);
            });

            modelBuilder.Entity<SysEmployees>(entity =>
            {
                entity.HasKey(e => e.EmpId)
                    .HasName("PK_SYS_EMPLOYEES");

                entity.ToTable("Sys_Employees");

                entity.Property(e => e.EmpId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AttenceNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Birthday).HasColumnType("datetime");

                entity.Property(e => e.Email)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EmpName).HasMaxLength(50);

                entity.Property(e => e.EmpNo)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.HomeAddr).HasMaxLength(200);

                entity.Property(e => e.IdcardNo)
                    .HasColumnName("IDCardNo")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.InsidePhone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MobilePhone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PostCode)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Qqcode)
                    .HasColumnName("QQCode")
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks).HasMaxLength(200);

                entity.Property(e => e.Telephone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Wxcode)
                    .HasColumnName("WXCode")
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<SysMenus>(entity =>
            {
                entity.HasKey(e => new { e.AppId, e.MenuId });

                entity.ToTable("Sys_Menus");

                entity.Property(e => e.AppId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MenuId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Action)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Area)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Controller)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.MenuName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.MenuType)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Remarks).HasMaxLength(200);
            });

            modelBuilder.Entity<SysPositions>(entity =>
            {
                entity.HasKey(e => new { e.CompId, e.PostId, e.DeptId })
                    .HasName("PK_SYS_POSITIONS");

                entity.ToTable("Sys_Positions");

                entity.Property(e => e.CompId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PostId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.DeptId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.ParentId)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.PostLevel)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.Property(e => e.PostName).HasMaxLength(100);

                entity.Property(e => e.Remarks).HasMaxLength(200);
            });

          

            modelBuilder.Entity<ViewEmployees>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_Employees");

                entity.Property(e => e.EmpId)
                 .HasMaxLength(50)
                 .IsUnicode(false);

                entity.Property(e => e.EmpNo)
                 .HasMaxLength(50)
                 .IsUnicode(false);

                entity.Property(e => e.MobilePhone)
                 .HasMaxLength(50)
                 .IsUnicode(false);

                entity.Property(e => e.Email)
                  .HasMaxLength(100)
                  .IsUnicode(false);

                entity.Property(e => e.EmpName).HasMaxLength(50);

                entity.Property(e => e.PostName).HasMaxLength(100);

                entity.Property(e => e.PostId)
                  .HasMaxLength(50)
                  .IsUnicode(false);

                entity.Property(e => e.DeptName).HasMaxLength(100);

                entity.Property(e => e.DeptId)
                  .HasMaxLength(50)
                  .IsUnicode(false);

                entity.Property(e => e.ShortName).HasMaxLength(50);

                entity.Property(e => e.CompId)
           .HasMaxLength(50)
           .IsUnicode(false);
            });

            modelBuilder.Entity<ViewUser>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("View_Users");
                
                entity.Property(e => e.Id).HasMaxLength(50)
                 .IsUnicode(false); 

                entity.Property(e => e.UserName).HasMaxLength(256);

                entity.Property(e => e.PasswordHash);

                entity.Property(e => e.Email).HasMaxLength(256);

                entity.Property(e => e.PhoneNumber).HasMaxLength(100);

                entity.Property(e => e.Creator).HasMaxLength(100);

                entity.Property(e => e.CreateTime).HasColumnType("datetime");

                entity.Property(e => e.RoleName).HasMaxLength(256);

                entity.Property(e => e.RoleId);

                entity.Property(e => e.DeptId);

                entity.Property(e => e.DeptName).HasMaxLength(100);

                entity.Property(e => e.PostId);

                entity.Property(e => e.PostName).HasMaxLength(100);

                entity.Property(e => e.CompId);

                entity.Property(e => e.ShortName).HasMaxLength(50);

            });

            modelBuilder.Entity<ViewOrganizationTree>(entity =>
            {
                entity.ToView("View_OrganizationTree");
                entity.Property(e => e.Id);
                entity.Property(e => e.ParentId);
                entity.Property(e => e.Name);
                entity.Property(e => e.DataType);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
