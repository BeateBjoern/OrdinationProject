namespace ordination_test;

using shared.Model;

[TestClass]
public class PatientTest : TestBase
{

    [TestMethod]
    public void PatientHasName()
    {
        string cpr = "160563-1234";
        string navn = "John";
        double vægt = 83;

        Patient patient = new Patient(1,cpr, navn, vægt);
        Assert.AreEqual(navn, patient.navn);
    }


    [TestMethod]
    public void TestDerAltidFejler()
    {
        string cpr = "160563-1234";
        string navn = "John";
        double vægt = 83;

        Patient patient = new Patient(1,cpr, navn, vægt);
        Assert.AreEqual("Egon", patient.navn);
    }
}