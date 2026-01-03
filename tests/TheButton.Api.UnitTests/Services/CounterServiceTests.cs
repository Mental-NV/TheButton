using Microsoft.VisualStudio.TestTools.UnitTesting;
using TheButton.Services;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TheButton.Api.UnitTests.Services;

[TestClass]
public class CounterServiceTests
{
    [TestMethod]
    public void GetCount_Initially_ReturnsZero()
    {
        var service = new CounterService();
        Assert.AreEqual(0, service.GetCount());
    }

    [TestMethod]
    public void Increment_IncreasesValueByOne()
    {
        var service = new CounterService();
        var result = service.Increment();
        Assert.AreEqual(1, result);
        Assert.AreEqual(1, service.GetCount());
    }

    [TestMethod]
    public void Increment_Concurrency_IsThreadSafe()
    {
        var service = new CounterService();
        var tasks = new List<Task>();
        int numberOfThreads = 100;
        int incrementsPerThread = 1000;

        for (int i = 0; i < numberOfThreads; i++)
        {
            tasks.Add(Task.Run(() =>
            {
                for (int j = 0; j < incrementsPerThread; j++)
                {
                    service.Increment();
                }
            }));
        }

        Task.WaitAll(tasks.ToArray());

        Assert.AreEqual(numberOfThreads * incrementsPerThread, service.GetCount());
    }
}
