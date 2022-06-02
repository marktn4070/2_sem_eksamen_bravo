using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2_sem_eksamen_bravo
{
    public class Customer
    {
        #region Coded by Mark
        public string CustomerID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Registered { get; set; }
        public string Gender { get; set; }
        public string Birth { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string RoadcodeID { get; set; }
        public string Municipality { get; set; }
        public string Road { get; set; }
        public string Zip { get; set; }
        #endregion

        #region Coded by James
        public void UpdateAddress()
        {
            string[] startAddress = SQL.GetRoadAndMunicipalityNames(this.RoadcodeID); //first vej, second kommune, third postnummer
            Road = startAddress[0];
            Municipality = startAddress[1];
            Zip = startAddress[2];
        }
        #endregion
    }
}
