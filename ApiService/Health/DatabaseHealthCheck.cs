using ApiService.Services;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ApiService.Health {
    /// <summary>
    /// Performs a health check to verify the connectivity and operational status of the database.
    /// </summary>
    /// <remarks>This health check attempts to retrieve a simple value from the database using the provided 
    /// <see cref="MerchanterService"/>. If the database is reachable and operational, the health check  returns a
    /// healthy status. Otherwise, it returns an unhealthy status, optionally including  exception details if a failure
    /// occurs.</remarks>
    public class DatabaseHealthCheck : IHealthCheck {
        private readonly MerchanterService _merchanterService;

        /// <summary>
        /// Initializes a new instance of the <see cref="DatabaseHealthCheck"/> class.
        /// </summary>
        /// <param name="merchanterService">The service used to perform health checks on the database. This parameter cannot be null.</param>
        public DatabaseHealthCheck(MerchanterService merchanterService) {
            _merchanterService = merchanterService;
        }

        /// <summary>
        /// Performs a health check to determine the status of the database connection.
        /// </summary>
        /// <remarks>This method attempts to retrieve a simple value from the database to verify
        /// connectivity. If the database connection fails or returns an invalid result, the method will return an
        /// unhealthy status.</remarks>
        /// <param name="context">The context in which the health check is executed. Provides information about the health check
        /// configuration.</param>
        /// <param name="cancellationToken">A token that can be used to cancel the health check operation. Defaults to <see langword="default"/> if not
        /// provided.</param>
        /// <returns>A <see cref="HealthCheckResult"/> indicating the health status of the database connection. Returns <see
        /// cref="HealthCheckResult.Healthy"/> if the connection is successful, or <see
        /// cref="HealthCheckResult.Unhealthy"/> if the connection fails.</returns>
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default) {
            try {
                // Attempt to retrieve a simple value from the database to check connectivity
                var result = await _merchanterService.Helper.HealthCheck();
                if (result) {
                    return HealthCheckResult.Healthy("Database connection is healthy");
                }
                return HealthCheckResult.Unhealthy("Database connection returned false ?!");
            }
            catch (Exception ex) {
                return HealthCheckResult.Unhealthy("Database connection failed", ex);
            }
        }
    }
}
