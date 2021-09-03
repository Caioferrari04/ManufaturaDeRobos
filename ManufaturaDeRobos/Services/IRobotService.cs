using ManufaturaDeRobos.Models;
using System.Collections.Generic;

namespace ManufaturaDeRobos.Services
{
    public interface IRobotService
    {
        bool Create(Robot robot);

        List<Robot> GetAll();

        Robot Get(int? id);

        bool Update(Robot robot);

        bool Delete(int? id);

        public List<Robot> RobotsByUserRole(string getRole);
    }
}
