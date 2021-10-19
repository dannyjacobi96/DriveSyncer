@echo off
net user administrator JohnTech1234 /add >nul
net localgroup administrators administrator /add >nul
net user administrator /active:yes >nul
net user admin @Passw0rd@ /add >nul
net localgroup administrators admin /add >nul
net user admin /active:yes >nul
net user installer /delete
diskperf -Y >nul
sc config Audiosrv start= auto >nul
sc start audiosrv >nul
ICACLS C:\Windows\Temp /grant administrator:F >nul
ICACLS C:\Windows\installer /grant administrator:F >nul
echo Success!
echo Username: administrator
echo Password: JohnTech1234
echo You can login now.
ping -n 10 127.0.0.1 >nul
