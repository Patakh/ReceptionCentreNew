﻿using System.ComponentModel.DataAnnotations.Schema;

namespace ReceptionCentreNew.Domain.Models.Entities.Functions;
public class ChartClaimInYear
{
    [Column("out_date")]
    public string OutDate { get; set; }
     
    [Column("out_count_claim")]
    public int OutCountClaim { get; set; }
}