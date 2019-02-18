--更新P_FailLog 表，增加字段
alter table P_FailLog add qty decimal(10,3) not null default(0.0);
alter table P_FailLog add ng_remark nvarchar(512) ;
alter table P_FailLog add repair_remark nvarchar(512) ;
alter table P_FailLog add p_date datetime ;

--更新P_Fail_Detail表，增加字段
alter table P_Fail_Detail add qty decimal(10,3) not null default(0.0);

--增加查询P_FailLog视图V_P_FailLog_Name，+ 工序名称
create view V_P_FailLog_Name
as  
SELECT dbo.P_FailLog.fid, dbo.P_FailLog.fguid, dbo.P_FailLog.order_no, dbo.P_FailLog.sfc, dbo.P_FailLog.from_process, dbo.P_FailLog.from_station, dbo.P_FailLog.from_emp, 
               dbo.P_FailLog.process_code, dbo.P_FailLog.Disposal_Process, dbo.P_FailLog.fail_times, dbo.P_FailLog.state, dbo.P_FailLog.input_time, dbo.P_FailLog.finish_time, dbo.P_FailLog.emp_code, 
               dbo.B_ProcessList.process_name AS from_process_name, A.process_name, dbo.P_FailLog.ng_remark, dbo.P_FailLog.repair_remark, dbo.B_ProcessList.processid, A.processid AS Expr1, 
               dbo.P_FailLog.qty
FROM  dbo.P_FailLog INNER JOIN
               dbo.B_ProcessList AS A ON dbo.P_FailLog.process_code = A.process_code INNER JOIN
               dbo.B_ProcessList ON dbo.P_FailLog.from_process = dbo.B_ProcessList.process_code

union 
SELECT dbo.P_FailLog.fid, dbo.P_FailLog.fguid, dbo.P_FailLog.order_no, dbo.P_FailLog.sfc, dbo.P_FailLog.from_process, dbo.P_FailLog.from_station, dbo.P_FailLog.from_emp, 
               dbo.P_FailLog.process_code, dbo.P_FailLog.Disposal_Process, dbo.P_FailLog.fail_times, dbo.P_FailLog.state, dbo.P_FailLog.input_time, dbo.P_FailLog.finish_time, dbo.P_FailLog.emp_code, 
               '结束' AS from_process_name,  A.process_name, dbo.P_FailLog.ng_remark, dbo.P_FailLog.repair_remark, 0 as processid, 0 AS Expr1, 
               dbo.P_FailLog.qty
FROM  dbo.P_FailLog INNER JOIN
               dbo.B_ProcessList AS A ON dbo.P_FailLog.process_code = A.process_code 
where dbo.P_FailLog.from_process='END' 

union 
SELECT dbo.P_FailLog.fid, dbo.P_FailLog.fguid, dbo.P_FailLog.order_no, dbo.P_FailLog.sfc, dbo.P_FailLog.from_process, dbo.P_FailLog.from_station, dbo.P_FailLog.from_emp, 
               dbo.P_FailLog.process_code, dbo.P_FailLog.Disposal_Process, dbo.P_FailLog.fail_times, dbo.P_FailLog.state, dbo.P_FailLog.input_time, dbo.P_FailLog.finish_time, dbo.P_FailLog.emp_code, 
                dbo.B_ProcessList.process_name AS from_process_name,  '结束'  as process_name, dbo.P_FailLog.ng_remark, dbo.P_FailLog.repair_remark, 0 as processid, 0 AS Expr1, 
               dbo.P_FailLog.qty
FROM  dbo.P_FailLog INNER JOIN
               dbo.B_ProcessList ON dbo.P_FailLog.from_process = dbo.B_ProcessList.process_code
where dbo.P_FailLog.process_code ='END' 
               
               
               
--修改P_Fail_Detail表列名
--ALTER TABLE P_Fail_Detail rename COLUMN type_code TO reasontype_code
sp_rename 'P_Fail_Detail.type_code','reasontype_code','column'

--增加视图V_Fail_Detail_NGName，
create view V_Fail_Detail_NGName
as  
SELECT dbo.P_FailLog.fid, dbo.P_Fail_Detail.fguid, dbo.P_Fail_Detail.order_no, dbo.P_Fail_Detail.sfc, dbo.P_Fail_Detail.ng_code, dbo.P_Fail_Detail.reason_code, dbo.P_Fail_Detail.reasontype_code, 
               dbo.P_Fail_Detail.input_time, dbo.P_Fail_Detail.finish_time, dbo.P_Fail_Detail.pass, dbo.B_NG_Code.ng_name, dbo.B_NG_Code.type_code, dbo.B_NG_Code.exec_proc, dbo.P_Fail_Detail.id, 
               dbo.B_NG_Code.ng_id, dbo.P_Fail_Detail.qty
