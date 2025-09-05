using CVA.View.Comissionamento.Controllers;
using SimpleInjector;

namespace CVA.View.Comissionamento.Helpers
{
    public class ContainerHelper
    {
        private Container _container;

        public ContainerHelper()
        {
            _container = new Container();
            var lifestye = Lifestyle.Singleton;
            _container.Register<SapFactory>(lifestye);
            _container.Register<DbHelper>(lifestye);
            _container.Register<MenuHelper>(lifestye);
            _container.Register<FilterHelper>(lifestye);
            _container.Register<FormHelper>(lifestye);
            _container.Register<DIHelper>(lifestye);
            _container.Register<ComissoesController>(lifestye);
        }

        public Container GetContainer()
        {
            return _container;
        }
    }
}
