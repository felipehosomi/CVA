using MODEL.Enumerators;
using MODEL.Interface;
using System;
using System.Collections.Generic;

namespace MODEL.Classes
{
    public class ActivityModel : IModel
    {
        public static int oObjectType { get { return (int)ObjectType.ProjectActivity; } }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime InitialDate { get; set; }
        public string FinalDate { get; set; }
        public ActivityModel PredecessorActivity { get; set; }
        public ActivityTypeModel Type { get; set; }
        public ProjectModel Project { get; set; }
        public List<FilesModel> Files { get; set; }
    }
}
