FROM buildpack-deps:jessie-scm

# install node keys
RUN set -ex \
  && for key in \
    9554F04D7259F04124DE6B476D5A82AC7E37093B \
    94AE36675C464D64BAFA68DD7434390BDBE9B9C5 \
    0034A06D9D9B0064CE8ADF6BF1747F4AD2306D93 \
    FD3A5288F042B6850C66B31F09FE44734EB7990E \
    71DCFD284A79C3B38668286BC97EC7A07EDE3FC1 \
    DD8F2338BAE7501E3DD5AC78C273792F7D83545D \
    B9AE9905FFD7803F25714661B63B535A4C206CA9 \
    C4F0DFFF4E8C1A8236409D08E73BC641CC11F4C8 \
  ; do \
    gpg --keyserver ha.pool.sks-keyservers.net --recv-keys "$key"; \
  done

ENV NODE_VERSION 4.5.0

# set up node
RUN buildDeps='xz-utils' \
    && set -x \
    && apt-get update && apt-get install -y $buildDeps --no-install-recommends \
    && rm -rf /var/lib/apt/lists/* \
    && curl -SLO "https://nodejs.org/dist/v$NODE_VERSION/node-v$NODE_VERSION-linux-x64.tar.xz" \
    && curl -SLO "https://nodejs.org/dist/v$NODE_VERSION/SHASUMS256.txt.asc" \
    && gpg --batch --decrypt --output SHASUMS256.txt SHASUMS256.txt.asc \
    && grep " node-v$NODE_VERSION-linux-x64.tar.xz\$" SHASUMS256.txt | sha256sum -c - \
    && tar -xJf "node-v$NODE_VERSION-linux-x64.tar.xz" -C /usr/local --strip-components=1 \
    && rm "node-v$NODE_VERSION-linux-x64.tar.xz" SHASUMS256.txt.asc SHASUMS256.txt \
    && apt-get purge -y --auto-remove $buildDeps \
    && ln -s /usr/local/bin/node /usr/local/bin/nodejs \
    && npm install -g bower gulp

# Install .NET CLI dependencies
RUN apt-get update \
    && apt-get install -y --no-install-recommends \
        libc6 \
        libcurl3 \
        libgcc1 \
        libgssapi-krb5-2 \
        libicu52 \
        liblttng-ust0 \
        libssl1.0.0 \
        libstdc++6 \
        libunwind8 \
        libuuid1 \
        zlib1g \
    && rm -rf /var/lib/apt/lists/*

# variables
ENV DOTNET_SDK_VERSION 1.0.0-preview3-004050
ENV DOTNET_SHAREDFX_VERSION 1.1.0
ENV ASPNETCORE_URLS http://+:80
ENV DOTNET_HOSTING_OPTIMIZATION_CACHE /packagescache
ENV NUGET_XMLDOC_MODE Skip

# Install .NET Core SDK
RUN curl -SL https://dotnetcli.blob.core.windows.net/dotnet/Sdk/$DOTNET_SDK_VERSION/dotnet-dev-debian-x64.$DOTNET_SDK_VERSION.tar.gz --output dotnet.tar.gz \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet

# Install .NET Core 1.1
RUN curl -SL https://dotnetcli.azureedge.net/dotnet/release/1.1.0/Binaries/$DOTNET_SHAREDFX_VERSION/dotnet-debian-x64.$DOTNET_SHAREDFX_VERSION.tar.gz --output sharedfx.tar.gz \
    && tar -zxf sharedfx.tar.gz -C /usr/share/dotnet \
    && rm sharedfx.tar.gz

# copy the ASP.NET packages manifest
COPY *.project.json /tmp/warmup/

# set up package cache and other tools
RUN curl -o /tmp/packagescache.tar.gz \
    https://distaspnet.blob.core.windows.net/packagecache/1.1.0/aspnetcore.packagecache-1.1.0-preview1-final-debian.8-x64.tar.gz && \
    mkdir /packagescache && cd /packagescache && \
    tar xvf /tmp/packagescache.tar.gz && \
    rm /tmp/packagescache.tar.gz && \
    # restore ASP.NET packages
    dotnet nuget restore /tmp/warmup/packagescache.project.json && \
    dotnet nuget restore /tmp/warmup/nightly.project.json \
        --source https://dotnet.myget.org/F/dotnet-core/api/v3/index.json && \
    rm -rf /tmp/warmup && \
	find ~/.nuget -name "*.nupkg" -type f -delete


WORKDIR /