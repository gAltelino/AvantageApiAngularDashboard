using System;
using System.Collections.Generic;
using Avantage.Api.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Avantage.Api
{
    public class Helpers
    {
        public static Random _rand = new Random();

        internal static string MakeCustomerName(List<Customer> customers)
        {
            var prefix = GetRandom(bizPrefix);
            var sufix = GetRandom(bizSuffix);
            var name = prefix + sufix;

            if (customers.Any(c => c.Name == name) && customers.Count < (bizPrefix.Count * bizSuffix.Count))
            {
                MakeCustomerName(customers);
            }

            return name;
        }

        internal static string MakeCustomerEmail(string ctmName)
        {
            return $"contact@{ctmName.ToLower()}.com";
        }

        internal static string GetRandomState()
        {
            return GetRandom(usStates);
        }

        internal static decimal GetRandomOrderTotal()
        {
            return _rand.Next(1000, 90000);
        }

        internal static DateTime GetRandomOrderPlaced()
        {
            var end = DateTime.Now;
            var start = DateTime.Now.AddDays(-90);

            var span = end - start;
            var newSpan = new TimeSpan(0, _rand.Next(0, (int)span.TotalMinutes), 0);

            return start + newSpan;
        }

        internal static DateTime? GetRandomOrderCompleted(DateTime orderPlaced)
        {
            var now = DateTime.Now;
            var minLeadTime = TimeSpan.FromDays(7);
            var timePassed = now - orderPlaced;

            if (timePassed < minLeadTime)
            {
                return null;
            }
            else
            {
                return orderPlaced.AddDays(_rand.Next(7, 14));
            }

        }

        private static readonly List<string> usStates = new List<string>()
        {
            "SP", "RJ", "MG", "ES", "RR", "AM"
        };

        private static string GetRandom(List<string> items)
        {

            return items[_rand.Next(items.Count)];
        }

        private static readonly List<string> bizPrefix = new List<string>()
        {
            "Bat",
            "Ranha",
            "What",
            "Static",
            "Bump"
        };

        private static readonly List<string> bizSuffix = new List<string>()
        {
            "Who",
            "Main",
            "Jun",
            "Stot",
            "Lil"
        };
    }
}