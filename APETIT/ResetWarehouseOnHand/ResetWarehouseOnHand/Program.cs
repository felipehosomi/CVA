using NLog;
using ResetWarehouseOnHand.Controllers;
using System;

namespace ResetWarehouseOnHand
{
    internal class Program
    {
        internal static Logger Logger;

        static Program()
        {
            Logger = LogManager.GetCurrentClassLogger();
        }

        public Program()
        {
        }

        private static void Main(string[] args)
        {
            try
            {
                SBOController.Connect();
                WarehouseController.SetInventoryGenExit();
            }
            catch (Exception ex)
            {
                Logger.Error(ex, ex.Message);
            }
        }
    }
}