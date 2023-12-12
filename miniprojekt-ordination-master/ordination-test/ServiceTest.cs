namespace ordination_test;

using Microsoft.EntityFrameworkCore;
using Serilog;
using Service;
using Data;
using shared.Model;
using Microsoft.Extensions.Logging;
using shared;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

[TestClass]
public class ServiceTest : TestBase
{

    [TestMethod]
    public void PatientsExist()
    {
        Assert.IsNotNull(service.GetPatienter());
    }

    //Test vi har lavet 
    //Test med gyldige værdier (har test tabel)
    [TestMethod]
    public void OpretDagligFast()
    {
        testLogger.LogInformation($"Test started at: {DateTime.Now}");

        Patient patient1 = service.GetPatienter().First();
        Laegemiddel lm1 = service.GetLaegemidler().First(); 

        //TC1, test
        testLogger.LogInformation("Antal ordinatoer før oprettelse:" + service.GetDagligFaste().Count());

        //Tjekker antal før oprettelse 
        int initialCount = service.GetDagligFaste().Count(); 
        
        service.OpretDagligFast(patient1.PatientId, lm1.LaegemiddelId,2, 2, 1, 0, DateTime.Now, DateTime.Now.AddDays(3));

        //Sammenligner efter 
        Assert.AreEqual(initialCount + 1 , service.GetDagligFaste().Count());


        testLogger.LogInformation("Antal ordinationer efter oprettelse:" + service.GetDagligFaste().Count());

        //Henter nyopretted ordination
        var addedDagligFast = service.GetDagligFaste().Last(); 

        //Tjekker for nul, samt om lægemiddel matcher 
        Assert.IsNotNull(addedDagligFast, "The added DagligFast should not be null");
        Assert.AreEqual(lm1.LaegemiddelId, addedDagligFast.laegemiddel.LaegemiddelId, "LaegemiddelId burde matche");

        testLogger.LogInformation($"Test finished at: {DateTime.Now}");

    }  

    //Test vi har lavet 
    //Test med gyldige værdier og valid input 
    [TestMethod]
    public void OpretPNTest()
    {
        testLogger.LogInformation($"Test started at: {DateTime.Now}");

        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        //Antal ordinationer før oprettelse 
        int initialCount = service.GetPNs().Count();

        service.OpretPN(patient.PatientId, lm.LaegemiddelId, 6, DateTime.Now, DateTime.Now.AddDays(3));

        //Antal ordinationer efter 
        int updatedCount = service.GetPNs().Count();


        // Sammenligner forventet resultat med faktisk resultat 
        Assert.AreEqual(initialCount + 1, updatedCount);
        var addedPN = service.GetPNs().Last(); 
        Assert.IsNotNull(addedPN, "Tilføjet PN burde ikke være nul");
        Assert.AreEqual(lm.LaegemiddelId, addedPN.laegemiddel.LaegemiddelId, "LaegemiddelId burde matche");

        testLogger.LogInformation($"Test finished at: {DateTime.Now}");

    }

    //Test vi har lavet 
    //Test med gyldige værdier og valid input 
    [TestMethod]
    public void OpretDagligSkaevTest()
    {

        testLogger.LogInformation($"Test started at: {DateTime.Now}");

        Patient patient = service.GetPatienter().First();
        Laegemiddel lm = service.GetLaegemidler().First();

        testLogger.LogInformation("Patient og lægemiddel: " + patient.PatientId + ", " +  lm.LaegemiddelId);

        Assert.AreEqual(1, service.GetDagligSkæve().Count());
        testLogger.LogInformation("Antal ordinationer før oprettelse:" + service.GetDagligSkæve().Count());

        //Ppretter en ny dagligSkæv med 7 dages periode 
        service.OpretDagligSkaev(patient.PatientId, lm.LaegemiddelId,
            new Dosis[]  {
                new Dosis(Util.CreateTimeOnly(12, 0, 0), 0.5),
                new Dosis(Util.CreateTimeOnly(12, 40, 0), 1),
                new Dosis(Util.CreateTimeOnly(16, 0, 0), 2.5),
                new Dosis(Util.CreateTimeOnly(18, 45, 0), 3)

            }, new DateTime(2023, 01, 01), new DateTime(2023, 01, 08));


        var addedDagligSkæv = service.GetDagligSkæve().Last(); 

        testLogger.LogInformation("Antal ordinationer efter oprettelse:" + service.GetDagligSkæve().Count());

        Assert.AreEqual(2, service.GetDagligSkæve().Count());
        Assert.IsNotNull(addedDagligSkæv, "The added DagligFast should not be null");
        Assert.AreEqual(lm.LaegemiddelId, addedDagligSkæv.laegemiddel.LaegemiddelId, "LaegemiddelId burde matche");

        testLogger.LogInformation($"Test finished at: {DateTime.Now}");


    }



