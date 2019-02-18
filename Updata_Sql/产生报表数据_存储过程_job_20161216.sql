
Create PROCEDURE [dbo].[sp_Create_Emp_kpi_ByDate_job]
--ALTER PROCEDURE [dbo].[sp_Create_Emp_kpi_ByDate_job]
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
	
	--获取指定查询时间
	if(@in_datetime<>'')
	begin
		--使用给定时间,如果给定时间，就按照给定日期查询
		set @p_datetime=CONVERT(datetime,@in_datetime);  
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
	
	--根据P_SFC_Process_IOLog表数据汇总到P_Emp_kpi_collect中
	insert into P_Emp_kpi_collect(emp_no,process_code,years,months,[days],[weeks],product_code,work_time,qty,fail_qty,fail_times)
	select out4.emp_code,out4.process_code,@p_year as years,@p_month as months,@p_day as [days],@p_weeks as [weeks],out4.product_code,out4.work_time,out4.qty,sum(out4.fail_qty) as fail_qty,sum(out4.fail_times) fail_times from 
	(select out2.emp_code,out2.process_code,out2.product_code,out2.work_time,out2.qty,out3.qty as fail_qty,out3.fail_times
	 from(select out1.emp_code,out1.process_code,out1.product_code,sum(work_time1) as work_time,sum(out1.initqty) as qty 
			from(select a.emp_code,a.order_no,b.product_code,a.sfc,a.initqty,a.process_code,DATEDIFF(second ,a.input_time,a.output_time) AS work_time1 
				from P_SFC_Process_IOLog a,P_WorkOrder b
				where 1=1
				and a.pass=1
				and convert(datetime,convert(varchar(10),a.p_date,120)+' 00:00:00.000')=@p_datetimeworkorder
				and a.order_no=b.order_no
			) out1 group by out1.emp_code,out1.process_code,out1.product_code
	) out2 left join (select pf1.emp_code,c.product_code,pf1.process_code,pf1.input_time,pf1.finish_time,pf1.p_date,pf1.qty,pf1.fail_times 
						from P_FailLog pf1,P_WorkOrder c 
						where 1=1 and pf1.order_no=c.order_no
					) out3 on out2.emp_code=out3.emp_code and out2.process_code=out3.process_code and out2.product_code=out3.product_code) out4
	group by out4.emp_code,out4.process_code,out4.product_code,out4.work_time,out4.qty
 
 select @p_datetime,@p_datetimeTmp
 RETURN 1; -- 设置返回结果0-失败，1-成功
END
