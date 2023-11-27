namespace ordination_test;

using Microsoft.EntityFrameworkCore;

using Service;
using Data;
using shared.Model;

[TestClass]
public class ServiceTest
{
    private DataService service;

    [TestInitialize]
    public void SetupBeforeEachTest()
    {
        var optionsBuilder = new DbContextOptionsBuilder<OrdinationContext>();
        optionsBuilder.UseInMemoryDatabase(databaseName: "test-database");
        var context = new OrdinationContext(optionsBuilder.Options);
        service = new DataService(context);
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

        Assert.AreEqual(1, service.GetDagligFaste().Count());

        service.OpretDagligFast(patient.PatientId, lm.LaegemiddelId,
            2, 2, 1, 0, DateTime.Now, DateTime.Now.AddDays(3));

        Assert.AreEqual(2, service.GetDagligFaste().Count());

        var addedDagligFast = service.GetDagligFaste().Last(); // Assuming a method like Last() to get the last added item

        Assert.IsNotNull(addedDagligFast, "The added DagligFast should not be null");
        Assert.AreEqual(lm.LaegemiddelId, addedDagligFast.laegemiddel.LaegemiddelId, "LaegemiddelId burde matche");

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

        var addedPN = service.GetPNs().Last(); // Assuming a method like Last() to get the last added item

        Assert.IsNotNull(addedPN, "Tilføjet PN burde ikke være nul");
        Assert.AreEqual(lm.LaegemiddelId, addedPN.laegemiddel.LaegemiddelId, "LaegemiddelId burde matche");
        //Would have asserter for patient Id with different model, but patientid is not a property of neither of the ordination type classes nor ordination

    }


    [TestMethod]
    public void OpretDagligSkaevTest()
    {

    }


    [TestMethod]
    public void GetAnbefaletDosisPerDøgnTest()
    {
        // Arrange
        var patient = new Patient { PatientId = 123, vaegt = 20 }; // Normal weight
        var medication = new Laegemiddel("Acetylsalicylsyre", 0.1, 0.15, 0.16, "Styk");


        service.AddPatient(patient);
        service.AddLaegemiddel(medication);

        // Act
        double TC1 = service.GetAnbefaletDosisPerDøgn(patient.PatientId, medication.LaegemiddelId);

        // Assert
        Assert.AreEqual(2, TC1);
    }






    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void TestAtKodenSmiderEnException()
    {
        // Herunder skal man så kalde noget kode,
        // der smider en exception.

        // Hvis koden _ikke_ smider en exception,
        // så fejler testen.

        Console.WriteLine("Her kommer der ikke en exception. Testen fejler.");
    }
}