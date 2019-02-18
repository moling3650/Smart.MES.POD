--2016-11-22
--更新P_Product_Efficiency_collect 表，增加字段
alter table P_Product_Efficiency_collect add Total_Qty DECIMAL(10,3) DEFAULT 0 ;