FROM  dbo.P_Fail_Detail INNER JOIN
               dbo.B_NG_Code ON dbo.P_Fail_Detail.ng_code = dbo.B_NG_Code.ng_code INNER JOIN
               dbo.P_FailLog ON dbo.P_Fail_Detail.fguid = dbo.P_FailLog.fguid
                      
--增加表B_NG_ReasonType
CREATE TABLE [dbo].[B_NG_ReasonType](
	[ReasonType_id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[reasontype_code] [nvarchar](30) NULL,
	[reasontype_name] [nvarchar](30) NULL,
	[description] [nvarchar](30) NULL,
 CONSTRAINT [PK_B_NG_ReasonType] PRIMARY KEY CLUSTERED 
(
	[ReasonType_id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]



--增加视图V_Order_Produc_Type_Name,根据工单获取产品编号，再得到产品类型（成品，半成品）
create view V_Order_Produc_Type_Name
as  
SELECT dbo.P_WorkOrder.order_no, dbo.P_WorkOrder.parent_order, dbo.P_WorkOrder.product_code, dbo.P_WorkOrder.qty, dbo.B_Product.product_name, dbo.B_Product.typecode, 
               dbo.B_Product_Type.type_name, dbo.P_WorkOrder.id, dbo.B_Product.product_id, dbo.B_Product_Type.type_id
FROM  dbo.P_WorkOrder INNER JOIN
               dbo.B_Product ON dbo.P_WorkOrder.product_code = dbo.B_Product.product_code INNER JOIN
               dbo.B_Product_Type ON dbo.B_Product.typecode = dbo.B_Product_Type.typecode
               
               
               
--增加B_ProcessType表数据
insert B_ProcessType(type_code,type_name,desccription)values('WX','维修','维修')

--增加表P_SFC_ProcessData_Back,备份被替换的批次信息
CREATE TABLE [dbo].P_SFC_ProcessData_Back(
	[id] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[fguid] [nvarchar](50) NULL,
	[order_no] [nvarchar](40) NULL,
	[SFC] [nvarchar](40) NULL,
	[mat_code] [nvarchar](30) NULL,
	[val] [nvarchar](30) NULL,
	[New_val] [nvarchar](40) NULL,
	[P_Date] [datetime] NULL,
	[Remark] [nvarchar](512) NULL,
 CONSTRAINT [PK_P_SFC_ProcessData_Back] PRIMARY KEY CLUSTERED 
(
	[id] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

--2016.11.09
--更新P_FailLog 表，增加字段
alter table P_FailLog add ws_code nvarchar(30) ;
--更新P_Fail_Detail表，增加字段
alter table P_Fail_Detail add ws_code nvarchar(30) ;

--View [dbo].[View_PFailLog_PWorkOrder]
CREATE VIEW [dbo].[View_PFailLog_PWorkOrder]
AS
SELECT     dbo.P_FailLog.fid, dbo.P_FailLog.fguid, dbo.P_FailLog.order_no, dbo.P_FailLog.sfc, dbo.P_FailLog.from_process, dbo.P_FailLog.from_station, 
                      dbo.P_FailLog.from_emp, dbo.P_FailLog.process_code, dbo.P_FailLog.Disposal_Process, dbo.P_FailLog.fail_times, dbo.P_FailLog.state, dbo.P_FailLog.input_time, 
                      dbo.P_FailLog.finish_time, dbo.P_FailLog.emp_code, dbo.P_FailLog.qty, dbo.P_FailLog.ng_remark, dbo.P_FailLog.repair_remark, dbo.P_FailLog.p_date, 
                      dbo.P_WorkOrder.product_code, dbo.P_WorkOrder.main_order
FROM         dbo.P_FailLog INNER JOIN
                      dbo.P_WorkOrder ON dbo.P_FailLog.order_no = dbo.P_WorkOrder.order_no


GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "P_FailLog"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 215
               Right = 209
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "P_WorkOrder"
            Begin Extent = 
               Top = 6
               Left = 247
               Bottom = 174
               Right = 411
            End
            DisplayFlags = 280
            TopColumn = 10
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 2055
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_PFailLog_PWorkOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_PFailLog_PWorkOrder'
GO

--View [dbo].[View_PSFCState_PWorkOrder] 
CREATE VIEW [dbo].[View_PSFCState_PWorkOrder]
AS
SELECT     dbo.P_SFC_State.id, dbo.P_SFC_State.order_no, dbo.P_SFC_State.SFC, dbo.P_SFC_State.qty, dbo.P_SFC_State.is_tray, dbo.P_SFC_State.fail_times, 
                      dbo.P_SFC_State.state, dbo.P_SFC_State.input_station, dbo.P_SFC_State.from_process, dbo.P_SFC_State.now_process, dbo.P_SFC_State.begin_time, 
                      dbo.P_SFC_State.process_time, dbo.P_SFC_State.end_time, dbo.P_SFC_State.grade_id, dbo.P_SFC_State.grade_type, dbo.P_SFC_State.begin_Date, 
                      dbo.P_SFC_State.begin_classcode, dbo.P_SFC_State.end_Date, dbo.P_SFC_State.end_classcode, dbo.P_SFC_State.workshop, dbo.P_WorkOrder.product_code, 
                      dbo.P_WorkOrder.main_order
FROM         dbo.P_SFC_State INNER JOIN
                      dbo.P_WorkOrder ON dbo.P_SFC_State.order_no = dbo.P_WorkOrder.order_no


GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPane1', @value=N'[0E232FF0-B466-11cf-A24F-00AA00A3EFFF, 1.00]
Begin DesignProperties = 
   Begin PaneConfigurations = 
      Begin PaneConfiguration = 0
         NumPanes = 4
         Configuration = "(H (1[40] 4[20] 2[20] 3) )"
      End
      Begin PaneConfiguration = 1
         NumPanes = 3
         Configuration = "(H (1 [50] 4 [25] 3))"
      End
      Begin PaneConfiguration = 2
         NumPanes = 3
         Configuration = "(H (1 [50] 2 [25] 3))"
      End
      Begin PaneConfiguration = 3
         NumPanes = 3
         Configuration = "(H (4 [30] 2 [40] 3))"
      End
      Begin PaneConfiguration = 4
         NumPanes = 2
         Configuration = "(H (1 [56] 3))"
      End
      Begin PaneConfiguration = 5
         NumPanes = 2
         Configuration = "(H (2 [66] 3))"
      End
      Begin PaneConfiguration = 6
         NumPanes = 2
         Configuration = "(H (4 [50] 3))"
      End
      Begin PaneConfiguration = 7
         NumPanes = 1
         Configuration = "(V (3))"
      End
      Begin PaneConfiguration = 8
         NumPanes = 3
         Configuration = "(H (1[56] 4[18] 2) )"
      End
      Begin PaneConfiguration = 9
         NumPanes = 2
         Configuration = "(H (1 [75] 4))"
      End
      Begin PaneConfiguration = 10
         NumPanes = 2
         Configuration = "(H (1[66] 2) )"
      End
      Begin PaneConfiguration = 11
         NumPanes = 2
         Configuration = "(H (4 [60] 2))"
      End
      Begin PaneConfiguration = 12
         NumPanes = 1
         Configuration = "(H (1) )"
      End
      Begin PaneConfiguration = 13
         NumPanes = 1
         Configuration = "(V (4))"
      End
      Begin PaneConfiguration = 14
         NumPanes = 1
         Configuration = "(V (2))"
      End
      ActivePaneConfig = 0
   End
   Begin DiagramPane = 
      Begin Origin = 
         Top = 0
         Left = 0
      End
      Begin Tables = 
         Begin Table = "P_SFC_State"
            Begin Extent = 
               Top = 6
               Left = 38
               Bottom = 215
               Right = 205
            End
            DisplayFlags = 280
            TopColumn = 0
         End
         Begin Table = "P_WorkOrder"
            Begin Extent = 
               Top = 6
               Left = 243
               Bottom = 210
               Right = 407
            End
            DisplayFlags = 280
            TopColumn = 8
         End
      End
   End
   Begin SQLPane = 
   End
   Begin DataPane = 
      Begin ParameterDefaults = ""
      End
   End
   Begin CriteriaPane = 
      Begin ColumnWidths = 11
         Column = 1440
         Alias = 900
         Table = 1740
         Output = 720
         Append = 1400
         NewValue = 1170
         SortType = 1350
         SortOrder = 1410
         GroupBy = 1350
         Filter = 1350
         Or = 1350
         Or = 1350
         Or = 1350
      End
   End
End
' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_PSFCState_PWorkOrder'
GO

EXEC sys.sp_addextendedproperty @name=N'MS_DiagramPaneCount', @value=1 , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'VIEW',@level1name=N'View_PSFCState_PWorkOrder'
GO

--20161112
--增加P_WorkOrder_collect表，统计P_WorkOrder中数据
CREATE TABLE [dbo].[P_WorkOrder_collect](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[workshop_code] [nvarchar](30) NOT NULL,
	[product_code] [nvarchar](30) NOT NULL,
	[years] [nvarchar](10) NOT NULL,
	[months] [nvarchar](10) NOT NULL,
	[days] [nvarchar](10) NOT NULL,
	[weeks] [nvarchar](10) NULL,
	[input_qty] [decimal](10, 3) NULL,
	[remarks] [nvarchar](50) NULL,
 CONSTRAINT [PK_P_WorkOrder_collect] PRIMARY KEY NONCLUSTERED 
(
	[workshop_code] ASC,
	[product_code] ASC,
	[years] ASC,
	[months] ASC,
	[days] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--增加P_SFC_State_collect表，统计P_SFC_State中数据
CREATE TABLE [dbo].[P_SFC_State_collect](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[workshop_code] [nvarchar](30) NOT NULL,
	[line_id] [int] NOT NULL,
	[product_code] [nvarchar](30) NOT NULL,
	[years] [nvarchar](10) NOT NULL,
	[months] [nvarchar](10) NOT NULL,
	[days] [nvarchar](10) NOT NULL,
	[weeks] [nvarchar](10) NULL,
	[output_qty] [decimal](10, 3) NULL,
	[remarks] [nvarchar](50) NULL,
 CONSTRAINT [PK_P_SFC_State_collect] PRIMARY KEY NONCLUSTERED 
(
	[workshop_code] ASC,
	[line_id] ASC,
	[product_code] ASC,
	[years] ASC,
	[months] ASC,
	[days] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--增加P_Fail_Detail_NgCode_collect表，统计P_Fail_Detail中不良现象数据
CREATE TABLE [dbo].[P_Fail_Detail_NgCode_collect](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[workshop_code] [nvarchar](30) NOT NULL,
	[product_code] [nvarchar](30) NOT NULL,
	[ng_code] [nvarchar](30) NOT NULL,
	[years] [nvarchar](10) NOT NULL,
	[months] [nvarchar](10) NOT NULL,
	[days] [nvarchar](10) NOT NULL,
	[weeks] [nvarchar](10) NULL,
	[Total_qty] [decimal](10, 3) NULL,
	[remarks] [nvarchar](50) NULL,
 CONSTRAINT [PK_P_Fail_Detail_NgCode_collect] PRIMARY KEY NONCLUSTERED 
(
	[workshop_code] ASC,
	[product_code] ASC,
	[ng_code] ASC,
	[years] ASC,
	[months] ASC,
	[days] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--增加P_Fail_Detail_NgReason_collect表，统计P_Fail_Detail中不良原因数据
CREATE TABLE [dbo].[P_Fail_Detail_NgReason_collect](
	[id] [int] IDENTITY(1,1) NOT NULL,
	[workshop_code] [nvarchar](30) NOT NULL,
	[product_code] [nvarchar](30) NOT NULL,
	[reason_code] [nvarchar](30) NOT NULL,
	[reasontype_code] [nvarchar](30) NOT NULL,
	[years] [nvarchar](10) NOT NULL,
	[months] [nvarchar](10) NOT NULL,
	[days] [nvarchar](10) NOT NULL,
	[weeks] [nvarchar](10) NULL,
	[Total_qty] [decimal](10, 3) NULL,
	[remarks] [nvarchar](50) NULL,
 CONSTRAINT [PK_P_Fail_Detail_NgReason_collect] PRIMARY KEY NONCLUSTERED 
(
	[workshop_code] ASC,
	[product_code] ASC,
	[reason_code] ASC,
	[reasontype_code] ASC,
	[years] ASC,
	[months] ASC,
	[days] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

--2016.11.14
--更新P_WorkOrder_collect 表，增加字段
alter table P_WorkOrder_collect add StandardEfficiency [int] NULL ;
--更新P_SFC_State_collect表，增加字段
alter table P_SFC_State_collect add PracticalEfficiency [int] NULL ;
--V_Fail_Detail_NGName 视图增加typecode的输出，直接在LEMES中修改过，没有提供脚本