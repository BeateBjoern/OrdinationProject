namespace ordination_test;

using Microsoft.Extensions.Logging;
using shared.Model;

[TestClass]
public class OrdinationTest : TestBase 
{

    //Test vi har lavet 
    [TestMethod]
    public void antalDageTest()
    {
        testLogger.LogInformation("Test started at: " + DateTime.Now);

        //Test med 10 dages periode 
        Ordination ordination10dage = new PN(1,DateTime.Now, DateTime.Now.AddDays(10), 10, new Laegemiddel("Acetylsalicylsyre", 0.1, 0.15, 0.16, "Styk"));

        int periode1 = ordination10dage.antalDage();

        testLogger.LogInformation("AntalDage() " + periode1);

        Assert.AreEqual(10, periode1);

        //Test med 1 dags periode 
        Ordination ordination1dag = new PN(2,DateTime.Now, DateTime.Now.AddDays(1), 10, new Laegemiddel("Acetylsalicylsyre", 0.1, 0.15, 0.16, "Styk"));

        int periode2 = ordination1dag.antalDage();

        testLogger.LogInformation("AntalDage() " + periode2);

        Assert.AreEqual(1, periode2);

        testLogger.LogInformation("Test finished at: " + DateTime.Now);


    }


}