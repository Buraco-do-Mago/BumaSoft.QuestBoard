{
  description = "TEMPLATE Flake";

  inputs = {
    nixpkgs.url = "github:nixos/nixpkgs?ref=nixpkgs-unstable";
    flake-utils.url = "github:numtide/flake-utils";
    rosetta.url = "github:lucaaaaum/rosetta";
  };

  outputs =
    {
      self,
      nixpkgs,
      flake-utils,
      rosetta,
    }:
    flake-utils.lib.eachDefaultSystem (
      system:
      let
        pkgs = import nixpkgs {
          inherit system;
        };

        sdk = pkgs.dotnet-sdk_10;
        runtime = pkgs.dotnet-runtime_10;
        aspnet = pkgs.dotnet-aspnetcore_10;

        defaultPackages = with pkgs; [
          sdk
          runtime
          aspnet
          dotnet-ef
          dbeaver-bin
          rosetta.packages.${system}.default
        ];

        version = "0.0.0";

        buildPackage =
          {
            name,
            projectPath,
          }:
          let
            projectPathString = builtins.replaceStrings [ "${toString ./.}/" ] [ "" ] (toString projectPath);
          in
          pkgs.buildDotnetModule {
            pname = name;
            version = version;
            src = ./.;
            projectFile = projectPathString;
            nugetDeps = ./deps.nix;
            dotnet-sdk = sdk;
            dotnet-runtime = aspnet;
          };
      in
      with pkgs;
      {
        devShells = {
          default = mkShell {
            buildInputs = defaultPackages;
          };
        };
        packages = {
          api = buildPackage {
            name = "TEMPLATE.Api";
            projectPath = ./src/TEMPLATE.Api/TEMPLATE.Api.csproj;
          };
          all = pkgs.symlinkJoin {
            name = "TEMPLATE.All";
            paths = [
              self.packages.${system}.api
            ];
          };
        };
      }
    );
}
