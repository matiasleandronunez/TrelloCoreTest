# Before starting

## What is Selenium Grid?

Selenium Grid allows the execution of WebDriver scripts on remote machines (virtual or real) by routing commands sent by the client to remote browser instances. It aims to provide an easy way to run tests in parallel on multiple machines.

Selenium Grid allows us to run tests in parallel on multiple machines, and to manage different browser versions and browser configurations centrally (instead of in each individual test).

 

Selenium grid 3 was used for this setup, but at the time of writing Selenium Grid 4 was in alpha.

## What is Docker?

Intro

Docker is an open platform for developing, shipping, and running applications. Docker enables you to separate your applications from your infrastructure so you can deliver software quickly. With Docker, you can manage your infrastructure in the same ways you manage your applications.

https://docs.docker.com/get-started/overview/

## Dockerfile

a Dockerfile is a plain text (no extension, must be named Dockerfile) that has a sequence of commands from which an image will be built. This image will be deployed into a container, and works as a template. Can be used to deploy as many containers as the user needs. 

## docker-compose.yml

A yml / yaml file that is named docker-compose. It has environment settings, dependencies and execution order for a whole infrastructure of docker containers to be deployed.  

## Purpose of this document

For the reader to be able to understand how to set up and deploy a testing grid / lab to be user in both automated and manual testing. 

# Requirements

Android emulators require a Linux x64 host to work. Base OS used for all that follows was Ubuntu 20.04 LTS, available at https://releases.ubuntu.com/20.04/

# Set up

### Installing docker

Core installation

The easiest way is by far using the package installer:
```
 $ sudo apt-get update
 $ sudo apt-get install docker-ce docker-ce-cli containerd.io
```
For alternatives, or installing older versions refer to https://docs.docker.com/engine/install/ubuntu/ (For Ubuntu distro)

Or to https://docs.docker.com/engine/install/ for different OS 

### docker-compose command

docker-compose may not be bundled with the engine in Linux. To install this component and be able to use it:

<lastversion> as the time of writing this document is 1.28.0
```
sudo curl -L "https://github.com/docker/compose/releases/download/<latestversion>/docker-compose-$(uname -s)-$(uname -m)" -o /usr/local/bin/docker-compose
sudo chmod +x /usr/local/bin/docker-compose
```
 
### Installing a visual interface for docker

You may need or find useful to have a visual interface for docker. This comes bundled with macOS and Windows versions but for Linux you need to install separately.

One good alternative is Dockstation.

Get the latest package for the OS (Ubuntu one is .deb) from  https://github.com/DockStation/dockstation/releases

Then install the package either from GNOME graphical UI or with apt install 

# Building the images

Most of the images used in this document are maintained by third parties and require no building, they can just be invoked in the compose file. 

But some must be built and stored locally or in a private repository (f.e: Tuned up android images, current webdrivers with older browsers)

In order to compose these images, they have to be built first. This is done by building from a dockerfile, using the following command

`$ docker build -t mytag . `

This will build using the dockerfile in the current location and tag the image as mytag

We can then use this image in the docker-compose or to run it independently as needed

## Base docker repos used

Base Images used for our Grid come from the official selenium docker ( ) and budtmo’s i ages from the docker-android proyect   which in turn is itself a development from the official appium docker images

The following custom are images currently used in the example:

Android 8.1 image (bundled with chrome 69) with matching 2.41 Chromedriver:

```
FROM budtmo/docker-android-x86-8.1

USER root

RUN wget -qP /tmp/ "https://chromedriver.storage.googleapis.com/2.41/chromedriver_linux64.zip"

RUN unzip -o /tmp/chromedriver_linux64.zip -d /root/

RUN rm /tmp/chromedriver*

RUN adb reconnect

Older Firefox Debug (debug in this case means it has a build it VNC server that allows rdp’ing in with a VNC client):

FROM selenium/node-firefox-debug:3.141.59-20210105

LABEL authors=SeleniumHQ

USER root

ARG FIREFOX_VERSION=77.0b9

RUN apt-get update -qqy \

  && apt-get -qqy --no-install-recommends install firefox \

  && rm -rf /var/lib/apt/lists/* /var/cache/apt/* \

  && wget --no-verbose -O /tmp/firefox.tar.bz2 https://download-installer.cdn.mozilla.net/pub/firefox/releases/$FIREFOX_VERSION/linux-x86_64/en-US/firefox-$FIREFOX_VERSION.tar.bz2 \

  && apt-get -y purge firefox \

  && rm -rf /opt/firefox \

  && tar -C /opt -xjf /tmp/firefox.tar.bz2 \

  && rm /tmp/firefox.tar.bz2 \

  && mv /opt/firefox /opt/firefox-$FIREFOX_VERSION \

  && ln -fs /opt/firefox-$FIREFOX_VERSION/firefox /usr/bin/firefox


USER 1200
```

# Launching the Grid / Lab

Once all images required are build, all that needs to be done is compose with the docker-compose.yaml file. In the directory where the file is, run:

` $ docker-compose up `

