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
            // Configure logging directly within the test class
            var logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "Logs", "test-log.txt");



            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File(logFilePath, rollingInterval: RollingInterval.Day)
                .CreateLogger();

            testLogger = new LoggerFactory().AddSerilog().CreateLogger<DataService>(); // Create a new instance of the logger

            var optionsBuilder = new DbContextOptionsBuilder<OrdinationContext>();
            optionsBuilder.UseInMemoryDatabase(databaseName: "test-database"); // Use an in-memory database for testing
            var context = new OrdinationContext(optionsBuilder.Options);
            service = new DataService(context, testLogger);
            service.SeedData();
        }
    }
    //public class TestBase
    //{

    //    //
    //    public DataService service;
    //    public ILogger<DataService> testLogger; //Bruger test-specifik logger 

    //    [TestInitialize]
    //    public void SetupBeforeEachTest()
    //    {
    //        TestStartup.ConfigureLogging();  //anvender logger konfigurationer fra startupclass
    //        testLogger = new LoggerFactory().AddSerilog().CreateLogger<DataService>();//opretter et nyt instance af logger 

    //        var optionsBuilder = new DbContextOptionsBuilder<OrdinationContext>();
    //        optionsBuilder.UseInMemoryDatabase(databaseName: "test-database"); //Bruger test-database
    //        var context = new OrdinationContext(optionsBuilder.Options);
    //        service = new DataService(context, testLogger); //instantierer serviceklassen med DBkontekst og vores definerede serilog log
    //        service.SeedData();
    //    }


    //}
}
