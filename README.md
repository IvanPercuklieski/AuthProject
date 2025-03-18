An authentication system from scratch, implementing key security measures such as
salting passwords and storing the salted hashes in the database.  Additionally, I
integrated two-factor authentication (2FA) by generating a unique one-time
password (OTP) for the user and sending it via email using Google’s SMTP server. This
system enhances security by verifying the user’s identity through both their password
and a time-sensitive OTP delivered to their email. It also includes role-based authorization,
ensuring users have access to the appropriate resources based on their roles.

How it works:

1. First to register, you enter an email, username, and password. The system checks if such a user exists in the
database. If not, it creates a TempUser with an ExpiryTime and OTP code. If the user already
exists, an exception is thrown indicating that the username or email is already taken.

2. If the registration is successful, the user is sent to verification. When creating the TempUser,
the OTP code is sent to the provided email. The OTP code is part of the TempUser, and it also
has an ExpiryTime. The OTP code must be entered within 3 minutes if not, it becomes invalid.

 ** Additionally, there is a background service running every 4 minutes that deletes any TempUsers
whose ExpiryTime has passed. This is so we can remove stuff we no longer need from the database. **

3. Once the OTP is successfully verified, you proceed to the login, where you only log in using the
username and password. After that a new OTP is sent your email address and you need to enter in in 3
minutes, if you fail you will need to ask for a new OTP to be sent.
