﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebApplication1
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class masterEntities : DbContext
    {
        public masterEntities()
            : base("name=masterEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<TableCurrency> TableCurrency { get; set; }
        public virtual DbSet<TableDepartment> TableDepartment { get; set; }
        public virtual DbSet<TableDividends> TableDividends { get; set; }
        public virtual DbSet<TableExchange> TableExchange { get; set; }
        public virtual DbSet<TablePortfolio> TablePortfolio { get; set; }
        public virtual DbSet<TableSector> TableSector { get; set; }
        public virtual DbSet<TableStock> TableStock { get; set; }
        public virtual DbSet<TableUser> TableUser { get; set; }
    }
}
