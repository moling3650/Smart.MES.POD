--Create PROCEDURE [dbo].[sp_Create_ReportDataByDate_job]
ALTER PROCEDURE [dbo].[sp_Create_ReportDataByDate_job]
@in_datetime nvarchar(30)    --ָ����ѯʱ�䣬����Ϊ�գ�Ϊ��ʱ���ݵ�ǰʱ���ѯ
AS
BEGIN
	declare @p_datetime datetime;			--��ѯʱ��
	declare @p_datetimeTmp datetime;		--��ѯʱ��,�޸�ʱ����
	declare @p_datetimeworkorder datetime;  --��ѯʱ��,ָ����ѯP_WorkOrder��planned_time
	declare @p_year int;					--��ѯ��
	declare @p_month int;					--��ѯ��
	declare @p_day int;						--��ѯ��
	declare @p_weeks int;					--��ѯ���µڼ���
	
	declare @i_beforeday int;				--���Բ�ѯ��ǰ����
	   
	set @i_beforeday = 0;
	--��ȡָ����ѯʱ��
	if(@in_datetime<>'')
	begin
		--ʹ�ø���ʱ��,�������ʱ�䣬�Ͱ��ո������ڲ�ѯ
		set @p_datetime=CONVERT(datetime,@in_datetime);  
		set @i_beforeday = 0; 
	end
	else
	begin
		--ȡ��ǰʱ�䣬���û�и���ʱ�䣬�Ͱ��յ�ǰ����ǰһ���ѯ
		set @p_datetime=dateadd(day,(-1),GETDATE());
	end
	
	--�����꣬�£��գ���
	set @p_datetimeTmp=convert(datetime,convert(varchar(10),@p_datetime,120)+' 07:55:55');
	set @p_datetimeworkorder=convert(datetime,convert(varchar(10),@p_datetime,120)+' 00:00:00.000');
	set @p_year=DATEPART(yyyy,@p_datetime);
	set @p_month=DATEPART(m,@p_datetime);
	set @p_day=DATEPART(d,@p_datetime);
	set @p_weeks=cast((datepart(wk,@p_datetime) - datepart(wk,convert(varchar(7),@p_datetime,120) + '-01') + 1) as varchar(2));
	
	--����P_WorkOrder�����ݻ��ܵ�P_WorkOrder_collect��
	
	with my1 as(
	select '����' lvl,* from P_WorkOrder where  planned_time=dateadd(day,(@i_beforeday),@p_datetimeworkorder) and parent_order =''
	 union all select p_sfc.* from my1, (select '�²�' lvl,* from P_WorkOrder) p_sfc where my1.order_no = p_sfc.parent_order
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
	----����P_SFC_State�����ݻ��ܵ�P_SFC_State_collect��
		
if object_id('tempdb..#Tmp_collect') is not null
begin
	delete from #Tmp_collect;
	drop table #Tmp_collect;   --ɾ����ʱ��#Tmp
end
create table #Tmp_collect --������ʱ��#Tmp
(
    ID   int IDENTITY (1,1)     not null, --������ID,����ÿ������һ����¼�ͻ��1
    workshop  varchar(30),  
    line_id   int,
    product_code varchar(30), 
    PracticalEfficiency int
    primary key (ID)      --����IDΪ��ʱ��#Tmp������     
);
declare @Curr_WorkOrder_sfc nvarchar(30);
declare @Curr_WorkOrder_product_code nvarchar(30);
declare @Curr_WorkOrder_input_station nvarchar(30);
--------------------------------- �����α�  --Local(�����α�)
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
-- ���α�
OPEN caaa;    
-- �����ͻ�ȡ�α�
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
----------------------------------- �ر��α�    
Close caaa    
----------------------------------- ɾ���α�    
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
	drop table #Tmp_collect   --ɾ����ʱ��#Tmp
end

	--����P_Product_Efficiency_collect��ͳ��P_SFC_State������
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
	
	--����P_Fail_Detail�����������ݻ��ܵ�P_Fail_Detail_NgCode_collect��
	insert into P_Fail_Detail_NgCode_collect(workshop_code,product_code,ng_code,years,months,[days],[weeks],[Total_qty],[remarks])
	select a.ws_code,b.product_code,a.ng_code,@p_year as years,@p_month as months,@p_day as [days],@p_weeks as [weeks], COUNT(a.ng_code) as [Total_qty],'' as [remarks] from P_Fail_Detail a,P_WorkOrder b
	where 1=1
	and a.order_no=b.order_no
	and a.input_time >=dateadd(day,(@i_beforeday),@p_datetimeTmp)
	and a.input_time<dateadd(day,(@i_beforeday+1),@p_datetimeTmp)
	group by a.ws_code,b.product_code,a.ng_code
	
	--����P_Fail_Detail����ԭ�����ݻ��ܵ�P_Fail_Detail_NgReason_collect��
	insert into P_Fail_Detail_NgReason_collect(workshop_code,product_code,[reason_code],[reasontype_code],years,months,[days],[weeks],[Total_qty],[remarks])
	select a.ws_code,b.product_code,a.reason_code,a.reasontype_code,@p_year as years,@p_month as months,@p_day as [days],@p_weeks as [weeks], COUNT(a.ws_code) as [Total_qty],'' as [remarks] from P_Fail_Detail a,P_WorkOrder b
	where 1=1
	and a.order_no=b.order_no
	and a.finish_time is not null
	and a.finish_time >=dateadd(day,(@i_beforeday),@p_datetimeTmp)
	and a.finish_time<dateadd(day,(@i_beforeday+1),@p_datetimeTmp)
	group by a.ws_code,b.product_code,a.reason_code,a.reasontype_code
 
 select @p_datetime,@p_datetimeTmp
 RETURN 1; -- ���÷��ؽ��0-ʧ�ܣ�1-�ɹ�
END
