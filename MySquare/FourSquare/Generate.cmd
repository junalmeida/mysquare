@echo off
%1\bin\xsd.exe ^
..\..\Resources\venues.xsd ^
/c /l:c# /n:MySquare.FourSquare

copy venues.cs Entities.cs
del venues.cs 

