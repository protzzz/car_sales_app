using System.Globalization;
using System.Xml.Linq;

namespace CarSalesApp
{
    public class Car
    {
        public string Model { get; set; }
        public DateTime Date { get; set; }
        public double Price { get; set; }
        public double Dph { get; set; }
    }

    public class WeekendSales
    {
        public string Model { get; set; }
        public double TotalWithoutDPH { get; set; }
        public double TotalWithDPH { get; set; }
    }

    public class CarSalesService
    {
        public List<Car> loadXMLWithPath(string path)
        {
            XElement xElement = XElement.Load(path);

            var cars = xElement.Elements("car")
                .Select(x => new Car
                {
                    Model = (string)x.Element("model")!,
                    Date = DateTime.ParseExact((string)x.Element("date")!, "d.MM.yyyy", CultureInfo.InvariantCulture),
                    Price = (double)x.Element("price")!,
                    Dph = (double)x.Element("dph")!
                }).ToList();

            return cars;
        }

        private List<WeekendSales> processWeekendSales(List<Car> cars)
        {
            var weekendSales = cars
                .Where(car => car.Date.DayOfWeek == DayOfWeek.Saturday || car.Date.DayOfWeek == DayOfWeek.Sunday);

            var groupedByModel = weekendSales
                .GroupBy(car => car.Model)
                .Select(group => new WeekendSales
                {
                    Model = group.Key,
                    TotalWithoutDPH = group.Sum(car => car.Price),
                    TotalWithDPH = group.Sum(car => car.Price * (1 + car.Dph / 100))
                })
                .OrderBy(car => car.Model)
                .ToList();

            return groupedByModel;
        }

        public List<WeekendSales> getWeekendSales(string path)
        {
            var cars = loadXMLWithPath(path);
            return processWeekendSales(cars);
        }
    }

    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            Application.Run(new Form1());
        }
    }
}