    //Test vi har lavet
    [TestMethod]
    public void GetAnbefaletDosisPerDøgnTest()
    {
        testLogger.LogInformation($"Test started at: {DateTime.Now}");
        //TC1-TC7
        // Opretter nye patienter med vægt værdier vi skal anvende i testen
        var p1 = new Patient { PatientId = 10, vaegt = 20 };  //TC1
        var p2 = new Patient { PatientId = 11, vaegt = 24.9 }; //TC2
        var p3 = new Patient { PatientId = 12, vaegt = 25 }; //TC3 (grænseværdi)
        var p4 = new Patient { PatientId = 13, vaegt = 65 }; //TC4 
        var p5 = new Patient { PatientId = 14, vaegt = 120 }; // TC5 (grænseværdi)
        var p6 = new Patient { PatientId = 15, vaegt = 120.1 }; //TC6 
        var p7 = new Patient { PatientId = 16, vaegt = 126 }; //TC7 


        var lmList = service.GetLaegemidler();
        var acetyl = lmList.FirstOrDefault(lm => lm.LaegemiddelId == 1);
        //var paracet = lmList.FirstOrDefault(lm => lm.LaegemiddelId == 2);
        //var fucidin = lmList.FirstOrDefault(lm => lm.LaegemiddelId == 3);
        //var methot = lmList.FirstOrDefault(lm => lm.LaegemiddelId == 4);
        //var prednis = lmList.FirstOrDefault(lm => lm.LaegemiddelId == 5);

        //tilføjer nye patienter med relevant vægt
        service.AddPatient(p1);
        service.AddPatient(p2);
        service.AddPatient(p3); 
        service.AddPatient(p4); 
        service.AddPatient(p5);
        service.AddPatient(p6);
        service.AddPatient(p7);


        var p6vaegt = service.GetPatienter().FirstOrDefault(p => p.PatientId == 15);
        testLogger.LogInformation($"Patient6 vægt: {p6vaegt.vaegt.ToString()}");
        testLogger.LogInformation($"Præparat og  enhedsfaktor:{acetyl.navn}, {acetyl.enhedPrKgPrDoegnTung}");
       


        // Henter anbefalet dosis
        double TC1 = service.GetAnbefaletDosisPerDøgn(p1.PatientId, acetyl.LaegemiddelId);
        double TC2 = service.GetAnbefaletDosisPerDøgn(p2.PatientId, acetyl.LaegemiddelId);
        double TC3 = service.GetAnbefaletDosisPerDøgn(p3.PatientId, acetyl.LaegemiddelId);
        double TC4 = service.GetAnbefaletDosisPerDøgn(p4.PatientId, acetyl.LaegemiddelId);
        double TC5 = service.GetAnbefaletDosisPerDøgn(p5.PatientId, acetyl.LaegemiddelId);
        double TC6 = service.GetAnbefaletDosisPerDøgn(p6.PatientId, acetyl.LaegemiddelId);
        double TC7 = service.GetAnbefaletDosisPerDøgn(p7.PatientId, acetyl.LaegemiddelId);


        testLogger.LogInformation($"TC6 anbefalet dosis value : {TC6}");


        // Sammenligner forventet resultat med faktisk resultat 
        Assert.AreEqual(2, TC1);
        Assert.AreEqual(2.49, TC2);
        Assert.AreEqual(3.75, TC3);
        Assert.AreEqual(9.75, TC4);
        Assert.AreEqual(18, TC5);
        Assert.AreEqual(19.216, TC6);
        Assert.AreEqual(20.16, TC7);



        testLogger.LogInformation($"Test finished at: {DateTime.Now}");
    }


    //Test vi har lavet 
    //Metode med ugyldig værdi (negativt tal og null ) 
    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void GetAnbefaletDosisPerDøgnTestMedNullVærdi()
    {
        testLogger.LogInformation($"Test started at: {DateTime.Now}");

        var p2 = new Patient { PatientId = 26, vaegt = 0 }; //TC9

        var lmList = service.GetLaegemidler();
        var acetyl = lmList.FirstOrDefault(lm => lm.LaegemiddelId == 1);

        service.AddPatient(p2);


        double TC9 = service.GetAnbefaletDosisPerDøgn(p2.PatientId, acetyl.LaegemiddelId);

        Assert.AreEqual(0, TC9);

        testLogger.LogInformation($"Test finished at: {DateTime.Now}");

    }


    //Test vi har lavet 
    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void GetAnbefaletDosisPerDøgnTestMedNegativVærdi()
    {
        testLogger.LogInformation($"Test started at: {DateTime.Now}");

        //TC8, med ugyldige værdi og nullværdi
        var p1 = new Patient { PatientId = 25, vaegt = -20 };  //TC8


        var lmList = service.GetLaegemidler();
        var acetyl = lmList.FirstOrDefault(lm => lm.LaegemiddelId == 1);

        service.AddPatient(p1);


        double TC8 = service.GetAnbefaletDosisPerDøgn(p1.PatientId, acetyl.LaegemiddelId);

        Assert.AreEqual(-2, TC8);

        testLogger.LogInformation($"Test finished at: {DateTime.Now}");

    }






    [TestMethod]
    public void TestAtKodenSmiderEnException()
    {
        testLogger.LogInformation($"Test started at: {DateTime.Now}");

        Patient testPatient1 = null;

        var lmList = service.GetLaegemidler();
        var medicin = lmList.FirstOrDefault(lm => lm.LaegemiddelId == 1);

        Assert.ThrowsException<ArgumentNullException>(() =>
        {
            // Metoden der forventes at smide en exception 
            var result = service.OpretDagligFast(1, -1, 0, 0, 4, 2, DateTime.Now, DateTime.Now.AddDays(3));
  
        });

        testLogger.LogInformation($"Test finished at: {DateTime.Now}");
    }
}