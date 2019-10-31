# FileUploadServer
Simple service listening to multipart request.

## Usage
### Server side

`dotnet FileUploadServer.dll directory=c:\files`

### Client side

`curl.exe -F 'data=@c:\temp\test.txt' http://localhost:5000`

File test.txt will be available in the directory `c:\files` 

