# chess-api

ChessAPI is a .NetCore web API which is consumed by [thrones-chess-web-playing](https://github.com/aydoganf/thrones-chess-web-playing)

This API has ability to store session info into a database also a json file. 

API end-points are:

###### [POST] /create:
- This end-point is used to create a new session.

###### [GET] /:
- This end-point is used to get session informations.

###### [POST] /:
- This end-point is used to command a movement
