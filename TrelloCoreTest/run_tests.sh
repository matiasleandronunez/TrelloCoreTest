#!/bin/bash
dotnet test -l:trx
while true; do sleep 1; done