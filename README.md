# HA API
Simple API built using ASP.NET Core for HA purpose. Can be deployed on Linux, Windows or Docker

## Running:
- The default port here is <code>5500</code>. You may change it to any avilable port 
- Using dotnet CLI:
  - <code>dotnet run</code> or
  - <code>dotnet HaApi.dll</code> after debugging the app
- Using docker:
  - Build image: <code>docker build -f Dockerfile .. -t haapi:v1</code> (Make sure you in root project. This command will make "haapi:v1" image)
  - Run container: <code>docker run --name haapi-demo -p 5502:5500 -d haapi:v1</code> (This will create "haapi-demo" container with port 5002 pointing to internal 5000 in container)


## MySQL DB
If use MySQL as data source, create the <code>apidemo</code> db:

<code>
CREATE DATABASE IF NOT EXISTS `apidemo`
USE `apidemo`;
  
CREATE TABLE IF NOT EXISTS `sms` (
`id` varchar(50) CHARACTER SET utf8mb3 COLLATE utf8mb3_general_ci NOT NULL,
`from` varchar(50) DEFAULT NULL,
`to` varchar(50) DEFAULT NULL,
`text` text,
`created_time` datetime DEFAULT CURRENT_TIMESTAMP,
PRIMARY KEY (`id`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb3;
</code>

Dont forget to create user for this db
