using System;

namespace ConsoleApp1
{
    public abstract class Point : IEntity
    {
        public virtual int ID { get; set; }
        public virtual string Name { get; set; }

        public virtual void ReadShortly()
        {
            Read();
        }

        public virtual void Read()
        {
            Console.WriteLine($"ID: {ID} Name: {Name}");
        }

        public virtual void Update()
        {
            Console.Write("Enter new name: ");
            Name = Console.ReadLine();
        }
    }
}
