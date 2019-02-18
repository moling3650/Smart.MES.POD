
Create PROCEDURE [dbo].[sp_Create_Emp_kpi_ByDate_job]
--ALTER PROCEDURE [dbo].[sp_Create_Emp_kpi_ByDate_job]
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
	
	--��ȡָ����ѯʱ��
	if(@in_datetime<>'')
	begin
		--ʹ�ø���ʱ��,�������ʱ�䣬�Ͱ��ո������ڲ�ѯ
		set @p_datetime=CONVERT(datetime,@in_datetime);  
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
	
	--����P_SFC_Process_IOLog�����ݻ��ܵ�P_Emp_kpi_collect��
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
 RETURN 1; -- ���÷��ؽ��0-ʧ�ܣ�1-�ɹ�
END
