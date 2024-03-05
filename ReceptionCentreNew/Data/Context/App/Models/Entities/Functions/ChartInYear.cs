using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.Identity.Client;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class ChartInYear
{
    [Column("out_date")]
    public string? OutDate {  get; set; }

    [Column("out_count_outgoing")]
    public int OutCountOutgoing {  get; set; }

    [Column("out_count_incoming")]
    public int OutCountIncoming {  get; set; }
}