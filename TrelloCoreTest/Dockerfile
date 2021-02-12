FROM mcr.microsoft.com/dotnet/core/sdk:3.1

WORKDIR /src

ADD . ./

RUN dotnet build -c Debug

RUN chmod 755 run_tests.sh

#Run tests and remain in Infinite loop, outputs to current dir TestResults.trx
ENTRYPOINT ["/bin/bash","/src/run_tests.sh"]