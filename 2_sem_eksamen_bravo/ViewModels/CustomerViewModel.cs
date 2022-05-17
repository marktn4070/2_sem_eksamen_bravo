using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_sem_eksamen_bravo.ViewModels
{
    public class CustomerViewModel : BaseViewModel
    {
        int AdresseID { get; set; }
        string ForNavn { get; set; }
        string EfterNavn { get; set; }
        byte Registreret { get; set; }
        string Køn { get; set; }
        DateTime FødsDato { get; set; }
        int MobilNr { get; set; }
        string Email { get; set; }


        public CustomerViewModel()
        {

        }

        public CustomerViewModel(int adresseid, string fornavn, string efternavn, byte registreret, string køn, DateTime fødsdato, int mobilnr, string email)
        {
            AdresseID = adresseid;
            ForNavn = fornavn;
            EfterNavn = efternavn;
            Registreret = registreret;
            Køn = køn;
            FødsDato = fødsdato;
            MobilNr = mobilnr;
            Email = email;
        }
    }
}
