using CCTest.Service.Contracts;
using CCTest.UnitTest.Services;
using Xunit;

namespace CCTest.UnitTest.Contrllers
{
    public class SupportControllerTest
    {
        private const string Ok = "Ok";
        private const string NOk = "NOk";
        private readonly ISupportService _supportService;

        public SupportControllerTest()
        {
            _supportService = new SupportServiceTest();
        }

        [Fact]
        public async void Get_OK_Response()
        {
            // Act
            var okResult = await _supportService.InitiateSupportRequest("Ok");
            // Assert
            Assert.Equal(okResult.Message, Ok);
        }
        [Fact]
        public async void Get_NOK_Response()
        {
            // Act
            var nOKResult = await _supportService.InitiateSupportRequest("NOk");
            // Assert
            Assert.Equal(nOKResult.Message, NOk);
        }
    }
}
