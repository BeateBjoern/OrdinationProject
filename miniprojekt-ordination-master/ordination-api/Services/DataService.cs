using Microsoft.EntityFrameworkCore;
using System.Text.Json;

using shared.Model;
using static shared.Util;
using Data;
using Serilog;

namespace Service;

public class DataService
{
    private OrdinationContext db { get; }
    private readonly ILogger<DataService> logger;

    public DataService(OrdinationContext db, ILogger<DataService> logger) {
        this.db = db;
        this.logger = logger;
    }

    /// <summary>
    /// Seeder noget nyt data i databasen, hvis det er nødvendigt.
    /// </summary>
    public void SeedData() {

        // Patients
        Patient[] patients = new Patient[5];
        patients[0] = db.Patienter.FirstOrDefault()!;

        if (patients[0] == null)
        {
            patients[0] = new Patient(1, "121256-0512", "Jane Jensen", 63.4);
            patients[1] = new Patient(2,"070985-1153", "Finn Madsen", 83.2);
            patients[2] = new Patient(3,"050972-1233", "Hans Jørgensen", 89.4);
            patients[3] = new Patient(4,"011064-1522", "Ulla Nielsen", 59.9);
            patients[4] = new Patient(5,"123456-1234", "Ib Hansen", 87.7);

            db.Patienter.Add(patients[0]);
            db.Patienter.Add(patients[1]);
            db.Patienter.Add(patients[2]);
            db.Patienter.Add(patients[3]);
            db.Patienter.Add(patients[4]);
            db.SaveChanges();
        }

        Laegemiddel[] laegemiddler = new Laegemiddel[5];
        laegemiddler[0] = db.Laegemiddler.FirstOrDefault()!;
        if (laegemiddler[0] == null)
        {
            laegemiddler[0] = new Laegemiddel("Acetylsalicylsyre", 0.1, 0.15, 0.16, "Styk");
            laegemiddler[1] = new Laegemiddel("Paracetamol", 1, 1.5, 2, "Ml");
            laegemiddler[2] = new Laegemiddel("Fucidin", 0.025, 0.025, 0.025, "Styk");
            laegemiddler[3] = new Laegemiddel("Methotrexat", 0.01, 0.015, 0.02, "Styk");
            laegemiddler[4] = new Laegemiddel("Prednisolon", 0.1, 0.15, 0.2, "Styk");

            db.Laegemiddler.Add(laegemiddler[0]);
            db.Laegemiddler.Add(laegemiddler[1]);
            db.Laegemiddler.Add(laegemiddler[2]);
            db.Laegemiddler.Add(laegemiddler[3]);
            db.Laegemiddler.Add(laegemiddler[4]);

            db.SaveChanges();
        }

        Ordination[] ordinationer = new Ordination[6];
        ordinationer[0] = db.Ordinationer.FirstOrDefault()!;
        if (ordinationer[0] == null) {
            Laegemiddel[] lm = db.Laegemiddler.ToArray();
            Patient[] p = db.Patienter.ToArray();

            ordinationer[0] = new PN(1, new DateTime(2021, 1, 1), new DateTime(2021, 1, 12), 123, lm[1]);
            ordinationer[1] = new PN(2, new DateTime(2021, 2, 12), new DateTime(2021, 2, 14), 3, lm[0]);
            ordinationer[2] = new PN(3,new DateTime(2021, 1, 20), new DateTime(2021, 1, 25), 5, lm[2]);
            ordinationer[3] = new PN(4,new DateTime(2021, 1, 1), new DateTime(2021, 1, 12), 123, lm[1]);
            ordinationer[4] = new DagligFast(5, new DateTime(2021, 1, 10), new DateTime(2021, 1, 12), lm[1], 2, 0, 1, 0);
            ordinationer[5] = new DagligSkæv(4, new DateTime(2021, 1, 23), new DateTime(2021, 1, 24), lm[2]);

            ((DagligSkæv)ordinationer[5]).doser = new Dosis[] {
                new Dosis(CreateTimeOnly(12, 0, 0), 0.5),
                new Dosis(CreateTimeOnly(12, 40, 0), 1),
                new Dosis(CreateTimeOnly(16, 0, 0), 2.5),
                new Dosis(CreateTimeOnly(18, 45, 0), 3)
            }.ToList();


            db.Ordinationer.Add(ordinationer[0]);
            db.Ordinationer.Add(ordinationer[1]);
            db.Ordinationer.Add(ordinationer[2]);
            db.Ordinationer.Add(ordinationer[3]);
            db.Ordinationer.Add(ordinationer[4]);
            db.Ordinationer.Add(ordinationer[5]);

            db.SaveChanges();

            p[0].ordinationer.Add(ordinationer[0]);
            p[0].ordinationer.Add(ordinationer[1]);
            p[2].ordinationer.Add(ordinationer[2]);
            p[3].ordinationer.Add(ordinationer[3]);
            p[1].ordinationer.Add(ordinationer[4]);
            p[1].ordinationer.Add(ordinationer[5]);

            db.SaveChanges();
        }
    }


