using Alpha.DataAccess;
using Alpha.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;

namespace Alpha.Web.App.Models
{
    public class SeedData
    {
        public static void EnsurePopulated(IApplicationBuilder app)
        {
            ApplicationDbContext context = app.ApplicationServices.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
            if (!context.AboutUs.Any())
            {
                context.AboutUs.AddRange(new AboutUs { Description = "I'm a web developer!", IsActive = true });
                context.SaveChanges();
            }
            if (context.Tags.Any()) { return; }
            context.Tags.AddRange(
                new Tag { Title = "Web Design", IsActive = true, Description = "Web Design description", Size = 1 },
                new Tag { Title = "Javascript", IsActive = false, Description = "Javascript description", Size = 2 },
                new Tag { Title = "C#", IsActive = true, Description = "C# description", Size = 3 },
                new Tag { Title = "HTML", IsActive = true, Description = "HTML description", Size = 4 },
                new Tag { Title = "ASP.NET Core 2", IsActive = true, Description = "ASP.NET Core 2 description", Size = 5 },
                new Tag { Title = "SQL Server", IsActive = false, Description = "SQL Server description", Size = 6 },
                new Tag { Title = "Umbraco", IsActive = true, Description = "Umbraco description", Size = 1 },
                new Tag { Title = "CMS", IsActive = true, Description = "CMS description", Size = 1 },
                new Tag { Title = "MVC", IsActive = true, Description = "MVC description", Size = 1 },
                new Tag { Title = "JQuery", IsActive = true, Description = "JQuery description", Size = 2 },
                new Tag { Title = "Bootstrap", IsActive = true, Description = "Bootstrap description", Size = 2 }
                );
            context.SaveChanges();

            context.Articles.AddRange(
                new Article
                {

                    IsActive = true,
                    Title = "This is a titel 1.",
                    Summary = "1- Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ducimus, vero, obcaecati, aut, error quam sapiente nemo saepe quibusdam sit excepturi nam quia corporis eligendi eos magni recusandae laborum minus inventore?",
                    Description = "1- Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.Lorem ipsum dolor sit amet, consectetur adipisicing elit.Eos, doloribus, dolorem iusto blanditiis unde eius illum consequuntur neque dicta incidunt ullam ea hic porro optio ratione repellat perspiciatis.Enim, iure! Lorem ipsum dolor sit amet, consectetur adipiscing elit.Integer posuere erat a ante."
                },
                new Article
                {

                    IsActive = true,
                    Title = "This is a titel 2.",
                    Summary = "2- Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ducimus, vero, obcaecati, aut, error quam sapiente nemo saepe quibusdam sit excepturi nam quia corporis eligendi eos magni recusandae laborum minus inventore?",
                    Description = "2- Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.Lorem ipsum dolor sit amet, consectetur adipisicing elit.Eos, doloribus, dolorem iusto blanditiis unde eius illum consequuntur neque dicta incidunt ullam ea hic porro optio ratione repellat perspiciatis.Enim, iure! Lorem ipsum dolor sit amet, consectetur adipiscing elit.Integer posuere erat a ante."
                },
                new Article
                {

                    IsActive = true,
                    Title = "This is a titel 3.",
                    Summary = "3- Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ducimus, vero, obcaecati, aut, error quam sapiente nemo saepe quibusdam sit excepturi nam quia corporis eligendi eos magni recusandae laborum minus inventore?",
                    Description = "3- Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.Lorem ipsum dolor sit amet, consectetur adipisicing elit.Eos, doloribus, dolorem iusto blanditiis unde eius illum consequuntur neque dicta incidunt ullam ea hic porro optio ratione repellat perspiciatis.Enim, iure! Lorem ipsum dolor sit amet, consectetur adipiscing elit.Integer posuere erat a ante."
                },
                new Article
                {

                    IsActive = true,
                    Title = "This is a titel 4.",
                    Summary = "4- Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ducimus, vero, obcaecati, aut, error quam sapiente nemo saepe quibusdam sit excepturi nam quia corporis eligendi eos magni recusandae laborum minus inventore?",
                    Description = "4- Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.Lorem ipsum dolor sit amet, consectetur adipisicing elit.Eos, doloribus, dolorem iusto blanditiis unde eius illum consequuntur neque dicta incidunt ullam ea hic porro optio ratione repellat perspiciatis.Enim, iure! Lorem ipsum dolor sit amet, consectetur adipiscing elit.Integer posuere erat a ante."
                },
                new Article
                {

                    IsActive = true,
                    Title = "This is a titel 5.",
                    Summary = "5- Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ducimus, vero, obcaecati, aut, error quam sapiente nemo saepe quibusdam sit excepturi nam quia corporis eligendi eos magni recusandae laborum minus inventore?",
                    Description = "5- Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.Lorem ipsum dolor sit amet, consectetur adipisicing elit.Eos, doloribus, dolorem iusto blanditiis unde eius illum consequuntur neque dicta incidunt ullam ea hic porro optio ratione repellat perspiciatis.Enim, iure! Lorem ipsum dolor sit amet, consectetur adipiscing elit.Integer posuere erat a ante."
                },
                new Article
                {

                    IsActive = true,
                    Title = "This is a titel 6.",
                    Summary = "6- Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ducimus, vero, obcaecati, aut, error quam sapiente nemo saepe quibusdam sit excepturi nam quia corporis eligendi eos magni recusandae laborum minus inventore?",
                    Description = "6- Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.Lorem ipsum dolor sit amet, consectetur adipisicing elit.Eos, doloribus, dolorem iusto blanditiis unde eius illum consequuntur neque dicta incidunt ullam ea hic porro optio ratione repellat perspiciatis.Enim, iure! Lorem ipsum dolor sit amet, consectetur adipiscing elit.Integer posuere erat a ante."
                },
                new Article
                {

                    IsActive = true,
                    Title = "This is a titel 7.",
                    Summary = "7- Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ducimus, vero, obcaecati, aut, error quam sapiente nemo saepe quibusdam sit excepturi nam quia corporis eligendi eos magni recusandae laborum minus inventore?",
                    Description = "7- Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.Lorem ipsum dolor sit amet, consectetur adipisicing elit.Eos, doloribus, dolorem iusto blanditiis unde eius illum consequuntur neque dicta incidunt ullam ea hic porro optio ratione repellat perspiciatis.Enim, iure! Lorem ipsum dolor sit amet, consectetur adipiscing elit.Integer posuere erat a ante."
                },
                new Article
                {

                    IsActive = true,
                    Title = "This is a titel 8.",
                    Summary = "8- Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ducimus, vero, obcaecati, aut, error quam sapiente nemo saepe quibusdam sit excepturi nam quia corporis eligendi eos magni recusandae laborum minus inventore?",
                    Description = "8- Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.Lorem ipsum dolor sit amet, consectetur adipisicing elit.Eos, doloribus, dolorem iusto blanditiis unde eius illum consequuntur neque dicta incidunt ullam ea hic porro optio ratione repellat perspiciatis.Enim, iure! Lorem ipsum dolor sit amet, consectetur adipiscing elit.Integer posuere erat a ante."
                },
                new Article
                {

                    IsActive = true,
                    Title = "This is a titel 9.",
                    Summary = "9- Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ducimus, vero, obcaecati, aut, error quam sapiente nemo saepe quibusdam sit excepturi nam quia corporis eligendi eos magni recusandae laborum minus inventore?",
                    Description = "9- Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.Lorem ipsum dolor sit amet, consectetur adipisicing elit.Eos, doloribus, dolorem iusto blanditiis unde eius illum consequuntur neque dicta incidunt ullam ea hic porro optio ratione repellat perspiciatis.Enim, iure! Lorem ipsum dolor sit amet, consectetur adipiscing elit.Integer posuere erat a ante."
                },
                new Article
                {

                    IsActive = true,
                    Title = "This is a titel 10.",
                    Summary = "10- Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ducimus, vero, obcaecati, aut, error quam sapiente nemo saepe quibusdam sit excepturi nam quia corporis eligendi eos magni recusandae laborum minus inventore?",
                    Description = "10- Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ut, tenetur natus doloremque laborum quos iste ipsum rerum obcaecati impedit odit illo dolorum ab tempora nihil dicta earum fugiat. Temporibus, voluptatibus.Lorem ipsum dolor sit amet, consectetur adipisicing elit.Eos, doloribus, dolorem iusto blanditiis unde eius illum consequuntur neque dicta incidunt ullam ea hic porro optio ratione repellat perspiciatis.Enim, iure! Lorem ipsum dolor sit amet, consectetur adipiscing elit.Integer posuere erat a ante."
                }
                );
            context.SaveChanges();

            context.ContactUs.Add(new ContactUs
            {

                FirstName = "Jahan",
                LastName = "Alem",
                Description = "10- Lorem ipsum dolor sit amet, consectetur adipisicing elit. Ducimus, vero, obcaecati, aut, error quam sapiente nemo saepe quibusdam sit excepturi nam quia corporis eligendi eos magni recusandae laborum minus inventore?",
                Email = "s.r.alem@gmail.com",
                Tel = "017621650345",
                Title = "Contact Us",
                IsActive = true
            });
            context.SaveChanges();
        }
    }
}
