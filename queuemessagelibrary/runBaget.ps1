cd C:\Git\nuget

docker inspect baget
docker run --rm -p 5555:80 --name baget --env-file baget.env -v "$(pwd)/baget-data:/var/baget" loicsharma/baget:latest