    //Metode vi har lavet 
    public void AddPatient(Patient patient)
    {
        db.Patienter.Add(patient);
        db.SaveChanges();
    }

    //Metode vi har lavet 
    public void AddLaegemiddel(Laegemiddel laegemiddel)
    {
        db.Laegemiddler.Add(laegemiddel);
        db.SaveChanges();
    }



    public List<PN> GetPNs() {
        return db.PNs.Include(o => o.laegemiddel).Include(o => o.dates).ToList();
    }

    public List<DagligFast> GetDagligFaste() {
        return db.DagligFaste
            .Include(o => o.laegemiddel)
            .Include(o => o.MorgenDosis)
            .Include(o => o.MiddagDosis)
            .Include(o => o.AftenDosis)            
            .Include(o => o.NatDosis)            
            .ToList();
    }

    public List<DagligSkæv> GetDagligSkæve() {
        return db.DagligSkæve
            .Include(o => o.laegemiddel)
            .Include(o => o.doser)
            .ToList();
    }

    public List<Patient> GetPatienter() {
        return db.Patienter.Include(p => p.ordinationer).ToList();
    }

    public List<Laegemiddel> GetLaegemidler() {
        return db.Laegemiddler.ToList();
    }



  //Metode vi har lavet
    public PN OpretPN(int patientId, int laegemiddelId, double antal, DateTime startDato, DateTime slutDato)
    {

         var laegeMiddel = GetLaegemidler().FirstOrDefault(m => m.LaegemiddelId == laegemiddelId);
         var patient = GetPatienter().FirstOrDefault(p => p.PatientId == patientId);

            if (laegeMiddel == null && patientId == null && antal >= 0 && startDato > slutDato)
            {

                throw new ArgumentNullException("Felter mangler eller ugyldig dato input. Startdato skal ligge før slutdato");

            }
            else
            {

                Console.WriteLine("Test OpretPn DataService: " + patientId);
                PN nyPn = new PN(patient.PatientId, startDato, slutDato, antal, laegeMiddel);
                patient.ordinationer.Add(nyPn);
                db.SaveChanges();
                return nyPn;
            }
            return null!;
    }


    //Metode vi har lavet
    public DagligFast OpretDagligFast(int patientId, int laegemiddelId, double antalMorgen, double antalMiddag, double antalAften, double antalNat, DateTime startDato, DateTime slutDato)
    {
            

            Patient patient = db.Patienter.FirstOrDefault(a => a.PatientId == patientId);
            Laegemiddel laegemiddel = db.Laegemiddler.FirstOrDefault(b => b.LaegemiddelId == laegemiddelId);

            if (patient == null || laegemiddel == null || antalMorgen <= null || antalMiddag <= null || antalAften <= null || antalNat <= null || startDato == null || slutDato == null || startDato > slutDato)
            {
                 throw new ArgumentNullException("Felter mangler eller ugyldig dato input. Startdato skal ligge før slutdato");
                 
            }
            else
            {
                var nyDagligFast = new DagligFast(patient.PatientId, startDato, slutDato, laegemiddel, antalMorgen, antalMiddag, antalAften, antalNat);
                patient.ordinationer.Add(nyDagligFast);
                Console.WriteLine(nyDagligFast.MorgenDosis);
                db.SaveChanges();
                return nyDagligFast;
             }         
       
    }


