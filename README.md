# HigherOrLower
## Web API for playing Higher or Lower game without a deck of cards

The game was developed using .NET 8. It uses Entity Framework Core 8.0 as the ORM.

---

The (API + DB) is available in a docker.
After pulling the repo, navigate to the *HigherOrLower* directory (where the DockerFile and the docker-compose-***.yml files are. From there, there are two ways of running the API:
1. To locally build the .NET solution and create its Docker image, and then run the docker, run:
> docker-compose -f docker-compose-building-locally.yml up --build

2. To use an already built Docker image available in docker hub, run:
> docker-compose -f docker-compose-getting-from-dockerHub.yml up

After running either of the "docker compose up" commands above and waiting for the DB to load (it can take 1/2 min for the SQL Server to fully load, please check the console), the API will be available at:
> http://localhost:8080/higherOrLower

----

If the docker does not work for some reason, it is also possible to run the API via IIS Express in Visual Studio, as long as SQL Server is installed in the machine
