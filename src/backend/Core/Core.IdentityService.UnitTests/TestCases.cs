using Core.CryptService.Impl;
using Core.IdentityService.UnitTests.Models;
using Xunit;

namespace Core.IdentityService.UnitTests
{
    public static class TestCases
    {
        public static TheoryData<SignInTestData> GetSignInTestData()
        {
            var hashService = new HashService();

            var theoryData = new TheoryData<SignInTestData>();

            theoryData.Add(new SignInTestData()
            {
                IsSucessOperation = true,
                UserName = "testuser1",
                Password = "test",
                HashPassword = hashService.GetHash("test"),
            });

            theoryData.Add(new SignInTestData()
            {
                UserName = "testuser2",
                Password = "test2",
                HashPassword = hashService.GetHash("test2"),
                IsSucessOperation = true
            });

            theoryData.Add(new SignInTestData()
            {
                UserName = "testuser13",
                Password = "test3",
                HashPassword = hashService.GetHash("test3"),
                IsSucessOperation = true
            });

            theoryData.Add(new SignInTestData()
            {
                UserName = "testuser14",
                Password = "test4",
                HashPassword = hashService.GetHash("test4"),
                IsSucessOperation = true
            });

            theoryData.Add(new SignInTestData()
            {
                UserName = "testuser15",
                Password = "test",
                HashPassword = hashService.GetHash("test"),
                IsSucessOperation = true
            });

            theoryData.Add(new SignInTestData()
            {
                UserName = "testuser15",
                Password = "te1st",
                HashPassword = hashService.GetHash("testasd2"),
                IsSucessOperation = false
            });

            theoryData.Add(new SignInTestData()
            {
                UserName = "testuser11",
                Password = "test",
                HashPassword = hashService.GetHash("te1123123123st"),
                IsSucessOperation = false
            });

            return theoryData;
        }

        public static TheoryData<SignUpTestData> GetSignUpTestData()
        {
            var theoryData = new TheoryData<SignUpTestData>();

            theoryData.Add(new SignUpTestData()
            {
                Email = "test123@mail.ru",
                UserName = "test1234",
                Password = "asdvcxvzqer112!",
                Phone = "+380956684159"
            });
            theoryData.Add(new SignUpTestData()
            {
                Email = "test1234@mail.ru",
                UserName = "test12345",
                Password = "asdvcxvzqer112!",
                Phone = "+380956684150"
            });
            theoryData.Add(new SignUpTestData()
            {
                Email = "test12356@mail.ru",
                UserName = "test125634",
                Password = "asdvcxv56zqer112!",
                Phone = "+38095668415659"
            });
            theoryData.Add(new SignUpTestData()
            {
                Email = "tes12t123@mail.ru",
                UserName = "te12st1234",
                Password = "asdv12cxvzqer112!",
                Phone = "+38095661284159"
            });
            theoryData.Add(new SignUpTestData()
            {
                Email = "test13223@mail.ru",
                UserName = "test431234",
                Password = "asdvcx5325vzqer112!",
                Phone = "+3809566841132459"
            });

            return theoryData;
        }

        public static TheoryData<UpdateRefreshTokenTestData> GetUpdateRefreshTokenTestData()
        {
            var result = new TheoryData<UpdateRefreshTokenTestData>();

            result.Add(new UpdateRefreshTokenTestData()
            {
                IsSucess = true,
                RefreshToken = "12345678910111234123"
            });
            result.Add(new UpdateRefreshTokenTestData()
            {
                IsSucess = true,
                RefreshToken = "123456789101112341231"
            });
            result.Add(new UpdateRefreshTokenTestData()
            {
                IsSucess = true,
                RefreshToken = "1234567891011123412311"
            });
            result.Add(new UpdateRefreshTokenTestData()
            {
                IsSucess = true,
                RefreshToken = "123456789101112341231132"
            });

            result.Add(new UpdateRefreshTokenTestData()
            {
                IsSucess = false,
                RefreshToken = "asdfafdsfcx"
            });

            result.Add(new UpdateRefreshTokenTestData()
            {
                IsSucess = false,
                RefreshToken = "acvdsvcfvbdagadsffads"
            });

            return result;
        }
    }
}
