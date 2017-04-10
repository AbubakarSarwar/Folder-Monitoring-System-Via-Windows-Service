# Folder-Monitoring-System-Via-Windows-Service

This was an assignment I did for the course Information Processing Techniques:

It consists of two different services;
The first service is supposed to check the specified folder every minute for any changes. If
there are any changes, it copies the new or updated files to another fixed location. If
there are no changes then it increases the delay by additional 2 minutes until it
reaches 1-hour delay. The delay doesn't exceed 1-hour gap.
The second service is used to send email to the local user (email address would be in the
configuration file). This service checks for changes in the specified folder every 15
minutes and if there is any change, it notifies the user with the filename and file Size. 
