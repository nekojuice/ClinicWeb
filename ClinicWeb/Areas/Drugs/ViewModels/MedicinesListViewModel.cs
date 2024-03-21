using System.ComponentModel.DataAnnotations;

namespace ClinicWeb.Areas.Drugs.ViewModels
{
    public class MedicinesListViewModel
    {
        [Display(Name = "藥品ID")]
        public int? DrugId { get; set; }

        [Display(Name = "藥品代碼")]
        public string? DrugCode { get; set; }

        [Display(Name = "藥品學名")]
        public string? GenericName { get; set; }

        [Display(Name = "藥品商品名")]
        public string? TradeName { get; set; }

        [Display(Name = "藥品中文名")]
        public string? DrugName { get; set; }
       
        [Display(Name = "藥品常用劑量")]
        public string? DrugDose { get; set; }

        [Display(Name = "藥品最大劑量")]
        public string? MaxDose { get; set; }

        [Display(Name = "藥品警語/禁忌")]
        public string? Precautions { get; set; }

        [Display(Name = "藥品注意事項")]
        public string? Warnings { get; set; }

        [Display(Name = "懷孕風險藥品分級類別")]
        public string? PregnancyCategory { get; set; }

        [Display(Name = "藥品圖片")]
        public string? Apperance { get; set; }

        [Display(Name = "保存方法")]
        public string? Storage { get; set; }

        [Display(Name = "藥商")]
        public string? Supplier { get; set; }

        [Display(Name = "廠牌")]
        public string? Brand { get; set; }

        [Display(Name = "用法")]
        public string? Dosage { get; set; }
        //口服、外用等

        //------------------------------------------------------
        //ViewModels/PartialModels


        
        
    }
}
