$acl = Get-Acl "C:\Windows\Temp"
$rule = New-Object  System.Security.Accesscontrol.FileSystemAccessRule("IIS_IUSRS","Write","Allow")
$acl.SetAccessRule($rule)
Set-Acl "C:\Windows\Temp" $acl