    //Metode vi har lavet 
    public DagligSkæv OpretDagligSkaev(int patientId, int laegemiddelId, Dosis[] doser, DateTime startDato, DateTime slutDato)
    {

            Patient patient = db.Patienter.FirstOrDefault(a => a.PatientId == patientId);
            Laegemiddel laegemiddel = db.Laegemiddler.FirstOrDefault(b => b.LaegemiddelId == laegemiddelId);
            double anbefaletDosis = GetAnbefaletDosisPerDøgn(patientId, laegemiddelId);
            double dosisTotal = doser.Sum(sum => sum.antal);
            

            if (patient == null || laegemiddel == null || doser == null || startDato == null || slutDato == null || startDato>slutDato)
            {
                throw new ArgumentNullException("Felter mangler eller ugyldig dato input. Startdato skal ligge før slutdato");

            }

            else if(dosisTotal!= 0 && dosisTotal < anbefaletDosis)
            {
                var nyDagligSkæv = new DagligSkæv(patient.PatientId, startDato, slutDato, laegemiddel);

                foreach (var dosis in doser)
                {
                    nyDagligSkæv.opretDosis(dosis.tid, dosis.antal);
                }

                patient.ordinationer.Add(nyDagligSkæv);
                db.SaveChanges();
                return nyDagligSkæv;
            }

        return null;
            
        }
       
    


    //Metode vi har lavet 
    public string AnvendOrdination(int id, Dato dato)
    {
        PN pn = db.PNs.FirstOrDefault(pn => pn.OrdinationId == id);

        Console.WriteLine("dato fra anvendordination: " + dato);

        if (pn == null)  //Tjekker for gyldig dato input 
        {
            throw new ArgumentNullException("PN ordination findes ikke");
    
        }
        else if(dato.dato < DateTime.Now.Date || dato.dato <= pn.startDen || dato.dato >= pn.slutDen)
        {
            return "Medicin kan ikke gives udenfor ordinationsperiode, eller med tilbagevirkende kraft";
        }  
        else
        {

            pn.givDosis(dato);
            db.SaveChanges();
            return "PN anvendt!";
        }
     
    }



    //Metode vi har lavet 
	public double GetAnbefaletDosisPerDøgn(int patientId, int laegemiddelId)
    {

        Patient patient = db.Patienter.FirstOrDefault(a => a.PatientId == patientId);
        Laegemiddel laegemiddel = db.Laegemiddler.FirstOrDefault(a => a.LaegemiddelId == laegemiddelId);
        double dosis = 0;

        if (laegemiddel == null && patient == null)
        {
            throw new ArgumentNullException("Patient og lægemiddel må ikke være null");
        }
        else if (patient.vaegt == 0)
        {
            throw new ArgumentNullException("Vægt skal være over 0");
        }
        else if(patient.vaegt < 0)
        {
            throw new ArgumentOutOfRangeException("Vægt har en ugyldig værdi");
        }

        else if (patient.vaegt < 25)
        {
            dosis = patient.vaegt * laegemiddel.enhedPrKgPrDoegnLet;
        }
        else if (25 <= patient.vaegt && patient.vaegt <= 120)
        {
            dosis = patient.vaegt * laegemiddel.enhedPrKgPrDoegnNormal;
        }
        else if (patient.vaegt > 120)
        {
            dosis = patient.vaegt * laegemiddel.enhedPrKgPrDoegnTung;
        }
        return dosis;
    }

}