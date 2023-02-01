# Authorization in Swagger

## Get authorization token from Firebase

```
https://www.googleapis.com/identitytoolkit/v3/relyingparty/verifyPassword?key=[your_Web_API_Key]
```

Web Api Key - can be found in Firebase project settings pade

<br>

## Use authorization token in Swagger
Click Authorize button (white-green button with padlock)  
Fill value field with:  
```
bearer [your_token]
```
Click Authorize