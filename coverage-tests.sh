#!/bin/bash

TEST_RESULT_DIR=..\\TestResult

echo "Testing UnitTests.csproj"

dotnet test CMS.API.Campaign.UnitTests/CMS.API.Campaign.UnitTests.csproj \
/p:CollectCoverage=True \
/p:CoverletOutputFormat=opencover \
/p:CoverletOutput="$TEST_RESULT_DIR\\TR-UnitTests.xml" \
-c Release \
