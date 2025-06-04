using NHibernate;
using NHibernate.Cfg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ConsoleApp1
{

    internal class Program
    {
        public static List<DeparturePoint> DeparturePoints = new List<DeparturePoint>();
        public static List<DestinationPoint> DestinationPoints = new List<DestinationPoint>();
        public static List<Plane> Planes = new List<Plane>();
        public static List<Flight> Flights = new List<Flight>();
        public static string DateTimeExample = "13/09/2025 19:30";
        public static string DateTimeFormat = "dd/MM/yyyy HH:mm";

        private const string RequestEntityIndexText = "\nEnter entity index:";
        private const string RequestActionIndexText = "\nEnter action index:";
        private const string RequestRowIndexText = "\nEnter row index:";

        private static ISessionFactory _sessionFactory;

        static void Main(string[] args)
        {
            var cfg = new Configuration();
            cfg.Configure();
            _sessionFactory = cfg.BuildSessionFactory();
            Type[] types = Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => typeof(IEntity).IsAssignableFrom(t) && t.IsClass && !t.IsAbstract).ToArray();
            List<IEntity> rows = new List<IEntity>();
            bool isExitRequested = false;
            while (!isExitRequested)
            {
                DeparturePoints = GetAllRows<DeparturePoint>();
                DestinationPoints = GetAllRows<DestinationPoint>();
                Planes = GetAllRows<Plane>();
                Flights = GetAllRows<Flight>();

                Console.WriteLine("\nEntities:");
                for (int i = 0; i < types.Length; i++)
                {
                    Console.WriteLine($"{i}: {types[i].Name}");
                }
                int entityIndex = GetNumFromInput(0, types.Length - 1, RequestEntityIndexText);
                rows = GetAllRows(types[entityIndex]);

                string[] actionNames = Enum.GetNames(typeof(ActionType));
                for (int i = 0; i < actionNames.Length; i++)
                {
                    Console.WriteLine($"{i}: {actionNames[i]}");
                }
                ActionType action = ActionType.Read;
                action = (ActionType)GetNumFromInput(0, actionNames.Length - 1, RequestActionIndexText);

                switch (action)
                {
                    case ActionType.Exit:
                        isExitRequested = true;
                        break;
                    case ActionType.Create:
                        Create(CreateEntity(types[entityIndex]));
                        break;
                    case ActionType.Read:
                        ReadAllShortly(rows);
                        int rowIndex = GetNumFromInput(0, rows.Count - 1, RequestRowIndexText);
                        rows[rowIndex].Read();
                        break;
                    case ActionType.Update:
                        ReadAllShortly(rows);
                        rowIndex = GetNumFromInput(0, rows.Count - 1, RequestRowIndexText);
                        Update(rows[rowIndex]);
                        break;
                    case ActionType.Delete:
                        ReadAllShortly(rows);
                        rowIndex = GetNumFromInput(0, rows.Count - 1, RequestRowIndexText);
                        Delete(rows[rowIndex]);
                        break;
                    case ActionType.ReadAllShortly:
                        ReadAllShortly(rows);
                        break;
                }
                Console.WriteLine("___________________________________________________");
            }
        }

        public static int GetNumFromInput(int minNum, int maxNum, string requestText)
        {
            Console.WriteLine(requestText);
            int index = -1;
            do
            {
                Console.Write($"Your choice ({minNum} - {maxNum}): ");
                bool isSuccesed = int.TryParse(Console.ReadLine(), out index);
                if (!isSuccesed) index = -1;
            } while (index < minNum || index > maxNum);
            Console.WriteLine();
            return index;
        }

        static List<IEntity> GetAllRows(Type type)
        {
            using (ISession session = _sessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    List<IEntity> res = new List<IEntity>();
                    session.CreateCriteria(type).List(res);
                    transaction.Commit();
                    return res;
                }
            }
        }

        static List<T> GetAllRows<T>() where T : IEntity
        {
            using (ISession session = _sessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    List<T> res = new List<T>();
                    session.CreateCriteria(typeof(T)).List(res);
                    transaction.Commit();
                    return res;
                }
            }
        }

        static IEntity CreateEntity(Type entityType)
        {
            IEntity entity = Activator.CreateInstance(entityType) as IEntity;
            entity.Update();
            return entity;
        }

        static void Create(IEntity newEntity)
        {
            using (ISession session = _sessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Save(newEntity);
                    transaction.Commit();
                }
            }
        }

        static void ReadAllShortly(List<IEntity> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                Console.Write($"{i} | ");
                list[i].ReadShortly();
            }
        }

        static void Update(IEntity entity)
        {
            using (ISession session = _sessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    entity.Update();
                    session.Update(entity);
                    transaction.Commit();
                }
            }
        }

        static void Delete(IEntity entity)
        {
            using (ISession session = _sessionFactory.OpenSession())
            {
                using (ITransaction transaction = session.BeginTransaction())
                {
                    session.Delete(entity);
                    transaction.Commit();
                }
            }
        }
    }
}
