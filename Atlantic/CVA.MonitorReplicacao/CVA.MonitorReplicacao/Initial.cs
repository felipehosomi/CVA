using CVA.MonitorReplicacao.DAO;
using CVA.MonitorReplicacao.MODEL;
using Dover.Framework.Attribute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CVA.MonitorReplicacao
{
    [Menu(FatherUID = "3328", UniqueID = FORM.B1Menus.Monitor, String = "CVA - Monitor Replicação", Type = SAPbouiCOM.BoMenuType.mt_STRING, Position = -1, Image = "")]
    [AddIn(Name = "CVA.MonitorReplicacao", Namespace = "CVA Consultoria", Description = "Addon de monitoramento de replicação")]
    public class Initial
    { 
    
    }
}
