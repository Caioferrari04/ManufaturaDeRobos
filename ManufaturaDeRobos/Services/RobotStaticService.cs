using ManufaturaDeRobos.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ManufaturaDeRobos.Services
{
    public class RobotStaticService : IRobotService
    {
        /*Aqui descansa um código desnecessariamente eficiente, que sonhou ter menos de 70 linhas, e realizou seu sonho.*/
        public List<Robot> All()
        {
            List<Robot> lista = new List<Robot>();
            for(int i = 0; i < 10; i++) lista.Add(new Robot() { Id = i+1, Model = $"Robô Skynet {i}", Price = (i+1)*10, Weight = (i+1)*5, Quantity = (i+1)*2, Sentience = false, WarRobot = false });
            return lista;
        }
        public virtual bool Create(Robot robot)
        {
            List<Robot> robots = All();
            robot.Id = robots.Max(p => p.Id) + 1;
            robots.Add(robots.Find(p => p.Model == robot.Model) == null ? robot : null);
            return robots.Find(p => p.Model == robot.Model) != null ? true : false;
        }
        public virtual Robot Get(int? id)
        {
            return All().Find(p => p.Id == id);
        }
        public virtual List<Robot> GetAll()
        {
            return All();
        }
        public virtual bool Update(Robot robot)
        {
            List<Robot> robots = All();
            Robot originalRobot = robots.Find(p => p.Model == robot.Model);
            robots.Remove(originalRobot != null ? originalRobot : null);
            robots.Add(originalRobot !=  null ? robot : null);
            return robots.Find(p => p.Model == robot.Model) != null ? true : false;
        }
        public virtual bool Delete(int? id)
        {
            List<Robot> robots = All();
            Robot originalRobot = robots.Find(p => p.Id == id);
            robots.Remove(originalRobot != null ? originalRobot : null);
            return robots.Find(p => p.Id == id) == null ? true : false;
        }

        public virtual List<Robot> RobotsByUserRole(string getRole)
        {
            throw new NotImplementedException();
        }
    }
}
