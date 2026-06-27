# Building Validate.exe from source

The `win64/Validate.exe` binary is built from https://github.com/KCL-Planning/VAL (KCL-Planning).
The repository ships pre-generated bison/flex parser sources, so no extra tools are needed.

## Requirements

- Visual Studio 2022 (Community edition is fine) with the **Desktop development with C++** workload
- CMake — bundled with VS 2022, found at:
  `C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\CommonExtensions\Microsoft\CMake\CMake\bin\cmake.exe`

## Steps

```bat
git clone --depth=1 https://github.com/KCL-Planning/VAL.git
cd VAL

:: Patch: build as static library so MSVC can link without DLL export annotations
:: Change  add_library(VAL SHARED  to  add_library(VAL STATIC  in CMakeLists.txt

mkdir build && cd build

cmake .. -G "Visual Studio 17 2022" -A x64
cmake --build . --config Release --target VAL
cmake --build . --config Release --target Validate
```

The binary is then at `build\bin\Release\Validate.exe`.
Copy it to `UnityPackage\Editor\Validation\Binaries\win64\Validate.exe`.

## Why STATIC instead of SHARED

VAL's CMakeLists only adds the `VAL_EXPORTS` define (needed for `__declspec(dllexport)`)
when building with GCC, not MSVC. Without it, global variables in the DLL are not exported
and the linker fails with ~97 unresolved externals. Building as a static library sidesteps
this entirely.
