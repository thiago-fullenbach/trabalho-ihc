FROM ubuntu:latest

# Instala sudo e wget
RUN apt-get update
RUN apt-get -y install sudo
RUN apt-get -y install ssl-cert
RUN sudo apt install -y wget

# Instala binarios do Node.js
WORKDIR /
RUN sudo apt install -y npm
RUN sudo apt install -y nodejs

# Sobe Frontend
WORKDIR /frontend
COPY ./ ./
RUN npm i

# Roda o servidor do lado do cliente React
EXPOSE 3000
CMD ["npm", "start"]