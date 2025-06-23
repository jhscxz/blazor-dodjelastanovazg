using DodjelaStanovaZG.Enums;

namespace DodjelaStanovaZG.Services.Interfaces;

public interface IExcelExportService
{
    Task<byte[]> ExportNatjecajAsync(long natjecajId, RezultatObrade? filter);
}