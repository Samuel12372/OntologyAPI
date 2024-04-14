using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UOM_Ontology_API
{
    [Serializable]
    public class OntologyData
    {
        public string ontologyId { get; set; }
        public string status { get; set; }
        public int numberOfTerms { get; set; }
        public Config config { get; set; }
        [Serializable]
        public class Config
        {
            public string title { get; set; }
            public string description { get; set; }
            
        }

    }
}
