Developer:
Mở console từ admin: 1. Start >> All Programs >> Visual Studio 2015 >> Visual Studio Tools >> Right Click "Developer Command Prompt for VS2015" and click "Run as administrator"

cd vào thư mục dự án: 
cd D:\Projects\Prentow\FolderWatcher\git\bin\debug

Cài đặt service
installutil /u FolderWatcher.exe
installutil FolderWatcher.exe

B1: Máy cài đặt: 
Vào thư mục có net framework và mở InstallUtil.exe run as Admin:
cd C:\Windows\Microsoft.NET\Framework\v4.0.30319
'InstallUtil.exe FolderWatcher.exe'

B2: Cầu hình đường dẫn sau:
<add key="LogPath" value="D:\Downloads\software\" /> => Đường dẫn thư mục log, ko tạo log nếu ko có đường dẫn
<add key="FolderPaths" value="D:\Downloads\software" /> thư mục monitor
<add key="ServicePaths" value="http://nav.prentow.com:9047/NAV2019Live/WS/PS%20VN/Codeunit/Test_khoi" /> Đường dẫn web service

B3: Start service
Vào windows services và chọn FolderWatcher để start services
