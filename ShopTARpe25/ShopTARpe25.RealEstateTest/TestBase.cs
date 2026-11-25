using Microsoft.Extensions.DependencyInjection;
using ShopTARpe25.ApplicationServices.Services;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using ShopTARpe25.RealEstateTest.Macros;
using Microsoft.Extensions.Hosting;
using ShopTARpe25.RealEstateTest.Mock;

namespace ShopTARpe25.RealEstateTest
{
    //iga koodirida ära kommenteerida, et mis mida meetod teeb ja miks seda on vaja
    public abstract class TestBase
    {

        // Teenuste pakkuja, mille kaudu saab testides vajalikke teenuseid kasutada.
        // Protected tähendab, et seda saavad kasutada ka pärivad testiklassid.
        protected IServiceProvider ServiceProvider { get; set; }

        // Konstruktor käivitatakse automaatselt TestBase klassi loomisel.
        protected TestBase()
        {
            // Loome uue teenuste kogu, kuhu registreerime kõik testides vajalikud teenused.
            var services = new ServiceCollection();

            //Kutsub välja meetodi, mis lisab teenused ja vajalikud sõltuvused
            SetupServices(services);

            //Selle kaudu saab teenuseid küsida
            ServiceProvider = services.BuildServiceProvider();
        }

        // Meetod testides kasutatavate teenuste registreerimiseks.
        // Virtual võimaldab pärivatel klassidel seda meetodit vajadusel üle kirjutada.
        protected virtual void SetupServices(IServiceCollection services)
        {

            services.AddScoped<IRealEstateServices, RealEstateServices>();
            //miks pidi lisama kaks servict juurde, kuigi me tahame testida ainult RealEstate CRUDi?
            //sest File-id on ikka seotud Real Estate Create-iga

            services.AddScoped<IFileServices, FileServices>();
            services.AddScoped<IHostEnvironment, MockIHostEnvironment>();

            // Registreerime andmebaasi konteksti.
            services.AddDbContext<ShopTARpe25Context>(options =>
            {
                // Kasutame testimiseks mälus olevat andmebaasi.
                // See tähendab, et päris SQL-andmebaasiga pole vaja ühendust luua.
                options.UseInMemoryDatabase("TEST");

                //eirab hoiatust, et mälusisene DB ei toeta tehinguid samamoodi nagu päris andmebaas
                options.ConfigureWarnings(warnings =>
                    warnings.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            //kutsub välja macrode klassi
            RegisterMacros(services);
        }

        //vabastab ressursid peale kasutamist, ei jäeks mällu midagi
        public void Dispose()
        {

        }

        //abimeetoid, millega klass ja selle alamklasid saavad teenuse tüübi järgi küsida
        protected T Svc<T>()
        {
           
            //otsib teenusepakkuja tüübi
            return ServiceProvider.GetRequiredService<T>();
        }

        
        //abimeetoid makrode leidmiseks ja registreerimiseks
        private void RegisterMacros(IServiceCollection services)
        {
            //saaks refleksiooniga otsinus kasutada
            var macroBaseType = typeof(IMacros);

            // Otsime IMacros liidese assembly'st kõik tüübid (klassid, liidesed jne).
            var macros = macroBaseType.Assembly.GetTypes()
                .Where(t =>
                    //valib tüübid, mis rakendavad IMacros liidest, aga ei ole ise liidesed
                    //ega abstraksed klassid
                    macroBaseType.IsAssignableFrom(t)

                    // Jätame välja liidesed, sest neid ei saa otse luua.
                    && !t.IsInterface

                    // Jätame välja abstraktsed klassid, sest neid ei saa otse luua.
                    && !t.IsAbstract);

            // Käime kõik leitud makroklassid ükshaaval läbi.
            foreach (var macro in macros)
            {
                // Registreerime iga makro Dependency Injection süsteemis Singleton-teenusena.
                // Singleton tähendab, et kogu ServiceProvideri eluea jooksul
                // kasutatakse ühest makroklassist ühte objekti.
                services.AddSingleton(macro);
            }
        }

    }
}
