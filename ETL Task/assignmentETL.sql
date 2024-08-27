/*
-- Create SourceDB
create database SourceDB;
use SourceDB;

-- Create TargetDB
create database TargetDB;
*/

create database DemoMigrateDb;
use DemoMigrateDb;

create table SourceTable(
id int identity(1,1) primary key,
content nvarchar(255),
createdAt datetime default getdate(),
updatedAt datetime,
isDeleted bit default 0
);


-- Modify TargetTable to support versioning for historical data
create table TargetTable(
    id int,
    version int default 1,
    content nvarchar(255),
    createdAt datetime,
    updatedAt datetime,
    isDeleted bit,
    primary key (id, version) -- Composite primary key to include id and version
);

-- Optional: Add an index to improve performance on querying by id
create nonclustered index idx_TargetTable_id
on TargetTable (id);

create or alter procedure migrate_table_proc
    @Debug_flag BIT
as
begin
   -- Insert new records from SourceTable to TargetTable
   if @Debug_flag=1
      print 'Inserting new records...';
   insert into TargetTable(id, content, createdAt, version)
   select s.id, s.content, s.createdAt, 1
   from SourceTable s
   left join TargetTable t on s.id=t.id
   where t.id is null;

   -- Update existing records from SourceTable to TargetTable
   if @Debug_flag = 1
       print 'Updating existing records...';

   -- Insert new versions of updated records
   insert into TargetTable(id, content, createdAt, updatedAt, version)
   select s.id, s.content, getdate(), getdate(),
       coalesce(t.version, 0) + 1
   from SourceTable s
   join TargetTable t on t.id = s.id
   where s.content <> t.content
      or s.updatedAt <> t.updatedAt;

   -- Soft delete in TargetTable
   if @Debug_flag = 1
       print 'Soft deleting records in TargetDB...';

   update t
   set t.isDeleted = 1,
       t.updatedAt = getdate()
   from TargetTable t
   join SourceTable s on t.id = s.id
   where s.isDeleted = 1;

   -- Hard delete in SourceTable
   if @Debug_flag = 1
       print 'Hard deleting records from SourceTable...';

   delete from SourceTable
   where isDeleted = 1;

   if @Debug_flag = 1
       print 'ETL Process completed';

end



/* Insert Test*/
insert into SourceTable (content) values ('Test Insert Content');
execute migrate_table_proc @Debug_flag = 1;
select * from TargetTable where content = 'Test Insert Content';
/* Insert Test*/


/* Update Test*/
insert into SourceTable (content) VALUES ('Initial Content');
execute migrate_table_proc @Debug_flag = 1;
update SourceTable set content = 'Updated Content' where content = 'Initial Content';
execute migrate_table_proc @Debug_flag = 1;
select * from TargetTable where content = 'Updated Content';
/* End Update Test*/


/* Delete Test*/
insert into SourceTable (content) VALUES ('Content to be Deleted');
execute migrate_table_proc @Debug_flag = 1;
update SourceTable set isDeleted = 1 where content = 'Content to be Deleted';
execute migrate_table_proc @Debug_flag = 1;
select * from TargetTable where content = 'Content to be Deleted' AND isDeleted = 1;
/* End Delete Test*/

select * from SourceTable
select * from TargetTable
truncate table SourceTable;
truncate table TargetTable;

drop table SourceTable;
drop table TargetTable;
