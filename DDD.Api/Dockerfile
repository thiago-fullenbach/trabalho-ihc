FROM ubuntu:latest

# Instala sudo e wget
RUN apt-get update
RUN apt-get -y install sudo
RUN apt-get -y install ssl-cert
RUN sudo apt install -y wget

# Instala binarios do .NET
WORKDIR /binarios_dotnet
RUN wget https://packages.microsoft.com/config/debian/11/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
RUN sudo dpkg -i packages-microsoft-prod.deb
RUN rm packages-microsoft-prod.deb

RUN sudo apt-get update && \
    sudo apt-get install -y apt-transport-https && \
    sudo apt-get update && \
    sudo apt-get install -y dotnet-sdk-6.0

# Sobe Backend
WORKDIR /backend
COPY ./ ./

# Roda o servidor de API
EXPOSE $PORT
CMD dotnet run ASPNETCORE_URLS=http://*:$PORT