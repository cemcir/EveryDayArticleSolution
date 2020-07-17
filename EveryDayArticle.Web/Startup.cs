using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EveryDayArticle.Business.Abstract;
using EveryDayArticle.Business.Concreate;
using EveryDayArticle.DataAccess;
using EveryDayArticle.DataAccess.Abstract;
using EveryDayArticle.DataAccess.Concreate;
using EveryDayArticle.Web;
using EveryDayArticle.Web.CustomValidation;
using EveryDayArticle.Web.Identity;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace EveryDayArticle.Web
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            services.AddDbContext<AppIdentityDbContext>(opts => {
                opts.UseSqlServer(Configuration["ConnectionStrings:SqlConStr"]);
            });

            services.AddIdentity<AppUser,AppRole>(opts=> {

                opts.User.AllowedUserNameCharacters = "abcçdefgğhıijklmnoöpqrsştuüvwxyzABCÇDEFGĞHIJKLMNOÖPQRSŞTUÜVWXYZ0123456789-._";
                opts.User.RequireUniqueEmail = true;
                opts.Password.RequiredLength = 4;
                opts.Password.RequireNonAlphanumeric = false;
                opts.Password.RequireLowercase = false;
                opts.Password.RequireUppercase = false;
                opts.Password.RequireDigit = false;

            }).AddPasswordValidator<CustomPasswordValidator>().AddUserValidator<CustomUserValidator>().AddErrorDescriber<CustomIdentityErrorDescriber>().AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

            CookieBuilder cookieBuilder = new CookieBuilder();

            cookieBuilder.Name = "MyBlog";
            cookieBuilder.HttpOnly = false;
            //cookieBuilder.Expiration = System.TimeSpan.FromDays(60);
            cookieBuilder.SameSite = SameSiteMode.Strict;
            cookieBuilder.SecurePolicy = CookieSecurePolicy.SameAsRequest;

            services.ConfigureApplicationCookie(opts => {
                opts.LoginPath = new PathString("/login");
                opts.Cookie = cookieBuilder;
                opts.SlidingExpiration = true;
                opts.ExpireTimeSpan = System.TimeSpan.FromDays(60);
                opts.AccessDeniedPath = new PathString("/access/denied");
            });

            services.AddDbContext<ArticleContext>(options =>
            {
                options.UseSqlServer(Configuration["ConnectionStrings:SqlConStr"].ToString(), o =>
                {
                    o.MigrationsAssembly("EveryDayArticle.DataAccess");
                });
            });
            
            /*
            services.AddDbContext<ArticleContext>(opts => opts.UseSqlServer(Configuration["ConnectionStrings:SqlConStr"]));
            */
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped(typeof(IService<>), typeof(Service<>));
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IArticleRepository, ArticleRepository>();
            services.AddScoped<ICommentRepository, CommentRepository>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<ICommentService,CommentService>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<Message, Message>();
            services.AddScoped<ILikedRepository,LikedRepository>();
            services.AddScoped<ILikedService, LikedService>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment()) {
                app.UseDeveloperExceptionPage();
            }
            else {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }


            app.UseHttpsRedirection();
            app.UseDeveloperExceptionPage();
            app.UseStatusCodePages();
            app.UseStaticFiles();
            app.UseAuthentication();

            //app.UseCookiePolicy();
            app.UseMvcWithDefaultRoute();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "Category",
                    template: "category/add",
                    defaults: new { controller = "Category", action = "AddCategory" }
                );

                routes.MapRoute(
                    name: "Categories",
                    template: "categories",
                    defaults: new { controller = "Category", action = "CategoryList" }
                );

                routes.MapRoute(
                    name:"CategoryDelete",
                    template:"category/delete",
                    defaults:new {controller="Category",action="DeleteCategory"}
                );

                routes.MapRoute(
                    name: "Login",
                    template:"login",
                    defaults:new {controller="Account",action="Login"}
                );

                routes.MapRoute(
                    name:"CategoryEdit",
                    template:"category/edit",
                    defaults: new {Controller="Category", Action="EditCategory"}
                );

                routes.MapRoute(
                    name:"Account",
                    template:"account",
                    defaults:new {Controller="Account",Action="SignUp"}
                );

                routes.MapRoute(
                    name:"About",
                    template:"about",
                    defaults:new {Controller="Home",Action="About"}
                );

                routes.MapRoute(
                    name:"Home",
                    template:"home",
                    defaults:new {Controller="Home",Action="Index"}
                );

                routes.MapRoute(
                    name:"Article",
                    template:"article/add",
                    defaults: new { Controller="Article",Action="AddArticle" }
                );

                routes.MapRoute(
                    name:"AdminArticle",
                    template:"article/list",
                    defaults:new { Controller="Article",Action="GetArticleList" }
                );

                routes.MapRoute(
                    name:"ArticleDelete",
                    template:"article/delete",
                    defaults:new { Controller="Article",Action="DeleteArticle" }
                );

                routes.MapRoute(
                    name:"Articles",
                    template:"article",
                    defaults:new { Controller="Article",Action="GetArticles" }
                );

                routes.MapRoute(
                    name:"AddComment",
                    template:"comment/add",
                    defaults:new {Controller="Comment" ,Action="AddComment"}
                );

                routes.MapRoute(
                    name:"Comments",
                    template:"comments",
                    defaults:new {Controller="Comment",Action="GetComments"}
                );

                routes.MapRoute(
                    name:"GetArticle",
                    template:"articles",
                    defaults:new { Controller="Article", Action="GetArticleByName"}
                );

                routes.MapRoute(
                    name:"GetArticleById",
                    template:"get/article",
                    defaults:new { Controller="Article",Action="GetArticleById" }
                );

                routes.MapRoute(
                    name:"GetComments",
                    template:"get/comment",
                    defaults:new {Controller="Comment",Action="GetCommentsById"}
                );

                routes.MapRoute(
                    name:"ArticleEdit",
                    template:"article/edit",
                    defaults:new { Controller="Article",Action="EditArticle" }
                );

                routes.MapRoute(
                    name:"Liked",    
                    template:"liked/add",
                    defaults:new { Controller="Liked" ,Action="AddLiked" }
                );

                routes.MapRoute(
                    name:"PasswordReset",
                    template:"reset",
                    defaults:new { Controller="Account" ,Action="ResetPassword" }
                );

                routes.MapRoute(
                    name:"PasswordConfirm",
                    template:"confirm",
                    defaults: new { Controller="Account" ,Action= "ResetPasswordConfirm" }
                );

                routes.MapRoute(
                    name:"Change",
                    template:"change",
                    defaults:new { Controller="Account" ,Action="PasswordChange"}
                );

                routes.MapRoute(
                    name:"UserEdit",
                    template:"account/edit",
                    defaults: new { Controller="Account",Action="UserEdit"}
                );

                routes.MapRoute(
                    name:"Logout",
                    template:"logout",
                    defaults:new {Controller="Account" ,Action="LogOut"}
                );

                routes.MapRoute(
                    name:"Member",
                    template:"users",
                    defaults:new { Controller="Admin", Action="Users" }
                );

                routes.MapRoute(
                    name:"Roles",
                    template:"roles",
                    defaults:new { Controller="Admin",Action="Roles" }
                );

                routes.MapRoute(
                    name:"Access",
                    template:"access/denied",
                    defaults: new { Controller="Account" ,Action="AccessDenied"}
                    
                );

                routes.MapRoute(
                    name:"RoleCreate",
                    template:"role/create",
                    defaults: new {Controller="Admin" ,Action="RoleCreate"}
                );

                routes.MapRoute(
                    name:"RoleDelete",
                    template:"role/delete",
                    defaults:new { Controller="Admin",Action="RoleDelete" }
                );

                routes.MapRoute(
                    name:"Profile",
                    template:"profile",
                    defaults:new { Controller="Account",Action="Profile" }
                );

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
