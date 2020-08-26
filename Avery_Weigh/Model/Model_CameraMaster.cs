using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Avery_Weigh.Model
{
    public class Model_CameraMaster
    {
        public int Id { get; set; }
        public string PlantCode { get; set; }
        public string MachineId { get; set; }
        public string Name { get; set; }
        public string CameraIdentification { get; set; }
        public string CameraIP { get; set; }
        public string CameraPORT { get; set; }
    }
}