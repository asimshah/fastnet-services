# fastnet-services
This repo contains the only the one remaining old-style windows service (based on topshelf, etc..): Fastnet.Timer.Services. This service is used to provide regular polling of a (local) webframe site.

This service uses webframe-libraries and webframe-event-system.

(In future where a service is required, aspnetcore will be used - this allows the service itself to be controlled via webapi)
