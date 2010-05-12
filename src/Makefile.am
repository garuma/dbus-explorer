
EXTRA_DIST =  

if ENABLE_DEBUG
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+ -debug -define:DEBUG
ASSEMBLY = bin/Debug/DBusExplorer.exe
ASSEMBLY_MDB = $(ASSEMBLY).mdb
COMPILE_TARGET = exe
PROJECT_REFERENCES = 
BUILD_DIR = bin/Debug

DBUSEXPLORER_EXE_MDB_SOURCE=bin/Debug/DBusExplorer.exe.mdb
DBUSEXPLORER_EXE_MDB=$(BUILD_DIR)/DBusExplorer.exe.mdb
DBUS_EXPLORER_DESKTOP_SOURCE=app.desktop

endif

if ENABLE_RELEASE
ASSEMBLY_COMPILER_COMMAND = gmcs
ASSEMBLY_COMPILER_FLAGS =  -noconfig -codepage:utf8 -warn:4 -optimize+
ASSEMBLY = bin/Release/DBusExplorer.exe
ASSEMBLY_MDB = 
COMPILE_TARGET = exe
PROJECT_REFERENCES = 
BUILD_DIR = bin/Release

DBUSEXPLORER_EXE_MDB=
DBUS_EXPLORER_DESKTOP_SOURCE=app.desktop

endif

AL=al2
SATELLITE_ASSEMBLY_NAME=.resources.dll

PROGRAMFILES = \
	$(DBUSEXPLORER_EXE_MDB)  

LINUX_DESKTOPAPPLICATIONS = \
	$(DBUS_EXPLORER_DESKTOP)  

BINARIES = \
	$(DBUS_EXPLORER)  


RESGEN=resgen2

all: $(ASSEMBLY) $(PROGRAMFILES) $(LINUX_DESKTOPAPPLICATIONS) $(BINARIES) 

FILES = \
	./Caller/BaseCaller.cs \
	./Caller/ReflectionVisitor.cs \
	./Caller/MethodCaller.cs \
	./Caller/Json.cs \
	./Caller/InvocationData.cs \
	./Caller/PropertyCaller.cs \
	./Caller/JsonChecker.cs \
	./Widgets/ImageAnimation.cs \
	./Widgets/TabWidget.cs \
	./Widgets/InformationView.cs \
	./Widgets/BusContentView.cs \
	./Widgets/LanguageWidget.cs \
	./Widgets/BusPageWidget.cs \
	./Widgets/PropertyInvokeDialog.cs \
	./Widgets/GenerationMenuWidget.cs \
	./Widgets/MethodInvokeDialog.cs \
	./Widgets/GenerationDialog.cs \
	./Generator/IGenerator.cs \
	./Generator/CSharpCodeDomGenerator.cs \
	./LangSupport/LangDefinitionPool.cs \
	./LangSupport/LangParser.cs \
	./LangSupport/LangDefinition.cs \
	./LangSupport/ILangDefinition.cs \
	./LangSupport/LangDefinitionService.cs \
	./gtk-gui/DBusExplorer.MainWindow.cs \
	./gtk-gui/DBusExplorer.LanguageWidget.cs \
	./gtk-gui/DBusExplorer.InformationView.cs \
	./gtk-gui/DBusExplorer.CustomBusDialog.cs \
	./gtk-gui/generated.cs \
	./gtk-gui/DBusExplorer.GenerationDialog.cs \
	./gtk-gui/DBusExplorer.MethodInvokeDialog.cs \
	./gtk-gui/DBusExplorer.PropertyInvokeDialog.cs \
	./gtk-gui/DBusExplorer.BusPageWidget.cs \
	./Base/Logging.cs \
	./Base/MainWindow.cs \
	./Base/AssemblyInfo.cs \
	./Base/Main.cs \
	./Base/DBusExplorator.cs \
	./Base/CustomBusDialog.cs \
	./Base/Common.cs \
	./Parser/Parser.cs \
	./Parser/TextVisitors/LangDefVisitor.cs \
	./Parser/ParserNg.cs \
	./Parser/Mapper.cs \
	./Parser/IParserVisitor.cs \
	./Representation/PathContainer.cs \
	./Representation/ElementFactory.cs \
	./Representation/IElement.cs \
	./Representation/Enums.cs \
	./Representation/ElementRepresentation.cs \
	./Representation/Interface.cs \
	./Representation/Element.cs \
	./Representation/Argument.cs

DATA_FILES = \
	app.desktop 

RESOURCES = \
	gtk-gui/gui.stetic \
	data/event.png \
	data/method.png \
	data/property.png \
	data/empty.png \
	data/dbus-explorer.png \
	data/process-working.png \
	gtk-gui/objects.xml \
	data/formatters/csharp.lang.xml 

EXTRAS = \
	dbus-explorer.in 

REFERENCES =  \
	$(GTK_SHARP_20_LIBS) \
	System \
	System.Core \
	Mono.Posix \
	System.Xml \
	$(NDESK_DBUS_10_LIBS) \
	$(NDESK_DBUS_GLIB_10_LIBS)

DLL_REFERENCES = 

CLEANFILES = $(PROGRAMFILES) $(LINUX_DESKTOPAPPLICATIONS) $(BINARIES) 

include $(top_srcdir)/Makefile.include

DBUS_EXPLORER = $(BUILD_DIR)/dbus-explorer
DBUS_EXPLORER_DESKTOP = $(BUILD_DIR)/dbus-explorer.desktop

$(eval $(call emit-deploy-wrapper,DBUS_EXPLORER,dbus-explorer,x))
$(eval $(call emit-deploy-target,DBUS_EXPLORER_DESKTOP))


$(eval $(call emit_resgen_targets))
$(build_xamlg_list): %.xaml.g.cs: %.xaml
	xamlg '$<'

$(ASSEMBLY) $(ASSEMBLY_MDB): $(build_sources) $(build_resources) $(build_datafiles) $(DLL_REFERENCES) $(PROJECT_REFERENCES) $(build_xamlg_list) $(build_satellite_assembly_list)
	mkdir -p $(shell dirname $(ASSEMBLY))
	$(ASSEMBLY_COMPILER_COMMAND) $(ASSEMBLY_COMPILER_FLAGS) -out:$(ASSEMBLY) -target:$(COMPILE_TARGET) $(build_sources_embed) $(build_resources_embed) $(build_references_ref)
