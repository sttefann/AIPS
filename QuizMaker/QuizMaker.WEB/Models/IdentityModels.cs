using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;

namespace QuizMaker.WEB.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    //New drived classes 
    public class UserRoleIntPk : IdentityUserRole<long>
    {
    }

    public class UserClaimIntPk : IdentityUserClaim<long>
    {
    }

    public class UserLoginIntPk : IdentityUserLogin<long>
    {
    }

    public class RoleIntPk : IdentityRole<long, UserRoleIntPk>
    {
        public RoleIntPk() { }
        public RoleIntPk(string name) { Name = name; }
    }

    public class UserStoreIntPk : UserStore<ApplicationUser, RoleIntPk, long,
        UserLoginIntPk, UserRoleIntPk, UserClaimIntPk>
    {
        public UserStoreIntPk(ApplicationDbContext context)
            : base(context)
        {
        }
    }

    public class RoleStoreIntPk : RoleStore<RoleIntPk, long, UserRoleIntPk>
    {
        public RoleStoreIntPk(ApplicationDbContext context)
            : base(context)
        {
        }
    }
    public class ApplicationUser : IdentityUser<long, UserLoginIntPk, UserRoleIntPk, UserClaimIntPk>
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser, long> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, RoleIntPk, long,
        UserLoginIntPk, UserRoleIntPk, UserClaimIntPk>
    {
        public ApplicationDbContext()
            : base("DefaultConnection")
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }

    public class ApplicationUserStore : UserStore<ApplicationUser, RoleIntPk, long,
        UserLoginIntPk, UserRoleIntPk,
        UserClaimIntPk>, IUserStore<ApplicationUser, long>,
        IDisposable
    {
        public ApplicationUserStore() : this(new ApplicationDbContext())
        {
            base.DisposeContext = true;
        }


        public ApplicationUserStore(DbContext context)
            : base(context)
        {

        }
    }
}

