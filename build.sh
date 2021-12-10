#!/usr/bin/env bash

###########################################################################
# INSTALL CAKE
###########################################################################

dotnet tool restore

###########################################################################
# BOOTSTRAP BUILD SCRIPT
###########################################################################
dotnet cake recipe.cake --bootstrap

###########################################################################
# RUN BUILD SCRIPT
###########################################################################

dotnet cake recipe.cake "$@"