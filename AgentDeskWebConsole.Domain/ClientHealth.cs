using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AgentDeskWebConsole.Domain
{
    public class ClientHealth
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string MedicalHistory { get; set; }
        public string CurrentMedications { get; set; }
        public string SmokingAlcoholHabits { get; set; }
        public string FamilyMedicalHistory { get; set; }
        public int? LastModifiedBy { get; set; }
        public int CreatedBy { get; set; }

    }
}
