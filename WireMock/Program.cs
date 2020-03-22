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
            var bodyFromExternalFundaApi = "{'Objects':[{'MakelaarNaam':'TestMakelaar1'},{'MakelaarNaam':'TestMakelaar1'},{'MakelaarNaam':'TestMakelaar2'}]}";
            var emptyBodyFromExternalFundaApi = "{'Objects':[]}";

            //http://localhost:44384/FundaApi/?type=koop&amp;zo=/amsterdam/&page=1&pagesize=25

            var stub = FluentMockServer.Start(new FluentMockServerSettings
            {
                Urls = new[] { "http://localhost:44384" },
                StartAdminInterface = true
            });
            stub
                .Given(
                    Request.Create().WithUrl("http://localhost:44384/FundaApi/?type=koop&amp;zo=/amsterdam/&page=1&pagesize=25").UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "text/plain")
                        .WithBody(bodyFromExternalFundaApi)
                );

            stub
                .Given(
                    Request.Create().WithUrl("http://localhost:44384/FundaApi/?type=koop&amp;zo=/amsterdam/&page=2&pagesize=25").UsingGet()
                )
                .RespondWith(
                    Response.Create()
                        .WithStatusCode(200)
                        .WithHeader("Content-Type", "text/plain")
                        .WithBody(emptyBodyFromExternalFundaApi)
                );

            Console.ReadLine();
        }
    }
}
