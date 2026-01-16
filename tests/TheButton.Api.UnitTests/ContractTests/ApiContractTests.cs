using VerifyMSTest;
using V2 = TheButton.Api.Features.V2.Counter;
using V3 = TheButton.Api.Features.V3.Counter;

namespace TheButton.Api.UnitTests.ContractTests;

[TestClass]
public class ApiContractTests : VerifyBase
{
    [TestMethod]
    public async Task CounterResponse_V2_Contract()
    {
        var response = new V2.CounterResponse(123);
        await Verify(response);
    }

    [TestMethod]
    public async Task CounterResponse_V3_Contract()
    {
        var response = new V3.CounterResponse(123, 45);
        await Verify(response);
    }
}
