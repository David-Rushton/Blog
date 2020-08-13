FROM mcr.microsoft.com/azure-functions/node:3.0-node12

ENV AzureWebJobsScriptRoot=/home/site/wwwroot \
    AzureFunctionsJobHost__Logging__Console__IsEnabled=true \
    FUNCTIONS_WORKER_RUNTIME=node \
    DB_NAME=blogdb \
    DB_CONTAINER=articles \
    DB_RW_CONNECTION_STRING=$}{DB_RW_CONNECTION_STRING}

COPY . /home/site/wwwroot

RUN cd /home/site/wwwroot && \
    npm install
