using MODEL.Classes;
using System;

namespace MODEL.Interface
{
    public class IModel
    {
        public int Id { get; set; }
        public DateTime Insert { get; set; }
        public DateTime Update { get; set; }
        public UserModel User { get; set; }
        public StatusModel Status { get; set; }
    }
}