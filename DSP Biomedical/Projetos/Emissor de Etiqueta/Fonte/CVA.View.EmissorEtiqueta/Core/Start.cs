using Dover.Framework;

namespace Core
{
    class Start
    {
        /// <summary>
        /// Start the application throught SAP Business One
        /// Using method for debug and compillate application
        /// applicatiion.Run is responsible from start DOVER Framework
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            Application application = new Application();
            application.Run();
        }
    }
}
