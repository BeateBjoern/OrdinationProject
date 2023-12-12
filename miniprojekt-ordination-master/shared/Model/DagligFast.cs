namespace shared.Model;
using static shared.Util;

public class DagligFast : Ordination {
	
	public Dosis MorgenDosis { get; set; } = new Dosis();
    public Dosis MiddagDosis { get; set; } = new Dosis();
    public Dosis AftenDosis { get; set; } = new Dosis();
    public Dosis NatDosis { get; set; } = new Dosis();

    public DagligFast(int _patientId, DateTime startDen, DateTime slutDen, Laegemiddel laegemiddel, double morgenAntal, double middagAntal, double aftenAntal, double natAntal) : base(_patientId, laegemiddel, startDen, slutDen) {
        MorgenDosis = new Dosis(CreateTimeOnly(6, 0, 0), morgenAntal);
        MiddagDosis = new Dosis(CreateTimeOnly(12, 0, 0), middagAntal);
        AftenDosis = new Dosis(CreateTimeOnly(18, 0, 0), aftenAntal);
        NatDosis = new Dosis(CreateTimeOnly(23, 59, 0), natAntal);
	}

    public DagligFast() : base(0, null!, new DateTime(), new DateTime()) {
    }

	//overriding metoder fra ordination 
	public override double samletDosis() {
		
		return base.antalDage() * doegnDosis();
	}

  public override double doegnDosis() {

		double sum = -1; //Sat til -1, så metoden returnerer -1 i tilfælde af fail 

		if(MorgenDosis.antal == 0 && MiddagDosis.antal == 0 && AftenDosis.antal == 0 && NatDosis.antal == 0)
        {
			throw new ArgumentNullException("Dosis må ikke være 0 eller under ");
           
        }
		else if(MorgenDosis.antal < 0 || MiddagDosis.antal < 0 || AftenDosis.antal < 0 || NatDosis.antal < 0)
		{
			throw new ArgumentOutOfRangeException("Dosis må ikke være et negativt tal");

		}
		else
		{
            sum = MorgenDosis.antal + MiddagDosis.antal + AftenDosis.antal + NatDosis.antal;
			return sum; 
        }

		
        
	}

    public Dosis[] getDoser() {
		Dosis[] doser = {MorgenDosis, MiddagDosis, AftenDosis, NatDosis};
		return doser;
	}

	public override String getType() {
		return "DagligFast";
	}
}
