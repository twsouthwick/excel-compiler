using System.Text;
using System.Threading.Channels;

namespace ExcelCompiler
{
    internal class Pipelines
    {
        public void Test()
        {
            var channel = Channel.CreateUnbounded<int>(new UnboundedChannelOptions
            {

            });
        }
    }
}
