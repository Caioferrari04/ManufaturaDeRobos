using ManufaturaDeRobos.Data;
using ManufaturaDeRobos.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ManufaturaDeRobos.Services
{
    public class RobotSqlService : IRobotService
    {
        ManufactoryContext context;
        public RobotSqlService(ManufactoryContext context)
        {
            this.context = context;
        }

        public bool Create(Robot robot)
        {
            try
            {
                robot.CreatedOn = DateTime.Now;
                context.Add(robot);
                context.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public Robot Get(int? id)
        {
            return context.Robot.FirstOrDefault(p => p.Id == id);
        }

        public List<Robot> GetAll()
        {
            return context.Robot.ToList();
        }

        public bool Update(Robot robot)
        {
            if (!context.Robot.Any(prod => prod.Id == robot.Id)) throw new Exception("Produto não existe!");
            try
            {
                robot.UpdatedOn = DateTime.Now;
                context.Update(robot);
                context.SaveChanges();
                return true;
            }
            catch { return false; }
        }
        
        public bool Delete(int? id)
        {
            if (!context.Robot.Any(prod => prod.Id == id)) throw new Exception("Produto não existe!");
            try {

                context.Remove(this.Get(id));
                context.SaveChanges();
                return true;
            }
            catch { return false; }
        }

        public List<Robot> RobotsByUserRole(string getRole)
        {
            try { 
                var query = from robot in context.Set<Robot>()
                              join user in context.Set<IdentityUser>()
                                on robot.CreatedById equals user.Id
                              join userRoles in context.Set<IdentityUserRole<string>>()
                                on user.Id equals userRoles.UserId
                              join role in context.Set<IdentityRole>()
                                on userRoles.RoleId equals role.Id
                              where role.Name.ToUpper() == getRole
                              select new Robot()
                              {
                                  Id = robot.Id,
                                  Model = robot.Model,
                                  Price = robot.Price,
                                  Weight = robot.Weight,
                                  WarRobot = robot.WarRobot,
                                  Sentience = robot.Sentience,
                                  CreatedOn = robot.CreatedOn,
                                  UpdatedOn = robot.UpdatedOn,
                                  CreatedBy = robot.CreatedBy,
                                  UpdatedBy = robot.UpdatedBy
                              };
                return query.ToList();
            } 
            catch
            {
                return null;
            }

        }
        public string GetUserIdFromName(string name)
        {
            return context.Users.FirstOrDefault(p => p.UserName == name).Id;
        }
    }
}
