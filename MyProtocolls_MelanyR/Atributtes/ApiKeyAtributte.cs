using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MyProtocolls_MelanyR.Atributtes
{

    //esta clase ayuda a limitar la forma en que se puede consumir un recurso de controlador(un end point)
    //basicamente vaos a crear una decoracion perzonalizada que incluya cierta funcionalidadya sea 
    //a todo un controladoro a un end point particular

    [AttributeUsage(validOn:AttributeTargets.All)]
    public sealed class ApiKeyAtributte : Attribute, IAsyncActionFilter
    {
        //especificamos cual es el clave: valor dentro de appsetings que queremos usar como ApiKey
        private readonly string _apiKey = "Progra6ApiKey";

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //aca validamos que en el body (en tipo json) del request valla la info de la ApiKey
            //si no va la info presentamos un mensaje de error indicando que falta ApiKey y que no
            //se puede consumir el recurso

            if (!context.HttpContext.Request.Headers.TryGetValue(_apiKey, out var ApiSalida))
            {
                context.Result = new ContentResult()
               {
                   StatusCode=401,
                   Content= "Llamada no contiene informacion de seguridad..."
                };
                return;

                //si no hay informacion de seguridad sale de la funcion y muestra este mensaje
            }

            //si viene info de seguridad falta validar si es la correcta para esto lo primero es extraer
            //el valor de Progra6ApiKey dentro de appsettings.json para poder comparar contra lo que viene ne el request
            var appSettings = context.HttpContext.RequestServices.GetRequiredService<IConfiguration>();
            var ApiKeyValue = appSettings.GetValue<string>(_apiKey);

            //falta comparar que las ApiKey sean iguales

            if(!ApiKeyValue.Equals(ApiSalida))
            {
                context.Result = new ContentResult()
                {
                  StatusCode =401,
                  Content="ApiKey invalidad..."
                };
                return;
            }

            await next();

        }



    }
}
