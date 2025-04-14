using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Text.Json.Serialization;
using System.Linq; // Add this using directive

namespace MerchanterApp.ApiService.Classes {
    public class IgnorePropertyFilter : IOperationFilter {
        public void Apply(OpenApiOperation operation, OperationFilterContext context) {
            if (context.ApiDescription == null || operation.Parameters == null)
                return;

            if (!context.ApiDescription.ParameterDescriptions.Any())
                return;

            var formParameters = context.ApiDescription.ParameterDescriptions
                .Where(p => p.Source.Equals(BindingSource.Form) && p.CustomAttributes().Any(attr => attr.GetType().Equals(typeof(JsonIgnoreAttribute))));

            foreach (var param in formParameters) {
                foreach (var content in operation.RequestBody.Content.Values) {
                    content.Schema.Properties.Remove(param.Name);
                }
            }

            var queryParameters = context.ApiDescription.ParameterDescriptions
                .Where(p => p.Source.Equals(BindingSource.Query) && p.CustomAttributes().Any(attr => attr.GetType().Equals(typeof(JsonIgnoreAttribute))));

            foreach (var param in queryParameters) {
                var parameterToRemove = operation.Parameters.SingleOrDefault(p => p.Name.Equals(param.Name));
                if (parameterToRemove != null) {
                    operation.Parameters.Remove(parameterToRemove);
                }
            }
        }
    }
}
