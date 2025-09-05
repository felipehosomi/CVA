using System;

namespace SBO.Hub.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]
    public class FormAttribute : Attribute
    {
        public int FormId { get; set; }

        public FormAttribute(int formId)
        {
            this.FormId = formId;
        }
    }
}
