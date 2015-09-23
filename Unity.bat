@echo off
cd /d %~dp0
for /f "usebackq" %%t in (`cd`) do set "PROJECT=%%t"
set "UNITY=%ProgramFiles%/Unity/Editor/Unity"
echo Unity : %UNITY%
echo Project : %PROJECT%
start "" "%ProgramFiles%/Unity/Editor/Unity" -projectPath "%PROJECT%"
ping -n 1 -w 5000 1.1.1.1 >nul
