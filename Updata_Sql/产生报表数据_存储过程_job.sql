--Create PROCEDURE [dbo].[sp_Create_ReportDataByDate_job]
ALTER PROCEDURE [dbo].[sp_Create_ReportDataByDate_job]
@in_datetime nvarchar(30)    --指定查询时间，可以为空，为空时根据当前时间查询
AS
BEGIN
	declare @p_datetime datetime;			--查询时间
	declare @p_datetimeTmp datetime;		--查询时间,修改时分秒
	declare @p_datetimeworkorder datetime;  --查询时间,指定查询P_WorkOrder表planned_time
	declare @p_year int;					--查询年
	declare @p_month int;					--查询月
	declare @p_day int;						--查询日
	declare @p_weeks int;					--查询本月第几周
	
	declare @i_beforeday int;				--测试查询以前天数
	   
	set @i_beforeday = 0;
	--获取指定查询时间
	if(@in_datetime<>'')
	begin
		--使用给定时间,如果给定时间，就按照给定日期查询
		set @p_datetime=CONVERT(datetime,@in_datetime);  
		set @i_beforeday = 0; 
	end
	else
	begin
		--取当前时间，如果没有给定时间，就按照当前日期前一天查询
		set @p_datetime=dateadd(day,(-1),GETDATE());
	end
	
	--设置年，月，日，周
	set @p_datetimeTmp=convert(datetime,convert(varchar(10),@p_datetime,120)+' 07:55:55');
	set @p_datetimeworkorder=convert(datetime,convert(varchar(10),@p_datetime,120)+' 00:00:00.000');
	set @p_year=DATEPART(yyyy,@p_datetime);
	set @p_month=DATEPART(m,@p_datetime);
	set @p_day=DATEPART(d,@p_datetime);
	set @p_weeks=cast((datepart(wk,@p_datetime) - datepart(wk,convert(varchar(7),@p_datetime,120) + '-01') + 1) as varchar(2));
	
	--根据P_WorkOrder表数据汇总到P_WorkOrder_collect中
	
	with my1 as(
	select '顶层' lvl,* from P_WorkOrder where  planned_time=dateadd(day,(@i_beforeday),@p_datetimeworkorder) and parent_order =''
	 union all select p_sfc.* from my1, (select '下层' lvl,* from P_WorkOrder) p_sfc where my1.order_no = p_sfc.parent_order
	)
	
	insert into P_WorkOrder_collect(workshop_code,product_code,years,months,[days],[weeks],[input_qty],[remarks],StandardEfficiency)
	select ord.workshop_code,ord.product_code,@p_year as years,@p_month as months,@p_day as [days],@p_weeks as [weeks],sum(ord.qty) [input_qty],'' as [remarks],SUM(temp2.sstand) StandardEfficiency from
	(
		select main_order,sum(sumStand) sstand from 
		(
			select a.order_no,product_code,qty,main_order,b.SumStand*a.qty sumStand from
			my1 a inner join 
			(select flow_code,SUM(standard_time) SumStand from B_Process_Flow_Detail group by flow_code) b
			on a.flow_code=b.flow_code
		) temp1 group by main_order
	) temp2 inner join P_WorkOrder ord on temp2.main_order=ord.order_no group by ord.product_code,ord.workshop_code

	--insert into P_WorkOrder_collect(workshop_code,product_code,years,months,[days],[weeks],[input_qty],[remarks])
	--select workshop_code,product_code,@p_year as years,@p_month as months,@p_day as [days],@p_weeks as [weeks],sum(qty) as [input_qty],'' as [remarks] from P_WorkOrder 
	--where 1=1
	----and planned_time = @p_datetimeworkorder
	--and planned_time=dateadd(day,(@i_beforeday),@p_datetimeworkorder)
	--and parent_order=''
	--group by workshop_code,product_code
	----根据P_SFC_State表数据汇总到P_SFC_State_collect中
		
if object_id('tempdb..#Tmp_collect') is not null
begin
	delete from #Tmp_collect;
	drop table #Tmp_collect;   --删除临时表#Tmp
