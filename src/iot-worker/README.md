# Configuration of the Node


Build app
dotnet publish -c Release -r linux-arm

Upload app to raspberry pi
scp -r .\bin\Release\net5.0\linux-arm\publish\* pi@192.168.0.124:/home/pi/iot-worker/