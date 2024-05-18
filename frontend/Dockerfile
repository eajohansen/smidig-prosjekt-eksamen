# Fetching the latest node image (non-Alpine)
FROM node:20
WORKDIR /app
COPY package.json /app
RUN npm install
COPY . .
ENV HOST=0.0.0.0
ENV NODE_ENV development
ENV CHOKIDAR_USEPOLLING=true
EXPOSE 3000