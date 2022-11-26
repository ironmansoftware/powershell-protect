#include "stdafx.h"

PEModule::PEModule(const TCHAR* pemodule)
{
    lnk = LoadLibraryEx(pemodule, NULL, LOAD_WITH_ALTERED_SEARCH_PATH);
}

PEModule::~PEModule()
{
    if (lnk != NULL) {
        FreeLibrary(lnk);
    }
}

FARPROC PEModule::getProc(LPCSTR lpProcName)
{
    if (lnk == NULL) return NULL;

    auto addr = GetProcAddress(lnk, lpProcName);

    return addr;
}