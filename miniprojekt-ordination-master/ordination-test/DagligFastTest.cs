namespace ordination_test;

using shared.Model;

[TestClass]
public class DagligFastTest
{

    [TestMethod]

    public void DoegnDosisTest()
    {
        Dosis morgenDosis = new Dosis { antal = 2, tid = DateTime.Now.Date.AddHours(8) }; // Example datetime for morning
        Dosis middagDosis = new Dosis { antal = 4, tid = DateTime.Now.Date.AddHours(12) }; // Example datetime for midday
        Dosis aftenDosis = new Dosis { antal = 3, tid = DateTime.Now.Date.AddHours(18) }; // Example datetime for evening
        Dosis natDosis = new Dosis { antal = 1, tid = DateTime.Now.Date.AddHours(23) }; // Example datetime for night

        DagligFastTest dagligFastTest = new DagligFastTest();

        // Assert
        double expectedTotalDosis = morgenDosis.antal + middagDosis.antal + aftenDosis.antal + natDosis.antal;
        //Assert.AreEqual(expectedTotalDosis, result, "Total dose should be the sum of individual doses.");
    }

    
   

   
}