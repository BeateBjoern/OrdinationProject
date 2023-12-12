namespace shared.Model;

public class DagligSkæv : Ordination {
    public List<Dosis> doser { get; set; } = new List<Dosis>();

    public DagligSkæv(int _patientId, DateTime startDen, DateTime slutDen, Laegemiddel laegemiddel) : base(_patientId, laegemiddel, startDen, slutDen) {
	}

    public DagligSkæv(int _patientId, DateTime startDen, DateTime slutDen, Laegemiddel laegemiddel, Dosis[] doser) : base(_patientId, laegemiddel, startDen, slutDen) {
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

    public override double doegnDosis()
    {
        double sum = -1;
        for (int i = 0; i < doser.Count; i++)
        {
            sum += doser[i].antal;
        }
        return sum;
    }

    public override String getType() {
		return "DagligSkæv";
	}
}
