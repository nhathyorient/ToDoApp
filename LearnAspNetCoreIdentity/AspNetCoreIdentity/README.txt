4 Essentials Parts:
	- UI: Identity.UI
	- Functionalities: Identity
	- Data Store: Identity.EntityFrameworkCore
	- Actual Database (Ex: SQL Server) : EntityFrameworkCore.Design; EntityFrameworkCore.Tools

In AspnetCoreIdentity theres some main services:
	- SigninManager: Help to verify credentials and generating security context and does Authentication
		- For Authorization, we still need to create our own custom policy and IAuthorizationHandler if needed. 
		By default It's handled by default generic AuthorizationHandler for basic by Role
	- UserManager: help us to get all user information in the database