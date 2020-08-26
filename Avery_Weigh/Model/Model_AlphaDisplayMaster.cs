using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Model
{
    public class Model_AlphaDisplayMaster
    {
        public int Id { get; set; }
        public string PlantCode { get; set; }
        public string MachineId { get; set; }
        public string Name { get; set; }
        public string AlphaDisplayIdentification { get; set; }
        public string AlphaDisplayIP { get; set; }
        public string AlphaDisplayPORT { get; set; }
    }
}