end
create table #Tmp_collect --创建临时表#Tmp
(
    ID   int IDENTITY (1,1)     not null, --创建列ID,并且每次新增一条记录就会加1
    workshop  varchar(30),  
    line_id   int,
    product_code varchar(30), 
    PracticalEfficiency int
    primary key (ID)      --定义ID为临时表#Tmp的主键     
);
declare @Curr_WorkOrder_sfc nvarchar(30);
declare @Curr_WorkOrder_product_code nvarchar(30);
declare @Curr_WorkOrder_input_station nvarchar(30);
--------------------------------- 创建游标  --Local(本地游标)
declare caaa CURSOR 
for select * from 
(
	select p2.SFC,p1.product_code,p2.input_station from P_WorkOrder p1 ,P_SFC_State p2
		where 1=1 
		and p1.order_no=p1.main_order 
		and p1.parent_order=''
		and p1.order_no=p2.order_no
		and p2.end_time>=dateadd(day,(@i_beforeday),@p_datetimeTmp)
		and p2.end_time<dateadd(day,(@i_beforeday+1),@p_datetimeTmp)
		and p2.state=2
) as lvl_one
-- 打开游标
OPEN caaa;    
-- 遍历和获取游标
fetch next from caaa into @Curr_WorkOrder_sfc,@Curr_WorkOrder_product_code,@Curr_WorkOrder_input_station ;
while @@fetch_status=0    
begin    
	with cte as  
	 (
	 select a.order_no,a.SFC,a.mat_code,a.val,@Curr_WorkOrder_product_code as product_code,@Curr_WorkOrder_input_station as input_station from P_SFC_ProcessData a where a.SFC=@Curr_WorkOrder_sfc
	 --select a.order_no,a.SFC,a.mat_code,a.val,ca.product_code from P_SFC_ProcessData a,Curr_WorkOrder ca where a.SFC=ca.SFC 
	 union all   
	 select k.order_no,k.SFC,k.mat_code,k.val,@Curr_WorkOrder_product_code as product_code,@Curr_WorkOrder_input_station as input_station from P_SFC_ProcessData k,cte ce where ce.val=k.SFC and ce.mat_code is not null
	 )
	 insert into #Tmp_collect(workshop,line_id,product_code,PracticalEfficiency)
	 select ex1.workshop, ex1.line_id,ex1.product_code,SUM(cz) as PracticalEfficiency from 
		 (select Distinct pio.workshop,pio.order_no,pio.SFC,pio.process_code,pio.output_time,aa.product_code,bs.line_id,datediff(second,pio.start_time,pio.output_time) as cz  
		 from P_SFC_Process_IOLog pio,cte aa,B_StationList bs
		 where 1=1
			--and aa.mat_code is null
			and pio.SFC=aa.SFC
 			and pio.output_time >=dateadd(day,(@i_beforeday),@p_datetimeTmp)
			and pio.output_time <dateadd(day,(@i_beforeday+1),@p_datetimeTmp)
			and aa.input_station = bs.station_code
			) ex1 
		group by ex1.workshop,ex1.product_code, ex1.line_id;
	fetch next from caaa into @Curr_WorkOrder_sfc,@Curr_WorkOrder_product_code,@Curr_WorkOrder_input_station ; 
