using System;
using static ConsoleApp1.Program;

namespace ConsoleApp1
{
    public class Ticket : IEntity
    {
        public virtual int ID { get; set; }
        public virtual int SeatNumber { get; set; }
        public virtual float Price { get; set; }
        public virtual int FlightID { get; set; }

        public virtual void ReadShortly()
        {
            Console.WriteLine($"Ticket #{ID}");
        }

        public virtual void Read()
        {
            Console.WriteLine($"Ticket #{ID}\n" +
                $"Seat number: {SeatNumber}\n" +
                $"Price: {Price.ToString("0.00")}$\n" +
                $"Flight ID: {FlightID}");
        }

        public virtual void Update()
        {
            Console.Write("Enter seat number: ");
            if (int.TryParse(Console.ReadLine(), out int num)) SeatNumber = num;
            else SeatNumber = 0;

            Console.Write("Enter price: ");
            if (float.TryParse(Console.ReadLine(), out float price)) Price = price;
            else Price = 0f;

            for (int i = 0; i < Flights.Count; i++)
            {
                Console.Write($"{i} | ");
                Flights[i].ReadShortly();
            }
            int index = GetNumFromInput(0, Flights.Count - 1, "Choose new flight:");
            FlightID = Flights[index].ID;
        }
    }
}
