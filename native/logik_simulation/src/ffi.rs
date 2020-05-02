use crate::data::Data;
use crate::data::component::{Component, Output, AND};
use std::iter;
// Converts a rust function definition into one that can be called from c (and by extension c#).
// A function with the signature 'fn <name>(<params>) <return> { <body> }'
/* Into a function with the signature 

#[allow(unsafe_code, unused_attributes)]
#[no_mangle]
pub unsafe extern "cdecl" fn <name>(<params>) <return> {
    <body>
}

*/
#[macro_use]
macro_rules! ffi {
    ($(fn $name:ident ( $( $arg_ident:ident : $arg_ty:ty),* ) $( -> $ret_ty:ty)? $body:block)*) => {
        $(
            #[allow(unsafe_code, unused_attributes)]
            #[no_mangle]
            pub unsafe extern "cdecl" fn $name( $($arg_ident : $arg_ty),* ) $(-> $ret_ty)? {
                $body
            }
        )*
    };
}

#[no_mangle]
pub extern "C" fn init() -> *mut Data {
    Box::into_raw(Box::new(Data::new()))
}

#[no_mangle]
pub extern "C" fn exit(data: *mut Data) {
    unsafe { drop(Box::from_raw(data)) };
}

#[no_mangle]
pub extern "C" fn add_subnet(data: *mut Data, id: usize) -> bool {
    let data = unsafe { &mut *data};
    
    data.add_subnet(id)
}

#[no_mangle]
pub extern "C" fn remove_subnet(data: *mut Data, id: usize) -> bool {
    let data = unsafe { &mut *data};
    
    data.remove_subnet(id)
}

#[no_mangle]
pub extern "C" fn add_component(data: *mut Data, component: usize) -> usize {
    let data = unsafe { &mut *data};
    
    let component: Box<dyn Component> = match component {
        0 => Box::new(Output {}),
        1 => Box::new(AND {}),
        _ => unreachable!() // UB
    };
    
    let i = component.inputs();
    let o = component.outputs();
    
    let res = data.add_component(component,
                       iter::repeat(None).take(i).collect(),
                       iter::repeat(None).take(o).collect());
    
    res.unwrap()
}

#[no_mangle]
pub extern "C" fn remove_component(data: *mut Data, id: usize) -> bool {
    let data = unsafe { &mut *data };
    
    data.remove_component(id)
}

#[no_mangle]
pub extern "C" fn link(data: *mut Data, component: usize, port: usize, subnet: usize, direction: bool) -> bool {
    let data = unsafe { &mut *data };
    
    data.link(component, port, subnet, direction)
}

#[no_mangle]
pub extern "C" fn unlink(data: *mut Data, component: usize, port: usize, subnet: usize) -> bool {
    let data = unsafe { &mut *data };
    
    data.unlink(component, port, subnet)
}