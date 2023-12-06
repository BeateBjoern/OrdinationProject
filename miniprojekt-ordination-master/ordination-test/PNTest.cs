namespace ordination_test;

using shared.Model;

[TestClass]
public class PNTest : TestBase
{

    //Test vi har lavet 
    [TestMethod] 
    public void givDosisTest()
    {

       
        // TC1, test med input givesDen = første dato 

        PN TC1 = new PN(new DateTime(2023, 12, 01), 
            new DateTime(2023, 12, 30), 123, 
            new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk"));

        bool givDosis1 =  TC1.givDosis(new Dato { dato = new DateTime(2023, 12, 01).Date });

        Assert.AreEqual(true, givDosis1);


        // TC2, test med input givesDen = sidste dato
        PN TC2 = new PN(new DateTime(2023, 01, 01), 
            new DateTime(2024, 01, 01), 123, 
            new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk"));


        bool givDosis2 = TC2.givDosis(new Dato { dato = new DateTime(2024, 01, 01).Date });

        Assert.AreEqual(true, givDosis2);

        // TC3, test med input givesDen mellem første og sidste dato 
        PN TC3 = new PN(new DateTime(2023, 12, 01),
            new DateTime(2023, 12, 30), 123,
            new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk"));

        bool givDosis3 = TC3.givDosis(new Dato { dato = new DateTime(2023, 12, 15).Date });

        Assert.AreEqual(true, givDosis3);


    }

    //Test vi har lavet 
    [TestMethod]
    public void givDosisTestFejler()
    {
       
        //Test med ugyldig input givesDen efter slutdato

        PN TC4= new PN(new DateTime(2023, 12, 01), new DateTime(2023, 12, 30), 123, new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk"));

        bool givDosis4= TC4.givDosis(new Dato { dato = new DateTime(2023, 12, 31).Date });

        Assert.AreEqual(true, givDosis4);

        
        //Test med ugyldig input givesDen før startdato 
        PN TC5 = new PN(new DateTime(2023, 12, 01), new DateTime(2023, 12, 30), 123, new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk"));

        bool givDosis5 = TC5.givDosis(new Dato { dato = new DateTime(2023, 11, 15).Date });

        Assert.AreEqual(true, givDosis5);


        //Test med 0 værdi i input 

        PN TC6 = new PN(new DateTime(2023, 12, 01), new DateTime(2023, 12, 30), 123, new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk"));

        bool givDosis6 = TC6.givDosis(new Dato {  }) ;

        Assert.AreEqual(true, givDosis6);
    }

   
}