using Microsoft.EntityFrameworkCore.Query.Internal;

namespace MasterNet.Application.Precios.GetPrecios;

public record PrecioResponse(
    Guid? Id,
    string? Nombre,
    decimal? PrecioActual,
    decimal? PrecioPromocion
)
{
    public PrecioResponse(): this(null, null, null, null)
    {
    }
}