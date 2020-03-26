using System;

namespace ProjectUniversal
{
    ///<sumary>
    /// Creates a default framework construction containing all
    /// the default configuration and services
    ///</sumary>
    public class DefaultFrameworkConstruction : FrameworkConstruction
    {
        #region Constructors

        ///<sumary>
        ///Default Constructor
        ///</sumary>
        public DefaultFrameworkConstruction()
        {
            // Configure.... 
            this.Configure()
                // and add default services
                .UseDefaultServices();
        }

        #endregion
    }
}
