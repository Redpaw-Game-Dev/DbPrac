using System;
using System.Globalization;
using System.Linq;
using static ConsoleApp1.Program;

namespace ConsoleApp1
{
    public class Flight : IEntity
    {
        public virtual int ID { get; set; }
        public virtual int DeparturePointID { get; set; }
        public virtual DateTime DepartureDateTime { get; set; }
        public virtual int DestinationPointID { get; set; }
        public virtual DateTime ArrivalDateTime { get; set; }
        public virtual int SoldTicketsCount { get; set; }
        public virtual int PlaneID { get; set; }

        public virtual void ReadShortly()
        {
            Console.WriteLine($"Flight #{ID}");
        }

        public virtual void Read()
        {
            Console.WriteLine($"Flight #{ID}\n" +
                $"From: {DeparturePoints.First(p => p.ID == DeparturePointID).Name}\n" +
                $"To: {DestinationPoints.First(p => p.ID == DestinationPointID).Name}\n" +
                $"Departure at: {DepartureDateTime}\n" +
                $"Arrival at: {ArrivalDateTime}\n" +
                $"Sold tickets: {SoldTicketsCount}\n" +
                $"Plane: {Planes.First(p => p.ID == PlaneID).Model}");
        }

        public virtual void Update()
        {
            for (int i = 0; i < DeparturePoints.Count; i++)
            {
                Console.Write($"{i} | ");
                DeparturePoints[i].ReadShortly();
            }
            int index = GetNumFromInput(0, DeparturePoints.Count - 1, "Choose new departure point:");
            DeparturePointID = DeparturePoints[index].ID;

            for (int i = 0; i < DestinationPoints.Count; i++)
            {
                Console.Write($"{i} | ");
                DestinationPoints[i].ReadShortly();
            }
            index = GetNumFromInput(0, DestinationPoints.Count - 1, "Choose new destination point:");
            DestinationPointID = DestinationPoints[index].ID;

            Console.Write($"Enter departure date and time {DateTimeExample}): ");
            while (!TryParseDateTime(Console.ReadLine(), out DateTime result))
            {
                DepartureDateTime = result;
            }

            Console.Write($"Enter arrival date and time {DateTimeExample}): ");
            while (!TryParseDateTime(Console.ReadLine(), out DateTime result))
            {
                ArrivalDateTime = result;
            }

            Console.Write("Enter sold tickets count: ");
            if (int.TryParse(Console.ReadLine(), out int num)) SoldTicketsCount = num;
            else SoldTicketsCount = 0;

            for (int i = 0; i < Planes.Count; i++)
            {
                Console.Write($"{i} | ");
                Planes[i].ReadShortly();
            }
            index = GetNumFromInput(0, Planes.Count - 1, "Choose new plane:");
            PlaneID = Planes[index].ID;
        }

        protected virtual bool TryParseDateTime(string input, out DateTime result)
        {
            return DateTime.TryParseExact(
                input,
                DateTimeFormat,
                CultureInfo.InvariantCulture,
                DateTimeStyles.None,
                out result
            );
        }
    }
}