If everything goes as expected, selenium grid and the attached nodes should be up running! This can be verified in the docker GUI.

### Example docker-compose file

This docker-compose file uses both custom images build locally and a couple of online image repos. This will launch a grid with Opera 73, Firefox 77, Firefox latest debug version (84 as time of writing), Chrome latest debug version (87 as time of writing), 2 Android emulators at v8.1 and v11.0 and 1 with a real device attached (Real devices require extra setup steps [TBD] )

```
version: "3"

services:

  selenium-hub:

    image: selenium/hub:latest

    container_name: selenium-hub

    ports:

      - "4444:4444"

  chrome-debug:

    image: selenium/node-chrome-debug:latest

    container_name: chrome_latest

    volumes:

      - /dev/shm:/dev/shm

    depends_on:

      - selenium-hub

    environment:

      - HUB_HOST=selenium-hub

      - HUB_PORT=4444

    ports:

      - "5900:5900"

  firefox-debug:

    image: selenium/node-firefox-debug:latest

    container_name: "firefox_latest"

    volumes:

      - /dev/shm:/dev/shm

    depends_on:

      - selenium-hub

    environment:

      - HUB_HOST=selenium-hub

      - HUB_PORT=4444

    ports:

      - "5901:5900"

  firefox-debug-77:

    image: firefox77:latest

    container_name: firefox77

    volumes:

      - /dev/shm:/dev/shm

    depends_on:

      - selenium-hub

    environment:

      - HUB_HOST=selenium-hub

      - HUB_PORT=4444

    ports:

      - "5902:5900"

  opera-debug:

    image: selenium/node-opera-debug:3.141.59-20210105

    container_name: opera_latest

    volumes:

      - /dev/shm:/dev/shm

    depends_on:

      - selenium-hub

    environment:

      - HUB_HOST=selenium-hub

      - HUB_PORT=4444

    ports:

      - "5903:5900"
     
  real_device:

    image: appium/appium

    depends_on:

      - selenium-hub

    network_mode: "service:selenium-hub"

    privileged: true

    volumes:

      - /dev/bus/usb:/dev/bus/usb

      - ~/.android:/root/.android

      - $PWD/example/sample_apk:/root/tmp

    environment:

      - CONNECT_TO_GRID=true

      - SELENIUM_HOST=selenium-hub

      # Enable it for msite testing

      - BROWSER_NAME=chrome
      
  samsung_s6_8_1:

    image: android81:latest

    privileged: true

    # Increase scale number if needed

    scale: 1

    depends_on: 

      - selenium-hub
      - real_device

    ports:

      - "6080:6080"

    volumes:

      - ./video-samsung_7.1.1:/tmp/video

    environment:
      - CHROME_DRIVER=88.0.4324.96
      - DEVICE=Samsung Galaxy S6
      - CONNECT_TO_GRID=true
      - APPIUM=true
      - SELENIUM_HOST=selenium-hub
      - SELENIUM_PORT=4444
      - MOBILE_WEB_TEST=true
      - AUTO_RECORD=true
      - SELENIUM_TIMEOUT=30
      - NO_PROXY="localhost"
 
  samsung_11.0:

    image:  budtmo/docker-android-x86-11.0

    privileged: true

    # Increase scale number if needed

    scale: 1

    depends_on:
      - selenium-hub
      - real_device

    ports:

      - "6081:6080"

    volumes:

      - ./video-samsung_11.0:/tmp/video

    environment:
      - DEVICE=Samsung Galaxy S10
      - CONNECT_TO_GRID=true
      - APPIUM=true
      - SELENIUM_HOST=selenium-hub
      - SELENIUM_PORT=4444
      - MOBILE_WEB_TEST=true
      - AUTO_RECORD=true
      - SELENIUM_TIMEOUT=30
      - NO_PROXY="localhost"
```

# Using the Grid / Lab

Grid’s natural use is for automation testing. It allows parallel execution and load balancing. With the steps provided however, what was actually build is a debug version which serves better as a testing Lab. 

This is because debug versions include VNC servers for RDP and GUI both not recommended for automation servers as they eat resources, and only allow 1 instance per node, whereas not debug containers allow multiple browser instances per node, only limitation being hardware. 

Changing this example from debug to automation use is as simple as modifying built images and retrieved images to their non debug versions and add the variables in the docker-compose for launching multiple instances. 

## Accessing Grid dashboard

If everything in the guide went well, then when accessing the hub at http://localhost:4444/grid/console you should see something like:

#noVNC to android containers

Android containers use noVNC, can be accessed directly through the browser at the port specified during docker-compose. For the example previously provided, the 2 Android emulators should be reachable at: 

http://localhost:6080/ and http://localhost:6081/

## VNC Browser containers

Debug Browser containers can be accessed with a VNC client by using the port specified during docker-compose. For the example previously provided, these would be ports 5900, 5901, 5902… one assigned to each container

default password is secret

## Useful commands

### List Containers

$ docker ps -a -q

### Stop all running containers

$ docker stop $(docker ps -a -q)

### Remove all containers

$ docker rm $(docker ps -a -q)

