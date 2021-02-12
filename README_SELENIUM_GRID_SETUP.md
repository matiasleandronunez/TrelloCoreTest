# Seting up your Selenium Grid

## Prerequisites

Docker, docker-compose are installed
For Android, Host OS must be Linux
All that follows was tested in an Ubuntu 20.04 LTS. Will probably work in other distros, but no warranties given
## Notes
Images provided as example. You can build your own images and compose them as needed
Images used are based on DEBUG versions, meaning they have VNC servers enabled and just 1 instance per node. This are intended for debug, of coursem and should NOT be used for actual automation.
## Installation

1. Clone the repository and unzip to a directory of your preference.
2. Launch bash / powershell and go to ` <root>/TestGrid/ `
3. Build the docker images that you want to deploy, in case for instance you want to use f.e. old browser or different webdriver versions or devices. The example custom images shipped with this example are in their own directories at ` <root>/TestGrid/<image> `
Sample build command for Firefox 77 image: `$ docker build -t firefox77 . `
4. Once all required images are built, go back to `<root>/TestGrid/` and compose the docker-compose.yaml:
`$ docker-compose up`


## Monitoring

- Launch a browser and navigate to http://localhost:4444/grid/console, you should be able to see the grid and it's nodes.
- You can VNC to containers if a VNC is enabled by pointing to the port specyfied in the docker compose file. For this example localhost:5900, 5901, etc.
- Android containers can be reached by noVNC. Just open a new tab and go to URI localhost and again the ports specifyied at docker compose. For this example localhost:6080 and localhost:6081


[Need more help?](https://github.com/matiasleandronunez/TrelloCoreTest/blob/master/README_SELENIUM_GRID_SETUP_FROM_ZERO.md)
