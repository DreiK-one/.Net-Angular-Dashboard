using Data.Entities;


namespace Data.DataCreation
{
    public class DataSeed
    {
        private readonly ApiContext _context;

        public DataSeed(ApiContext context)
        {
            _context = context;
        }

        public void SeedData(int nCustomers, int nOrders)
        {
            if (!_context.Customers.Any())
            {
                SeedCustomers(nCustomers);
                _context.SaveChanges();
            }

            if (!_context.Orders.Any())
            {
                SeedOrders(nOrders);
                _context.SaveChanges();
            }

            if (!_context.Servers.Any())
            {
                SeedServers();
                _context.SaveChanges();
            }
        }

        private void SeedCustomers(int number)
        {
            var customers = BuildCustomersList(number);

            foreach (var customer in customers)
            {
                _context.Customers.Add(customer);
            }
        }

        private void SeedOrders(int number)
        {
            var orders = BuildOrderList(number);

            foreach (var order in orders)
            {
                _context.Orders.Add(order);
            }
        }

        private void SeedServers()
        {
            List<Server> servers = BuildServerList();

            foreach (var server in servers)
            {
                _context.Servers.Add(server);
            }
        }

        private List<Customer> BuildCustomersList(int nCustomers)
        {
            var customers = new List<Customer>();
            var names = new List<string>();

            for (var i = 1; i <= nCustomers; i++)
            {
                var name = Helpers.MakeUniqueCustomerName(names);
                names.Add(name);

                customers.Add(new Customer
                {
                    Name = name,
                    Email = Helpers.MakeCustomerEmail(name),
                    State = Helpers.GetRandomState()
                });
            }

            return customers;
        }

        private List<Order> BuildOrderList(int nOrders)
        {
            var orders = new List<Order>();
            var rand = new Random();

            for (var i = 1; i <= nOrders; i++)
            {
                var randCustomerId = rand.Next(1, _context.Customers.Count());
                var placed = Helpers.GetRandomOrderPlaced();
                var completed = Helpers.GetRandomOrderCompleted(placed);
                var customers = _context.Customers.ToList();

                orders.Add(new Order
                {
                    Customer = customers.First(c => c.Id == randCustomerId),
                    Total = Helpers.GetRandomOrderTotal(),
                    Placed = placed,
                    Completed = completed
                });
            }

            return orders;
        }

        private List<Server> BuildServerList()
        {
            return new List<Server>()
            {
                new Server
                {
                    Name = "Dev-Web",
                    IsOnline = true
                },

                new Server
                {
                    Name = "Dev-Analysis",
                    IsOnline = true
                },

                new Server
                {
                    Name = "Dev-Mail",
                    IsOnline = true
                },

                new Server
                {
                    Name = "QA-Web",
                    IsOnline = true
                },

                new Server
                {
                    Name = "QA-Analysis",
                    IsOnline = true
                },

                new Server
                {
                    Name = "QA-Mail",
                    IsOnline = true
                },

                new Server
                {
                    Name = "Prod-Web",
                    IsOnline = true
                },

                new Server
                {
                    Name = "Prod-Analysis",
                    IsOnline = true
                },

                new Server
                {
                    Name = "Prod-Mail",
                    IsOnline = true
                },
                new Server
                {
                    Name = "Common-Support",
                    IsOnline = true
                }
            };

        }
    }
}
