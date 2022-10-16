using System;

namespace ClassLibrary1
{
    public class Class1
    {
        public Class1()
        {
            await using var client = new ServiceBusClient(connectionString);
        }
    }
}
