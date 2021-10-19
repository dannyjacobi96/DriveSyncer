
[CmdletBinding()]
[Alias("tree")]
[OutputType([string[]])]
Param
(
    [Parameter(Position=0)]
    $Path = (pwd),

    [Parameter()]
    [int]$MaxDepth=[int]::MaxValue,

    #Show the full name of the directory at each sublevel
    [Parameter()]
    [Switch]$ShowDirectory, 

    #List of wildcard matches. If a directoryname matches one of these, it will be skipped.
    [Parameter()]
    [String[]]$NotLike = $null,
 
    #List of wildcard matches. If a directoryname matches one of these, it will be shown.
    [Parameter()]
    [String[]]$Like = $null
 
)
function Show-Tree
{
    [CmdletBinding()]
    [Alias("tree")]
    [OutputType([string[]])]
    Param
    (
        [Parameter(Position=0)]
        $Path = (pwd),

        [Parameter()]
        [int]$MaxDepth=[int]::MaxValue,

        #Show the full name of the directory at each sublevel
        [Parameter()]
        [Switch]$ShowDirectory, 

        #List of wildcard matches. If a directoryname matches one of these, it will be skipped.
        [Parameter()]
        [String[]]$NotLike = $null,
 
        #List of wildcard matches. If a directoryname matches one of these, it will be shown.
        [Parameter()]
        [String[]]$Like = $null,
 
        #Internal parameter used in recursion for formating purposes
        [Parameter()]
        [int]$_Depth=0
    )

    if ($_Depth -ge $MaxDepth)
    {
        return
    }
    $FirstDirectoryShown = $False
    $start = "| " * $_Depth
    :NextDirectory foreach ($d in Get-ChildItem $path -ErrorAction Ignore |where PSIsContainer -eq $true)
    {
        foreach ($pattern in $NotLike)
        {
            if ($d.PSChildName -like $pattern)
            {
                Write-Verbose "Skipping $($d.PSChildName). Not like $Pattern"
                continue NextDirectory;
            }
        }
        $ShowThisDirectory = $false
        if (!$like)
        {
            $ShowThisDirectory = $true
        }else
        {
            foreach ($pattern in $Like)
            {
                if ($d.PSChildName -like $pattern)
                {
                    Write-Verbose "Including $($d.PSChildName). Like $Pattern"
                    $ShowThisDirectory = $true
                    Break;
                }
            }            
        }
        # When we dir a Get-ChildItem, we get the OS view of the object so here we need to transform
        # it into the PowerShell view of the path (e.g. to deal with PSDrives with deep ROOT directories)
        $ProviderPath     = $ExecutionContext.SessionState.Path.GetResolvedProviderPathFromPSPath($d.PSPath, [Ref]$d.PSProvider)
        $RootRelativePath = $ProviderPath.SubString($d.PSDrive.Root.Length)
        $PSDriveFullPath  = Join-Path ($d.PSDrive.Name + ":") $RootRelativePath

        if ($ShowThisDirectory)
        {
            if (($FirstDirectoryShown -eq $FALSE) -and $ShowDirectory)
            {
                $FirstDirectoryShown = $True
                Write-Output ("{0}{1}" -f $start,$Path)
            }
            Write-Output ("{0}+---{1}" -f $start, (Split-Path $PSDriveFullPath -Leaf))
        }
        show-Tree -path:$PSDriveFullPath -_Depth:($_Depth + 1) -ShowDirectory:$ShowDirectory -MaxDepth:$MaxDepth -NotLike:$NotLike -Like:$Like
    }    
}
show-tree @PSBOundParameters
<#
 
cls
#show-tree exchange:\ -Maxdepth 3 -ShowDirectory
#show-tree C:\ -ShowDirectory -Like *ea*
#show-tree C:\windows -Maxdepth 3 -ShowDirectory -NotLike temp,amd*
cls;show-tree C:\windows -Maxdepth 6 -ShowDirectory -NotLike temp,amd*,wow*,msil*,x86*,Microsoft*,w*,s*
#cls;show-tree HKLM:\SOFTWARE\c* -ShowDirectory -NotLike classes*,cli*
#>
#EOF