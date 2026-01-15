# frontend/Dockerfile - VERSIÓN CORREGIDA
# Build Angular app
FROM node:20-alpine AS build
WORKDIR /app

COPY package*.json ./
RUN npm install

COPY . .
RUN npm run build -- --configuration=production

# Verifica estructura
RUN echo "=== Estructura de dist ==="
RUN ls -la /app/dist/
RUN echo "=== Contenido de dist/browser ==="
RUN ls -la /app/dist/browser/

# Serve with nginx
FROM nginx:alpine

# Limpia todo
RUN rm -f /etc/nginx/conf.d/default.conf
RUN rm -rf /usr/share/nginx/html/*

# Copia configuración
COPY nginx.conf /etc/nginx/conf.d/

# IMPORTANTE: Copia desde dist/browser/
COPY --from=build /app/dist/browser/* /usr/share/nginx/html/
COPY --from=build /app/dist/browser/. /usr/share/nginx/html/
# Permisos
RUN chmod -R 755 /usr/share/nginx/html

EXPOSE 80
CMD ["nginx", "-g", "daemon off;"]
