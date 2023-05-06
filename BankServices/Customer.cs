using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BankServices
{
    public class Customer: IComparable<Customer>
    {
        public int CustomerID { get; }

        public int ServiceTime { get; }

        public int ArriveTime { get; }

        public int StartTime { get; set; }

        public int LeaveTime { get; set; }

        public int ServerID { get; set; }

        public Customer(int customerID, int arriveTime, int serviceTime)
        {
            CustomerID = customerID;
            ArriveTime = arriveTime;
            ServiceTime = serviceTime;
            StartTime = -1;
            LeaveTime = -1;
            ServerID = -1;
        }

        public int CompareTo(Customer? c)
        {
            if (c is null)
                return 1;
            else
            {
                return this.ArriveTime.CompareTo(c.ArriveTime);
            }
        }

        public override string ToString()
        {
            return $"Customer with ID {this.CustomerID}";
        }
    }
}
