
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[P_GetSFC]
@SFC nvarchar(30),
@mat_code nvarchar(30)
AS
BEGIN

with my1 as(select '¶¥²ã' lvl,* from P_SFC_ProcessData where SFC=@SFC
 union all select p_sfc.* from my1, (select 'ÏÂ²ã' lvl,* from P_SFC_ProcessData) p_sfc 
 where my1.val = p_sfc.SFC and my1.mat_code is not null
)
 SELECT A.* FROM my1 A where mat_code=@mat_code 
 
 
END

GO


