using System;
using System.Collections.Generic;
using SA_Management.Models;
using Microsoft.EntityFrameworkCore;

namespace SA_Management.DataAccessLayer.Data
{
	public class AppDbContext : Microsoft.EntityFrameworkCore.DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{

		}
		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Relationship: ShareCompany has many Trades
			modelBuilder.Entity<Trade>()
				.HasOne(t => t.ShareCompany)
				.WithMany(sc => sc.Trades)
				.HasForeignKey(t => t.CompanyID);

			// Relationship: ShareCompany has one CompanyPortfolio
			modelBuilder.Entity<CompanyPortfolio>()
				.HasOne(cp => cp.ShareCompany)
				.WithOne(sc => sc.CompanyPortfolio)
				.HasForeignKey<CompanyPortfolio>(cp => cp.CompanyID);

			// Relationship: Trade has many TradeDetails
			modelBuilder.Entity<TradeDetails>()
				.HasOne(td => td.Trade)
				.WithMany(t => t.TradeDetails)
				.HasForeignKey(td => td.TradeID);

			// modelBuilder.Entity<CompositeKey>()
			// .HasKey(t=>new{t.BrokerID,t.TradeDetailsID});

		}

		public DbSet<Trade> Trades { get; set; }
		public DbSet<ShareCompany> ShareCompanies { get; set; }
		public DbSet<TradeDetails> TradeDetails { get; set; }
		public DbSet<CompanyPortfolio> CompanyPortfolios { get; set; }
		public DbSet<BrokerDetails> BrokerDetails { get; set; }

	}
}

