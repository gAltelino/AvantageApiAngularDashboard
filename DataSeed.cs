using System;
using System.Collections.Generic;
using System.Linq;
using Avantage.Api.Models;

namespace Avantage.Api
{
    public class DataSeed
    {
        private readonly ApiContext _ctx;

        public DataSeed(ApiContext ctx)
        {
            _ctx = ctx;
        }

        public void SeedData(int nCustomers, int nOrders)
        {
            if (!_ctx.Customers.Any())
            {
                SeedCustomers(nCustomers);
                _ctx.SaveChanges();
            }

            if (!_ctx.Orders.Any())
            {
                SeedOrders(nOrders);
                _ctx.SaveChanges();
            }

            if (!_ctx.Servers.Any())
            {
                SeedServers();
                _ctx.SaveChanges();
            }
        }

        private void SeedOrders(int n)
        {
            var orders = BuildOrderList(n);

            _ctx.Orders.AddRange(orders);
        }

        private void SeedCustomers(int n)
        {
            var customers = BuildCustomerList(n);
            _ctx.Customers.AddRange(customers);
        }

        private void SeedServers()
        {
            var servers = BuildServerList();
            _ctx.Servers.AddRange(servers);
        }

        private List<Server> BuildServerList()
        {

            var servers = new List<Server>(){
    new Server{
        Id = 1,
        Name = "Dev-Web",
        IsOnline = true
    },
    new Server{
        Id = 2,
        Name = "Dev-Mail",
        IsOnline = true
    },
    new Server{
        Id = 3,
        Name = "Dev-Services",
        IsOnline = true
    },
    new Server{
        Id = 4,
        Name = "QA-Web",
        IsOnline = true
    },
    new Server{
        Id = 5,
        Name = "QA-Mail",
        IsOnline = true
    },
    new Server{
        Id = 6,
        Name = "QA-Services",
        IsOnline = true
    },
     new Server{
        Id = 7,
        Name = "Prod-Web",
        IsOnline = true
    },
    new Server{
        Id = 8,
        Name = "Prod-Mail",
        IsOnline = true
    },
    new Server{
        Id = 9,
        Name = "Prod-Services",
        IsOnline = true
    }
};

            return servers;

        }

        private List<Order> BuildOrderList(int n)
        {
            var orders = new List<Order>();
            var rand = new Random();

            for (int i = 1; i < n; i++)
            {

                var randCustomerId = rand.Next(1, _ctx.Customers.Count()-1);
                var placed = Helpers.GetRandomOrderPlaced();
                var completed = Helpers.GetRandomOrderCompleted(placed);
                var customersList = _ctx.Customers.ToList();

                orders.Add(new Order
                {
                    Id = i,
                    Customer = customersList.First(c => c.Id == randCustomerId),
                    Total = Helpers.GetRandomOrderTotal(),
                    Placed = placed,
                    Completed = completed
                });
            }

            return orders;

        }

        private List<Customer> BuildCustomerList(int n)
        {
            var customers = new List<Customer>();

            for (int i = 1; i < n; i++)
            {
                var name = Helpers.MakeCustomerName(customers);
                var email = Helpers.MakeCustomerEmail(name);
                var state = Helpers.GetRandomState();

                customers.Add(new Customer
                {
                    Id = i,
                    Name = name,
                    Email = email,
                    State = state
                });
            }

            return customers;

        }

    }
}