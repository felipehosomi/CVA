using System.Windows.Forms;

namespace CVA_RepConfig.HelperForms
{
    public static class HelperForm
    {
        public static void ClearControl(this Panel panel)
        {
            panel.Controls.Clear();
        }
    }
}