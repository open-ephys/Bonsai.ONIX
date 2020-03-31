## TODO

 - So much trouble getting the correct native dll to copy to the build directory.
 - I can run the clroepcie-test program fine
 - I can nuget package the clroni library fine and I can see the native liboepcie.dll in there
 - When I run this bonsai package, the liboni.dll is not copied to build directory. I have to do it manually.
 - Also note that this dll needs to placed in whatever has been marked as the startup project's bin/<arch>/<Debug/Release> directory (e.g. if you are using the prototyping project as startup).