using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace ProjectUniversal
{
    ///<sumary>
    /// The construction information when starting up and configuring this framework
    ///</sumary>
    public class FrameworkConstruction
    {
        #region Public Properties

        /// <summary>
        /// The services that will get used and compiled once the framework is built
        /// </summary>
        public IServiceCollection Services { get; set; }

        /// <summary>
        /// The environment used for this framework
        /// </summary>
        public FrameworkEnvironment Environment { get; set; }

        /// <summary>
        /// The configuration used for this framework
        /// </summary>
        public IConfiguration Configuration { get; set; }

        #endregion

        #region Constructors

        ///<sumary>
        ///Default Constructor
        ///</sumary>
        public FrameworkConstruction()
        {
            #region Initialize

            // Create a new list of dependencies
            Services = new ServiceCollection();

            #endregion

            #region Environment

            // Create environment details
            Environment = new FrameworkEnvironment();

            // Inject environment into services
            Services.AddSingleton(Environment);

            #endregion            
        }

        #endregion
    }
}
