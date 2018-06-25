Monday	4-6pm	9-12pm		Refactor and CustomerDetail.html   
Tuesday	12-1pm	4-6pm	9-12pm	Add Customer Detail
Wednesday	12-1pm	4-6pm	9-12pm	Confirm Order Detail/Edit Order Detail
Thursday	12-1pm	4-6pm	9-12pm	PDF Create
Friday		4-6pm	9-12pm	PDF Download
Saturday	ALL DAY			Convert Quote to Invoice (Add Invoice Detail)
Sunday	ALL DAY			Confirm Invoice Detail/ Edit Invoice Detail


alter table OrderHeads
add [CompanyProfileId] [int] default(0) NOT NULL
go

alter table Users
add [CompanyProfileId] [int] default(0) NOT NULL
go


///////////////////////////////////////////////////////////////////

CREATE TABLE [dbo].[TaxRate](
  [TaxRateId] [int] identity(1, 1) NOT NULL,
  [TaxCode] [varchar](255) NOT NULL,
  [Rate] [decimal](19, 4) NOT NULL,
  [StartDate] [datetime2] NOT NULL,
  [EndDate] [datetime2] NOT NULL,
  [CreatedBy] [varchar](255) NOT NULL,
  [CreateDate] [datetime2] NOT NULL,
  [UpdatedBy] [varchar](255) NULL,
  [UpdateDate] [datetime2] NULL,
) 
GO

insert taxrate ( TaxCode, Rate, StartDate, EndDate, CreatedBy, CreateDate )
values ('VAT', 0.15, '1 April 2018', '1 January 2018', 'sa', getdate())


////////////////////////////////////////////////////////////////////


CREATE TABLE [dbo].[CompanyProfiles](
  [CompanyProfileId] [int] identity(1, 1) NOT NULL,
  [DisplayName] [varchar](255) NOT NULL,
  [LegalName] [varchar](255) NOT NULL,
  [VatRegistrationNo] [varchar](255) NULL,
  [EmailAddress] [varchar](255) NULL,
  [TelephoneNo] [varchar](255) NULL,
  [FaxNo] [varchar](255) NULL,
  [OrderNoSeed] [int] default(1) NOT NULL,
  [CreatedBy] [varchar](255) NOT NULL,
  [CreateDate] [datetime2] NOT NULL,
  [UpdatedBy] [varchar](255) NULL,
  [UpdateDate] [datetime2] NULL,
) 
GO

insert CompanyProfiles (DisplayName, LegalName, VatRegistrationNo, EmailAddress, TelephoneNo, FaxNo, CompanyAddressDetailId, CompanyBankingDetailId, OrderNoSeed, CreatedBy, CreateDate)
values ('MIA CC', 'MIA CC', 'VAT23242/01', 'gayamia@gmail.com', '0217124544', '0217124549', 1, 1, 1, 'sa', getdate())
////////////////////////////////////////////////////////////////////

CREATE TABLE [dbo].[CompanyAddressDetails](
  [CompanyAddressDetailId] [int] identity(1, 1) NOT NULL,
  [CompanyProfileId] [int] NOT NULL,
  [AddressLine1] [varchar](255) NOT NULL,
  [AddressLine2] [varchar](255) NOT NULL,
  [AddressCity] [varchar](255) NOT NULL,
  [AddressCountry] [varchar](255) NOT NULL,
  [AddressPostal] [varchar](255) NOT NULL,  
  [CreatedBy] [varchar](255) NOT NULL,
  [CreateDate] [datetime2] NOT NULL,
  [UpdatedBy] [varchar](255) NULL,
  [UpdateDate] [datetime2] NULL,
) 
GO

insert CompanyAddressDetails
values ('25 Anzio Crescent', 'Strandfontein', 'Cape Town', 'Republic of South Africa', '7798', 'sa', getdate(),'sa', getdate())

////////////////////////////////////////////////////////////////////


CREATE TABLE [dbo].[CompanyBankingDetails](
  [CompanyBankingDetailId] [int] identity(1, 1) NOT NULL,
  [CompanyProfileId] [int] NOT NULL,
  [BankCode] [varchar](255) NOT NULL,
  [AccountNo] [varchar](255) NOT NULL,
  [AccountType] [varchar](255) NOT NULL,
  [AccountHolder] [varchar](255) NOT NULL,
  [BranchCode] varchar](255) NULL,
  [CreatedBy] [varchar](255) NOT NULL,
  [CreateDate] [datetime2] NOT NULL,
  [UpdatedBy] [varchar](255) NULL,
  [UpdateDate] [datetime2] NULL,
) 
GO

insert CompanyBankingDetails
values (1, 'ABSA' ,'00454345','SAVINGS','J BLACKSMITH','632005', 'sa', getdate(),'sa', getdate())

////////////////////////////////////////////////////////////////////

