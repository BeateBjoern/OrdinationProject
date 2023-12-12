namespace shared.Model;

public class PN : Ordination {
	public double antalEnheder { get; set; }
    public List<Dato> dates { get; set; } = new List<Dato>();

 

    public PN (int _patientId, DateTime startDen, DateTime slutDen, double antalEnheder, Laegemiddel laegemiddel) : base(_patientId, laegemiddel, startDen, slutDen) {
		this.antalEnheder = antalEnheder;

    }

    public PN() : base(0, null!, new DateTime(), new DateTime())
    {
    }


    //Metode vi har lavet 
    public bool givDosis(Dato givesDen)
    {

        if (givesDen == null)
        {
            throw new ArgumentNullException("dato kan ikke være nul");
        }
        else if(givesDen.dato < startDen || givesDen.dato > slutDen)
        {
            throw new ArgumentOutOfRangeException("Datoen du har valgt er udenfor gyldig periode");
        }
        else
        {
            dates.Add(givesDen);
            return true;
        }

      
    }

    //Metode vi har lavet 
    public override double doegnDosis()
    {
        double sum = 0;

        if (dates.Count() > 0)
        {
            DateTime min = dates.First().dato;
            DateTime max = dates.Last().dato;

            foreach (Dato d in dates)
            {
                if (d.dato < min)
                {
                    min = d.dato;
                }
                if (d.dato > max)
                {
                    max = d.dato;
                }
            }
            int dage = (int)(max - min).TotalDays + 1;
            sum = samletDosis() / dage;
        }
        return sum;
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
