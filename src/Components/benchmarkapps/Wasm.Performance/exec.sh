#!/usr/bin/env bash

/opt/bin/start-selenium-standalone.sh&
./app/Wasm.Performance.Driver $StressRunDuration&
./aot-app/Wasm.Performance.Driver $StressRunDuration
