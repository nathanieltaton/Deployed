USE [TEST]
GO
/****** Object:  StoredProcedure [dbo].[DeployedSP]    Script Date: 7/17/2024 5:51:36 PM ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
ALTER Procedure [dbo].[DeployedSP] @fname varchar(50), @lname varchar(50), @schoolname varchar(50)
AS
Select p.Firstname, p.LastName, C.CityName, S.SchoolName from Person  P
Join 
City C
on C.Id = p.CityId
Join School S
on S.Id = P.SchoolId
Where 
p.Firstname like @fname 
and 
p.Lastname like @lname
and 
s.SchoolName like @schoolname
and
p.IsActive = 1 and c.IsActive = 1 and s.IsActive = 1
order by
Lastname,FirstName
