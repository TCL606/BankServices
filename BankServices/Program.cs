using System;
using System.Threading;
using System.IO;
using System.Text;
using System.Runtime.InteropServices;
using System.Reflection;
using CommandLine;

namespace BankServices
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Arguments? options = null;
            _ = Parser.Default.ParseArguments<Arguments>(args).WithParsed(o => { options = o; });
            if (options == null)
            {
                Console.WriteLine("Argument parsing failed!");
                return;
            }

            List<Customer> customerList = new List<Customer>();
            using (var sr = new StreamReader(options.FileName, Encoding.UTF8))
            {
                var customerInfo = sr.ReadLine();
                while (customerInfo is not null)
                {
                    var customerInfoStringList = customerInfo?.Split(' ');
                    if (customerInfoStringList is not null)
                    {
                        customerList.Add(new Customer(Convert.ToInt32(customerInfoStringList[0]), Convert.ToInt32(customerInfoStringList[1]), Convert.ToInt32(customerInfoStringList[2])));
                    }
                    customerInfo = sr.ReadLine();
                }
            }
            customerList.Sort();
            foreach (var customer in customerList)
            {
                Console.WriteLine($"{customer}, Arrive Time: {customer.ArriveTime}, Service Time: {customer.ServiceTime}");
            }
            Console.WriteLine("==========================================================================================");
            Console.WriteLine();

            var timeInterval = options.TimeInterval;
            var bank = new Bank(options.ServerNum, timeInterval);

            var sleepTime = customerList[0].ArriveTime;
            Thread.Sleep(sleepTime * timeInterval);
            bank.JoinCustomer(customerList[0]);
            for (int i = 1; i < customerList.Count; i++)
            {
                sleepTime = customerList[i].ArriveTime - customerList[i - 1].ArriveTime;
                Thread.Sleep(sleepTime * timeInterval);
                bank.JoinCustomer(customerList[i]);
            }
            var maxPossibleSleepTime = customerList.Select(x => x.ServiceTime).Sum() + customerList[0].ArriveTime - customerList[customerList.Count - 1].ArriveTime;
            Thread.Sleep(maxPossibleSleepTime * timeInterval + timeInterval);

            Console.WriteLine();
            Console.WriteLine("================================================================================");
            foreach (var customer in customerList)
            {
                Console.WriteLine($"{customer} , Arrive Time: {customer.ArriveTime}, Start Time: {customer.StartTime}, Leave Time: {customer.LeaveTime}, Server ID: {customer.ServerID}");
            }

        }
    }
}