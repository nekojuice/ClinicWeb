﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ClinicWeb.Models;

public partial class CasesNewBornList
{
    public int MemberId { get; set; }

    public int NewBornId { get; set; }

    public string Name { get; set; }

    public DateTime BirthDate { get; set; }

    public bool Gender { get; set; }

    public string BloodType { get; set; }

    public string IceName { get; set; }

    public string IceNumber { get; set; }

    public virtual MemberMemberList Member { get; set; }

    public virtual ICollection<MemberCare> MemberCare { get; set; } = new List<MemberCare>();
}