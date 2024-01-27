﻿// <auto-generated> This file has been auto generated by EF Core Power Tools. </auto-generated>
#nullable disable
using System;
using System.Collections.Generic;

namespace ClinicWeb.Models;

public partial class TOrder
{
    public int Id { get; set; }

    public string FOrderId { get; set; }

    public DateTime FOrderDate { get; set; }

    public DateTime? FCheckPayDate { get; set; }

    public DateTime? FShipDate { get; set; }

    public DateTime? FGetDate { get; set; }

    public int FShipPrice { get; set; }

    public int FOrderPrice { get; set; }

    public string FReceiverName { get; set; }

    public string FReceiverPhone { get; set; }

    public string FReceiverAddress { get; set; }

    public string FMemo { get; set; }

    public string FAccountFiveNumber { get; set; }

    public string FPayType { get; set; }

    public string FShipType { get; set; }

    public int FMemberId { get; set; }

    public int? FCouponIdforAmount { get; set; }

    public int? FCouponIdforPercent { get; set; }

    public int? FCouponIdforShip { get; set; }

    public virtual TCoupon FCouponIdforAmountNavigation { get; set; }

    public virtual TCoupon FCouponIdforPercentNavigation { get; set; }

    public virtual TCoupon FCouponIdforShipNavigation { get; set; }

    public virtual MemberMemberList FMember { get; set; }

    public virtual ICollection<TOrderDetail> TOrderDetail { get; set; } = new List<TOrderDetail>();
}