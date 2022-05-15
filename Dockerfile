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

# Instala binarios do Node.js
WORKDIR /
RUN sudo apt install -y npm
RUN sudo apt install -y nodejs

# Sobe Backend
WORKDIR /backend
COPY ./PontoFacil.Api ./

# Sobe Frontend
WORKDIR /frontend
COPY ./ponto-facil ./
RUN npm i

# Executa os processos
EXPOSE 3000 5086
WORKDIR /
COPY dockerwrapper.sh dockerwrapper.sh
CMD ./dockerwrapper.sh