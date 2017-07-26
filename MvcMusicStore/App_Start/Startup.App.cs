using System.Configuration;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MvcMusicStore.Models;
using Owin;

namespace MvcMusicStore
{
    public partial class Startup
    {
        private const string RoleName = "Administrator";

        public void ConfigureApp(IAppBuilder app)
        {
            using (var context = new MusicStoreEntities())
            {
                //This is the original code, works fine on localdb but not a real-world scenario to deploy your db 
                //on every app start
                //Currently the deployment template creates a db and sources the data from a bacpac so none of this is needed
                //context.Database.Delete();
                //context.Database.Create();
                //new SampleData().Seed(context);
            }
            //remove this for the demo so no one has unexpected admin access to the test app
            //uncomment and update web.config if you need to use the admin features, they aren't needed for the demo
            //CreateAdminUser().Wait();
        }

        private async Task CreateAdminUser()
        {
            var username = ConfigurationManager.AppSettings["DefaultAdminUsername"];
            var password = ConfigurationManager.AppSettings["DefaultAdminPassword"];

            using (var context = new ApplicationDbContext())
            {
                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

                var role = new IdentityRole(RoleName);

                var result = await roleManager.RoleExistsAsync(RoleName);
                if (!result)
                {
                    await roleManager.CreateAsync(role);
                }

                var user = await userManager.FindByNameAsync(username);
                if (user == null)
                {
                    user = new ApplicationUser { UserName = username };
                    await userManager.CreateAsync(user, password);
                    await userManager.AddToRoleAsync(user.Id, RoleName);
                }
            }
        }
    }
}