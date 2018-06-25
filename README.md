# IoT-Database-Server
Simple HTTP Database Server for IoT Projects




# Documentation

## USAGE
http://[HOSTNAME]:[PORT]/http/?[OPTION1]&[OPTION2]&[...]

## OPTIONS
### u=[USER]	-	Username input		(not allowed: '=', '%', ' ')
### k=[KEY]		-	Key input		(not allowed: '=', '%', ' ')
### v=[VAR]		-	Select variable		(not allowed: '=', '%', ' ')
### a=[ACTION]	-	Select action		(not allowed: '=', '%', ' ')
### c=[CONTENT]	-	Set (new) content	(not allowed: '=', '%', ' ')

## ACTIONS
### r	-	read		-	read selected variable
### n	-	new		-	creates new variable with specified name and content
### e	-	edit		-	overrides selected variable with content
### d	-	delete		-	deletes selected variable
### l	-	list		-	list all variables

## EXAMPLE
### Read [VAR]:
	http://[HOSTNAME]:[PORT]/http/?u=[USER]&k=[KEY]&a=r&v=[VAR]

### Create [VAR] with [CONTENT]:
	http://[HOSTNAME]:[PORT]/http/?u=[USER]&k=[KEY]&a=n&v=[VAR]&c=[CONTENT]

### List all variables:
	http://[HOSTNAME]:[PORT]/http/?u=[USER]&k=[KEY]&a=l

### Delete [VAR]:
	http://[HOSTNAME]:[PORT]/http/?u=[USER]&k=[KEY]&a=d&v=[VAR]

### Replace content of [VAR] with [CONTENT]:
	http://[HOSTNAME]:[PORT]/http/?u=[USER]&k=[KEY]&a=e&v=[VAR]&c=[CONTENT]
