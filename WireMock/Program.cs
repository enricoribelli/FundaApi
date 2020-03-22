using System;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;
using WireMock.Settings;

namespace WireMock
{
    class Program
    {
        static void Main(string[] args)
        {
            var bodyFromExternalFundaApi = "{{'Objects':[{{'MakelaarNaam':'TestMakelaar1'}},{{'MakelaarNaam':'TestMakelaar2'}}]}}";

            var stub = FluentMockServer.Start(new FluentMockServerSettings
            {
                Urls = new[] { "http://localhost:44384" },
                StartAdminInterface = true
            });
            stub
                .Given(
                    Request.Create().WithPath("/FundaApi?*").UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "text/plain")
                        .WithBody(bodyFromExternalFundaApi)
                );

            Console.ReadLine();
        }
    }
}
