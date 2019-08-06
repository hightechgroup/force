using Force.Ddd;
using Xunit;

namespace Force.Tests.Ddd
{
    public class ResultTests
    {
        [Fact]
        public void A()
        {
            var r1 = new Result<int, string>(1);
            var r2 = new Result<int, string>("false");
            r1.Match(x => 1.ToString(), x => x);

            var a = from r11 in r1 select r1;
            
            var b = 
                from r11 in r1 
                from r22 in r2     
                select r11 + r22;
        }
    }
}