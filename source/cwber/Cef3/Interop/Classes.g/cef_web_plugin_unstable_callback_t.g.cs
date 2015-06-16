//
// DO NOT MODIFY! THIS IS AUTOGENERATED FILE!
//
namespace Cef3.Interop
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using System.Runtime.InteropServices;
    using System.Security;
    
    [StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
    [SuppressMessage("Microsoft.Design", "CA1049:TypesThatOwnNativeResourcesShouldBeDisposable")]
    internal unsafe struct cef_web_plugin_unstable_callback_t
    {
        internal cef_base_t _base;
        internal IntPtr _is_unstable;
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate int add_ref_delegate(cef_web_plugin_unstable_callback_t* self);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate int release_delegate(cef_web_plugin_unstable_callback_t* self);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate int get_refct_delegate(cef_web_plugin_unstable_callback_t* self);
        
        [UnmanagedFunctionPointer(libcef.CEF_CALLBACK)]
        #if !DEBUG
        [SuppressUnmanagedCodeSecurity]
        #endif
        internal delegate void is_unstable_delegate(cef_web_plugin_unstable_callback_t* self, cef_string_t* path, int unstable);
        
        private static int _sizeof;
        
        static cef_web_plugin_unstable_callback_t()
        {
            _sizeof = Marshal.SizeOf(typeof(cef_web_plugin_unstable_callback_t));
        }
        
        internal static cef_web_plugin_unstable_callback_t* Alloc()
        {
            var ptr = (cef_web_plugin_unstable_callback_t*)Marshal.AllocHGlobal(_sizeof);
            *ptr = new cef_web_plugin_unstable_callback_t();
            ptr->_base._size = (UIntPtr)_sizeof;
            return ptr;
        }
        
        internal static void Free(cef_web_plugin_unstable_callback_t* ptr)
        {
            Marshal.FreeHGlobal((IntPtr)ptr);
        }
        
    }
}
