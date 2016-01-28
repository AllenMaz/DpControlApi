using DpControl.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DpControl.UnitTest.UtilityTests
{
    public class ResponseHandlerTest
    {
        [Theory]
        [InlineData(new object[] { 404, "Cound not found" })]
        public void TestReturnError(int httpStatusCode,string message)
        {
            string errorJson = ResponseHandler.ReturnError(httpStatusCode,message);
            Assert.IsType(typeof(string), errorJson);
        }
    }
}
