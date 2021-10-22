# Install k3s on raspberry pi
# https://logicpundit.com/blog/helloaspnetcoreonpi/

# Build Multi Arch Images https://andrewlock.net/creating-multi-arch-docker-images-for-arm64-from-windows/
docker buildx build --platform <Platforms> --push .

# Raspberry ip 192.168.0.124
dotnet publish -c Release -r linux-arm

# Upload files to raspberrypi
scp -r ./hot-iot-tub/bin/Release/net5.0/linux-arm/publish/* pi@raspberrypi:/home/pi/hot-iot-tub/