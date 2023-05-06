using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace BankServices
{
    public class Bank
    {
        private Queue<Customer> customersQueue = new();
        private object customersQueueLock = new();

        private int timeInterval;
        private int initTime;

        private Semaphore attendantSema;
        private Dictionary<int, bool> availServers;
        private object serversLock = new();

        public int ServersNum => availServers.Count;

        public void JoinCustomer(Customer customer)
        {
            lock (this.customersQueueLock)
            {
                customersQueue.Enqueue(customer);
            }
            Console.WriteLine($"{customer} arrives at time {(Environment.TickCount - this.initTime) / this.timeInterval}");
            new Thread(() => TryServeCustomer()).Start();
        }

        public void TryServeCustomer()
        {
            this.attendantSema.WaitOne();
            Customer customer;
            lock (this.customersQueueLock)
            {
                customer = this.customersQueue.Dequeue();
            }
            lock (this.serversLock)
            {
                var server = this.availServers.First(kvp => !kvp.Value);
                this.availServers[server.Key] = true;
                customer.ServerID = server.Key;
            }
            customer.StartTime = (Environment.TickCount - this.initTime) / this.timeInterval;
            new Thread(() =>
            {
                Console.WriteLine($"{customer} starts at time {customer.StartTime}. Being served at ServerID {customer.ServerID} for {customer.ServiceTime} time");
                Thread.Sleep(customer.ServiceTime * this.timeInterval);
                customer.LeaveTime = customer.StartTime + customer.ServiceTime;
                Console.WriteLine($"End serving {customer} at time {customer.LeaveTime}");
                lock (this.serversLock)
                {
                    this.availServers[customer.ServerID] = false;
                }
                this.attendantSema.Release();
            }).Start();
        }

        public Bank(int attendantNum, int timeInterval)
        {
            this.attendantSema = new(attendantNum, attendantNum);
            this.availServers = new();
            for (int i = 0; i < attendantNum; i++)
                this.availServers.Add(i, false);
            this.initTime = Environment.TickCount;
            this.timeInterval = timeInterval;
        }
    }
}
