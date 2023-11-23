namespace shared.Model;

public class PN : Ordination {
	public double antalEnheder { get; set; }
    public List<Dato> dates { get; set; } = new List<Dato>();
    public int patientId { get; set; }

    public PN (int patientId, DateTime startDen, DateTime slutDen, double antalEnheder, Laegemiddel laegemiddel) : base( patientId, laegemiddel, startDen, slutDen) {
		this.antalEnheder = antalEnheder;

    }

    public PN() : base(0, null!, new DateTime(), new DateTime())
    {
    }

    /// <summary>
    /// Registrerer at der er givet en dosis på dagen givesDen
    /// Returnerer true hvis givesDen er inden for ordinationens gyldighedsperiode og datoen huskes
    /// Returner false ellers og datoen givesDen ignoreres
    /// </summary>
    public bool givDosis(Dato givesDen) {
        
        if(givesDen == null) { return false; }
        dates.Add(givesDen);
        return true;
    }

    public override double doegnDosis() {

        double antalGangeAnvendt = getAntalGangeGivet();
        var firstDate = dates.First().dato;
        var lastDate = dates.Last().dato;
        double samletDosis = antalGangeAnvendt * antalEnheder; //samlet dosis * enheder givet i periodne
        int periode = (int)lastDate.Subtract(firstDate).TotalDays; //antal dage fra første dag givet til seneste dag givet 
        return samletDosis / periode; 
    }


    public override double samletDosis() {
        return dates.Count() * antalEnheder;

    }

    public int getAntalGangeGivet() {
        return dates.Count();

    }

	public override String getType() {
		return "PN";

	}
}
