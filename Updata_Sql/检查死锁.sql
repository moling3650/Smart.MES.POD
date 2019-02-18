USE master;
--找到SPID  
SELECT  *
FROM    sysprocesses
WHERE   blocked <> 0;

--根据SPID找到OBJID
EXEC sp_lock;
--根据OBJID找到表名
SELECT  OBJECT_NAME(85575343);
