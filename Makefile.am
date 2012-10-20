
EXTRA_DIST =  expansions.m4

if ENABLE_DEBUG
 SUBDIRS =  src
endif
if ENABLE_RELEASE
 SUBDIRS =  src
endif

run:
	cd src/bin/Debug; mono --runtime=v4.0 DBusExplorer.exe
