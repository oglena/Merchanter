using Merchanter;
using Merchanter.Classes.Settings;

namespace ApiService.Services {
    /// <summary>
    /// Defines the contract for a service that manages merchant-related operations.
    /// </summary>
    /// <remarks>This interface provides properties for accessing database helpers, global settings,  and
    /// customer-specific identifiers. Implementations of this interface are expected  to facilitate merchant-related
    /// functionality.</remarks>
    public interface IMerchanterService {
        /// <summary>
        /// Gets or sets the database helper instance used for database operations.
        /// </summary>
        public DbHelper Helper { get; set; }

        /// <summary>
        /// Gets or sets the global settings for the merchanter configuration.
        /// </summary>
        public SettingsMerchanter? Global { get; set; }
        
        /// <summary>
        /// Gets or sets the unique identifier for a customer.
        /// </summary>
        public int CustomerId { get; set; }
    }

    /// <summary>
    /// Provides services for managing merchant-related operations, including customer identification and global
    /// settings configuration.
    /// </summary>
    /// <remarks>This class implements the <see cref="IMerchanterService"/> interface and serves as a central
    /// point for merchant-related functionality. It initializes a database helper for interacting with the underlying
    /// database and exposes properties for customer identification and global settings.</remarks>
    public class MerchanterService :IMerchanterService {
        /// <inheritdoc />
        public int CustomerId { get; set; }
        /// <inheritdoc />
        public DbHelper Helper { get; set; }
        /// <inheritdoc />
        public SettingsMerchanter? Global { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MerchanterService"/> class.
        /// </summary>
        /// <remarks>This constructor sets up the service by initializing a helper object with the
        /// required server,  user credentials, database information, and port configuration.</remarks>
        public MerchanterService() {
            this.Helper = new(Constants.Server, Constants.User, Constants.Password, Constants.Database, Constants.Port);
        }
    }
}
