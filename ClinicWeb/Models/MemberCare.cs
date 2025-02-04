﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ClinicWeb.Models;

[Table("Member_Care")]
public partial class MemberCare
{
    [Column("NewBorn_ID")]
    public int NewBornId { get; set; }

    [Key]
    [Column("Care_ID")]
    public int CareId { get; set; }

    [Column("Care_Date", TypeName = "datetime")]
    public DateTime CareDate { get; set; }

    [Column("Record_Type")]
    [StringLength(50)]
    public string RecordType { get; set; }

    [Column("Record_dcp")]
    [StringLength(50)]
    public string RecordDcp { get; set; }

    [ForeignKey("NewBornId")]
    [InverseProperty("MemberCare")]
    public virtual CasesNewBornList NewBorn { get; set; }
}