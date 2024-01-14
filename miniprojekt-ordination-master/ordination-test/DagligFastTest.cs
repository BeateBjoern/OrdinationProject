namespace ordination_test;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using shared.Model;

[TestClass]
public class DagligFastTest : TestBase
{

    //Test vi har lavet, med gyldige v�rdier 
    //Har tilh�rende Testcase specifikation
    [TestMethod]
    public void DoegnDosisTest()
    {

        testLogger.LogInformation("Test started at: " + DateTime.Now);

        //TC1 Test med 1 styk(t�t p� gr�nsev�rdi = 0 ) 
        DagligFast TC1 = new DagligFast(1,
            new DateTime(2023, 01, 01), new DateTime(2023, 12, 07),
            new Laegemiddel("Acetylsalicylsyre", 0.1, 0.15, 0.16, "Styk"),
            1, 0, 0, 0);

        double doegnDosisTC1 = TC1.doegnDosis();

        Assert.AreEqual(1, doegnDosisTC1);

        testLogger.LogInformation("TC1 Doegndosis resultat: " + doegnDosisTC1);

        //TC2 Med gyldig v�rdi 10 styk
        DagligFast TC2 = new DagligFast(2,
            new DateTime(2023, 01, 01), new DateTime(2024, 01, 01),
            new Laegemiddel("Acetylsalicylsyre", 0.1, 0.15, 0.16, "Styk"),
            2, 3, 4, 1);

        double doegnDosisTC2 = TC2.doegnDosis();

        Assert.AreEqual(10, doegnDosisTC2);

        testLogger.LogInformation("TC2 Doegndosis resultat: " + doegnDosisTC2);
        testLogger.LogInformation("Test finished at: " + DateTime.Now);

    }

    //Test vi har lavet 
    //Test med ugyldige v�rdier, test der burde fejle 
    [TestMethod]
    public void DoegnDosisTestNegativVærdi()
    {
        testLogger.LogInformation("Test started at: " + DateTime.Now);

        // TC3 Test med negativ v�rdi som input p� dosis
        DagligFast TC3 = new DagligFast(2,
            new DateTime(2023, 01, 01), new DateTime(2023, 12, 07),
            new Laegemiddel("Acetylsalicylsyre", 0.1, 0.15, 0.16, "Styk"),
            1, -1, 1, 1);


        Assert.ThrowsException<ArgumentOutOfRangeException>(() => TC3.doegnDosis());


        testLogger.LogInformation("Test finished at: " + DateTime.Now);
    }


    [TestMethod]
    public void DoegnDosisTestNullVærdi()
    {
        testLogger.LogInformation("Test started at: " + DateTime.Now);

        // TC4 Test med 0 som input p� alle dosis
        DagligFast TC4 = new DagligFast(1,
            new DateTime(2023, 01, 01), new DateTime(2024, 01, 01),
            new Laegemiddel("Acetylsalicylsyre", 0.1, 0.15, 0.16, "Styk"),
            0, 0, 0, 0);

        Assert.ThrowsException<ArgumentNullException>(() => TC4.doegnDosis());

        testLogger.LogInformation("Test finished at: " + DateTime.Now);
    }


}