public class OrderHead
{
public int OrderId { get; set; }
public string OrderNo { get; set; }
public decimal SubTotal { get; set; }
public decimal VatTotal { get; set; }
public decimal DiscountTotal { get; set; }
public decimal OrderTotal { get; set; }
public decimal VatRate { get; set; }
public int OrderNoSeed { get; set; }
public DateTime CreateDate { get; set; }
public string CreateUser { get; set; }
public DateTime? UpdateDate { get; set; }
public string UpdateUser { get; set; }
public List<OrderDetail> OrderDetails { get; set; }

public OrderHead()
{
  OrderDetails = new List<OrderDetail>();
}
}

// UI. ordersAPI. securityAPI
public class UserModel
{
	public string Username { get; set; }
	public string ApiSessionToken { get; set; }
	public bool IsAuthenticated { get; set; }
	public int CompanyProfileId { get; set; }
}

///////////////////////////////////////////////////////////////////
// UI. ordersAPI. 

public class FindCompanyOrdersRequestModel
{
	public string CompanyProfileId { get; set; }
}

/////////////////////////////////////////////////////////////////////
// ordersAPI. 

public class TaxRate
{
	public int TaxRateId { get; set; }
	public string TaxCode { get; set; }
	public decimal Rate { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
	public DateTime CreateDate { get; set; }
	public string CreateUser { get; set; }
	public DateTime? UpdateDate { get; set; }
	public string UpdateUser { get; set; }
}

//////////////////////////////////////////////////////////////////

// ordersAPI. 
public class TaxRateMapping : IEntityTypeConfiguration<TaxRate>
{
	public void Configure(EntityTypeBuilder<TaxRate> builder)
	{
	  builder.ToTable("TaxRate");

	  builder.HasKey("TaxRateId");
	}
}

/////////////////////////////////////////////////////////////////////
// ordersAPI. 

public class CompanyProfile
{
	public int CompanyProfileId { get; set; }
	public string DisplayName { get; set; }
	public string LegalName { get; set; }
	public string VatRegistrationNo { get; set; }
	public string EmailAddress { get; set; }
	public string TelephoneNo { get; set; }
	public string FaxNo { get; set; }
	public List<CompanyBankingDetail> BankDetails { get; set; }
	public List<CompanyAddressDetail> AddressDetails { get; set; }
	public int OrderNoSeed { get; set; }
	public string CreateUser { get; set; }
	public DateTime CreateDate { get; set; }
	public string UpdateUser { get; set; }
	public DateTime? UpdateDate { get; set; }
}

//////////////////////////////////////////////////////////////////

// ordersAPI. 
public class CompanyProfileMapping : IEntityTypeConfiguration<CompanyProfile>
{
	public void Configure(EntityTypeBuilder<CompanyProfile> builder)
	{
	  builder.ToTable("CompanyProfiles");

	  builder.HasKey("CompanyProfileId");
	  
	  builder.HasMany(cp => cp.BankDetails);
	  builder.HasMany(cp => cp.AddressDetails);
	}
}

/////////////////////////////////////////////////////////////////////
// ordersAPI.  

public class CompanyAddressDetail
{
	public int CompanyAddressDetailId { get; set; }
	public int CompanyProfileId { get; set; }
	public string AddressLine1 { get; set; }
	public string AddressLine2 { get; set; }
	public string AddressCity { get; set; }
	public string AddressCountry { get; set; }
	public string AddressPostal { get; set; }
	public string CreateUser { get; set; }
	public DateTime CreateDate { get; set; }
	public string UpdateUser { get; set; }
	public DateTime? UpdateDate { get; set; }
}

//////////////////////////////////////////////////////////////////

// ordersAPI. 
public class CompanyAddressDetailMapping : IEntityTypeConfiguration<CompanyAddressDetail>
{
	public void Configure(EntityTypeBuilder<CompanyAddressDetail> builder)
	{
	  builder.ToTable("CompanyAddressDetails");

	  builder.HasKey("CompanyAddressDetailId");
	  
	}
}

/////////////////////////////////////////////////////////////////////
// ordersAPI.  

public class CompanyBankingDetail
{
	public int CompanyBankingDetailId { get; set; }
	public int CompanyProfileId { get; set; }
	public string BankCode { get; set; }
	public string AccountNo { get; set; }
	public string AccountType { get; set; }
	public string AccountHolder { get; set; }
	public string BranchCode { get; set; }
	public string CreateUser { get; set; }
	public DateTime CreateDate { get; set; }
	public string UpdateUser { get; set; }
	public DateTime? UpdateDate { get; set; }
}

//////////////////////////////////////////////////////////////////

// ordersAPI. 
public class CompanyBankingDetailMapping : IEntityTypeConfiguration<CompanyBankingDetail>
{
	public void Configure(EntityTypeBuilder<CompanyBankingDetail> builder)
	{
	  builder.ToTable("CompanyBankingDetails");

	  builder.HasKey("CompanyBankingDetailId");
	  
	}
}

//////////////////// //////////////////////////////////////
/// Context class
//  ordersAPI. 

public DbSet<TaxRate> TaxRates { get; set; }
public DbSet<CompanyProfile> CompanyProfiles { get; set; }

modelBuilder.ApplyConfiguration(new TaxRateMapping());
modelBuilder.ApplyConfiguration(new CompanyProfileMapping());
modelBuilder.ApplyConfiguration(new CompanyAddressDetailMapping());
modelBuilder.ApplyConfiguration(new CompanyBankingDetailMapping());

//////////////////// //////////////////// //////////////////// //////////////////// //////////////////// 
// ordersAPI. 
public static class OrdersConstants
{
	public const string VatTaxCode = "VAT";
}

//////////////////////////////////////////

public class GetCompanyOrderNoSeedRequestModel
{
	public string CompanyProfileId { get; set; }
}

public class FindCompanyOrdersRequestModel
{
	public string CompanyProfileId { get; set; }
}

public class FindCompanyOrdersPeriodRequestModel
{
	public string CompanyProfileId { get; set; }
	public DateTime StartDate { get; set; }
	public DateTime EndDate { get; set; }
}

///////////////////////////////////////////

// ordersAPI. 
[HttpPost, Route("vatrate")]
public decimal GetVatRate()
{
  var vatTax = Context.TaxRates.First(r => r.TaxCode = OrdersConstants.VatTaxCode && r.StartDate.Date <= DateTime.Now.Date && r.EndDate.Date >= DateTime.Now.Date);

  return vatTax.Rate;
}

[HttpPost, Route("getseed")]
public int GetOrderNoSeed([FromBody]GetCompanyOrderNoSeedRequestModel requestModel )
{
  return Context.CompanyProfiles.First(cp => cp.CompanyProfileId == requestModel.CompanyProfileId).OrderNoSeed;
}

[HttpPost, Route("add")]
public int AddOrder([FromBody]AddOrderRequestModel requestModel) 
{
  var existingOrder = Context.Orders.FirstOrDefault(o => o.OrderNo == requestModel.OrderNo);

  if (existingOrder != null)
	return existingOrder.OrderId;

  var newOrder = new OrderHead
  {
	OrderNo = requestModel.OrderNo,
	SubTotal = 0M,
	VatTotal = 0M,
	DiscountTotal = 0M,
	OrderTotal = 0M,
	VatRate = 0.15M, // to get from the context
	CreateDate = DateTime.Now,
	CreateUser = requestModel.Username
  };

  Context.Orders.Add(newOrder);
  Context.CompanyProfiles.First(cp => cp.CompanyProfileId == requestModel.CompanyProfileId).OrderNoSeed += 1;
  Context.SaveChanges();

  return newOrder.OrderId; 
}

[HttpPost, Route("user")]
public List<HomeOrdersModel> GetUserOrders([FromBody]FindUserOrdersRequestModel requestModel)
{
  var noOrders = new List<HomeOrdersModel>();

  var userOrders = Context.Orders.Where(o => o.CreateUser == requestModel.Username).OrderByDescending(o => o.CreateDate)
	.Take(50).Select(o => new HomeOrdersModel
	{
	  OrderId = o.OrderId,
	  OrderNo = o.OrderNo,
	  CreateDate = o.CreateDate.ToShortDateString(),
	  Total = o.OrderTotal.ToString("R # ###.#0")
	}).ToList();

  return userOrders ?? noOrders;
}

[HttpPost, Route("company")]
public List<HomeOrdersModel> GetCompanyOrders([FromBody]FindCompanyOrdersRequestModel requestModel)
{
  var noOrders = new List<HomeOrdersModel>();

  var companyOrders = Context.Orders.Where(o => o.CompanyProfileId == requestModel.CompanyProfileId).OrderByDescending(o => o.CreateDate)
	.Take(50).Select(o => new HomeOrdersModel
	{
	  OrderId = o.OrderId,
	  OrderNo = o.OrderNo,
	  CreateDate = o.CreateDate.ToShortDateString(),
	  Total = o.OrderTotal.ToString("R # ###.#0")
	}).ToList();

  return companyOrders ?? noOrders;
}

[HttpPost, Route("companyperiod")]
public List<HomeOrdersModel> GetCompanyOrdersInPeriod([FromBody]FindCompanyOrdersPeriodRequestModel requestModel)
{
  var noOrders = new List<HomeOrdersModel>();

  var userOrders = Context.Orders.Where(o => o.CompanyProfileId == requestModel.CompanyProfileId && o.CreateDate.Date >= requestModel.StartDate && o.CreateDate.Date <= requestModel.EndDate)
	.Select(o => new HomeOrdersModel
	{
	  OrderId = o.OrderId,
	  OrderNo = o.OrderNo,
	  CreateDate = o.CreateDate.ToShortDateString(),
	  Total = o.OrderTotal.ToString("R 0 000.00")
	}).ToList();

  return userOrders ?? noOrders;
}
	
	