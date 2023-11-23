namespace shared.Model;

public class DagligSkæv : Ordination {
    public List<Dosis> doser { get; set; } = new List<Dosis>();

	public int patientId { get; set; } 
    public DagligSkæv(int patientId, DateTime startDen, DateTime slutDen, Laegemiddel laegemiddel) : base(patientId, laegemiddel, startDen, slutDen) {
	}

    public DagligSkæv(int patientId, DateTime startDen, DateTime slutDen, Laegemiddel laegemiddel, Dosis[] doser) : base(patientId, laegemiddel, startDen, slutDen) {
        this.doser = doser.ToList();
    }    

    public DagligSkæv() : base(0, null!, new DateTime(), new DateTime()) {
    }

	public void opretDosis(DateTime tid, double antal) {
        doser.Add(new Dosis(tid, antal));
    }

	public override double samletDosis() {
		return base.antalDage() * doegnDosis();
	}

	public override double doegnDosis() {
		// TODO: Implement!
        return -1;
	}

	public override String getType() {
		return "DagligSkæv";
	}
}
