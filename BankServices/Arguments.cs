using CommandLine;

namespace BankServices
{
    public class Arguments
    {
        [Option("fileName", Required = true, HelpText = "Test File Name")]
        public string FileName { get; set; } = "";

        [Option("serverNum", Required = false, HelpText = "Servers Number")]
        public int ServerNum { get; set; } = 2;

        [Option("timeInterval", Required = false, HelpText = "Simulate Time Interval")]
        public int TimeInterval { get; set; } = 200;
    }
}