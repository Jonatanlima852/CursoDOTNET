using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using ApiCatalogo.Models;

namespace ApiCatalogo.Extensions; // define um namespace para estender funcionalidades da aplicação

public static class ApiCatalogoExtensions
{
    public static void ConfigureExceptionHandler(this IApplicationBuilder app)
    {
        // configurando um middleware para tratar exceções de forma centralizada
        app.UseExceptionHandler(appError => 
        {
            // em caso de erro, define o que será feito   
            appError.Run(async context => 
            {
                // definindo status da resposta e como erro interno e o tipo de conte´do da resposta  
                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                context.Response.ContentType = "application/json";

                // obtendo mais informações sobre o erro a partir do Handler
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if(contextFeature != null)
                {
                    // enviando a resposta em json em detalhes
                    await context.Response.WriteAsync(new ErrorDetails()
                    {
                        StatusCode = context.Response.StatusCode,
                        Message = contextFeature.Error.Message,
                        Trace = contextFeature.Error.StackTrace //Rastro da pilha de chamadas
                    }.ToString());
                }
            });
        });
    }
}