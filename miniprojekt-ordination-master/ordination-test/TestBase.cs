using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ordination_test
{
    public class TestBase
    {
        public DataService service;
        public ILogger<DataService> testLogger; //Bruger test-specifik logger 

        [TestInitialize]
        public void SetupBeforeEachTest()
        {
            TestStartup.ConfigureLogging();  //anvender logger konfigurationer defineret i startupclass
            testLogger = new LoggerFactory().AddSerilog().CreateLogger<DataService>();//opretter et nyt instance af logger 

            var optionsBuilder = new DbContextOptionsBuilder<OrdinationContext>();
            optionsBuilder.UseInMemoryDatabase(databaseName: "test-database");
            var context = new OrdinationContext(optionsBuilder.Options);
            service = new DataService(context, testLogger);
            service.SeedData();
        }

        
    }
}
