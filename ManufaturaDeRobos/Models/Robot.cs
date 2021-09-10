using Microsoft.AspNetCore.Identity;
using System;
namespace ManufaturaDeRobos.Models
{
    public class Robot
    {
        
        public int Id { get; set; } //ID de fabricação do robô
        
        public string Model { get; set; } //Modelo do robô

        public double Price { get; set; }//Preço em decimal do robô

        public int Weight { get; set; } //Peso do robô

        public bool Sentience { get; set; } //Robô tem racionalidade? Emoções? Sim ou não.

        public bool WarRobot { get; set; } //O robô é equipado/feito para guerra?

        public int Quantity { get; set; } //Quantos robôs temos em estoque.

        public DateTime? CreatedOn { get; set; }

        public string CreatedById { get; set; }

        public IdentityUser CreatedBy { get; set; }

        public DateTime? UpdatedOn { get; set; }

        public string UpdatedById { get; set; }

        public IdentityUser UpdatedBy { get; set; }
    }
}
