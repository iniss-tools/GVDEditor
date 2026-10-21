; Instalator GVDEditora. Spolocny obsah instalatorov je v ToolsCore\Installer\AppSetup.iss.
; Pred kompilaciou treba zostavit Release: dotnet build GVDEditor\GVDEditor.csproj -c Release

#define MyAppName "GVDEditor"
#define MyAppExeName "GVDEditor.exe"
#define MyAppDir SourcePath + "GVDEditor\bin\Release\net10.0-windows"
#define MyOutputBaseFilename "gvdeditor-setup"
; server MCP pre agentov bezi v procese editora
#define NeedAspNetCore
; import z ELIS bezi v 32-bitovom procese ELISBridge (TT.dll je 32-bitova)
#define NeedRuntimeX86

#include "..\ToolsCore\Installer\AppSetup.iss"
