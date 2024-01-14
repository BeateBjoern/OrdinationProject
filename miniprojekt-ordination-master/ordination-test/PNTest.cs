namespace ordination_test;

using shared.Model;

[TestClass]
public class PNTest : TestBase
{

    //Test vi har lavet 
    [TestMethod] 
    public void givDosisTest()
    {

       
        // TC1, test med input givesDen = f�rste dato 

        PN TC1 = new PN(1,new DateTime(2023, 12, 01), 
            new DateTime(2023, 12, 30), 123, 
            new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk"));

        bool givDosis1 =  TC1.givDosis (new Dato { dato = new DateTime(2023, 12, 01).Date });

        Assert.AreEqual(true, givDosis1);


        // TC2, test med input givesDen = sidste dato
        PN TC2 = new PN(2,new DateTime(2023, 01, 01), 
            new DateTime(2024, 01, 01), 123, 
            new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk"));


        bool givDosis2 = TC2.givDosis(new Dato { dato = new DateTime(2024, 01, 01).Date });

        Assert.AreEqual(true, givDosis2);

        // TC3, test med input givesDen mellem f�rste og sidste dato 
        PN TC3 = new PN(3,new DateTime(2023, 12, 01),
            new DateTime(2023, 12, 30), 123,
            new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk"));

        bool givDosis3 = TC3.givDosis(new Dato { dato = new DateTime(2023, 12, 15).Date });

        Assert.AreEqual(true, givDosis3);


    }

    //Test vi har lavet 
    [TestMethod]
    public void givDosisTestUgyldigDato()
    {
        PN TC4 = new PN(4, new DateTime(2023, 12, 01), new DateTime(2023, 12, 30), 123, new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk"));

        Assert.ThrowsException<ArgumentOutOfRangeException>(() => TC4.givDosis(new Dato { dato = new DateTime(2023, 12, 31).Date }));
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => TC4.givDosis(new Dato { dato = new DateTime(2023, 11, 15).Date }));
    }

    [TestMethod]
    public void givDosisTestNullVærdi()
    {

        // Test med 0 v�rdi i input
        PN TC6 = new PN(6, new DateTime(2023, 12, 01), new DateTime(2023, 12, 30), 123, new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk"));

        Assert.ThrowsException<ArgumentNullException>(() => TC6.givDosis(null));


    }


}