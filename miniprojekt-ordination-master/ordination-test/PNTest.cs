namespace ordination_test;

using shared.Model;

[TestClass]
public class PNTest : TestBase
{


    [TestMethod] 

    public void givDosisTest()
    {

       

        // TC2, test med input givesDen på første dato 

        PN tc2 = new PN(new DateTime(2023, 01, 01), new DateTime(2023, 02, 01), 123, new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk"));

        bool givDosis_tc2 = tc2.givDosis(new Dato { dato = new DateTime(2023, 01, 01).Date });

        Assert.AreEqual(true, givDosis_tc2);

        // TC3 
        PN tc3 = new PN(new DateTime(2023, 01, 01), new DateTime(2024, 01, 01), 123, new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk"));

        bool givDosis_tc3 = tc3.givDosis(new Dato { dato = new DateTime(2024, 01, 01).Date });

        Assert.AreEqual(true, givDosis_tc3);

        // TC1, test med input givesDen mellem første og sidste dato 
        PN TC1 = new PN(new DateTime(2023, 12, 01),
            new DateTime(2023, 12, 30), 123,
            new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk"));

        bool givDosis1 = TC1.givDosis(new Dato { dato = new DateTime(2023, 12, 15).Date });

        Assert.AreEqual(true, givDosis1);


    }

    public void givDosisTestFejler()
    {
       
        //Test med invalide input data 

        PN tc4 = new PN(new DateTime(2023, 01, 01), new DateTime(2023, 01, 12), 123, new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk"));

        bool givDosis4= tc4.givDosis(new Dato { dato = new DateTime(2022, 12, 31).Date });

        Assert.AreEqual(false, givDosis4);

 
        PN tc5 = new PN(new DateTime(2023, 01, 1), new DateTime(2023, 01, 12), 123, new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk"));

        bool givDosis_tc5 = tc5.givDosis(new Dato { dato = new DateTime(2023, 01, 13).Date });

        Assert.AreEqual(false, givDosis_tc5);
    }

   
}