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

    public override double doegnDosis()
    {
        Console.WriteLine("Test doegnDosis PN " + dates.Count);
        double antalGangeAnvendt = getAntalGangeGivet();

        if (dates != null && dates.Any())
        {
            DateTime firstDate = dates.FirstOrDefault().dato;
            DateTime lastDate = dates.Last().dato;
           

            if (firstDate != null && lastDate != null)
            {
                Console.WriteLine("Test doegnDosis PN" + firstDate + " " + lastDate);
                double samletDosis = antalGangeAnvendt * antalEnheder;
                int periode = (int)lastDate.Subtract(firstDate).TotalDays;

                return samletDosis / periode;  // Check for division by zero
            }
            else
            {
                // Handle the case when firstDate or lastDate is null
                // For example, throw an exception, return a default value, or take appropriate action
                Console.WriteLine("Error: Dates collection contains null elements");
            }
        }
        else
        {
            // Handle the case when dates is null or empty
            // For example, throw an exception, return a default value, or take appropriate action
            Console.WriteLine("Error: Dates collection is null or empty");
        }

        return 0.0;
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
