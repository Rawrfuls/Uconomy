using Rocket.API.DependencyInjection;
using Rocket.API.Economy;

namespace fr34kyn01535.Uconomy.Properties
{
    public class DependencyRegistrator : IDependencyRegistrator
    {
        public void Register(IDependencyContainer container, IDependencyResolver resolver)
        {
            container.RegisterSingletonType<IEconomyProvider, UconomyEconomyProvider>();
        }
    }
}