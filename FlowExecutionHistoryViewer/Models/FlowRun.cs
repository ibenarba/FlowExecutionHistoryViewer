using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowExecutionHistoryViewer.Models
{
    public class FlowRun
    {
        [DisplayName("ID Exécution")]
        public string Id { get; set; }

        [DisplayName("Statut")]
        public string Status { get; set; }

        [DisplayName("Début")]
        public DateTime StartDate { get; set; }

        [DisplayName("Fin")]
        public DateTime EndDate { get; set; }

        [DisplayName("Durée")]
        public string Duration => (EndDate - StartDate).ToString(@"hh\:mm\:ss");

        [Browsable(false)] // On cache l'URL technique de la grille
        public string Url { get; set; }
    }
}
