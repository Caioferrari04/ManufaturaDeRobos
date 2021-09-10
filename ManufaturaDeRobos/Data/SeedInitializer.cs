using ManufaturaDeRobos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace ManufaturaDeRobos.Data
{
    public static class SeedInitializer
    {
        public static void Initializer(IHost app)
        {
            using (var scope = app.Services.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;
                var cxt = serviceProvider.GetRequiredService<ManufactoryContext>();
                Random rand = new Random();
                cxt.Database.Migrate();
                if (!cxt.Users.Any())
                {
                    cxt.Users.Add(new IdentityUser { 
                        UserName = "SYS_ADMIN",
                        PasswordHash = "sys_admin_21324512",
                    });
                }
                cxt.SaveChanges();
                var system = cxt.Users.FirstOrDefault(p => p.UserName == "SYS_ADMIN");
                if (!cxt.Robot.Any())
                {
                    int quantidadeIndices = 100;
                    string[] modelosFirstName = new string[] { "Destroyer", "Humanity", "Omnissiah", "Carrier", "Maid", "Butler", "Doll", "MainFrame", "Builder", "Viral", "Terminator", "Servitor", "Skynet", "Annihilator", "World", "Flyer", "Cybernetic", "Automaton", "Scrappie", "Bracer", "Tera", "Bionic", "Corius", "Experiment" };
                    string[] modelosLastName = new string[] { " of Humanity", " Skull", $" type.{rand.Next(0, 19999)}", "'s end", "'s destroyer", "'s Own", "'s Son", "'s Last", " the Great One", " the Little", " Analysis Drone", " Primitive Data Destruction", " Algorithm Entity", " Laboratorium Drone", " Neutralization Technician", " Emergency Repair Entity" };
                    for (int i = 0; i < quantidadeIndices; i++)
                    {
                        string lastName = rand.Next(0, 2) == 1 ? modelosLastName[rand.Next(0, modelosLastName.Length)] : "";
                        cxt.Robot.AddRange(
                            new Robot
                            {
                                Model = $"{modelosFirstName[rand.Next(0, modelosLastName.Length)]}{lastName}",
                                Price = rand.Next(200, 300000000),
                                Weight = rand.Next(1, 20000),
                                Sentience = rand.Next(0, 2) == 1 ? true : false,
                                WarRobot = rand.Next(0, 2) == 1 ? true : false,
                                Quantity = rand.Next(0, 1000),
                                CreatedOn = DateTime.Now,
                                UpdatedOn = DateTime.Now,
                                CreatedById = system.Id,
                                UpdatedById = system.Id,
                                CreatedBy = system,
                                UpdatedBy = system
                            }
                        );
                    }
                    cxt.SaveChanges();
                }
            }
        }
    }
}
