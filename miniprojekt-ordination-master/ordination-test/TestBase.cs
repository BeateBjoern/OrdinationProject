using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
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
        public ILogger<DataService> testLogger; // Test-specific logger 

        [TestInitialize]
        public void SetupBeforeEachTest()
        {
            
            var logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Logs", "test-log.txt");


            //Logger konfiguration 
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information) //laveste logging niveau
                .Enrich.FromLogContext()
                .WriteTo.Console() //Udskriv til consol 
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day) //gem i .txtfil 
                .CreateLogger();

            testLogger = new LoggerFactory().AddSerilog().CreateLogger<DataService>(); // opretter instans af log

            var optionsBuilder = new DbContextOptionsBuilder<OrdinationContext>(); //angiver db kontekst  med ordinationcontext
            optionsBuilder.UseInMemoryDatabase(databaseName: "test-database"); // Bruger in-memory DB for testing kontekst
            var context = new OrdinationContext(optionsBuilder.Options); //instantierer ordinationkontekst
            service = new DataService(context, testLogger); //instantierer service med kontekst og logger
            service.SeedData();
        }
    }

}
