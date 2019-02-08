@echo off & setlocal EnableDelayedExpansion

for /f "delims=" %%i in ('dir /a-d /b *.xlsx') do (  
    ::echo %%i
    python excel2Class.py %%i
)

pause