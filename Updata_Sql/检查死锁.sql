USE master;
--�ҵ�SPID  
SELECT  *
FROM    sysprocesses
WHERE   blocked <> 0;

--����SPID�ҵ�OBJID
EXEC sp_lock;
--����OBJID�ҵ�����
SELECT  OBJECT_NAME(85575343);
