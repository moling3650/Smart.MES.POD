--���ӱ�B_StationEquipment
CREATE TABLE [dbo].[B_StationEquipment](
	[id] [INT] IDENTITY(1,1) NOT NULL,
	[station_code] [NVARCHAR](30) NOT NULL,
	[equipment_code] [NVARCHAR](30) NOT NULL,
	[remark] [NVARCHAR](128) NULL,
 CONSTRAINT [PK_B_StationEquipment] PRIMARY KEY NONCLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--����P_FailLog �������ֶ�
alter table P_FailLog add equipment_code [NVARCHAR](30) DEFAULT NULL ;

--����P_SFC_Process_IOLog �������ֶ�
alter table P_SFC_Process_IOLog add initqty decimal(10, 3) DEFAULT 0 ;