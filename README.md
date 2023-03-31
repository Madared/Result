A Result type for CSharp which allows for error passing without exceptions
=

This package is an attempt to allow consumers to pass exceptions and custom errors upstream without having to try and catch exceptions that may change over time
thus decreasing the maintenance cost of changes and bugfixes downstream.
This also allows for more functionality within error with an Error interface which can be extended to provide better usability.
	Ex: Having a method to return the appropriate **ObjectResult** for a failure of a rest api;
