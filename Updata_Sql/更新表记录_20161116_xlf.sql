--增加P_Product_Efficiency_collect表，统计P_SFC_State中数据
CREATE TABLE [dbo].[P_Product_Efficiency_collect](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[workshop_code] [nvarchar](30) NOT NULL,
	[product_code] [nvarchar](30) NOT NULL,
	[years] [nvarchar](10) NOT NULL,
	[months] [nvarchar](10) NOT NULL,
	[days] [nvarchar](10) NOT NULL,
	[weeks] [nvarchar](10) NULL,
	[spt] [int] NULL,
	[curr_spt] [int] NULL,
	[remarks] [nvarchar](50) NULL,
 CONSTRAINT [PK_P_Product_Efficiency_collect] PRIMARY KEY NONCLUSTERED 
(
	[workshop_code] ASC,
	[product_code] ASC,
	[years] ASC,
	[months] ASC,
	[days] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO
