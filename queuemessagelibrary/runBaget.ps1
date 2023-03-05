cd C:\Git\nuget

docker run --rm -p 5555:80 --name baget --env-file baget.env -v "$(pwd)/baget-data:/var/baget" loicsharma/baget:latest
docker container inspect  -f '{{ .NetworkSettings.IPAddress }}' baget