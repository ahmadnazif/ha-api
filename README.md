# HA API
Simple HTTP API built using ASP.NET Core initially for testing my HA flow. Can be deployed on Linux, Windows or Docker.

## Running:
- The default app port here is `5500`. You may change it in "appsettings.json" to any avilable port such as 80 (typically when using Docker).
- Using dotnet CLI:
  - `dotnet run` or
  - `dotnet HaApi.dll` after debugging the app
- Using docker:
  - In "Dockerfile", make sure to `EXPOSE` the same port as stated in "appsettings.json" file.
  - Build image: `docker build -f Dockerfile .. -t haapi:v1` (This will create "haapi:v1" image)
  - Run container: `docker run --name haapi-demo -p 5502:5500 -d haapi:v1` (This will create "haapi-demo" container in detatched mode with port `5502` pointing to internal `5500` in container)


## MySQL DB
If you use MySQL as data source, create the "apidemo" db:
<br />

~~~~sql
CREATE DATABASE IF NOT EXISTS `apidemo`;
USE `apidemo`;

CREATE TABLE IF NOT EXISTS `apidemo` (
`id` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
`from` varchar(50) DEFAULT NULL,
`to` varchar(50) DEFAULT NULL,
`text` text,
`created_time` datetime DEFAULT CURRENT_TIMESTAMP,
PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
~~~~
<br />
Dont forget to create user for this db
