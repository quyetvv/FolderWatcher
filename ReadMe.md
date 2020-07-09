Developer:
Mở console từ admin: 1. Start >> All Programs >> Visual Studio 2015 >> Visual Studio Tools >> Right Click "Developer Command Prompt for VS2015" and click "Run as administrator"

cd vào thư mục dự án: 
cd D:\Projects\Prentow\FolderWatcher\git\bin\debug

Cài đặt service
installutil /u FolderWatcher.exe
installutil FolderWatcher.exe

Máy cài đặt: 
Vào thư mục có net framework và mở InstallUtil.exe run as Admin:
cd C:\Windows\Microsoft.NET\Framework\v4.0.30319
'InstallUtil.exe FolderWatcher.exe'