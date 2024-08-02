# CRDIC - Commandline Runtime Dependency Injection Configuration

Control the dependency injection container, required services, and service parameters via configuration at runtime in a console app.

This allows the application to be written to support different deployment scenarios based on configuration.

For example:

- Container configuration, service components used, or service configuration can be loaded based on environment such as Development, QA, or Production.
- Different service providers be used or migrated, for example SQLServerCache / Redis, based on configuration.
- A new component can be deployed alongside an old component, with the ability to immediately revert to the old component via configuration.
- Can support A/B testing scenarios.
