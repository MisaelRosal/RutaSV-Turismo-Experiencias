#Etapa de construcción de la aplicación React (Vite)
FROM node:20-alpine AS build
WORKDIR /app

#Copiar dependencias e instalar dependencias 
COPY package*.json ./
RUN npm install

#Copiar el resto de los archivos y generar los archivos estaticos de producción
COPY . .
RUN npm run build

FROM nginx:alpine 
COPY --from=build /app/dist /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
