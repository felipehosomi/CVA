using System.Collections.Generic;

namespace CVA.View.EDoc.Model
{
    public class EDocModel
    {
        public EDoc0000Model EDoc0000Model { get; set; }
        public EDoc0001Model EDoc0001Model { get; set; }
        public EDoc0005Model EDoc0005Model { get; set; }
        public EDoc0030Model EDoc0030Model { get; set; }
        public EDoc0100Model EDoc0100Model { get; set; }
        public List<EDoc0150Model> EDoc0150Model { get; set; }
        public List<EDoc0200Model> EDoc0200Model { get; set; }
        public List<EDoc0400Model> EDoc0400Model { get; set; }
        public EDoc0990Model EDoc0990Model { get; set; }
        public EDocC001Model EDocC001Model { get; set; }
        public List<EDocC020Model> EDocC020Model { get; set; }
        public List<EDocC300Model> EDocC300Model { get; set; }
        public EDocC990Model EDocC990Model { get; set; }
        public EDoc9001Model EDoc9001Model { get; set; }
        public List<EDoc9900Model> EDoc9900Model { get; set; }
        public EDoc9990Model EDoc9990Model { get; set; }
        public EDoc9999Model EDoc9999Model { get; set; }
    }
}
