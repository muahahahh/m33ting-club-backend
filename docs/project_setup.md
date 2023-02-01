# Project setup

## Setup Web Project
1. Create PostgreSQL database.
2. Run Initialization/initialize.sql
3. Setup firebase (more info on Confluence)
4. Setup S3 Bucket (to upload images)
5. Override settings from appsettings.json
6. Run M33tingClub.Web


## Setup Integration Tests
1. Create PostgreSQL database.
2. Run Initialization/initialize.sql
3. Setup firebase (more info on Confluence)
4. Setup S3 Bucket (to upload images)
5. Override settings from appsettings.json
6. Add firebase_configuration.json  
path: M33tingClub.IntegrationTests\Authentication\firebase-configuration.json  
note: this file must be copy to output directory after build  
content:  
```
{
    "apiKey": "[your_api_key_to_created_firebase_project]"
}
```
7. Run M33tingClub.IntegrationTests