1. Ha docker installert på maskinen
2. Start docker
3. Skriv "docker-compose up --build" i hovedmappa. (docker compose up --build uten bindestrek på linux)
4. Skriv npm install og npm run dev i frontend mappen
5. Voila,  
http://localhost:5173 for frontend  
http://localhost:5500 for backend  
http://localhost:9999 for databasen  

for å endre database setup:
- "dotnet ef migrations add navnetPaaEndringene" mac-brukere: (dotnet-ef migrations add navnetPaaEndringene)
- REBUILD prosjektet med step 3.
