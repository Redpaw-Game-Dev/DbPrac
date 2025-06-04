using System;

namespace ConsoleApp1
{
    public class Plane : IEntity
    {
        public virtual int ID { get; set; }
        public virtual string Model { get; set; }
        public virtual int SeatsCount { get; set; }

        public virtual void ReadShortly()
        {
            Console.WriteLine($"Model: {Model}");
        }

        public virtual void Read()
        {
            Console.WriteLine($"ID: {ID} | Model: {Model} | Seats count: {SeatsCount}");
        }

        public virtual void Update()
        {
            Console.Write("Enter new model name: ");
            Model = Console.ReadLine();
            Console.Write("Enter new seats count: ");
            if (int.TryParse(Console.ReadLine(), out int num)) SeatsCount = num;
            else SeatsCount = 0;
        }
    }
}
