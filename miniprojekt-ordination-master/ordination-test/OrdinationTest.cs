namespace ordination_test;

using Microsoft.Extensions.Logging;
using shared.Model;

[TestClass]
public class OrdinationTest : TestBase 
{


    [TestMethod]
    public void antalDageTest()
    {
        testLogger.LogInformation("Test started at: " + DateTime.Now);

        //Test med 10 dages periode 
        Ordination testOrdination = new PN(DateTime.Now, DateTime.Now.AddDays(10), 10, new Laegemiddel("Acetylsalicylsyre", 0.1, 0.15, 0.16, "Styk"));

        int _antalDage= testOrdination.antalDage();

        testLogger.LogInformation("AntalDage() " + _antalDage);


        Assert.AreEqual(10, _antalDage);


        testLogger.LogInformation("Test finished at: " + DateTime.Now);


    }


}