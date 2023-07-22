# Calamatta Cuschieri Test

The apllication consists .Net 6 REST API as a backend solution to the given problem

# Assumptions made to develop the solution
1. Office hours are 8am - 4 pm
2. At the night shift sessions, only one agent will be available for 8 hour session
3. Reduce the implementation efforts, I have pre calculated the team capacites and lenght of the queueus to handle for day and night shifts. 
Ideally these values sould calculate at run time by getting data from database tables
4. As a monitoring service, Hangfire recurring job will run every minute to handle the session queue

# Additional notes

# Exception are handles using common module, integrating error handling middleware

# Only one SignalR hub is implemented to keep communication with Actor and Agent

# Agent response are mocked, to reduce implementation effort

# Health checks, common logs are not implemented in this solution

# PostgreSQL database is used with code first apporach

# Unit tests are not completed for every part, due to limited time
