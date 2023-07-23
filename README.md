# Calamatta Cuschieri Test

The apllication consists .Net 6 REST API as a backend solution to the given problem

# Assumptions made to develop the solution
1. Office hours are 8am - 4 pm
2. At the night shift sessions, only one agent will be available for 8 hour session
3. Reduce the implementation efforts, I have pre calculated the team capacites and lenght of the queueus to handle for day and night shifts. 
Ideally these values sould calculate at run time by getting data from database tables
4. As a monitoring service, Hangfire recurring job will run every minute to handle the session queue


# Additional notes

1. Exception are handles using common module, integrating error handling middleware

2. Only one SignalR hub is implemented to keep communication with Actor and Agent

3. Agent response are mocked, to reduce implementation effort

4. Health checks, common logs are not implemented in this solution

5. PostgreSQL database is used with code first apporach

6. Unit tests are not completed for every part

# Improvement points
1. Agent capacity, queue length/storage to be store in the database tables and calculate at the run time
2. Use message broker service to store queue data, so this can be implemented as a separate service
3. Values added in the configuration can be move to more suitable/secure storage