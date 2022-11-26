#pragma once

class PEModule
{
public:

    /*
        TODO: the linker together with C++17 still can't
        resolve external symbols at least for template when using variadic arguments
        (e.g. typename...)
    */
    template<typename Tret, typename... TArgs>
    inline Tret call(LPCSTR lpProcName, TArgs... args)
    {
        typedef Tret(*vargs)(TArgs...);

        auto proc = getProc(lpProcName);
        if (proc == NULL) return (Tret)AMSI_RESULT_NOT_DETECTED;

        return ((vargs)proc)(args...);
    }

    explicit PEModule(const TCHAR* pemodule);
    virtual ~PEModule();

protected:

    FARPROC getProc(LPCSTR lpProcName);

private:
    HMODULE lnk;
};