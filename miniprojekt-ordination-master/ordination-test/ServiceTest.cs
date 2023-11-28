namespace ordination_test;

using Microsoft.EntityFrameworkCore;
using Serilog;
using Service;
using Data;
using shared.Model;
using Microsoft.Extensions.Logging;

[TestClass]
public class ServiceTest
{
    private DataService service;
    private ILogger<DataService> testLogger; //Bruger test-specifik logger 

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

    [TestMethod]
    public void PatientsExist()
    {
        Assert.IsNotNull(service.GetPatienter());
    }


    [TestMethod]
    public void OpretDagligFast()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();
        double initialCount = service.GetDagligFaste().Count();

        testLogger.LogInformation("Antal dagligfaste ordinationer:" + initialCount);
        Assert.AreEqual(1, service.GetDagligFaste().Count());
      

        service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            2, 2, 1, 0, DateTime.Now, DateTime.Now.AddDays(3));

        double updatedCount = service.GetDagligFaste().Count();

        testLogger.LogInformation("Antal dagligfaste ordinationer:" + updatedCount);
        Assert.AreEqual(2, service.GetDagligFaste().Count());

        var addedDagligFast = service.GetDagligFaste().Last(); // Finder sidste tilføjede ordination for at eftertjekke data

        Assert.IsNotNull(addedDagligFast, "The added DagligFast should not be null");
        Assert.AreEqual(lm.LaegemiddelId, addedDagligFast.laegemiddel.LaegemiddelId, "LaegemiddelId burde matche");
        testLogger.LogInformation("Test finished");
        //Kunne med fordel have tjekket på patientId, men patientId er ikke tilgængelig parameter på ordination eller dagligfast

    }


    [TestMethod]
    public void OpretPNTest()
    {
        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        int initialCount = service.GetPNs().Count();

        service.OpretPN(patient.PatientId, lm.LaegemiddelId, 6, DateTime.Now, DateTime.Now.AddDays(3));

        int updatedCount = service.GetPNs().Count();
        Assert.AreEqual(initialCount + 1, updatedCount);

        var addedPN = service.GetPNs().Last(); //Finder sidste tilføjede pn ordination for at eftertjekke data 

        Assert.IsNotNull(addedPN, "Tilføjet PN burde ikke være nul");
        Assert.AreEqual(lm.LaegemiddelId, addedPN.laegemiddel.LaegemiddelId, "LaegemiddelId burde matche");
        //Kunne med fordel have tjekket på patientId, men patientId er ikke tilgængelig parameter på ordination eller dagligfast

    }


    [TestMethod]
    public void OpretDagligSkaevTest()
    {

        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();
        Dosis[] doser = new Dosis[3]
        {
            new Dosis { DosisId = 20, tid = DateTime.Parse("0001-01-01T06:30:00"), antal = 2 },
            new Dosis { DosisId = 21, tid = DateTime.Parse("0001-01-01T011:30:00"), antal = 4 },
            new Dosis { DosisId = 22, tid = DateTime.Parse("0001-01-01T017:30:00"), antal = 2 }
        };

        Assert.AreEqual(1, service.GetDagligSkæve().Count());

        service.OpretDagligSkaev(patient.PatientId, lm.LaegemiddelId, doser, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(2, service.GetDagligFaste().Count());

        var addedDagligSkæv = service.GetDagligSkæve().Last(); // Finder sidste tilføjede ordination for at eftertjekke data

        Assert.IsNotNull(addedDagligSkæv, "The added DagligFast should not be null");
        Assert.AreEqual(lm.LaegemiddelId, addedDagligSkæv.laegemiddel.LaegemiddelId, "LaegemiddelId burde matche");
        //Kunne med fordel have tjekket på patientId, men patientId er ikke tilgængelig parameter på ordination eller dagligfast

    }


    [TestMethod]
    public void GetAnbefaletDosisPerDøgnTest()
    {
        // Arrange
        var p1 = new Patient { PatientId = 10, vaegt = 20 };  //TC3
        var p2 = new Patient { PatientId = 11, vaegt = 24.9 }; //TC4
        var p3 = new Patient { PatientId = 12, vaegt = 25 }; //TC5 (grænseværdi)
        var p4 = new Patient { PatientId = 13, vaegt = 65 }; //TC6 
        var p5 = new Patient { PatientId = 14, vaegt = 119.9 }; // TC7
        var p6 = new Patient { PatientId = 15, vaegt = 120 }; //TC8 (grænseværdi)
        var p7 = new Patient { PatientId = 16, vaegt = 126 }; //TC9 


        var lmList = service.GetLaegemidler();
        var acetyl = lmList.FirstOrDefault(lm => lm.LaegemiddelId == 1);
        var paracet = lmList.FirstOrDefault(lm => lm.LaegemiddelId == 2);
        var fucidin = lmList.FirstOrDefault(lm => lm.LaegemiddelId == 3);
        var methot = lmList.FirstOrDefault(lm => lm.LaegemiddelId == 4);
        var prednis = lmList.FirstOrDefault(lm => lm.LaegemiddelId == 5);

        service.AddPatient(p1);
        service.AddPatient(p2);
        service.AddPatient(p3); 
        service.AddPatient(p4); 
        service.AddPatient(p5);
        service.AddPatient(p6);
        service.AddPatient(p7);


        // Act
        double TC3 = service.GetAnbefaletDosisPerDøgn(p1.PatientId, acetyl.LaegemiddelId);
        double TC4 = service.GetAnbefaletDosisPerDøgn(p2.PatientId, acetyl.LaegemiddelId);
        double TC5 = service.GetAnbefaletDosisPerDøgn(p3.PatientId, acetyl.LaegemiddelId);
        double TC6 = service.GetAnbefaletDosisPerDøgn(p4.PatientId, acetyl.LaegemiddelId);
        double TC7 = service.GetAnbefaletDosisPerDøgn(p5.PatientId, acetyl.LaegemiddelId);
        double TC8 = service.GetAnbefaletDosisPerDøgn(p6.PatientId, acetyl.LaegemiddelId);
        double TC9 = service.GetAnbefaletDosisPerDøgn(p7.PatientId, acetyl.LaegemiddelId);

        // Assert
        Assert.AreEqual(2, TC3);
        Assert.AreEqual(2.49, TC4);
        
    }






    //[TestMethod]
    //[ExpectedException(typeof(ArgumentNullException))]
    //public void TestAtKodenSmiderEnException()
    //{
    //    Patient testPatient1 = null;
    //    int x = null!; 
     
    //    var lmList = service.GetLaegemidler();
    //    var medicin = lmList.FirstOrDefault(lm => lm.LaegemiddelId == 1);

    //    // Act
    //    var result = service.OpretDagligFast(-1,  , 0, 0, 4, 2, DateTime.Now, DateTime.Now.AddDays(3));

    //    Console.WriteLine("Her kommer der ikke en exception. Testen fejler.");
    //}
}