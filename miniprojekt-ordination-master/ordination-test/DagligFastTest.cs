namespace ordination_test;
using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Client;
using shared.Model;

[TestClass]
public class DagligFastTest : TestBase
{

    //Test med gyldige værdier 
    [TestMethod]
    public void DoegnDosisTest()
    {

        testLogger.LogInformation("Test started at: " + DateTime.Now);
        //TC1 Test med 1 styk(tæt på grænseværdi = 0 ) 
        DagligFast TC1 = new DagligFast(
            new DateTime(2023, 01, 01), new DateTime(2023, 12, 07),
            new Laegemiddel("Acetylsalicylsyre", 0.1, 0.15, 0.16, "Styk"),
            1, 0, 0, 0);

        double doegnDosisTC1 = TC1.doegnDosis();

        Assert.AreEqual(1, doegnDosisTC1);

        testLogger.LogInformation("TC1 Doegndosis resultat: " + doegnDosisTC1);

        //TC2 Med gyldig værdi 10 styk
        DagligFast TC2 = new DagligFast(
            new DateTime(2023, 01, 01), new DateTime(2024, 01, 01),
            new Laegemiddel("Acetylsalicylsyre", 0.1, 0.15, 0.16, "Styk"),
            2, 3, 4, 1);

        double doegnDosisTC2 = TC2.doegnDosis();

        Assert.AreEqual(10, doegnDosisTC2);

        testLogger.LogInformation("TC2 Doegndosis resultat: " + doegnDosisTC2);
        testLogger.LogInformation("Test finished at: " + DateTime.Now);

    }


    //Test med ugyldige værdier der burde fejle 
    [TestMethod]
    public void DoegnDosisTestFejler()
    {
       
        //TC3 Test med negativ værdi
        DagligFast TC3 = new DagligFast(
            new DateTime(2023, 01, 01), new DateTime(2023, 12, 07),
            new Laegemiddel("Acetylsalicylsyre", 0.1, 0.15, 0.16, "Styk"),
            -1, 1, 1, 1);

        double doegnDosisTC3 = TC3.doegnDosis();

        Assert.AreEqual(1, doegnDosisTC3);


        //TC4 Test med 0 input
        DagligFast TC4 = new DagligFast(
            new DateTime(2023, 01, 01), new DateTime(2024, 01, 01),
            new Laegemiddel("Acetylsalicylsyre", 0.1, 0.15, 0.16, "Styk"),
            0,0,0,0);

        double doegnDosisTC4 = TC4.doegnDosis();

        Assert.AreEqual(1, doegnDosisTC4);



    }


}