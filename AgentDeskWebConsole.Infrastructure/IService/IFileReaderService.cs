using System.Collections.Generic;

namespace AgentDeskWebConsole.Infrastructure.IService
{
    public interface IFileReaderService
    {
        List<Domain.ImportLI> ReadExcel(string filePath);
    }
}
