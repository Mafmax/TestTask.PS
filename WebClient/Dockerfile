FROM node:16

WORKDIR /usr/src/app
COPY src/package*.json ./
COPY src/. .
RUN npm install

EXPOSE 3000

CMD ["npm", "start"]