end    
----------------------------------- 关闭游标    
Close caaa    
----------------------------------- 删除游标    
Deallocate caaa  

	insert into P_SFC_State_collect(workshop_code,line_id,product_code,years,months,[days],[weeks],[output_qty],[remarks],PracticalEfficiency)
	select ew1.workshop,ew1.line_id,ew1.product_code,@p_year as years,@p_month as months,@p_day as [days],@p_weeks as [weeks],ew1.output_qty ,'' as [remarks],tc1.PracticalEfficiency from 
	(
		select a.workshop,b.line_id,c.product_code,sum(a.qty) as [output_qty] from P_SFC_State a,B_StationList b,P_WorkOrder c
		where 1=1
		and a.end_time>=dateadd(day,(@i_beforeday),@p_datetimeTmp)
		and a.end_time<dateadd(day,(@i_beforeday+1),@p_datetimeTmp)
		and a.input_station=b.station_code
		and a.order_no=c.order_no
		and c.parent_order=''
		and a.state=2
		group by a.workshop,b.line_id,c.product_code
	) ew1, (select tc.workshop,tc.line_id,tc.product_code,SUM(tc.PracticalEfficiency) as PracticalEfficiency from #Tmp_collect tc group by  tc.workshop,tc.line_id,tc.product_code) tc1
where ew1.workshop=tc1.workshop and ew1.line_id=tc1.line_id and ew1.product_code=tc1.product_code;
--select * from #Tmp_collect;
if object_id('tempdb..#Tmp_collect') is not null
begin
	drop table #Tmp_collect   --删除临时表#Tmp
end

	--增加P_Product_Efficiency_collect表，统计P_SFC_State中数据
	insert into P_Product_Efficiency_collect(workshop_code,product_code,years,months,[days],[weeks],[spt],[curr_spt],[remarks],[Total_Qty])
	select ex2.workshop_code,ex2.product_code,@p_year as years,@p_month as months,@p_day as [days],@p_weeks as [weeks],bp.spt,aaa,'' as remarks,qty from 
	(
		select ex1.workshop_code,ex1.product_code,SUM(ex1.qty) AS qty,SUM(cz) as aaa from
		(	
			select p1.workshop_code,p2.SFC,p2.qty,p1.product_code,p2.input_station,datediff(second,p2.begin_time,p2.end_time) as cz from P_WorkOrder p1 ,P_SFC_State p2
					where 1=1 
					--and p1.order_no=p1.main_order 
					--and p1.parent_order=''
					and p1.order_no=p2.order_no
					and p2.begin_Date=@p_datetimeworkorder										
					and p2.end_Date=@p_datetimeworkorder
					and p2.state=2
		) ex1
		group by ex1.workshop_code,ex1.product_code
	)ex2 inner join B_Product bp on ex2.product_code=bp.product_code;
	
	--根据P_Fail_Detail表不良现象数据汇总到P_Fail_Detail_NgCode_collect中
	insert into P_Fail_Detail_NgCode_collect(workshop_code,product_code,ng_code,years,months,[days],[weeks],[Total_qty],[remarks])
	select a.ws_code,b.product_code,a.ng_code,@p_year as years,@p_month as months,@p_day as [days],@p_weeks as [weeks], COUNT(a.ng_code) as [Total_qty],'' as [remarks] from P_Fail_Detail a,P_WorkOrder b
	where 1=1
	and a.order_no=b.order_no
	and a.input_time >=dateadd(day,(@i_beforeday),@p_datetimeTmp)
	and a.input_time<dateadd(day,(@i_beforeday+1),@p_datetimeTmp)
	group by a.ws_code,b.product_code,a.ng_code
	
	--根据P_Fail_Detail表不良原因数据汇总到P_Fail_Detail_NgReason_collect中
	insert into P_Fail_Detail_NgReason_collect(workshop_code,product_code,[reason_code],[reasontype_code],years,months,[days],[weeks],[Total_qty],[remarks])
	select a.ws_code,b.product_code,a.reason_code,a.reasontype_code,@p_year as years,@p_month as months,@p_day as [days],@p_weeks as [weeks], COUNT(a.ws_code) as [Total_qty],'' as [remarks] from P_Fail_Detail a,P_WorkOrder b
	where 1=1
	and a.order_no=b.order_no
	and a.finish_time is not null
	and a.finish_time >=dateadd(day,(@i_beforeday),@p_datetimeTmp)
	and a.finish_time<dateadd(day,(@i_beforeday+1),@p_datetimeTmp)
	group by a.ws_code,b.product_code,a.reason_code,a.reasontype_code
 
 select @p_datetime,@p_datetimeTmp
 RETURN 1; -- 设置返回结果0-失败，1-成功
END
