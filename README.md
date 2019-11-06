# FileUploadServer
Simple service listening for multipart (file upload) requests and saving them in a predefined directory.

## Usage
### Server side

`dotnet FileUploadServer.dll directory=c:\files`
`dotnet FileUploadServer.dll directory=c:\files extensions=.txt,.pdf`

### Client side

`curl.exe -F 'data=@c:\temp\test.txt' http://localhost:5000`

File test.txt will be available in the directory `c:\files` 

